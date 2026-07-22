import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth';

export const roleGuard = (rolesPermitidos: string[]): CanActivateFn => {
  return () => {
    const authService = inject(AuthService);
    const router = inject(Router);

    if (!authService.estaAutenticado()) {
      return router.createUrlTree(['/login']);
    }

    if (authService.tieneRol(rolesPermitidos)) {
      return true;
    }

    return router.createUrlTree([authService.getRutaInicial()]);
  };
};
