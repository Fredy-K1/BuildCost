import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginRequest, RegisterRequest, AuthResponse, RegisterResponse, UsuarioSesion } from '../../shared/models/auth.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly apiUrl = 'http://localhost:5003/api/auth';
  private readonly http = inject(HttpClient);

  login(credenciales: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/login`, credenciales)
      .pipe(tap((respuesta) => this.guardarSesion(respuesta)));
  }

  register(payload: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.apiUrl}/register`, payload);
  }

  recover(payload: { email: string; password: string }): Observable<RegisterResponse> {
    return this.http.put<RegisterResponse>(`${this.apiUrl}/recover`, payload);
  }

  private guardarSesion(respuesta: AuthResponse): void {
    if (!respuesta?.token || !respuesta?.role) return;

    const usuario: UsuarioSesion = {
      userId: respuesta.userId ?? respuesta.id ?? null,
      name: respuesta.name ?? '',
      email: respuesta.email ?? '',
      role: respuesta.role,
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
    return usuario ? JSON.parse(usuario) as UsuarioSesion : null;
  }

  estaAutenticado(): boolean {
    return Boolean(this.getToken());
  }

  tieneRol(rolesPermitidos: string[]): boolean {
    const role = this.getRole()?.trim().toLowerCase();
    return role ? rolesPermitidos.map(r => r.trim().toLowerCase()).includes(role) : false;
  }

  getRutaInicial(): string {
    const role = this.getRole()?.trim().toLowerCase() ?? ''; 
    const rutas: Record<string, string> = {
      cliente: '/usuario',
      contratista: '/contratista',
    };
    return rutas[role] ?? '/login';
  }

  logout(): void {
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('user_role');
    localStorage.removeItem('usuarioLogueado');
  }
}
