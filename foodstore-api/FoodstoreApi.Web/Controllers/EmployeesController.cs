using System;
using FoodstoreApi.Usecase.Interfaces;
using FoodstoreApi.Web.Authorization;
using FoodstoreApi.Usecase.DTOs.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodstoreApi.Web.ApiResponse;

namespace FoodstoreApi.Web.Controllers;

[ApiController]
[Route("api/admin/employees")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeesController(IEmployeeService service)
    {
        _service = service;
    }

    [HttpGet]
    [RequirePermission("employee.view")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var employees = await _service.GetAllAsync(cancellationToken);
        return ApiResult.Success(employees);
    }

    [HttpGet("{id:guid}")]
    [RequirePermission("employee.view")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var employee = await _service.GetByIdAsync(id, cancellationToken);
        if (employee == null) return ApiResult.NotFound();
        return ApiResult.Success(employee);
    }

    [HttpPost]
    [RequirePermission("employee.create")]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _service.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            return ApiResult.BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    [RequirePermission("employee.update")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeDto dto, CancellationToken cancellationToken)
    {
        var ok = await _service.UpdateAsync(id, dto, cancellationToken);
        if (!ok) return ApiResult.NotFound();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [RequirePermission("employee.delete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var ok = await _service.DeleteAsync(id, cancellationToken);
            if (!ok) return ApiResult.NotFound();
            return NoContent();
        }
        catch (Exception)
        {
            return ApiResult.BadRequest("This employee cannot be deleted because they are linked to an order or other data.");
        }
    }
}




