# Guía de validación de datos de entrada en Onion Architecture
 
Este documento describe la forma de implementar la validación de los datos de entrada al construir el `Application Core` de un caso de uso bajo `Onion Architecture`.
 
## Reglas
 
1. Sin librerías (por ejemplo, Fluentvalidation) ni atributos de validación (Data Annotations).

2. Las reglas se escriben como código explícito que acumula todos los errores, no se detiene en el primero, no lanza excepciones de control de flujo.

3. Las reglas de una entidad viven en su `Model Validator` público llamado `{{EntityName}}Validator`, que expone:

   - `ValidateProperty`: valida una sola propiedad y devuelve sus mensajes de error (vacía si es válida). Se utiliza cuando un caso de uso valide/modifique una única propiedad sin revalidar toda la entidad.

   - `Validate`: recibe los datos a validar y devuelve la lista con todos los mensajes de error (vacía si es válido). Se apoya en `ValidateProperty` para no duplicar reglas.
 
4. El `Model Validator` lo invoca el método fábrica de la entidad, que devuelve el `Result` (entidad o errores).

5. El método fábrica `Create` de la entidad invoca `Validate` y devuelve la entidad si es válida, o los errores.
 
## Ejemplo de implementación de un Model Validator
 
```csharp

public sealed class PizzaValidator

{

    public IReadOnlyList<string> Validate(string name, decimal price)

    {

        var errors = new List<string>();

        errors.AddRange(ValidateProperty(nameof(Pizza.Name), name));

        errors.AddRange(ValidateProperty(nameof(Pizza.Price), price));        

        return errors;

    }
 
    public IReadOnlyList<string> ValidateProperty(string propertyName, object? value) => propertyName switch

    {

        nameof(Pizza.Name) when string.IsNullOrWhiteSpace(value as string) => new[] { "El nombre es requerido." },

        nameof(Pizza.Price) when value is decimal p && p <= 0 => new[] { "El precio debe ser mayor que cero." },        

        _ => Array.Empty<string>()

    };

}

```