import { Component, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarUserComponent } from '../../../shared/components/nabvar-user/nabvar-user';
import { AuthService } from '../../../core/services/auth';

@Component({
  selector: 'app-cotizaciones',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarUserComponent],
  templateUrl: './cotizaciones.html',
  styleUrl: './cotizaciones.css',
})

      export class Cotizaciones {
}