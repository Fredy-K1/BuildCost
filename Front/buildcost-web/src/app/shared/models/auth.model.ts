export interface RegisterRequest {
  name:     string;
  apaterno: string;
  amaterno: string;
  telefono: string;
  email:    string;
  password: string;
  role:     string;
}

export interface LoginRequest {
  email:    string;
  password: string;
}

export interface AuthResponse {
  token: string;
  role:  string;
  id?: string;
  userId?: string;
  name?: string;
  email?: string;
  message?: string;
}

export interface RegisterResponse {
  message?: string;
}

export interface UsuarioSesion {
  userId: string | null;
  name: string;
  email: string;
  role: string;
}
