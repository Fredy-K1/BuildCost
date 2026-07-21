import { Component, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarUserComponent } from '../../../shared/components/nabvar-user/nabvar-user';
import { Auth } from '../../../core/services/auth';

@Component({
  selector: 'app-home-user',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarUserComponent],
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
