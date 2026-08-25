using FoodstoreApi.Infrastructure.Data;
using FoodstoreApi.Infrastructure.Handlers;
using FoodstoreApi.Infrastructure.Repositories;
using FoodstoreApi.Usecase.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoodstoreApi.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StoreDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IMenuItemRepository, MenuItemRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ISourceRepository, SourceRepository>();
        services.AddScoped<IAddonRepository, AddonRepository>();
        services.AddScoped<IDiscountRepository, DiscountRepository>();
        services.AddScoped<IComboRepository, ComboRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IBlogRepository, BlogRepository>();
        services.AddScoped<IMediaRepository, MediaRepository>();
        services.AddScoped<IPaymentSettingRepository, PaymentSettingRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IBlogCategoryRepository, BlogCategoryRepository>();
        services.AddScoped<IBlogRevisionRepository, BlogRevisionRepository>();
        services.AddScoped<IBlogBlockRepository, BlogBlockRepository>();
        services.AddScoped<IBlogSettingRepository, BlogSettingRepository>();

        services.AddScoped<IEInvoiceRepository, EInvoiceRepository>();

        services.AddScoped<IMediaService, MediaHandler>();

        // Redis 8.10 Infrastructure Service Registration
        var redisConnString = configuration.GetConnectionString("Redis") ?? "redis:6379,abortConnect=false";
        services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(sp =>
            StackExchange.Redis.ConnectionMultiplexer.Connect(redisConnString));
        services.AddScoped<IRedisService, FoodstoreApi.Infrastructure.Caching.RedisService>();

        return services;
    }
}
