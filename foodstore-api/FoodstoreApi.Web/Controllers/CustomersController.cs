using FoodstoreApi.Usecase.Interfaces;
using FoodstoreApi.Usecase.DTOs.Customer;
using FoodstoreApi.Web.Authorization;
using FoodstoreApi.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodstoreApi.Web.ApiResponse;

namespace FoodstoreApi.Web.Controllers;

[Route("api/admin/customers")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost("~/api/customers/register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] CustomerRegisterDto dto)
    {
        try
        {
            var customer = await _customerService.RegisterAsync(dto);
            return ApiResult.Success(customer);
        }
        catch (Exception ex)
        {
            return ApiResult.BadRequest(ex.Message);
        }
    }

    [HttpPost("~/api/customers/login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] CustomerLoginDto dto)
    {
        try
        {
            var result = await _customerService.LoginAsync(dto);

            Response.Cookies.Append("auth_token", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return ApiResult.Success(result);
        }
        catch (Exception ex)
        {
            return ApiResult.BadRequest(ex.Message);
        }
    }

    [HttpGet("~/api/customers/me")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetMe()
    {
        var userId = User.GetUserId();
        var customer = await _customerService.GetCustomerByIdAsync(userId);
        if (customer == null) return ApiResult.NotFound();
        return ApiResult.Success(customer);
    }

    [HttpPut("~/api/customers/me")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> UpdateProfile([FromBody] CustomerUpdateDto dto)
    {
        var userId = User.GetUserId();
        var customer = await _customerService.UpdateProfileAsync(userId, dto);
        if (customer == null) return ApiResult.NotFound();
        return ApiResult.Success(customer);
    }

    [HttpGet("lookup/{phone}")]
    [RequirePermission("customer.view")]
    public async Task<IActionResult> LookupByPhone(string phone)
    {
        var customer = await _customerService.GetByPhoneAsync(phone);
        if (customer == null) return ApiResult.NotFound();
        return ApiResult.Success(customer);
    }

    [HttpGet]
    [RequirePermission("customer.view")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var customers = await _customerService.GetAllAsync(cancellationToken);
        return ApiResult.Success(customers);
    }

    [HttpGet("{id:guid}")]
    [RequirePermission("customer.view")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetByIdAsync(id, cancellationToken);
        if (customer == null) return ApiResult.NotFound();
        return ApiResult.Success(customer);
    }

    [HttpPost]
    [RequirePermission("customer.create")]
    public async Task<IActionResult> Create(CreateCustomerDto dto, CancellationToken cancellationToken)
    {
        var created = await _customerService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [RequirePermission("customer.update")]
    public async Task<IActionResult> Update(Guid id, UpdateCustomerAdminDto dto, CancellationToken cancellationToken)
    {
        var ok = await _customerService.UpdateAsync(id, dto, cancellationToken);
        if (!ok) return ApiResult.NotFound();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission("customer.delete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var ok = await _customerService.DeleteAsync(id, cancellationToken);
        if (!ok) return ApiResult.NotFound();
        return NoContent();
    }

    [HttpPost("{id:guid}/add-points")]
    [RequirePermission("customer.update")]
    public async Task<IActionResult> AddPoints(Guid id, [FromBody] AddPointsDto dto)
    {
        if (dto.Points <= 0)
            return ApiResult.BadRequest("The number of points must be larger than 0");

        var ok = await _customerService.AddPointsAsync(id, dto.Points, dto.Reason);
        if (!ok) return ApiResult.NotFound();
        var customer = await _customerService.GetByIdAsync(id);
        return ApiResult.Success(customer);
    }

    [HttpGet("{id:guid}/orders")]
    [RequirePermission("customer.view")]
    public async Task<IActionResult> GetOrderHistory(Guid id)
    {
        var orders = await _customerService.GetOrderHistoryAsync(id);
        return ApiResult.Success(orders);
    }

    [HttpGet("birthdays")]
    [RequirePermission("customer.view")]
    public async Task<IActionResult> GetUpcomingBirthdays()
    {
        var data = await _customerService.GetUpcomingBirthdaysAsync();
        return ApiResult.Success(data);
    }

    [HttpGet("leaderboard")]
    [RequirePermission("customer.view")]
    public async Task<IActionResult> GetLeaderboard([FromQuery] int count = 100, CancellationToken cancellationToken = default)
    {
        var leaderboard = await _customerService.GetTopLeaderboardAsync(count, cancellationToken);
        return ApiResult.Success(leaderboard);
    }
}
