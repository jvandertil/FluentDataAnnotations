namespace FluentDataAnnotations.Validation
{
    public interface IValidationError
    {
        string Property { get; }
        string Message { get; }
    }
}