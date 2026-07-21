import { Component, OnInit, PLATFORM_ID, computed, inject, signal } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { Auth } from '../../core/services/auth';

interface PerfilUsuario {
  id: string;
  name: string;
  apaterno: string;
  amaterno: string;
  telefono: string;
  email: string;
  role: string;
  createdAt?: string;
}

interface ActualizarPerfilRequest {
  name: string;
  apaterno: string;
  amaterno: string;
  telefono: string;
}

interface UsuarioLocal {
  id?: string;
  Id?: string;
  name?: string;
  Name?: string;
  email?: string;
  role?: string;
  token?: string;
  [key: string]: unknown;
}

interface JwtPayload {
  nameid?: string;
  sub?: string;
  [key: string]: unknown;
}

@Component({
  selector: 'app-perfil',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './perfil.html',
  styleUrl: './perfil.css',
})
export class Perfil implements OnInit {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly auth = inject(Auth);
  private readonly platformId = inject(PLATFORM_ID);

  private readonly authUrl = 'http://localhost:5003/api/auth';

  perfil = signal<PerfilUsuario | null>(null);
  loading = signal(false);
  saving = signal(false);
  editando = signal(false);
  errorMsg = signal('');
  successMsg = signal('');

  formulario: ActualizarPerfilRequest = {
    name: '',
    apaterno: '',
    amaterno: '',
    telefono: '',
  };

  nombreCompleto = computed(() => {
    const usuario = this.perfil();

    if (!usuario) {
      return '';
    }

    return [usuario.name, usuario.apaterno, usuario.amaterno]
      .filter((valor) => valor?.trim())
      .join(' ');
  });

  iniciales = computed(() => {
    const usuario = this.perfil();

    if (!usuario) {
      return '';
    }

    const primeraInicial = usuario.name?.trim().charAt(0) ?? '';

    const segundaInicial = usuario.apaterno?.trim().charAt(0) ?? '';

    return (primeraInicial + segundaInicial).toUpperCase();
  });

  ngOnInit(): void {
    this.cargarPerfil();
  }

  cargarPerfil(): void {
    const usuarioId = this.obtenerUsuarioId();

    if (!usuarioId) {
      this.errorMsg.set('No fue posible identificar al usuario de la sesión.');
      return;
    }

    this.loading.set(true);
    this.editando.set(false);
    this.errorMsg.set('');
    this.successMsg.set('');

    this.http.get<PerfilUsuario>(`${this.authUrl}/perfil/${usuarioId}`).subscribe({
      next: (perfil) => {
        this.perfil.set(perfil);
        this.copiarPerfilAlFormulario(perfil);
        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.loading.set(false);

        this.mostrarError(error, 'No fue posible cargar el perfil.');
      },
    });
  }

  activarEdicion(): void {
    const usuario = this.perfil();

    if (!usuario) {
      return;
    }

    this.copiarPerfilAlFormulario(usuario);
    this.errorMsg.set('');
    this.successMsg.set('');
    this.editando.set(true);
  }

  cancelarEdicion(): void {
    const usuario = this.perfil();

    if (usuario) {
      this.copiarPerfilAlFormulario(usuario);
    }

    this.editando.set(false);
    this.errorMsg.set('');
    this.successMsg.set('');
  }

  guardarPerfil(): void {
    const payload: ActualizarPerfilRequest = {
      name: this.formulario.name.trim(),
      apaterno: this.formulario.apaterno.trim(),
      amaterno: this.formulario.amaterno.trim(),
      telefono: this.formulario.telefono.trim(),
    };

    if (!payload.name) {
      this.errorMsg.set('Ingresa tu nombre.');
      return;
    }

    if (!payload.apaterno) {
      this.errorMsg.set('Ingresa tu apellido paterno.');
      return;
    }

    if (!payload.telefono) {
      this.errorMsg.set('Ingresa tu número de teléfono.');
      return;
    }

    this.saving.set(true);
    this.errorMsg.set('');
    this.successMsg.set('');

    this.http.put<{ message?: string }>(`${this.authUrl}/actualizar`, payload).subscribe({
      next: (respuesta) => {
        this.perfil.update((usuario) => {
          if (!usuario) {
            return usuario;
          }

          return {
            ...usuario,
            ...payload,
          };
        });

        this.actualizarUsuarioLocal(payload);

        this.saving.set(false);
        this.editando.set(false);

        this.successMsg.set(respuesta.message ?? 'Perfil actualizado correctamente.');
      },
      error: (error: HttpErrorResponse) => {
        this.saving.set(false);

        this.mostrarError(error, 'No fue posible actualizar el perfil.');
      },
    });
  }

  cerrarSesion(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }

  private copiarPerfilAlFormulario(perfil: PerfilUsuario): void {
    this.formulario = {
      name: perfil.name ?? '',
      apaterno: perfil.apaterno ?? '',
      amaterno: perfil.amaterno ?? '',
      telefono: perfil.telefono ?? '',
    };
  }

  private obtenerUsuarioId(): string | null {
    if (!isPlatformBrowser(this.platformId)) {
      return null;
    }

    const usuarioGuardado = localStorage.getItem('usuarioLogueado');

    if (usuarioGuardado) {
      try {
        const usuario = JSON.parse(usuarioGuardado) as UsuarioLocal;

        const id = usuario.id ?? usuario.Id;

        if (typeof id === 'string' && id.trim()) {
          return id;
        }
      } catch {
        localStorage.removeItem('usuarioLogueado');
      }
    }

    return this.obtenerIdDesdeToken();
  }

  private obtenerIdDesdeToken(): string | null {
    const token = this.auth.getToken();

    if (!token) {
      return null;
    }

    try {
      const partes = token.split('.');

      if (partes.length !== 3) {
        return null;
      }

      const base64 = partes[1].replace(/-/g, '+').replace(/_/g, '/');

      const contenido = base64.padEnd(Math.ceil(base64.length / 4) * 4, '=');

      const payload = JSON.parse(atob(contenido)) as JwtPayload;

      const claimIdentificador =
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';

      const identificador = payload.nameid ?? payload[claimIdentificador] ?? payload.sub;

      return typeof identificador === 'string' ? identificador : null;
    } catch {
      return null;
    }
  }

  private actualizarUsuarioLocal(payload: ActualizarPerfilRequest): void {
    if (!isPlatformBrowser(this.platformId)) {
      return;
    }

    const usuarioGuardado = localStorage.getItem('usuarioLogueado');

    if (!usuarioGuardado) {
      return;
    }

    try {
      const usuario = JSON.parse(usuarioGuardado) as UsuarioLocal;

      usuario.name = payload.name;

      localStorage.setItem('usuarioLogueado', JSON.stringify(usuario));
    } catch {
      localStorage.removeItem('usuarioLogueado');
    }
  }

  private mostrarError(error: HttpErrorResponse, mensajePredeterminado: string): void {
    if (error.status === 0) {
      this.errorMsg.set('No fue posible conectar con CustomerService.');
      return;
    }

    if (error.status === 400) {
      this.errorMsg.set(
        error.error?.message ?? error.error?.title ?? 'Los datos enviados no son válidos.',
      );
      return;
    }

    if (error.status === 401) {
      this.errorMsg.set('La sesión no es válida o ha expirado.');
      return;
    }

    if (error.status === 403) {
      this.errorMsg.set('No tienes permisos para realizar esta operación.');
      return;
    }

    if (error.status === 404) {
      this.errorMsg.set('No se encontró el perfil del usuario.');
      return;
    }

    this.errorMsg.set(error.error?.message ?? error.error?.title ?? mensajePredeterminado);
  }
}
