import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth';
import { RegisterRequest } from '../../../shared/models/auth.model';

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './registro.html',
  styleUrls: ['./registro.css'],
})
export class RegistroComponent {
  usuario = {
    nombre: '',
    apaterno: '',
    amaterno: '',
    telefono: '',
    tipo_usuario: '',
    correo: '',
    password: '',
  };

  loading = false;
  errorMsg = '';

  constructor(private authService: AuthService, private router: Router) {}

  registrar(): void {
    const { nombre, apaterno, correo, password, tipo_usuario } = this.usuario;

    if (!nombre || !apaterno || !correo || !password || !tipo_usuario) {
      this.errorMsg = 'Por favor llene los campos obligatorios: Nombre, Apellido Paterno, Correo, Contraseña y Tipo.';
      return;
    }

    this.loading = true;
    this.errorMsg = '';

    const payload: RegisterRequest = {
      name: this.usuario.nombre.trim(),
      apaterno: this.usuario.apaterno.trim(),
      amaterno: this.usuario.amaterno.trim(),
      telefono: this.usuario.telefono.trim(),
      email: this.usuario.correo.trim(),
      password: this.usuario.password,
      role: this.usuario.tipo_usuario,
    };

    this.authService.register(payload).subscribe({
      next: (res) => {
        this.loading = false;
        alert(res.message ?? 'Usuario registrado con éxito.');
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.loading = false;
        this.errorMsg = err.error?.message ?? 'Error al registrar el usuario. Verifica los datos.';
      },
    });
  }
}
