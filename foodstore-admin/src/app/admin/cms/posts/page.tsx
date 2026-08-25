"use client"

import * as React from "react"
import { Plus, Trash2, FileEdit } from "lucide-react"
import { useRouter } from "next/navigation"
import { toast } from "sonner"
import type { ColumnDef } from "@tanstack/react-table"

import { Breadcrumb, BreadcrumbItem, BreadcrumbList, BreadcrumbPage } from "@/components/ui/breadcrumb"
import { Button } from "@/components/ui/button"
import { Separator } from "@/components/ui/separator"
import { SidebarTrigger } from "@/components/ui/sidebar"
import { DataTable } from "@/components/data-table"
import { DeleteConfirmDialog } from "@/components/confirm-dialog"
import { StatusBadge } from "@/components/status-badge"
import { blogService } from "@/lib/services/blog-service"
import { formatDateTime } from "@/lib/utils"
import type { BlogPost } from "@/lib/types"

const statusLabels: Record<string, string> = {
  draft: "Bản nháp",
  reviewed: "Đã duyệt",
  published: "Đã xuất bản",
  scheduled: "Đã lên lịch",
}

export default function PostsPage() {
  const router = useRouter()
  const [data, setData] = React.useState<BlogPost[]>([])
  const [loading, setLoading] = React.useState(true)
  const [deleteOpen, setDeleteOpen] = React.useState(false)
  const [deleting, setDeleting] = React.useState(false)
  const [deleteTarget, setDeleteTarget] = React.useState<BlogPost | null>(null)

  

  const loadData = React.useCallback(async () => {
    setLoading(true)
    try { const res = await blogService.getAll(); setData(res) }
    catch { toast.error("Không thể tải bài viết") }
    finally { setLoading(false) }
  }, [])

  React.useEffect(() => { loadData() }, [loadData])

  const openCreate = () => { router.push("/admin/cms/posts/new") }
  const openEdit = (item: BlogPost) => { router.push(`/admin/cms/posts/${item.id}`) }

  const confirmDelete = (item: BlogPost) => { setDeleteTarget(item); setDeleteOpen(true) }
  const handleDelete = async () => {
    if (!deleteTarget) return
    setDeleting(true)
    try { await blogService.delete(deleteTarget.id); toast.success("Xóa bài viết thành công"); setDeleteOpen(false); loadData() }
    catch { toast.error("Không thể xóa bài viết") }
    finally { setDeleting(false) }
  }

  const columns: ColumnDef<BlogPost>[] = [
    {
      accessorKey: "title",
      header: "Tiêu đề",
      cell: ({ row }) => (
        <div className="flex items-center gap-2 max-w-[300px]">
          <span className="truncate font-medium">{row.original.title}</span>
        </div>
      ),
    },
    {
      accessorKey: "status",
      header: "Trạng thái",
      cell: ({ row }) => <StatusBadge status={row.original.status ?? "draft"} customLabels={statusLabels} />,
    },
    {
      accessorKey: "authorName",
      header: "Tác giả",
    },
    {
      accessorKey: "viewCount",
      header: "Lượt xem",
    },
    {
      accessorKey: "createdAt",
      header: "Ngày tạo",
      cell: ({ row }) => formatDateTime(row.original.createdAt),
    },
    {
      id: "actions",
      cell: ({ row }) => (
        <div className="flex items-center gap-1">
          <Button variant="ghost" size="icon" onClick={() => openEdit(row.original)} title="Mở trình soạn thảo">
            <FileEdit className="size-4" />
          </Button>
          <Button variant="ghost" size="icon" onClick={() => confirmDelete(row.original)}>
            <Trash2 className="size-4 text-destructive" />
          </Button>
        </div>
      ),
    },
  ]

  return (
    <>
      <header className="flex h-16 shrink-0 items-center gap-2 transition-[width,height] ease-linear group-has-data-[collapsible=icon]/sidebar-wrapper:h-12">
        <div className="flex items-center gap-2 px-4">
          <SidebarTrigger className="-ml-1" />
          <Separator orientation="vertical" className="mr-2 h-4" />
          <Breadcrumb><BreadcrumbList><BreadcrumbItem><BreadcrumbPage>Bài viết</BreadcrumbPage></BreadcrumbItem></BreadcrumbList></Breadcrumb>
        </div>
      </header>
      <div className="flex flex-1 flex-col gap-4 p-4 pt-0">
        <div className="flex items-center justify-between">
          <h1 className="text-lg font-semibold">Quản lý bài viết</h1>
          <Button onClick={openCreate} className="gap-1.5"><Plus className="size-4" />Thêm bài viết</Button>
        </div>
        <DataTable columns={columns} data={data} loading={loading} />

        

        <DeleteConfirmDialog
          open={deleteOpen}
          onOpenChange={setDeleteOpen}
          onConfirm={handleDelete}
          loading={deleting}
          itemName={deleteTarget?.title}
        />
      </div>
    </>
  )
}