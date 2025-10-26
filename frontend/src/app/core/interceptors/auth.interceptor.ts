import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, filter, take, switchMap } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

  constructor(private authService: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Add auth header if user has a token and request is to our API
    // For logout requests, always try to add token even if expired
    if (this.hasToken() && this.isApiRequest(req)) {
      // console.log('Interceptor: Adding auth header to request:', req.url);
      req = this.addToken(req);
      // console.log('Interceptor: Request headers after adding token:', req.headers.keys());
    } else if (this.isApiRequest(req)) {
      console.log('Interceptor: No token available for API request:', req.url);
    }

    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        console.log('Interceptor: HTTP Error:', error.status, error.url);
        if (error.status === 401 && this.isApiRequest(req)) {
          return this.handle401Error(req, next);
        }
        return throwError(() => error);
      })
    );
  }

  private addToken(request: HttpRequest<any>): HttpRequest<any> {
    const token = this.authService.getToken();
    if (token) {
      const newRequest = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
      // console.log('Interceptor: Added Authorization header:', `Bearer ${token.substring(0, 20)}...`);
      return newRequest;
    }
    return request;
  }

  private isApiRequest(request: HttpRequest<any>): boolean {
    return request.url.includes('/api/');
  }

  private hasToken(): boolean {
    return !!this.authService.getToken();
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      return this.authService.refreshToken().pipe(
        switchMap((response: any) => {
          this.isRefreshing = false;
          this.refreshTokenSubject.next(response.token);
          return next.handle(this.addToken(request));
        }),
        catchError((error) => {
          this.isRefreshing = false;
          this.authService.logout().subscribe();
          return throwError(() => error);
        })
      );
    } else {
      return this.refreshTokenSubject.pipe(
        filter(token => token !== null),
        take(1),
        switchMap(() => next.handle(this.addToken(request)))
      );
    }
  }
}
