"use client"

import * as React from "react"
import { usePathname, useRouter } from "next/navigation"

import { NavMain } from "@/components/nav-main"
import { NavUser } from "@/components/nav-user"
import { SettingsDialog } from "@/components/settings-dialog"
import { TeamSwitcher } from "@/components/team-switcher"
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarHeader,
  SidebarRail,
} from "@/components/ui/sidebar"
import {
  GalleryVerticalEndIcon,
  FileTextIcon,
  LayoutDashboardIcon,
  ShoppingCartIcon,
  PackageIcon,
  UsersIcon,
  Settings2Icon,
  TruckIcon,
  ClipboardListIcon,
  PercentIcon,
  BookOpenIcon,
  SmartphoneIcon,
  TagsIcon,
  UserCogIcon,
  RadioIcon,
  Trophy,
  ReceiptTextIcon,
} from "lucide-react"

type Module = "food" | "employees" | "crm" | "cms"

const teams: { name: string; logo: React.ReactNode; plan: string }[] = [
  {
    name: "Quản lý quán ăn",
    logo: <GalleryVerticalEndIcon />,
    plan: "Thực đơn & Đơn hàng",
  },
  {
    name: "Quản lý nhân viên",
    logo: <UserCogIcon />,
    plan: "Nhân sự & Phân quyền",
  },
  {
    name: "Quản lý khách hàng",
    logo: <UsersIcon />,
    plan: "CRM & POS",
  },
  {
    name: "Quản lý bài viết",
    logo: <FileTextIcon />,
    plan: "Nội dung & Tin tức",
  },
]

const currentModuleName: Record<Module, string> = {
  food: "Quản lý quán ăn",
  employees: "Quản lý nhân viên",
  crm: "Quản lý khách hàng",
  cms: "Quản lý bài viết",
}

const foodNav: {
  title: string; url?: string; icon: React.ReactNode; isActive?: boolean;
  items?: { title: string; url: string }[]
}[] = [
  {
    title: "Tổng quan",
    icon: <LayoutDashboardIcon />,
    isActive: true,
    items: [
      { title: "Tổng quan", url: "/admin/food/dashboard" },
      { title: "Thống kê & AI", url: "/admin/food/analytics" },
    ],
  },
  {
    title: "Đơn hàng",
    icon: <ShoppingCartIcon />,
    items: [
      { title: "Tất cả đơn hàng", url: "/admin/food/orders" },
      { title: "Chờ xử lý", url: "/admin/food/orders?status=pending" },
    ],
  },
  {
    title: "Sản phẩm",
    icon: <PackageIcon />,
    items: [
      { title: "Tất cả sản phẩm", url: "/admin/food/products" },
      { title: "Danh mục", url: "/admin/food/categories" },
      { title: "Topping", url: "/admin/food/toppings" },
      { title: "Combo", url: "/admin/food/combos" },
    ],
  },
  {
    title: "Nhà cung cấp",
    url: "/admin/food/suppliers",
    icon: <TruckIcon />,
  },
  {
    title: "Nguồn đơn",
    url: "/admin/food/sources",
    icon: <RadioIcon />,
  },
  {
    title: "Khuyến mãi",
    icon: <PercentIcon />,
    items: [
      { title: "Mã giảm giá", url: "/admin/food/discounts" },
    ],
  },
  {
    title: "Thanh toán",
    url: "/admin/food/payment-settings",
    icon: <ClipboardListIcon />,
  },
  {
    title: "Hóa đơn điện tử",
    icon: <ReceiptTextIcon />,
    items: [
      { title: "Tổng quan", url: "/admin/food/e-invoice/dashboard" },
      { title: "Giao dịch", url: "/admin/food/e-invoice/transactions" },
      { title: "Nhà cung cấp", url: "/admin/food/e-invoice/providers" },
      { title: "Cài đặt", url: "/admin/food/e-invoice/settings" },
    ],
  },
]

const employeeNav = [
  {
    title: "Tổng quan",
    url: "/admin/employees/dashboard",
    icon: <LayoutDashboardIcon />,
    isActive: true,
  },
  {
    title: "Nhân viên",
    icon: <UserCogIcon />,
    items: [
      { title: "Tất cả nhân viên", url: "/admin/employees/all" },
      { title: "Vai trò", url: "/admin/employees/roles" },
      { title: "Phân quyền", url: "/admin/employees/role-permissions" },
    ],
  },
]

const crmNav = [
  {
    title: "Tổng quan",
    url: "/admin/crm/dashboard",
    icon: <LayoutDashboardIcon />,
    isActive: true,
  },
  {
    title: "Khách hàng",
    url: "/admin/crm/customers",
    icon: <UsersIcon />,
  },
  {
    title: "Bảng xếp hạng điểm",
    url: "/admin/crm/leaderboard",
    icon: <Trophy />,
  },
]

const cmsNav = [
  {
    title: "Tổng quan",
    url: "/admin/cms/dashboard",
    icon: <LayoutDashboardIcon />,
    isActive: true,
  },
  {
    title: "Bài viết",
    url: "/admin/cms/posts",
    icon: <FileTextIcon />,
  },
  {
    title: "Thẻ",
    url: "/admin/cms/tags",
    icon: <TagsIcon />,
  },
  {
    title: "Danh mục",
    url: "/admin/cms/categories",
    icon: <BookOpenIcon />,
  },
  {
    title: "Cài đặt",
    url: "/admin/cms/settings",
    icon: <Settings2Icon />,
  },
]

const moduleMap: Record<Module, typeof foodNav> = {
  food: foodNav,
  employees: employeeNav,
  crm: crmNav,
  cms: cmsNav,
}

function getModuleFromPath(path: string): Module {
  if (path.startsWith("/admin/employees")) return "employees"
  if (path.startsWith("/admin/crm")) return "crm"
  if (path.startsWith("/admin/cms")) return "cms"
  return "food"
}

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
  const pathname = usePathname()
  const router = useRouter()
  const [settingsTab, setSettingsTab] = React.useState<"profile" | "settings" | null>(null)

  const currentModule = getModuleFromPath(pathname)
  const navItems = moduleMap[currentModule]
  const activeTeam = teams.find((t) => t.name === currentModuleName[currentModule]) ?? teams[0]

  function handleTeamChange(team: { name: string; logo: React.ReactNode; plan: string }) {
    const routeMap: Record<string, Module> = {
      "Quản lý quán ăn": "food",
      "Quản lý nhân viên": "employees",
      "Quản lý khách hàng": "crm",
      "Quản lý bài viết": "cms",
    }
    router.push(`/admin/${routeMap[team.name] ?? "food"}`)
  }

  return (
    <>
      <Sidebar collapsible="icon" {...props}>
        <SidebarHeader>
          <TeamSwitcher teams={teams} activeTeam={activeTeam} onTeamChange={handleTeamChange} />
        </SidebarHeader>
        <SidebarContent>
          <NavMain items={navItems} />
        </SidebarContent>
        <SidebarFooter>
          <NavUser onOpenSettings={setSettingsTab} />
        </SidebarFooter>
        <SidebarRail />
      </Sidebar>
      <SettingsDialog tab={settingsTab} onClose={() => setSettingsTab(null)} />
    </>
  )
}
