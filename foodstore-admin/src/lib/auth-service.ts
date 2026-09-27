import { apiClient } from "./api-client";

export interface AuthUser {
  id: string;
  name: string;
  username: string;
  email: string;
  roleName: string;
  permissions: string[];
  phone?: string;
  avatarUrl?: string;
}

interface ApiResponse<T> {
  data: T;
}

interface LoginPayload {
  token: string;
  user: AuthUser;
  expiresIn: number;
  refreshToken?: string;
}

let authToken: string | null = null;

function getTokenFromCookie(): string | null {
  if (typeof document === "undefined") return null;
  const cookies = document.cookie.split("; ");
  const authCookie = cookies.find((c) => c.startsWith("auth_token="));
  return authCookie ? authCookie.split("=")[1] : null;
}

function setTokenCookie(token: string, expiresIn: number) {
  if (typeof document === "undefined") return;
  const expiryDate = new Date();
  expiryDate.setSeconds(expiryDate.getSeconds() + expiresIn);
  document.cookie = `auth_token=${token}; path=/; expires=${expiryDate.toUTCString()}; SameSite=Lax`;
}

function clearTokenCookie() {
  if (typeof document === "undefined") return;
  document.cookie = "auth_token=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC; SameSite=Lax";
}

export function getAuthToken(): string | null {
  authToken = getTokenFromCookie();
  return authToken;
}

export const authService = {
  async login(username: string, password: string): Promise<LoginPayload> {
    const res = await apiClient<ApiResponse<LoginPayload>>("/auth/login", {
      method: "POST",
      body: JSON.stringify({ username, password }),
      skipAuth: true,
    });
    authToken = res.data.token;
    setTokenCookie(res.data.token, res.data.expiresIn);
    return res.data;
  },

  async logout(): Promise<void> {
    try {
      await apiClient<ApiResponse<unknown>>("/auth/logout", {
        method: "POST",
        token: authToken ?? undefined,
      });
    } catch {
      // ignore
    }
    authToken = null;
    clearTokenCookie();
  },

  async getProfile(): Promise<AuthUser | null> {
    try {
      const token = getAuthToken();
      const res = await apiClient<ApiResponse<AuthUser>>("/auth/profile", {
        token: token ?? undefined,
      });
      return res.data;
    } catch {
      return null;
    }
  },

  async updateProfile(dto: {
    name?: string;
    email?: string;
    phone?: string;
    avatarUrl?: string;
  }): Promise<AuthUser> {
    const token = getAuthToken();
    const res = await apiClient<ApiResponse<AuthUser>>("/auth/profile", {
      method: "PUT",
      body: JSON.stringify(dto),
      token: token ?? undefined,
    });
    return res.data;
  },

  async uploadAvatar(file: File): Promise<string> {
    const formData = new FormData();
    formData.append("file", file);
    formData.append("folder", "avatars");
    const token = getAuthToken();
    const res = await apiClient<ApiResponse<{ fileUrl: string }>>("/media/upload", {
      method: "POST",
      body: formData,
      token: token ?? undefined,
    });
    return res.data.fileUrl;
  },
};
