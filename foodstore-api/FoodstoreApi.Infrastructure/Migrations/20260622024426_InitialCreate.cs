using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FoodstoreApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "addons",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    price = table.Column<decimal>(type: "numeric(10,0)", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    default_route = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_system = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    banned = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_verified = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "blog_categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_blog_categories_blog_categories_parent_id",
                        column: x => x.parent_id,
                        principalTable: "blog_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "blog_settings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "combos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    combo_price = table.Column<decimal>(type: "numeric(10,0)", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_combos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "discounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    type = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    value = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    max_discount_amount = table.Column<decimal>(type: "numeric(10,0)", nullable: true),
                    min_order_amount = table.Column<decimal>(type: "numeric(10,0)", nullable: true),
                    usage_limit = table.Column<int>(type: "integer", nullable: true),
                    used_count = table.Column<int>(type: "integer", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: true),
                    start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "e_invoice_providers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    provider_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    config_json = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_e_invoice_providers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payment_settings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    bank_account = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    bank_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    bank_account_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    template = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "compact2"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sources",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sources", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    color = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false, defaultValue: "#f59e0b"),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    avatar_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    loyalty_points = table.Column<int>(type: "integer", nullable: false),
                    membership_level = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    birthday = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.id);
                    table.ForeignKey(
                        name: "FK_customers_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    avatar_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    hire_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    status = table.Column<short>(type: "smallint", nullable: false),
                    last_login = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.id);
                    table.ForeignKey(
                        name: "FK_employees_AspNetRoles_role_id",
                        column: x => x.role_id,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_employees_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    revoked_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_refresh_tokens_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "menu_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    price = table.Column<decimal>(type: "numeric(10,0)", nullable: false),
                    image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_menu_items_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "e_invoice_settings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    default_provider_id = table.Column<Guid>(type: "uuid", nullable: true),
                    auto_issue = table.Column<bool>(type: "boolean", nullable: false),
                    default_template_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    default_serial_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    digital_signature_config = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_e_invoice_settings", x => x.id);
                    table.ForeignKey(
                        name: "FK_e_invoice_settings_e_invoice_providers_default_provider_id",
                        column: x => x.default_provider_id,
                        principalTable: "e_invoice_providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "blog_posts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    excerpt = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    thumbnail_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValue: "draft"),
                    author_id = table.Column<Guid>(type: "uuid", nullable: true),
                    published_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    meta_title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    meta_description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    focus_keyword = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    keywords = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    canonical_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    og_image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    reading_time = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    word_count = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    seo_score = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    scheduled_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    reviewed_by = table.Column<Guid>(type: "uuid", nullable: true),
                    reviewed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_featured = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    template = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, defaultValue: "default"),
                    view_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    allow_comments = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_posts", x => x.id);
                    table.ForeignKey(
                        name: "FK_blog_posts_employees_author_id",
                        column: x => x.author_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_blog_posts_employees_reviewed_by",
                        column: x => x.reviewed_by,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "media",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    folder = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "misc"),
                    file_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    file_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    file_size = table.Column<long>(type: "bigint", nullable: true),
                    alt_text = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    uploaded_by_employee = table.Column<Guid>(type: "uuid", nullable: true),
                    uploaded_by_customer = table.Column<Guid>(type: "uuid", nullable: true),
                    entity_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media", x => x.id);
                    table.ForeignKey(
                        name: "FK_media_customers_uploaded_by_customer",
                        column: x => x.uploaded_by_customer,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_media_employees_uploaded_by_employee",
                        column: x => x.uploaded_by_employee,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    source_id = table.Column<Guid>(type: "uuid", nullable: true),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: true),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    discount_id = table.Column<Guid>(type: "uuid", nullable: true),
                    subtotal_amount = table.Column<decimal>(type: "numeric(10,0)", nullable: true),
                    discount_amount = table.Column<decimal>(type: "numeric(10,0)", nullable: true),
                    vat_amount = table.Column<decimal>(type: "numeric(10,0)", nullable: true),
                    total_amount = table.Column<decimal>(type: "numeric(10,0)", nullable: true),
                    qr_payment_url = table.Column<string>(type: "text", nullable: true),
                    paid_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    printed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_orders_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_orders_discounts_discount_id",
                        column: x => x.discount_id,
                        principalTable: "discounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_orders_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_orders_employees_updated_by",
                        column: x => x.updated_by,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_orders_sources_source_id",
                        column: x => x.source_id,
                        principalTable: "sources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "combo_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    combo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    menu_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_combo_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_combo_items_combos_combo_id",
                        column: x => x.combo_id,
                        principalTable: "combos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_combo_items_menu_items_menu_item_id",
                        column: x => x.menu_item_id,
                        principalTable: "menu_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "menu_item_addons",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    menu_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    addon_id = table.Column<Guid>(type: "uuid", nullable: false),
                    price_override = table.Column<decimal>(type: "numeric(10,0)", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu_item_addons", x => x.id);
                    table.ForeignKey(
                        name: "FK_menu_item_addons_addons_addon_id",
                        column: x => x.addon_id,
                        principalTable: "addons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_menu_item_addons_menu_items_menu_item_id",
                        column: x => x.menu_item_id,
                        principalTable: "menu_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "blog_post_blocks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    post_id = table.Column<Guid>(type: "uuid", nullable: false),
                    block_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    block_data = table.Column<string>(type: "text", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_post_blocks", x => x.id);
                    table.ForeignKey(
                        name: "FK_blog_post_blocks_blog_posts_post_id",
                        column: x => x.post_id,
                        principalTable: "blog_posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "blog_post_categories",
                columns: table => new
                {
                    post_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_post_categories", x => new { x.post_id, x.category_id });
                    table.ForeignKey(
                        name: "FK_blog_post_categories_blog_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "blog_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_blog_post_categories_blog_posts_post_id",
                        column: x => x.post_id,
                        principalTable: "blog_posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "blog_post_revisions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    post_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    content = table.Column<string>(type: "text", nullable: true),
                    excerpt = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    thumbnail_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    meta_title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    meta_description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    focus_keyword = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    keywords = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    canonical_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    og_image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    word_count = table.Column<int>(type: "integer", nullable: true),
                    reading_time = table.Column<int>(type: "integer", nullable: true),
                    blocks_json = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_post_revisions", x => x.id);
                    table.ForeignKey(
                        name: "FK_blog_post_revisions_blog_posts_post_id",
                        column: x => x.post_id,
                        principalTable: "blog_posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "blog_post_tags",
                columns: table => new
                {
                    post_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tag_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_post_tags", x => new { x.post_id, x.tag_id });
                    table.ForeignKey(
                        name: "FK_blog_post_tags_blog_posts_post_id",
                        column: x => x.post_id,
                        principalTable: "blog_posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_blog_post_tags_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "e_invoices",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider_id = table.Column<Guid>(type: "uuid", nullable: true),
                    invoice_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    template_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    serial_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "draft"),
                    total_amount = table.Column<decimal>(type: "numeric(10,0)", nullable: false),
                    vat_amount = table.Column<decimal>(type: "numeric(10,0)", nullable: false),
                    buyer_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    buyer_tax_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    buyer_address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    pdf_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    xml_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    provider_response = table.Column<string>(type: "text", nullable: true),
                    error_message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    issued_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    cancelled_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    cancel_reason = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_e_invoices", x => x.id);
                    table.ForeignKey(
                        name: "FK_e_invoices_e_invoice_providers_provider_id",
                        column: x => x.provider_id,
                        principalTable: "e_invoice_providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_e_invoices_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    menu_item_id = table.Column<Guid>(type: "uuid", nullable: true),
                    combo_id = table.Column<Guid>(type: "uuid", nullable: true),
                    menu_item_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(10,0)", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    total_price = table.Column<decimal>(type: "numeric(10,0)", nullable: false),
                    note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_items_combos_combo_id",
                        column: x => x.combo_id,
                        principalTable: "combos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_order_items_menu_items_menu_item_id",
                        column: x => x.menu_item_id,
                        principalTable: "menu_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_order_items_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_status_history",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    from_status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    to_status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    changed_by = table.Column<Guid>(type: "uuid", nullable: true),
                    changed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_status_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_status_history_employees_changed_by",
                        column: x => x.changed_by,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_order_status_history_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(10,0)", nullable: false),
                    method = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    reference_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    paid_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_payments_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order_item_addons",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    addon_id = table.Column<Guid>(type: "uuid", nullable: false),
                    addon_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: true),
                    unit_price = table.Column<decimal>(type: "numeric(10,0)", nullable: false),
                    total_price = table.Column<decimal>(type: "numeric(10,0)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_item_addons", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_item_addons_addons_addon_id",
                        column: x => x.addon_id,
                        principalTable: "addons",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_order_item_addons_order_items_order_item_id",
                        column: x => x.order_item_id,
                        principalTable: "order_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_blog_categories_parent_id",
                table: "blog_categories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_blog_categories_slug",
                table: "blog_categories",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_blog_post_blocks_post_id",
                table: "blog_post_blocks",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_blog_post_categories_category_id",
                table: "blog_post_categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_blog_post_revisions_post_id_created_at",
                table: "blog_post_revisions",
                columns: new[] { "post_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_blog_post_tags_tag_id",
                table: "blog_post_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_blog_posts_author_id",
                table: "blog_posts",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_blog_posts_reviewed_by",
                table: "blog_posts",
                column: "reviewed_by");

            migrationBuilder.CreateIndex(
                name: "IX_blog_settings_key",
                table: "blog_settings",
                column: "key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_combo_items_combo_id_menu_item_id",
                table: "combo_items",
                columns: new[] { "combo_id", "menu_item_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_combo_items_menu_item_id",
                table: "combo_items",
                column: "menu_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_customers_customer_code",
                table: "customers",
                column: "customer_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_customers_user_id",
                table: "customers",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_discounts_code",
                table: "discounts",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_e_invoice_settings_default_provider_id",
                table: "e_invoice_settings",
                column: "default_provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_e_invoices_invoice_number",
                table: "e_invoices",
                column: "invoice_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_e_invoices_order_id",
                table: "e_invoices",
                column: "order_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_e_invoices_provider_id",
                table: "e_invoices",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_employees_employee_code",
                table: "employees",
                column: "employee_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_role_id",
                table: "employees",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_employees_user_id",
                table: "employees",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_media_uploaded_by_customer",
                table: "media",
                column: "uploaded_by_customer");

            migrationBuilder.CreateIndex(
                name: "IX_media_uploaded_by_employee",
                table: "media",
                column: "uploaded_by_employee");

            migrationBuilder.CreateIndex(
                name: "IX_menu_item_addons_addon_id",
                table: "menu_item_addons",
                column: "addon_id");

            migrationBuilder.CreateIndex(
                name: "IX_menu_item_addons_menu_item_id_addon_id",
                table: "menu_item_addons",
                columns: new[] { "menu_item_id", "addon_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_menu_items_category_id",
                table: "menu_items",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_addons_addon_id",
                table: "order_item_addons",
                column: "addon_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_addons_order_item_id",
                table: "order_item_addons",
                column: "order_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_combo_id",
                table: "order_items",
                column: "combo_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_menu_item_id",
                table: "order_items",
                column: "menu_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_order_id",
                table: "order_items",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_status_history_changed_by",
                table: "order_status_history",
                column: "changed_by");

            migrationBuilder.CreateIndex(
                name: "IX_order_status_history_order_id",
                table: "order_status_history",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_customer_id",
                table: "orders",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_discount_id",
                table: "orders",
                column: "discount_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_employee_id",
                table: "orders",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_order_code",
                table: "orders",
                column: "order_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_source_id",
                table: "orders",
                column: "source_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_updated_by",
                table: "orders",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_payments_order_id",
                table: "payments",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_token",
                table: "refresh_tokens",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tags_slug",
                table: "tags",
                column: "slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "blog_post_blocks");

            migrationBuilder.DropTable(
                name: "blog_post_categories");

            migrationBuilder.DropTable(
                name: "blog_post_revisions");

            migrationBuilder.DropTable(
                name: "blog_post_tags");

            migrationBuilder.DropTable(
                name: "blog_settings");

            migrationBuilder.DropTable(
                name: "combo_items");

            migrationBuilder.DropTable(
                name: "e_invoice_settings");

            migrationBuilder.DropTable(
                name: "e_invoices");

            migrationBuilder.DropTable(
                name: "media");

            migrationBuilder.DropTable(
                name: "menu_item_addons");

            migrationBuilder.DropTable(
                name: "order_item_addons");

            migrationBuilder.DropTable(
                name: "order_status_history");

            migrationBuilder.DropTable(
                name: "payment_settings");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "blog_categories");

            migrationBuilder.DropTable(
                name: "blog_posts");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "e_invoice_providers");

            migrationBuilder.DropTable(
                name: "addons");

            migrationBuilder.DropTable(
                name: "order_items");

            migrationBuilder.DropTable(
                name: "combos");

            migrationBuilder.DropTable(
                name: "menu_items");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "discounts");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "sources");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}

