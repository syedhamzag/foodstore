import { clsx, type ClassValue } from "clsx"
import { twMerge } from "tailwind-merge"

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

export function toGMT7(date: string | Date | null | undefined): Date | null {
  if (!date) return null;
  const d = typeof date === "string" ? new Date(date) : date;
  if (isNaN(d.getTime())) return null;
  return new Date(d.getTime() + 7 * 60 * 60 * 1000);
}

export function formatDateTime(date: string | Date | null | undefined): string {
  const d = toGMT7(date);
  if (!d) return "—";
  return d.toLocaleString("en-SA", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
    hour: "2-digit",
    minute: "2-digit",
  });
}

export function formatDate(date: string | Date | null | undefined): string {
  const d = toGMT7(date);
  if (!d) return "—";
  return d.toLocaleString("en-SA", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
  });
}

export function formatCurrency(amount: number): string {
  return new Intl.NumberFormat("en-SA", { style: "currency", currency: "SAR" }).format(amount);
}

export function formatTime(date: string | Date | null | undefined): string {
  const d = toGMT7(date);
  if (!d) return "—";
  return d.toLocaleString("en-SA", {
    hour: "2-digit",
    minute: "2-digit",
  });
}
