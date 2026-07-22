export interface PerfilUsuario {
  id: string;
  name: string;
  apaterno: string;
  amaterno: string;
  telefono: string;
  email: string;
  role: string;
  createdAt?: string;
}

export interface ActualizarPerfilRequest {
  name: string;
  apaterno: string;
  amaterno: string;
  telefono: string;
}

export interface UsuarioLocal {
  id: string;
  name: string;
  email: string;
  role: string;
  token?: string;
}

export interface JwtPayload {
  nameid?: string;
  sub?: string;
  [key: string]: unknown;
}
