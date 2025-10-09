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

    [HttpGet("add")]
    public ActionResult<double> Add(double left, double right)
        => Execute(left, right, CalculatorOperation.Addition);

    [HttpGet("subtract")]
    public ActionResult<double> Subtract(double left, double right)
        => Execute(left, right, CalculatorOperation.Subtraction);

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
