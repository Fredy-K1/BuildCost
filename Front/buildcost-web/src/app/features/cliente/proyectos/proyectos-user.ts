import { Component, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../../core/services/auth';
import { NavbarUserComponent } from '../../../shared/components/nabvar-user/nabvar-user';

@Component({
  selector: 'app-proyectos',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarUserComponent],
  templateUrl: './proyectos.html',
  styleUrl: './proyectos.css',
})
export class Proyectos_user {}
