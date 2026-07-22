import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../../core/services/auth';

export interface ProyectoAceptado {
  id: number;
  folio: string;
  nombre: string;
  ubicacion: string;
  cliente: string;
  whatsapp?: string;
  total: number;
  fechaAceptado: string | Date;
  estado: string;
  pdfUrl?: string;
}

@Component({
  selector: 'app-proyectos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './proyectos.html',
  styleUrl: './proyectos.css',
})
export class Proyectos implements OnInit {
  private readonly http = inject(HttpClient);
  private readonly auth = inject(AuthService);

  private readonly apiUrl = 'http://localhost:5001/api/Proyectos/aceptados';

  proyectos = signal<ProyectoAceptado[]>([]);
  loading = signal<boolean>(false);
  busqueda = signal<string>('');
  errorMsg = signal<string>('');

  proyectosFiltrados = computed(() => {
    const termino = this.busqueda().trim().toLowerCase();

    if (!termino) {
      return this.proyectos();
    }

    return this.proyectos().filter(
      (p) =>
        p.folio.toLowerCase().includes(termino) ||
        p.nombre.toLowerCase().includes(termino) ||
        p.cliente.toLowerCase().includes(termino) ||
        p.ubicacion.toLowerCase().includes(termino)
    );
  });

  ngOnInit(): void {
    this.cargarProyectos();
  }

  cargarProyectos(): void {
    this.loading.set(true);
    this.errorMsg.set('');

    this.http.get<ProyectoAceptado[]>(this.apiUrl).subscribe({
      next: (data) => {
        this.proyectos.set(data);
        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.loading.set(false);
        
        if (error.status === 0 || error.status === 404) {
          this.proyectos.set([
            {
              id: 1,
              folio: 'PRO-001',
              nombre: 'Construcción de vivienda',
              ubicacion: 'Túxpam de Rodríguez Cano, Veracruz',
              cliente: 'Juan P.',
              whatsapp: '521234567890',
              total: 125000,
              fechaAceptado: '2026-07-19',
              estado: 'Aceptado',
            },
          ]);
        } else {
          this.errorMsg.set('No fue posible obtener la lista de proyectos aceptados.');
        }
      },
    });
  }

  descargarPdf(proyecto: ProyectoAceptado): void {
    if (proyecto.pdfUrl) {
      window.open(proyecto.pdfUrl, '_blank');
      return;
    }

    const downloadUrl = `${this.apiUrl}/${proyecto.id}/pdf`;
    window.open(downloadUrl, '_blank');
  }
}