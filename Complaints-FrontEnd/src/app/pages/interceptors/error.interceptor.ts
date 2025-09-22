import { HttpInterceptorFn } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { inject } from '@angular/core';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      console.log(' Interceptor caught error with status:', error.status);

      if (error.status === 0 || error.status >= 500) {
        alert('⚠️ Server is down. Redirecting...');
        router.navigate(['/serverdown'], { replaceUrl: true });
      }

      return throwError(() => error);
    })
  );
};
