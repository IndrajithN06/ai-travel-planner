import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import {
  LoginRequest,
  RegisterRequest,
  AuthResponse,
  User,
  RefreshTokenRequest,
  ChangePasswordRequest,
  ForgotPasswordRequest,
  ResetPasswordRequest
} from '../../models/user/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly API_URL = environment.apiUrl || 'https://localhost:7121/api';
  private readonly TOKEN_KEY = 'auth_token';
  private readonly REFRESH_TOKEN_KEY = 'refresh_token';
  private readonly USER_KEY = 'current_user';

  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(private http: HttpClient) {
    this.initializeAuth();
  }

  private initializeAuth(): void {
    const token = this.getToken();
    const user = this.getCurrentUser();

    if (token && user) {
      this.currentUserSubject.next(user);
      this.isAuthenticatedSubject.next(true);
    }
  }

  login(loginRequest: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.API_URL}/auth/login`, loginRequest)
      .pipe(
        tap(response => {
          if (response.success && response.token && response.user) {
            this.setToken(response.token);
            this.setRefreshToken(response.refreshToken || '');
            this.setCurrentUser(response.user);
            this.currentUserSubject.next(response.user);
            this.isAuthenticatedSubject.next(true);
          }
        }),
        catchError(this.handleError)
      );
  }

  register(registerRequest: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.API_URL}/auth/register`, registerRequest)
      .pipe(
        tap(response => {
          if (response.success && response.token && response.user) {
            this.setToken(response.token);
            this.setRefreshToken(response.refreshToken || '');
            this.setCurrentUser(response.user);
            this.currentUserSubject.next(response.user);
            this.isAuthenticatedSubject.next(true);
          }
        }),
        catchError(this.handleError)
      );
  }

  logout(): Observable<any> {
    const refreshToken = this.getRefreshToken();
    const token = this.getToken();
    
    console.log('Logout attempt:', { 
      hasToken: !!token, 
      hasRefreshToken: !!refreshToken,
      tokenExpired: token ? this.isTokenExpired(token) : 'no token',
      tokenPreview: token ? token.substring(0, 20) + '...' : 'no token',
      apiUrl: this.API_URL
    });
    
    if (refreshToken && token) {
      // Check if token is expired
      if (this.isTokenExpired(token)) {
        console.log('Token is expired, clearing local data only');
        this.clearAuthData();
        return new Observable(observer => observer.next({}));
      }
      
      // Try server logout with valid tokens
      console.log('Making logout request to:', `${this.API_URL}/auth/logout`);
      return this.http.post(`${this.API_URL}/auth/logout`, { RefreshToken: refreshToken })
        .pipe(
          tap(() => {
            console.log('Logout successful');
            this.clearAuthData();
          }),
          catchError((error) => {
            console.error('Logout error:', error);
            console.error('Error details:', {
              status: error.status,
              statusText: error.statusText,
              url: error.url,
              message: error.message
            });
            // Even if server logout fails, clear local data
            this.clearAuthData();
            return new Observable(observer => observer.next({}));
          })
        );
    } else {
      // If no tokens available, just clear local data
      console.log('No tokens available, clearing local data only');
      this.clearAuthData();
      return new Observable(observer => observer.next({}));
    }
  }



  refreshToken(): Observable<AuthResponse> {
    const refreshToken = this.getRefreshToken();
    if (!refreshToken) {
      return throwError(() => new Error('No refresh token available'));
    }

    return this.http.post<AuthResponse>(`${this.API_URL}/auth/refresh-token`, { refreshToken })
      .pipe(
        tap(response => {
          if (response.success && response.token) {
            this.setToken(response.token);
            if (response.refreshToken) {
              this.setRefreshToken(response.refreshToken);
            }
          }
        }),
        catchError(error => {
          this.clearAuthData();
          return throwError(() => error);
        })
      );
  }

  getCurrentUserInfo(): Observable<User> {
    console.log('Getting current user info from:', `${this.API_URL}/auth/me`);
    return this.http.get<User>(`${this.API_URL}/auth/me`)
      .pipe(
        tap(user => {
          console.log('Successfully got user info:', user);
          this.setCurrentUser(user);
          this.currentUserSubject.next(user);
        }),
        catchError(error => {
          console.error('Error getting user info:', error);
          return this.handleError(error);
        })
      );
  }

  changePassword(changePasswordRequest: ChangePasswordRequest): Observable<any> {
    return this.http.post(`${this.API_URL}/auth/change-password`, changePasswordRequest)
      .pipe(catchError(this.handleError));
  }

  forgotPassword(forgotPasswordRequest: ForgotPasswordRequest): Observable<any> {
    return this.http.post(`${this.API_URL}/auth/forgot-password`, forgotPasswordRequest)
      .pipe(catchError(this.handleError));
  }

  resetPassword(resetPasswordRequest: ResetPasswordRequest): Observable<any> {
    return this.http.post(`${this.API_URL}/auth/reset-password`, resetPasswordRequest)
      .pipe(catchError(this.handleError));
  }

  // Token management methods
  getToken(): string | null {
    var token=localStorage.getItem(this.TOKEN_KEY);
    // console.log('Retrieved token from localStorage:', token ? token.substring(0, 20) + '...' : 'no token');
    return token;
  }

  private setToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_TOKEN_KEY);
  }

  private setRefreshToken(refreshToken: string): void {
    localStorage.setItem(this.REFRESH_TOKEN_KEY, refreshToken);
  }

  getCurrentUser(): User | null {
    const userStr = localStorage.getItem(this.USER_KEY);
    return userStr ? JSON.parse(userStr) : null;
  }

  private setCurrentUser(user: User): void {
    localStorage.setItem(this.USER_KEY, JSON.stringify(user));
  }

  private clearAuthData(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
    this.currentUserSubject.next(null);
    this.isAuthenticatedSubject.next(false);
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    return !!token && !this.isTokenExpired(token);
  }

  private isTokenExpired(token: string): boolean {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const currentTime = Date.now() / 1000;
      return payload.exp < currentTime;
    } catch {
      return true;
    }
  }

  getAuthHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  private handleError(error: any): Observable<never> {
    console.error('Auth service error:', error);
    return throwError(() => error);
  }
}
