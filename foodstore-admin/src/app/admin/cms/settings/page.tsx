"use client"

import * as React from "react"
import { Edit, Save } from "lucide-react"
import { toast } from "sonner"

import { Breadcrumb, BreadcrumbItem, BreadcrumbList, BreadcrumbPage } from "@/components/ui/breadcrumb"
import { Button } from "@/components/ui/button"
import { Label } from "@/components/ui/label"
import { Input } from "@/components/ui/input"
import { Textarea } from "@/components/ui/textarea"
import { Separator } from "@/components/ui/separator"
import { SidebarTrigger } from "@/components/ui/sidebar"
import { blogSettingService } from "@/lib/services/blog-setting-service"
import type { BlogSettingDto } from "@/lib/types"

export default function BlogSettingsPage() {
  const [settings, setSettings] = React.useState<BlogSettingDto[]>([])
  const [loading, setLoading] = React.useState(true)
  const [editingKey, setEditingKey] = React.useState<string | null>(null)
  const [editValue, setEditValue] = React.useState("")
  const [editDesc, setEditDesc] = React.useState("")

  const loadData = React.useCallback(async () => {
    setLoading(true)
    try { const res = await blogSettingService.getAll(); setSettings(res) }
    catch { toast.error("Không thể tải cài đặt") }
    finally { setLoading(false) }
  }, [])

  React.useEffect(() => { loadData() }, [loadData])

  const startEdit = (s: BlogSettingDto) => { setEditingKey(s.key); setEditValue(s.value); setEditDesc(s.description ?? "") }
  const cancelEdit = () => { setEditingKey(null); setEditValue(""); setEditDesc("") }

  const handleSave = async (key: string) => {
    try {
      await blogSettingService.upsert(key, { value: editValue, description: editDesc || undefined })
      toast.success("Đã lưu cài đặt")
      cancelEdit()
      loadData()
    } catch (e) { toast.error((e as Error).message) }
  }

  const settingLabels: Record<string, string> = {
    site_name: "Tên trang web",
    site_description: "Mô tả trang web",
    posts_per_page: "Bài viết mỗi trang",
    default_template: "Template mặc định",
    facebook_url: "Facebook URL",
    twitter_url: "Twitter URL",
    instagram_url: "Instagram URL",
    footer_text: "Footer text",
    contact_email: "Email liên hệ",
  }

  return (
    <>
      <header className="flex h-16 shrink-0 items-center gap-2 transition-[width,height] ease-linear group-has-data-[collapsible=icon]/sidebar-wrapper:h-12">
        <div className="flex items-center gap-2 px-4">
          <SidebarTrigger className="-ml-1" /><Separator orientation="vertical" className="mr-2 h-4" />
          <Breadcrumb><BreadcrumbList><BreadcrumbItem><BreadcrumbPage>Cài đặt CMS</BreadcrumbPage></BreadcrumbItem></BreadcrumbList></Breadcrumb>
        </div>
      </header>
      <div className="flex flex-1 flex-col gap-4 p-4 pt-0">
        <h1 className="text-lg font-semibold">Cài đặt CMS</h1>
        {loading ? (
          <p className="text-muted-foreground">Đang tải...</p>
        ) : (
          <div className="grid gap-4 max-w-2xl">
            {settings.length === 0 && <p className="text-muted-foreground">Chưa có cài đặt nào. Thêm cài đặt bằng cách lưu giá trị mới.</p>}
            {settings.map((s) => (
              <div key={s.key} className="rounded-lg border p-4">
                <div className="flex items-center justify-between mb-2">
                  <div>
                    <Label className="font-medium">{settingLabels[s.key] ?? s.key}</Label>
                    <p className="text-xs text-muted-foreground">{s.description}</p>
                  </div>
                  <Button variant="ghost" size="sm" onClick={() => startEdit(s)}><Edit className="size-4" /></Button>
                </div>
                {editingKey === s.key ? (
                  <div className="grid gap-2">
                    <Input value={editValue} onChange={(e) => setEditValue(e.target.value)} />
                    <Textarea value={editDesc} onChange={(e) => setEditDesc(e.target.value)} placeholder="Mô tả" rows={2} />
                    <div className="flex gap-2">
                      <Button size="sm" onClick={() => handleSave(s.key)}><Save className="size-4 mr-1" />Lưu</Button>
                      <Button size="sm" variant="outline" onClick={cancelEdit}>Hủy</Button>
                    </div>
                  </div>
                ) : (
                  <p className="text-sm">{s.value}</p>
                )}
              </div>
            ))}
          </div>
        )}
        <div className="max-w-2xl">
          <p className="text-xs text-muted-foreground mt-4">Mẹo: Các cài đặt mới có thể được thêm bằng cách gọi API trực tiếp. Các key phổ biến: site_name, posts_per_page, default_template, facebook_url, contact_email.</p>
        </div>
      </div>
    </>
  )
}