import { Routes } from '@angular/router';

import { LoginComponent } from './features/auth/login/login';
import { RegistroComponent } from './features/auth/registro/registro';
import { RecuperarContrasenaComponent } from './features/auth/recuperar-contrasena/recuperar-contrasena';

import { Home } from './features/contratista/home/home';
import { Materiales } from './features/contratista/materiales/materiales';
import { DatosM2 } from './features/contratista/datos-m2/datos-m2';
import { Proyectos } from './features/contratista/proyectos/proyectos-admin';

import { HomeUser } from './features/cliente/home-user/home-user';
import { Proyectos_user } from './features/cliente/proyectos/proyectos-user';
import { Cotizaciones } from './features/cliente/cotizaciones/cotizaciones';

import { Perfil } from './features/perfil/perfil';

import { authGuard } from './core/guards/auth-guard';
import { roleGuard } from './core/guards/role-guard';

export const routes: Routes = [
  { path: '',redirectTo: 'login',pathMatch: 'full'},
  { path: 'login',component: LoginComponent},
  { path: 'registro',component: RegistroComponent},
  { path: 'recuperar-contrasena',component: RecuperarContrasenaComponent},

  { path: 'contratista',component: Home,canActivate: [authGuard,roleGuard(['contratista'])]},
  { path: 'materiales',component: Materiales,canActivate: [authGuard,roleGuard(['contratista'])]},
  { path: 'datosM2',component: DatosM2,canActivate: [authGuard,roleGuard(['contratista'])]},
  { path: 'proyectos',component: Proyectos,canActivate: [authGuard,roleGuard(['contratista'])]},
  
  { path: 'usuario',component: HomeUser,canActivate: [authGuard,roleGuard(['cliente'])]},
  { path: 'proyectos_user',component: Proyectos_user,canActivate: [authGuard,roleGuard(['cliente'])]},
  { path: 'cotizaciones',component: Cotizaciones,canActivate: [authGuard,roleGuard(['cliente'])]},

  
  { path: 'perfil',component: Perfil,canActivate: [authGuard]},
  { path: 'inicio',redirectTo: 'contratista',pathMatch: 'full'},
  { path: 'home',redirectTo: 'usuario',pathMatch: 'full'},
  { path: '**',redirectTo: 'login'}
];