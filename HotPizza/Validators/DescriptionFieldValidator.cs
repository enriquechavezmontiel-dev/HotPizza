namespace HotPizza.Validators;

public class DescriptionFieldValidator : RequiredTextFieldValidator
{
    public DescriptionFieldValidator()
        : base("La descripción es obligatoria.")
    {
    }
}
