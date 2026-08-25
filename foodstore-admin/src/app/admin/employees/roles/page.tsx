"use client"

import * as React from "react"
import { Edit, Plus, Trash2 } from "lucide-react"
import type { ColumnDef } from "@tanstack/react-table"
import { toast } from "sonner"

import { Breadcrumb, BreadcrumbItem, BreadcrumbList, BreadcrumbPage } from "@/components/ui/breadcrumb"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Separator } from "@/components/ui/separator"
import { SidebarTrigger } from "@/components/ui/sidebar"
import { Switch } from "@/components/ui/switch"
import { DataTable } from "@/components/data-table"
import { CrudSheet } from "@/components/crud-sheet"
import { DeleteConfirmDialog } from "@/components/confirm-dialog"
import { StatusBadge } from "@/components/status-badge"
import { roleService } from "@/lib/services/role-service"
import type { Role, RoleCreateDto, RoleUpdateDto } from "@/lib/types"

export default function RolesPage() {
  const [data, setData] = React.useState<Role[]>([])
  const [loading, setLoading] = React.useState(true)
  const [sheetOpen, setSheetOpen] = React.useState(false)
  const [editing, setEditing] = React.useState<Role | null>(null)
  const [submitting, setSubmitting] = React.useState(false)
  const [deleteOpen, setDeleteOpen] = React.useState(false)
  const [deleting, setDeleting] = React.useState(false)
  const [deleteTarget, setDeleteTarget] = React.useState<Role | null>(null)

  const [formName, setFormName] = React.useState("")
  const [formDescription, setFormDescription] = React.useState("")
  const [formDefaultRoute, setFormDefaultRoute] = React.useState("")
  const [formIsActive, setFormIsActive] = React.useState(true)

  const loadData = React.useCallback(async () => {
    setLoading(true)
    try { const res = await roleService.getAll(); setData(res) }
    catch { toast.error("Không thể tải vai trò") }
    finally { setLoading(false) }
  }, [])

  React.useEffect(() => { loadData() }, [loadData])

  const resetForm = () => { setFormName(""); setFormDescription(""); setFormDefaultRoute(""); setFormIsActive(true); setEditing(null) }
  const openCreate = () => { resetForm(); setSheetOpen(true) }
  const openEdit = (item: Role) => { setEditing(item); setFormName(item.name); setFormDescription(item.description ?? ""); setFormDefaultRoute(item.defaultRoute ?? ""); setFormIsActive(item.isActive); setSheetOpen(true) }

  const handleSubmit = async () => {
    if (!formName.trim()) { toast.error("Vui lòng nhập tên vai trò"); return }
    setSubmitting(true)
    try {
      const dto = { name: formName.trim(), description: formDescription || undefined, defaultRoute: formDefaultRoute || undefined, isActive: formIsActive }
      if (editing) { await roleService.update(editing.id, dto as RoleUpdateDto); toast.success("Cập nhật vai trò thành công") }
      else { await roleService.create(dto as RoleCreateDto); toast.success("Thêm vai trò thành công") }
      setSheetOpen(false); loadData()
    } catch (e) { toast.error((e as Error).message) }
    finally { setSubmitting(false) }
  }

  const confirmDelete = (item: Role) => { setDeleteTarget(item); setDeleteOpen(true) }
  const handleDelete = async () => {
    if (!deleteTarget) return
    setDeleting(true)
    try { await roleService.delete(deleteTarget.id); toast.success("Xóa vai trò thành công"); setDeleteOpen(false); loadData() }
    catch { toast.error("Không thể xóa vai trò") }
    finally { setDeleting(false) }
  }

  const protectedRoles = ["root", "customer"]

  const columns: ColumnDef<Role>[] = [
    { id: "name", accessorKey: "name", header: "Tên vai trò" },
    { id: "description", accessorKey: "description", header: "Mô tả", cell: ({ row }) => row.original.description || "—" },
    { id: "defaultRoute", accessorKey: "defaultRoute", header: "Route", cell: ({ row }) => row.original.defaultRoute || "—" },
    { id: "isActive", header: "Trạng thái", cell: ({ row }) => <StatusBadge status={row.original.isActive} /> },
    { id: "actions", header: "", cell: ({ row }) => {
      const isProtected = protectedRoles.includes(row.original.name.toLowerCase())
      return (
        <div className="flex justify-end gap-1">
          <Button variant="ghost" size="icon-sm" disabled={isProtected} onClick={() => openEdit(row.original)}><Edit className="size-4" /></Button>
          <Button variant="ghost" size="icon-sm" disabled={isProtected} onClick={() => confirmDelete(row.original)}><Trash2 className="size-4 text-destructive" /></Button>
        </div>
      )
    }},
  ]

  return (
    <>
      <header className="flex h-16 shrink-0 items-center gap-2 transition-[width,height] ease-linear group-has-data-[collapsible=icon]/sidebar-wrapper:h-12">
        <div className="flex items-center gap-2 px-4">
          <SidebarTrigger className="-ml-1" />
          <Separator orientation="vertical" className="mr-2 h-4" />
          <Breadcrumb><BreadcrumbList><BreadcrumbItem><BreadcrumbPage>Vai trò</BreadcrumbPage></BreadcrumbItem></BreadcrumbList></Breadcrumb>
        </div>
      </header>
      <div className="flex flex-1 flex-col gap-4 p-4 pt-0">
        <DataTable columns={columns} data={data} searchKey="name" searchPlaceholder="Tìm vai trò..." loading={loading}
          toolbarActions={<Button className="gap-1.5" onClick={openCreate}><Plus className="size-4" />Thêm vai trò</Button>} />
      </div>
      <CrudSheet open={sheetOpen} onOpenChange={setSheetOpen} title={editing ? "Sửa vai trò" : "Thêm vai trò"} onSubmit={handleSubmit} submitting={submitting}>
        <div className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="name">Tên vai trò</Label>
            <Input id="name" value={formName} onChange={(e) => setFormName(e.target.value)} placeholder="VD: Quản lý, Nhân viên..." />
          </div>
          <div className="space-y-2">
            <Label htmlFor="description">Mô tả</Label>
            <Input id="description" value={formDescription} onChange={(e) => setFormDescription(e.target.value)} />
          </div>
          <div className="space-y-2">
            <Label htmlFor="defaultRoute">Route mặc định</Label>
            <Input id="defaultRoute" value={formDefaultRoute} onChange={(e) => setFormDefaultRoute(e.target.value)} placeholder="VD: /admin/food/orders" />
          </div>
          <div className="flex items-center gap-2">
            <Switch id="isActive" checked={formIsActive} onCheckedChange={setFormIsActive} />
            <Label htmlFor="isActive">Hoạt động</Label>
          </div>
        </div>
      </CrudSheet>
      <DeleteConfirmDialog open={deleteOpen} onOpenChange={setDeleteOpen} onConfirm={handleDelete} loading={deleting} itemName={deleteTarget?.name} />
    </>
  )
}
