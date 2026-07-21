import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginRequest, RegisterRequest } from '../../shared/models/auth.model';

export interface AuthResponse {
  token: string;
  role: string;
  id?: string;
  userId?: string;
  name?: string;
  email?: string;
  municipio?: string;
}

export interface RegisterResponse {
  message?: string;
}

export interface UsuarioSesion {
  userId: string | null;
  name: string;
  email: string;
  role: string;
  municipio: string;
}

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private readonly customerServiceUrl = 'http://localhost:5003/api/auth';

  private readonly http = inject(HttpClient);

  login(credenciales: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.customerServiceUrl}/login`, credenciales)
      .pipe(tap((respuesta) => this.guardarSesion(respuesta)));
  }

  register(payload: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.customerServiceUrl}/register`, payload);
  }

  recover(payload: { email: string; password: string }): Observable<RegisterResponse> {
    return this.http.put<RegisterResponse>(`${this.customerServiceUrl}/recover`, payload);
  }

  guardarSesion(respuesta: AuthResponse): void {
    if (!respuesta?.token || !respuesta?.role) {
      return;
    }

    const usuario: UsuarioSesion = {
      userId: respuesta.userId ?? respuesta.id ?? null,
      name: respuesta.name ?? '',
      email: respuesta.email ?? '',
      role: respuesta.role,
      municipio: respuesta.municipio ?? '',
    };

    localStorage.setItem('jwt_token', respuesta.token);
    localStorage.setItem('user_role', respuesta.role);
    localStorage.setItem('usuarioLogueado', JSON.stringify(usuario));
  }

  getToken(): string | null {
    return localStorage.getItem('jwt_token');
  }

  getRole(): string | null {
    return localStorage.getItem('user_role');
  }

  getUsuario(): UsuarioSesion | null {
    const usuario = localStorage.getItem('usuarioLogueado');

    if (!usuario) {
      return null;
    }

    try {
      return JSON.parse(usuario) as UsuarioSesion;
    } catch {
      return null;
    }
  }

  estaAutenticado(): boolean {
    return Boolean(this.getToken());
  }

  tieneRol(rolesPermitidos: string[]): boolean {
    const role = this.getRole()?.trim().toLowerCase();

    if (!role) {
      return false;
    }

    return rolesPermitidos.some((rol) => rol.trim().toLowerCase() === role);
  }

  getRutaInicial(): string {
    const role = this.getRole()?.trim().toLowerCase();

    switch (role) {
      case 'cliente':
        return '/usuario';

      case 'contratista':
        return '/contratista';

      default:
        return '/login';
    }
  }

  logout(): void {
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('user_role');
    localStorage.removeItem('usuarioLogueado');
  }
}
