"use client"

import * as React from "react"
import { useParams, useRouter } from "next/navigation"
import { Save, ArrowLeft, Eye, Clock, Send, Copy, Check } from "lucide-react"
import { toast } from "sonner"

import { Button } from "@/components/ui/button"
import { Label } from "@/components/ui/label"
import { Input } from "@/components/ui/input"
import { Textarea } from "@/components/ui/textarea"
import { Switch } from "@/components/ui/switch"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"

import { Separator } from "@/components/ui/separator"
import { SidebarTrigger } from "@/components/ui/sidebar"
import { Tiptap } from "@/components/editor/tiptap"
import { blogService } from "@/lib/services/blog-service"
import { blogCategoryService } from "@/lib/services/blog-category-service"
import { blogTagService } from "@/lib/services/blog-tag-service"
import type { BlogPost, BlogCategoryDto, TagDto, CreateBlogPostDto, UpdateBlogPostDto } from "@/lib/types"

export default function PostEditorPage() {
  const params = useParams()
  const router = useRouter()
  const id = params.id as string
  const isNew = id === "new"

  const [loading, setLoading] = React.useState(!isNew)
  const [saving, setSaving] = React.useState(false)
  const [post, setPost] = React.useState<BlogPost | null>(null)

  const [title, setTitle] = React.useState("")
  const [slug, setSlug] = React.useState("")
  const [excerpt, setExcerpt] = React.useState("")
  const [content, setContent] = React.useState("")
  const [thumbnailUrl, setThumbnailUrl] = React.useState("")
  const [status, setStatus] = React.useState("draft")
  const [isFeatured, setIsFeatured] = React.useState(false)
  const [allowComments, setAllowComments] = React.useState(true)
  const [metaTitle, setMetaTitle] = React.useState("")
  const [metaDescription, setMetaDescription] = React.useState("")
  const [focusKeyword, setFocusKeyword] = React.useState("")
  const [ogImageUrl, setOgImageUrl] = React.useState("")
  const [selectedCategoryIds, setSelectedCategoryIds] = React.useState<string[]>([])
  const [selectedTagIds, setSelectedTagIds] = React.useState<string[]>([])

  const [categories, setCategories] = React.useState<BlogCategoryDto[]>([])
  const [tags, setTags] = React.useState<TagDto[]>([])

  React.useEffect(() => {
    blogCategoryService.getAll().then(setCategories).catch(() => {})
    blogTagService.getAll().then(setTags).catch(() => {})
  }, [])

  React.useEffect(() => {
    if (!isNew) {
      (async () => {
        try {
          const p = await blogService.getById(id)
          setPost(p)
          setTitle(p.title)
          setSlug(p.slug)
          setExcerpt(p.excerpt ?? "")
          setContent(p.content ?? "")
          setThumbnailUrl(p.thumbnailUrl ?? "")
          setStatus(p.status ?? "draft")
          setIsFeatured(p.isFeatured)
          setAllowComments(p.allowComments)
          setMetaTitle(p.metaTitle ?? "")
          setMetaDescription(p.metaDescription ?? "")
          setFocusKeyword(p.focusKeyword ?? "")
          setOgImageUrl(p.ogImageUrl ?? "")
          setSelectedCategoryIds(p.categories?.map(c => c.id) ?? [])
          setSelectedTagIds(p.tags?.map(t => t.id) ?? [])
        } catch { toast.error("Không thể tải bài viết"); router.push("/admin/cms/posts") }
        finally { setLoading(false) }
      })()
    }
  }, [id, isNew, router])

  const buildDto = (): CreateBlogPostDto | UpdateBlogPostDto => ({
    title,
    excerpt: excerpt || undefined,
    content: content || undefined,
    thumbnailUrl: thumbnailUrl || undefined,
    status,
    metaTitle: metaTitle || undefined,
    metaDescription: metaDescription || undefined,
    focusKeyword: focusKeyword || undefined,
    ogImageUrl: ogImageUrl || undefined,
    isFeatured,
    allowComments,
    tagIds: selectedTagIds.length > 0 ? selectedTagIds : undefined,
    categoryIds: selectedCategoryIds.length > 0 ? selectedCategoryIds : undefined,
  })

  const handleSave = async (overrideStatus?: string) => {
    if (!title.trim()) { toast.error("Vui lòng nhập tiêu đề"); return }
    setSaving(true)
    const finalStatus = overrideStatus ?? status
    try {
      if (isNew) {
        const created = await blogService.create({ ...buildDto(), status: finalStatus } as CreateBlogPostDto)
        toast.success("Tạo bài viết thành công")
        router.push(`/admin/cms/posts/${created.id}`)
      } else {
        await blogService.update(id, { ...buildDto(), status: finalStatus } as UpdateBlogPostDto)
        toast.success("Đã lưu bài viết")
      }
    } catch (e) { toast.error((e as Error).message) }
    finally { setSaving(false) }
  }

  const handlePublish = async () => {
    if (isNew) { await handleSave("published"); return }
    setSaving(true)
    try {
      await blogService.changeStatus(id, "published")
      setStatus("published")
      toast.success("Đã xuất bản bài viết")
    } catch (e) { toast.error((e as Error).message) }
    finally { setSaving(false) }
  }

  const handleSchedule = async () => {
    const date = window.prompt("Nhập ngày giờ xuất bản (YYYY-MM-DD HH:MM):")
    if (!date) return
    if (isNew) { setStatus("scheduled"); await handleSave("scheduled"); return }
    setSaving(true)
    try {
      await blogService.schedule(id, new Date(date).toISOString())
      setStatus("scheduled")
      toast.success("Đã lên lịch xuất bản")
    } catch (e) { toast.error((e as Error).message) }
    finally { setSaving(false) }
  }

  const toggleCategory = (catId: string) => {
    setSelectedCategoryIds(prev => prev.includes(catId) ? prev.filter(c => c !== catId) : [...prev, catId])
  }
  const toggleTag = (tagId: string) => {
    setSelectedTagIds(prev => prev.includes(tagId) ? prev.filter(t => t !== tagId) : [...prev, tagId])
  }

  if (loading) return <div className="p-8"><p className="text-muted-foreground">Đang tải...</p></div>

  return (
    <div className="flex flex-col gap-4 p-4">
      <header className="flex items-center justify-between">
        <div className="flex items-center gap-3">
          <Button variant="ghost" size="icon" onClick={() => router.push("/admin/cms/posts")}><ArrowLeft className="size-5" /></Button>
          <div>
            <h1 className="text-lg font-semibold">{isNew ? "Thêm bài viết mới" : "Sửa bài viết"}</h1>
            {!isNew && post && <p className="text-xs text-muted-foreground">Slug: /blog/{post.slug} • Lượt xem: {post.viewCount}</p>}
          </div>
        </div>
        <div className="flex items-center gap-2">
          <select value={status} onChange={(e) => setStatus(e.target.value)} className="flex h-9 w-36 rounded-md border border-input bg-background px-3 py-1 text-sm">
            <option value="draft">Bản nháp</option>
            <option value="reviewed">Đã duyệt</option>
            <option value="published">Xuất bản</option>
            <option value="scheduled">Lên lịch</option>
          </select>
          <Button variant="outline" onClick={() => handleSave()} disabled={saving}>
            <Save className="size-4 mr-1" />{saving ? "Đang lưu..." : "Lưu"}
          </Button>
          {status !== "published" && (
            <Button onClick={handlePublish} disabled={saving}>
              <Eye className="size-4 mr-1" />Xuất bản
            </Button>
          )}
          <Button variant="outline" onClick={handleSchedule} disabled={saving}>
            <Clock className="size-4 mr-1" />Lên lịch
          </Button>
        </div>
      </header>

      <Tabs defaultValue="content" className="w-full">
        <TabsList>
          <TabsTrigger value="content">Nội dung</TabsTrigger>
          <TabsTrigger value="seo">SEO</TabsTrigger>
          <TabsTrigger value="settings">Cài đặt</TabsTrigger>
        </TabsList>

        <TabsContent value="content" className="space-y-4 mt-4">
          <div className="grid gap-4">
            <div>
              <Label>Tiêu đề *</Label>
              <Input value={title} onChange={(e) => setTitle(e.target.value)} placeholder="Tiêu đề bài viết" className="text-lg font-semibold" />
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <Label>Đường dẫn (Slug)</Label>
                <Input value={slug} onChange={(e) => setSlug(e.target.value)} placeholder="tu-dong-tao-tu-tieu-de" />
              </div>
              <div>
                <Label>Ảnh đại diện (URL)</Label>
                <Input value={thumbnailUrl} onChange={(e) => setThumbnailUrl(e.target.value)} placeholder="https://..." />
              </div>
            </div>
            <div>
              <Label>Mô tả ngắn</Label>
              <Textarea value={excerpt} onChange={(e) => setExcerpt(e.target.value)} rows={2} placeholder="Mô tả ngắn xuất hiện trong danh sách bài viết" />
            </div>
            <div>
              <Label>Nội dung</Label>
              <Tiptap content={content} onChange={setContent} />
            </div>
          </div>
        </TabsContent>

        <TabsContent value="seo" className="space-y-4 mt-4">
          <div className="grid gap-4 max-w-2xl">
            <div className="grid gap-2">
              <Label>Meta Title</Label>
              <Input value={metaTitle} onChange={(e) => setMetaTitle(e.target.value)} placeholder="Tối ưu: 50-60 ký tự" />
              <p className="text-xs text-muted-foreground">{metaTitle.length}/60</p>
            </div>
            <div className="grid gap-2">
              <Label>Meta Description</Label>
              <Textarea value={metaDescription} onChange={(e) => setMetaDescription(e.target.value)} rows={2} placeholder="Tối ưu: 150-160 ký tự" />
              <p className="text-xs text-muted-foreground">{metaDescription.length}/160</p>
            </div>
            <div className="grid gap-2">
              <Label>Focus Keyword</Label>
              <Input value={focusKeyword} onChange={(e) => setFocusKeyword(e.target.value)} placeholder="Từ khóa chính của bài viết" />
            </div>
            <div className="grid gap-2">
              <Label>OG Image URL</Label>
              <Input value={ogImageUrl} onChange={(e) => setOgImageUrl(e.target.value)} placeholder="Ảnh chia sẻ mạng xã hội" />
            </div>
          </div>
        </TabsContent>

        <TabsContent value="settings" className="space-y-4 mt-4">
          <div className="grid gap-6 max-w-2xl">
            <div>
              <Label className="mb-2 block">Danh mục</Label>
              <div className="flex flex-wrap gap-2">
                {categories.map(cat => (
                  <Button key={cat.id} variant={selectedCategoryIds.includes(cat.id) ? "default" : "outline"} size="sm" onClick={() => toggleCategory(cat.id)}>
                    {cat.name}
                  </Button>
                ))}
              </div>
            </div>
            <div>
              <Label className="mb-2 block">Thẻ</Label>
              <div className="flex flex-wrap gap-2">
                {tags.map(tag => (
                  <Button key={tag.id} variant={selectedTagIds.includes(tag.id) ? "default" : "outline"} size="sm" onClick={() => toggleTag(tag.id)}>
                    {tag.name}
                  </Button>
                ))}
              </div>
            </div>
            <Separator />
            <div className="flex items-center justify-between">
              <div><Label>Bài viết nổi bật</Label><p className="text-xs text-muted-foreground">Hiển thị ở mục nổi bật</p></div>
              <Switch checked={isFeatured} onCheckedChange={setIsFeatured} />
            </div>
            <div className="flex items-center justify-between">
              <div><Label>Cho phép bình luận</Label></div>
              <Switch checked={allowComments} onCheckedChange={setAllowComments} />
            </div>
          </div>
        </TabsContent>
      </Tabs>
    </div>
  )
}