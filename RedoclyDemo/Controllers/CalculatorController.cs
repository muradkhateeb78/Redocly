using Microsoft.AspNetCore.Mvc;
using RedoclyDemo.Handlers;

namespace RedoclyDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class CalculatorController : ControllerBase
{
    private readonly CalculatorHandler _handler;

    public CalculatorController(CalculatorHandler handler)
    {
        _handler = handler;
    }
    [Tags("GS GET Endpoints")]
    [HttpGet("add")]
    public ActionResult<double> Add(double left, double right)
        => Execute(left, right, CalculatorOperation.Addition);
    [Tags("GS GET Endpoints")]
    [HttpGet("subtract")]
    public ActionResult<double> Subtract(double left, double right)
        => Execute(left, right, CalculatorOperation.Subtraction);
    [Tags("GS GET Endpoints")]
    [HttpGet("multiply")]
    public ActionResult<double> Multiply(double left, double right)
        => Execute(left, right, CalculatorOperation.Multiplication);

    [HttpGet("divide")]
    public ActionResult<double> Divide(double left, double right)
        => Execute(left, right, CalculatorOperation.Division);

    [HttpGet("multipleAdd")]
    public ActionResult<double> MultipleAdd(double left, double right)
    {
        var result = _handler.MultiplyThenAdd(left, right);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new calculator operation request and returns the result.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("operations")]
    [Tags("Other GS Endpoints")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<CalculationResponse> CreateCalculation([FromBody] CalculationRequest request)
    {
        try
        {
            var result = _handler.PerformOperation(request.Left, request.Right, request.Operation);
            var response = new CalculationResponse(
                request.Operation.ToString(),
                result,
                "Calculation created successfully.");

            return Created(Request.Path, response);
        }
        catch (DivideByZeroException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// Replaces an existing calculator operation request and returns the updated result.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>

    [HttpPut("operations")]
    [Tags("Other GS Endpoints")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<CalculationResponse> ReplaceCalculation([FromBody] CalculationRequest request)
    {
        try
        {
            var result = _handler.PerformOperation(request.Left, request.Right, request.Operation);
            var response = new CalculationResponse(
                request.Operation.ToString(),
                result,
                "Calculation replaced successfully.");

            return Ok(response);
        }
        catch (DivideByZeroException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Partially updates a calculator operation request and returns the recalculated result.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>

    [HttpPatch("operations")]
    [Tags("Other GS Endpoints")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<CalculationResponse> UpdateCalculation([FromBody] CalculationPatchRequest request)
    {
        try
        {
            var operation = request.Operation ?? CalculatorOperation.Addition;
            var left = request.Left ?? 0;
            var right = request.Right ?? 0;
            var result = _handler.PerformOperation(left, right, operation);
            var response = new CalculationResponse(
                operation.ToString(),
                result,
                "Calculation updated successfully with a patch endpoint.");

            return Ok(response);
        }
        catch (DivideByZeroException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a calculator operation request for the specified operation type.
    /// </summary>
    /// <param name="operation"></param>
    /// <returns></returns>

    [HttpDelete("operations/{operation}")]
    [Tags("Other GS Endpoints")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult DeleteCalculation(CalculatorOperation operation)
    {
        Response.Headers["X-Calculator-Message"] = $"Calculation for {operation} deleted successfully.";
        return NoContent();
    }

    private ActionResult<double> Execute(double left, double right, CalculatorOperation operation)
    {
        try
        {
            var result = _handler.PerformOperation(left, right, operation);
            return Ok(result);
        }
        catch (DivideByZeroException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public record CalculationRequest(double Left, double Right, CalculatorOperation Operation);

public record CalculationPatchRequest(double? Left, double? Right, CalculatorOperation? Operation);

public record CalculationResponse(string Operation, double Result, string Message);
