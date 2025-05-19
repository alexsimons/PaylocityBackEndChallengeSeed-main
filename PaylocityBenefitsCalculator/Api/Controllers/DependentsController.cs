using Api.Dtos.Dependent;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public DependentsController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependent = await _employeeService.GetDependentByIdAsync(id);
        if (dependent == null)
        {
            return NotFound(new ApiResponse<GetDependentDto>
            {
                Success = false,
                Message = "Dependent not found"
            });
        }

        var response = new ApiResponse<GetDependentDto>
        {
            Data = dependent,
            Success = true
        };

        return Ok(response);
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = await _employeeService.GetAllDependentsAsync();
        var result = new ApiResponse<List<GetDependentDto>>
        {
            Data = dependents,
            Success = true
        };
        
        return Ok(result);
    }
}
