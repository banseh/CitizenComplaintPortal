import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authLockedGuard: CanActivateFn = (route, state) => {
const router = inject(Router);

  if (localStorage.getItem('token') !== null) {
    router.navigate(['/home']);
    return false;
  } else {
    return true;
  }
};
