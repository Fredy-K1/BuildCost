import { Component, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
<<<<<<< HEAD
import { NavbarUserComponent } from '../../../shared/components/nabvar-user/nabvar-user';
import { AuthService } from '../../../core/services/auth';
=======
import { Auth } from '../../../core/services/auth';
>>>>>>> 8679d5f (Actualiza frontend)

@Component({
  selector: 'app-home-user',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './home-user.html',
  styleUrls: ['./home-user.css'],
})
export class HomeUser {
  modalNuevoAbierto = signal(false);
  modalDetalleAbierto = signal(false);

  abrirModalNuevo(): void {
    this.modalNuevoAbierto.set(true);
  }

  cerrarModalNuevo(): void {
    this.modalNuevoAbierto.set(false);
  }

  abrirModalDetalle(): void {
    this.modalDetalleAbierto.set(true);
  }

  cerrarModalDetalle(): void {
    this.modalDetalleAbierto.set(false);
  }
}
