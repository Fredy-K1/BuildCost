import { Component, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
<<<<<<< HEAD
import { AuthService } from '../../../core/services/auth';
import { NavbarUserComponent } from '../../../shared/components/nabvar-user/nabvar-user';
=======
import { Auth } from '../../../core/services/auth';
>>>>>>> 8679d5f (Actualiza frontend)

@Component({
  selector: 'app-proyectos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './proyectos.html',
  styleUrl: './proyectos.css',
})
export class Proyectos_user {}
