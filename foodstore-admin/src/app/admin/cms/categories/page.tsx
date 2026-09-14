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
import { blogCategoryService } from "@/lib/services/blog-category-service"
import { formatDateTime } from "@/lib/utils"
import type { BlogCategoryDto, CreateBlogCategoryDto, UpdateBlogCategoryDto } from "@/lib/types"

export default function BlogCategoriesPage() {
  const [data, setData] = React.useState<BlogCategoryDto[]>([])
  const [loading, setLoading] = React.useState(true)
  const [sheetOpen, setSheetOpen] = React.useState(false)
  const [editing, setEditing] = React.useState<BlogCategoryDto | null>(null)
  const [submitting, setSubmitting] = React.useState(false)
  const [deleteOpen, setDeleteOpen] = React.useState(false)
  const [deleting, setDeleting] = React.useState(false)
  const [deleteTarget, setDeleteTarget] = React.useState<BlogCategoryDto | null>(null)

  const [formName, setFormName] = React.useState("")
  const [formDescription, setFormDescription] = React.useState("")

  const loadData = React.useCallback(async () => {
    setLoading(true)
    try { const res = await blogCategoryService.getAll(); setData(res) }
    catch { toast.error("Unable to load the category.") }
    finally { setLoading(false) }
  }, [])

  React.useEffect(() => { loadData() }, [loadData])

  const resetForm = () => { setFormName(""); setFormDescription(""); setEditing(null) }
  const openCreate = () => { resetForm(); setSheetOpen(true) }
  const openEdit = (item: BlogCategoryDto) => { setEditing(item); setFormName(item.name); setFormDescription(item.description ?? ""); setSheetOpen(true) }

  const handleSubmit = async () => {
    if (!formName.trim()) { toast.error("Please enter the category name."); return }
    setSubmitting(true)
    try {
      if (editing) { await blogCategoryService.update(editing.id, { name: formName.trim(), description: formDescription || undefined }); toast.success("Category updated successfully.") }
      else { await blogCategoryService.create({ name: formName.trim(), description: formDescription || undefined }); toast.success("Category added successfully.") }
      setSheetOpen(false); loadData()
    } catch (e) { toast.error((e as Error).message) }
    finally { setSubmitting(false) }
  }

  const confirmDelete = (item: BlogCategoryDto) => { setDeleteTarget(item); setDeleteOpen(true) }
  const handleDelete = async () => {
    if (!deleteTarget) return; setDeleting(true)
    try { await blogCategoryService.delete(deleteTarget.id); toast.success("Category deleted successfully."); setDeleteOpen(false); loadData() }
    catch { toast.error("Cannot delete category") }
    finally { setDeleting(false) }
  }

  const columns: ColumnDef<BlogCategoryDto>[] = [
    { accessorKey: "name", header: "Category name" },
    { accessorKey: "slug", header: "Slug" },
    { accessorKey: "postCount", header: "Number of articles" },
    { accessorKey: "sortOrder", header: "Order" },
    { accessorKey: "createdAt", header: "Creation date", cell: ({ row }) => formatDateTime(row.original.createdAt) },
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
          <Breadcrumb><BreadcrumbList><BreadcrumbItem><BreadcrumbPage>List of articles</BreadcrumbPage></BreadcrumbItem></BreadcrumbList></Breadcrumb>
        </div>
      </header>
      <div className="flex flex-1 flex-col gap-4 p-4 pt-0">
        <div className="flex items-center justify-between">
          <h1 className="text-lg font-semibold">Portfolio Management</h1>
          <Button onClick={openCreate} className="gap-1.5"><Plus className="size-4" />Add categories</Button>
        </div>
        <DataTable columns={columns} data={data} loading={loading} />
        <CrudSheet open={sheetOpen} onOpenChange={(v) => { setSheetOpen(v); if (!v) resetForm() }} title={editing ? "Edit category" : "Add category"} onSubmit={handleSubmit} submitting={submitting} submitLabel={editing ? "Update" : "Create"}>
          <div className="grid gap-4">
            <div className="grid gap-2"><Label>Category name *</Label><Input value={formName} onChange={(e) => setFormName(e.target.value)} /></div>
            <div className="grid gap-2"><Label>Describe</Label><Input value={formDescription} onChange={(e) => setFormDescription(e.target.value)} /></div>
          </div>
        </CrudSheet>
        <DeleteConfirmDialog open={deleteOpen} onOpenChange={setDeleteOpen} onConfirm={handleDelete} loading={deleting} itemName={deleteTarget?.name} />
      </div>
    </>
  )
}