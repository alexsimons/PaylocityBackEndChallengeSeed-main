using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var result = await _employeeService.GetEmployeeByIdAsync(id);
        if (result == null)
        {
            return NotFound(new ApiResponse<GetEmployeeDto>
            {
                Success = false,
                Message = "Employee not found"
            });
        }

        var response = new ApiResponse<GetEmployeeDto>
        {
            Data = result,
            Success = true
        };
        
        return Ok(response);
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        //task: use a more realistic production approach
        var employees = await _employeeService.GetAllEmployeesAsync();
        var result = new ApiResponse<List<GetEmployeeDto>>
        {
            Data = employees,
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Get paycheck for employee by id")]
    [HttpGet("{id}/paycheck")]
    public async Task<ActionResult<ApiResponse<PaycheckDto>>> GetPaycheck(int id)
    {
        var paycheck = await _employeeService.CalculatePaycheckAsync(id);
        if (paycheck == null)
        {
            return NotFound(new ApiResponse<PaycheckDto>
            {
                Success = false,
                Message = "Employee not found"
            });
        }

        var response = new ApiResponse<PaycheckDto>
        {
            Data = paycheck,
            Success = true
        };

        return Ok(response);
    }
}
