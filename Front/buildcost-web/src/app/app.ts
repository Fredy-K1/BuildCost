import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterOutlet } from '@angular/router';
import { Sidebar } from './shared/components/sidebar/sidebar';
import { NavbarUserComponent } from './shared/components/nabvar-user/nabvar-user';
import { AuthService } from './core/services/auth';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, Sidebar, NavbarUserComponent],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected readonly title = signal('BuildCostFront');

  isSidebarCollapsed = signal(false);

  constructor(
    private readonly router: Router,
    private readonly authService: AuthService,
  ) {}

  toggleSidebar(): void {
    this.isSidebarCollapsed.update((valor) => !valor);
  }

  showSidebar(): boolean {
    return this.obtenerRol() === 'contratista' && !this.esRutaPublica();
  }

  showNavbarUser(): boolean {
    return this.obtenerRol() === 'cliente' && !this.esRutaPublica();
  }

  private obtenerRol(): string {
    return this.authService.getRole()?.trim().toLowerCase() ?? '';
  }

  private esRutaPublica(): boolean {
    const rutaActual = this.router.url.split('?')[0].split('#')[0];

    const rutasPublicas = ['/', '/login', '/registro', '/recuperar-contrasena'];

    return rutasPublicas.includes(rutaActual);
  }
}
