import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { Auth } from '../../../core/services/auth';

@Component({
  selector: 'app-recuperar-contrasena',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './recuperar-contrasena.html',
  styleUrls: ['./recuperar-contrasena.css'],
})
export class RecuperarContrasenaComponent {
  datos = {
    email: '',
    nuevaContrasena: '',
  };

  loading = false;
  errorMsg = '';

  constructor(
    private authService: Auth,
    private router: Router,
  ) {}

  recuperar() {
    if (!this.datos.email || !this.datos.nuevaContrasena) {
      this.errorMsg = 'Por favor, llena ambos campos.';
      return;
    }

    this.loading = true;
    this.errorMsg = '';

    const payload = {
      email: this.datos.email,
      password: this.datos.nuevaContrasena,
    };

    this.authService.recover(payload).subscribe({
      next: (respuesta) => {
        this.loading = false;
        alert(respuesta.message || 'Contraseña actualizada con éxito.');
        this.router.navigate(['/login']);
      },
      error: (error) => {
        this.loading = false;
        this.errorMsg = error.error?.message || 'Ese correo no está registrado o hubo un error.';
      },
    });
  }
}
