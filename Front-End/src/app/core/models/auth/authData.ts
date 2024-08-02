export interface LoginFormData {
  username: string;
  password: string;
}
export interface RestartpasswordFormData {
  token: string;
  email: string;
  newPassword: string;
  confirmPassword: string;
}

export interface RegisterFormData {
  username: string;
  email: string;
  password: string;
}
