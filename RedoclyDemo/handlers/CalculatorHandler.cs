namespace RedoclyDemo.Handlers;

public enum CalculatorOperation
{
    Addition,
    Subtraction,
    Multiplication,
    Division
}

public class CalculatorHandler
{
    public double PerformOperation(double left, double right, CalculatorOperation operation)
    {
        return operation switch
        {
            CalculatorOperation.Addition => left + right,
            CalculatorOperation.Subtraction => left - right,
            CalculatorOperation.Multiplication => left * right,
            CalculatorOperation.Division => HandleDivision(left, right),
            _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, "Unsupported calculator operation.")
        };
    }

    public double MultiplyThenAdd(double left, double right)
    {
        var product = PerformOperation(left, right, CalculatorOperation.Multiplication);
        var sum = PerformOperation(left, right, CalculatorOperation.Addition);
        return product + sum;
    }

    private static double HandleDivision(double left, double right)
    {
        if (right == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero.");
        }

        return left / right;
    }
}
