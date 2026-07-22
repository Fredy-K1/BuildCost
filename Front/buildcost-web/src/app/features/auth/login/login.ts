import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth';
import { LoginRequest } from '../../../shared/models/auth.model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent {
  credenciales: LoginRequest = { email: '', password: '' };
  loading = false;
  errorMsg = '';

  constructor(private authService: AuthService, private router: Router) {}

  login(): void {
    const email = this.credenciales.email.trim();
    const password = this.credenciales.password;

    if (!email && !password) {
      this.errorMsg = 'Por favor ingresa tu correo y tu contraseña.';
      return;
    } else if (!email) {
      this.errorMsg = 'Por favor ingresa tu correo electrónico.';
      return;
    } else if (!password) {
      this.errorMsg = 'Por favor ingresa tu contraseña.';
      return;
    }

    this.loading = true;
    this.errorMsg = '';

    this.authService.login({ email, password }).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigateByUrl(this.authService.getRutaInicial());
      },
      error: (err) => {
        this.loading = false;
        this.errorMsg = err.error?.message ?? err.error?.title ?? 'Credenciales incorrectas.';
      }
    });
  }
}