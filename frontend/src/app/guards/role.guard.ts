import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const roleGuard: CanActivateFn = (route, state) => {
  const roles = JSON.parse(localStorage.getItem('roles') || '[]'); 
  const requiredRoles = route.data['roles'] as string[];
  const hasRequiredRole = requiredRoles.some(role => roles.includes(role));
  const router = inject(Router);

  if (!hasRequiredRole) {
    console.warn('Access denied. Redirecting to /home.');
    router.navigate(['/home']);
  }

  return hasRequiredRole;
};
