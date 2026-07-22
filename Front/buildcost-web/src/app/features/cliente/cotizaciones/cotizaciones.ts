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
  selector: 'app-cotizaciones',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './cotizaciones.html',
  styleUrl: './cotizaciones.css',
})

      export class Cotizaciones {
}