export interface User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  fullName: string;
  phoneNumber?: string;
  country?: string;
  city?: string;
  dateOfBirth: Date;
  gender: string;
  isEmailVerified: boolean;
  createdDate: Date;
  lastLoginDate?: Date;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
  phoneNumber?: string;
  country?: string;
  city?: string;
  dateOfBirth: Date;
  gender: string;
}

export interface AuthResponse {
  success: boolean;
  message: string;
  token?: string;
  refreshToken?: string;
  expiresAt?: Date;
  user?: User;
}

export interface RefreshTokenRequest {
  refreshToken: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}

export interface ForgotPasswordRequest {
  email: string;
}

export interface ResetPasswordRequest {
  token: string;
  newPassword: string;
  confirmNewPassword: string;
}
