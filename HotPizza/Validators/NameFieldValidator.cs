namespace HotPizza.Validators;

public class NameFieldValidator : RequiredTextFieldValidator
{
    public NameFieldValidator()
        : base("El nombre es obligatorio.")
    {
    }
}
