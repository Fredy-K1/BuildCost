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

export interface LoginResponse {
  token: string;
  role:  string;
  message: string;
}