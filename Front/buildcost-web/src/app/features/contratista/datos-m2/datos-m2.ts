import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

interface DatoM2 {
  id: number;
  dataType: string;
  value: number;
  description: string;
  contratistaId?: string;
}

interface DatoM2Request {
  dataType: string;
  value: number;
  description: string;
}

@Component({
  selector: 'app-datos-m2',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './datos-m2.html',
  styleUrl: './datos-m2.css',
})
export class DatosM2 implements OnInit {
  private readonly http = inject(HttpClient);

  private readonly datosUrl = 'http://localhost:5001/api/Datos';

  datos = signal<DatoM2[]>([]);
  loading = signal(false);
  saving = signal(false);
  errorMsg = signal('');
  successMsg = signal('');
  modalAbierto = signal(false);
  editandoId = signal<number | null>(null);
  busqueda = signal('');

  formulario: DatoM2Request = {
    dataType: '',
    value: 0,
    description: '',
  };

  totalDatos = computed(() => this.datos().length);

  valorPromedio = computed(() => {
    const lista = this.datos();

    if (lista.length === 0) {
      return 0;
    }

    const total = lista.reduce((acumulado, dato) => acumulado + Number(dato.value), 0);

    return total / lista.length;
  });

  datosFiltrados = computed(() => {
    const termino = this.busqueda().trim().toLowerCase();

    if (!termino) {
      return this.datos();
    }

    return this.datos().filter(
      (dato) =>
        dato.dataType.toLowerCase().includes(termino) ||
        dato.description.toLowerCase().includes(termino),
    );
  });

  ngOnInit(): void {
    this.cargarDatos();
  }

  cargarDatos(): void {
    this.loading.set(true);
    this.errorMsg.set('');

    this.http.get<DatoM2[]>(this.datosUrl).subscribe({
      next: (datos) => {
        this.datos.set(datos);
        this.loading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.loading.set(false);

        this.mostrarError(error, 'No fue posible cargar los datos por m².');
      },
    });
  }

  abrirModalNuevo(): void {
    this.limpiarFormulario();
    this.modalAbierto.set(true);
  }

  abrirModalEditar(dato: DatoM2): void {
    this.editandoId.set(dato.id);

    this.formulario = {
      dataType: dato.dataType,
      value: dato.value,
      description: dato.description,
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

  guardarDato(): void {
    const payload: DatoM2Request = {
      dataType: this.formulario.dataType.trim(),
      value: Number(this.formulario.value),
      description: this.formulario.description.trim(),
    };

    if (!payload.dataType || payload.value <= 0) {
      this.errorMsg.set('Ingresa el tipo de dato y un valor mayor a cero.');
      return;
    }

    this.saving.set(true);
    this.errorMsg.set('');
    this.successMsg.set('');

    const id = this.editandoId();

    if (id === null) {
      this.crearDato(payload);
      return;
    }

    this.actualizarDato(id, payload);
  }

  eliminarDato(dato: DatoM2): void {
    const confirmar = window.confirm(`¿Deseas eliminar el dato "${dato.dataType}"?`);

    if (!confirmar) {
      return;
    }

    this.errorMsg.set('');
    this.successMsg.set('');

    this.http.delete(`${this.datosUrl}/${dato.id}`).subscribe({
      next: () => {
        this.datos.update((lista) => lista.filter((item) => item.id !== dato.id));

        if (this.editandoId() === dato.id) {
          this.modalAbierto.set(false);
          this.limpiarFormulario();
        }

        this.successMsg.set('Dato por m² eliminado correctamente.');
      },
      error: (error: HttpErrorResponse) => {
        this.mostrarError(error, 'No fue posible eliminar el dato por m².');
      },
    });
  }

  private crearDato(payload: DatoM2Request): void {
    this.http.post<DatoM2>(this.datosUrl, payload).subscribe({
      next: (dato) => {
        this.datos.update((lista) => [dato, ...lista]);

        this.saving.set(false);
        this.modalAbierto.set(false);
        this.limpiarFormulario(false);

        this.successMsg.set('Dato por m² registrado correctamente.');
      },
      error: (error: HttpErrorResponse) => {
        this.saving.set(false);

        this.mostrarError(error, 'No fue posible registrar el dato por m².');
      },
    });
  }

  private actualizarDato(id: number, payload: DatoM2Request): void {
    this.http.put<DatoM2>(`${this.datosUrl}/${id}`, payload).subscribe({
      next: (datoActualizado) => {
        this.datos.update((lista) =>
          lista.map((dato) => (dato.id === id ? datoActualizado : dato)),
        );

        this.saving.set(false);
        this.modalAbierto.set(false);
        this.limpiarFormulario(false);

        this.successMsg.set('Dato por m² actualizado correctamente.');
      },
      error: (error: HttpErrorResponse) => {
        this.saving.set(false);

        this.mostrarError(error, 'No fue posible actualizar el dato por m².');
      },
    });
  }

  private limpiarFormulario(limpiarMensajes = true): void {
    this.formulario = {
      dataType: '',
      value: 0,
      description: '',
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
      this.errorMsg.set(error.error?.message ?? 'El dato solicitado no existe.');
      return;
    }

    this.errorMsg.set(error.error?.message ?? mensajePredeterminado);
  }
}
