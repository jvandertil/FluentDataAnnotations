namespace FluentDataAnnotations.Validation
{
    public interface IEntityValidator
    {
        IValidationState Validate(object entity);
    }
}