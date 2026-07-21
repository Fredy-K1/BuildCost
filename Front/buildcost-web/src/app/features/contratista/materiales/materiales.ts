import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

interface Material {
  id: number;
  name: string;
  unit: string;
  price: number;
  createdAt?: string;
  contratistaId?: string;
}

interface MaterialRequest {
  name: string;
  unit: string;
  price: number;
}

@Component({
  selector: 'app-materiales',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './materiales.html',
  styleUrl: './materiales.css',
})
export class Materiales implements OnInit {
  private readonly http = inject(HttpClient);

  private readonly materialsUrl = 'http://localhost:5001/api/Materials';

  materiales = signal<Material[]>([]);
  loading = signal(false);
  saving = signal(false);
  deletingId = signal<number | null>(null);
  errorMsg = signal('');
  successMsg = signal('');
  editandoId = signal<number | null>(null);
  modalAbierto = signal(false);
  busqueda = signal('');

  formulario: MaterialRequest = {
    name: '',
    unit: '',
    price: 0,
  };

  totalMateriales = computed(() => this.materiales().length);

  materialesFiltrados = computed(() => {
    const termino = this.busqueda().trim().toLowerCase();

    if (!termino) {
      return this.materiales();
    }

    return this.materiales().filter((material) => {
      const nombre = material.name?.toLowerCase() ?? '';

      const unidad = material.unit?.toLowerCase() ?? '';

      return nombre.includes(termino) || unidad.includes(termino);
    });
  });

  ngOnInit(): void {
    this.cargarMateriales();
  }

  cargarMateriales(): void {
    this.loading.set(true);
    this.errorMsg.set('');

    this.http.get<Material[]>(this.materialsUrl).subscribe({
      next: (materiales) => {
        this.materiales.set(Array.isArray(materiales) ? materiales : []);

        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.loading.set(false);

        this.mostrarError(error, 'No fue posible cargar los materiales.');
      },
    });
  }

  abrirModalNuevo(): void {
    this.limpiarFormulario();
    this.modalAbierto.set(true);
  }

  abrirModalEditar(material: Material): void {
    this.editandoId.set(material.id);

    this.formulario = {
      name: material.name,
      unit: material.unit,
      price: Number(material.price),
    };

    this.errorMsg.set('');
    this.successMsg.set('');
    this.modalAbierto.set(true);
  }

  cerrarModal(): void {
    if (this.saving()) {
      return;
    }

    this.modalAbierto.set(false);
    this.limpiarFormulario();
  }

  guardarMaterial(): void {
    const payload: MaterialRequest = {
      name: this.formulario.name.trim(),
      unit: this.formulario.unit.trim(),
      price: Number(this.formulario.price),
    };

    if (!payload.name) {
      this.errorMsg.set('Ingresa el nombre del material.');
      return;
    }

    if (!payload.unit) {
      this.errorMsg.set('Ingresa la unidad o especificación.');
      return;
    }

    if (!Number.isFinite(payload.price) || payload.price <= 0) {
      this.errorMsg.set('Ingresa un precio mayor a cero.');
      return;
    }

    this.saving.set(true);
    this.errorMsg.set('');
    this.successMsg.set('');

    const id = this.editandoId();

    if (id === null) {
      this.crearMaterial(payload);
      return;
    }

    this.actualizarMaterial(id, payload);
  }

  eliminarMaterial(material: Material): void {
    const confirmar = window.confirm(`¿Deseas eliminar el material "${material.name}"?`);

    if (!confirmar) {
      return;
    }

    this.deletingId.set(material.id);
    this.errorMsg.set('');
    this.successMsg.set('');

    this.http.delete(`${this.materialsUrl}/${material.id}`).subscribe({
      next: () => {
        this.materiales.update((lista) => lista.filter((item) => item.id !== material.id));

        this.deletingId.set(null);

        if (this.editandoId() === material.id) {
          this.modalAbierto.set(false);
          this.limpiarFormulario();
        }

        this.successMsg.set('Material eliminado correctamente.');
      },
      error: (error: HttpErrorResponse) => {
        this.deletingId.set(null);

        this.mostrarError(error, 'No fue posible eliminar el material.');
      },
    });
  }

  private crearMaterial(payload: MaterialRequest): void {
    this.http.post<Material>(this.materialsUrl, payload).subscribe({
      next: (material) => {
        this.materiales.update((lista) => [material, ...lista]);

        this.saving.set(false);
        this.modalAbierto.set(false);
        this.limpiarFormulario(false);

        this.successMsg.set('Material registrado correctamente.');
      },
      error: (error: HttpErrorResponse) => {
        this.saving.set(false);

        this.mostrarError(error, 'No fue posible registrar el material.');
      },
    });
  }

  private actualizarMaterial(id: number, payload: MaterialRequest): void {
    this.http.put<Material>(`${this.materialsUrl}/${id}`, payload).subscribe({
      next: (materialActualizado) => {
        this.materiales.update((lista) =>
          lista.map((material) => (material.id === id ? materialActualizado : material)),
        );

        this.saving.set(false);
        this.modalAbierto.set(false);
        this.limpiarFormulario(false);

        this.successMsg.set('Material actualizado correctamente.');
      },
      error: (error: HttpErrorResponse) => {
        this.saving.set(false);

        this.mostrarError(error, 'No fue posible actualizar el material.');
      },
    });
  }

  private limpiarFormulario(limpiarMensajes = true): void {
    this.formulario = {
      name: '',
      unit: '',
      price: 0,
    };

    this.editandoId.set(null);

    if (limpiarMensajes) {
      this.errorMsg.set('');
      this.successMsg.set('');
    }
  }

  private mostrarError(error: HttpErrorResponse, mensajePredeterminado: string): void {
    if (error.status === 0) {
      this.errorMsg.set('No fue posible conectar con ProductService.');
      return;
    }

    if (error.status === 401) {
      this.errorMsg.set('La sesión no es válida o ha expirado.');
      return;
    }

    if (error.status === 403) {
      this.errorMsg.set('No tienes permisos para realizar esta operación.');
      return;
    }

    if (error.status === 404) {
      this.errorMsg.set(
        error.error?.message ?? error.error?.title ?? 'El material solicitado no existe.',
      );
      return;
    }

    this.errorMsg.set(error.error?.message ?? error.error?.title ?? mensajePredeterminado);
  }
}
