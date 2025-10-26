import { ApplicationConfig, inject } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    // provide the interceptor class in DI
    AuthInterceptor,
    provideHttpClient(
      withInterceptors([
        (req, nextFn) => {
          const interceptor = inject(AuthInterceptor);
          // Wrap functional nextFn in an object to satisfy HttpHandler
          const next: any = { handle: (r: any) => nextFn(r) };
          return interceptor.intercept(req, next);
        }
      ])
    )
  ]
};
