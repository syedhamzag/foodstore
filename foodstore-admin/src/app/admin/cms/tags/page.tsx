"use client"

import * as React from "react"
import { Edit, Plus, Trash2 } from "lucide-react"
import { toast } from "sonner"
import type { ColumnDef } from "@tanstack/react-table"

import { Breadcrumb, BreadcrumbItem, BreadcrumbList, BreadcrumbPage } from "@/components/ui/breadcrumb"
import { Button } from "@/components/ui/button"
import { Label } from "@/components/ui/label"
import { Input } from "@/components/ui/input"
import { Separator } from "@/components/ui/separator"
import { SidebarTrigger } from "@/components/ui/sidebar"
import { DataTable } from "@/components/data-table"
import { CrudSheet } from "@/components/crud-sheet"
import { DeleteConfirmDialog } from "@/components/confirm-dialog"
import { blogTagService } from "@/lib/services/blog-tag-service"
import { formatDateTime } from "@/lib/utils"
import type { TagDto, CreateTagDto, UpdateTagDto } from "@/lib/types"

export default function TagsPage() {
  const [data, setData] = React.useState<TagDto[]>([])
  const [loading, setLoading] = React.useState(true)
  const [sheetOpen, setSheetOpen] = React.useState(false)
  const [editing, setEditing] = React.useState<TagDto | null>(null)
  const [submitting, setSubmitting] = React.useState(false)
  const [deleteOpen, setDeleteOpen] = React.useState(false)
  const [deleting, setDeleting] = React.useState(false)
  const [deleteTarget, setDeleteTarget] = React.useState<TagDto | null>(null)

  const [formName, setFormName] = React.useState("")
  const [formColor, setFormColor] = React.useState("#f59e0b")

  const loadData = React.useCallback(async () => {
    setLoading(true)
    try { const res = await blogTagService.getAll(); setData(res) }
    catch { toast.error("Không thể tải thẻ") }
    finally { setLoading(false) }
  }, [])

  React.useEffect(() => { loadData() }, [loadData])

  const resetForm = () => { setFormName(""); setFormColor("#f59e0b"); setEditing(null) }
  const openCreate = () => { resetForm(); setSheetOpen(true) }
  const openEdit = (item: TagDto) => { setEditing(item); setFormName(item.name); setFormColor(item.color); setSheetOpen(true) }

  const handleSubmit = async () => {
    if (!formName.trim()) { toast.error("Vui lòng nhập tên thẻ"); return }
    setSubmitting(true)
    try {
      if (editing) { await blogTagService.update(editing.id, { name: formName.trim(), color: formColor }); toast.success("Cập nhật thẻ thành công") }
      else { await blogTagService.create({ name: formName.trim(), color: formColor }); toast.success("Thêm thẻ thành công") }
      setSheetOpen(false); loadData()
    } catch (e) { toast.error((e as Error).message) }
    finally { setSubmitting(false) }
  }

  const confirmDelete = (item: TagDto) => { setDeleteTarget(item); setDeleteOpen(true) }
  const handleDelete = async () => {
    if (!deleteTarget) return; setDeleting(true)
    try { await blogTagService.delete(deleteTarget.id); toast.success("Xóa thẻ thành công"); setDeleteOpen(false); loadData() }
    catch { toast.error("Không thể xóa thẻ") }
    finally { setDeleting(false) }
  }

  const columns: ColumnDef<TagDto>[] = [
    {
      accessorKey: "name", header: "Tên thẻ",
      cell: ({ row }) => <div className="flex items-center gap-2"><div className="size-3 rounded-full" style={{ backgroundColor: row.original.color }} />{row.original.name}</div>,
    },
    { accessorKey: "slug", header: "Slug" },
    { accessorKey: "postCount", header: "Số bài viết" },
    { accessorKey: "createdAt", header: "Ngày tạo", cell: ({ row }) => formatDateTime(row.original.createdAt) },
    {
      id: "actions",
      cell: ({ row }) => (
        <div className="flex gap-1">
          <Button variant="ghost" size="icon" onClick={() => openEdit(row.original)}><Edit className="size-4" /></Button>
          <Button variant="ghost" size="icon" onClick={() => confirmDelete(row.original)}><Trash2 className="size-4 text-destructive" /></Button>
        </div>
      ),
    },
  ]

  return (
    <>
      <header className="flex h-16 shrink-0 items-center gap-2 transition-[width,height] ease-linear group-has-data-[collapsible=icon]/sidebar-wrapper:h-12">
        <div className="flex items-center gap-2 px-4">
          <SidebarTrigger className="-ml-1" /><Separator orientation="vertical" className="mr-2 h-4" />
          <Breadcrumb><BreadcrumbList><BreadcrumbItem><BreadcrumbPage>Thẻ</BreadcrumbPage></BreadcrumbItem></BreadcrumbList></Breadcrumb>
        </div>
      </header>
      <div className="flex flex-1 flex-col gap-4 p-4 pt-0">
        <div className="flex items-center justify-between">
          <h1 className="text-lg font-semibold">Quản lý thẻ</h1>
          <Button onClick={openCreate} className="gap-1.5"><Plus className="size-4" />Thêm thẻ</Button>
        </div>
        <DataTable columns={columns} data={data} loading={loading} />
        <CrudSheet open={sheetOpen} onOpenChange={(v) => { setSheetOpen(v); if (!v) resetForm() }} title={editing ? "Sửa thẻ" : "Thêm thẻ"} onSubmit={handleSubmit} submitting={submitting} submitLabel={editing ? "Cập nhật" : "Tạo"}>
          <div className="grid gap-4">
            <div className="grid gap-2"><Label>Tên thẻ *</Label><Input value={formName} onChange={(e) => setFormName(e.target.value)} /></div>
            <div className="grid gap-2"><Label>Màu sắc</Label><input type="color" value={formColor} onChange={(e) => setFormColor(e.target.value)} className="h-10 w-full rounded-md border p-1" /></div>
          </div>
        </CrudSheet>
        <DeleteConfirmDialog open={deleteOpen} onOpenChange={setDeleteOpen} onConfirm={handleDelete} loading={deleting} itemName={deleteTarget?.name} />
      </div>
    </>
  )
}