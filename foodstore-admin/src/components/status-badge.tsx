"use client"

import { Badge } from "@/components/ui/badge"

const statusConfig: Record<string, { label: string; variant: "default" | "secondary" | "destructive" | "outline" | "ghost" | "link" }> = {
  active: { label: "Active", variant: "default" },
  inactive: { label: "Ngừng Active", variant: "secondary" },
  pending: { label: "Chờ xử lý", variant: "outline" },
  confirmed: { label: "Đã xác nhận", variant: "default" },
  preparing: { label: "Đang nấu", variant: "ghost" },
  ready: { label: "Sẵn sàng", variant: "default" },
  served: { label: "Đã phục vụ", variant: "secondary" },
  paid: { label: "Đã thanh toán", variant: "default" },
  cancelled: { label: "Đã Cancel", variant: "destructive" },
  true: { label: "Active", variant: "default" },
  false: { label: "Ngừng Active", variant: "secondary" },
}

interface StatusBadgeProps {
  status: string | boolean | null | undefined
  customLabels?: Record<string, string>
}

export function StatusBadge({ status, customLabels }: StatusBadgeProps) {
  const key = status === true || status === false ? String(status) : (status ?? "inactive")
  const config = statusConfig[key] ?? { label: key, variant: "outline" as const }

  return (
    <Badge variant={config.variant}>
      {customLabels?.[key] ?? config.label}
    </Badge>
  )
}
