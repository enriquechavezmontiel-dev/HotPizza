# MSMDotNet.Result — Guía de referencia rápida
 
Referencia para agentes y desarrolladores sobre el uso del paquete *NuGet* `MSMDotNet.Result` para implementar el patrón `Result`.
 
- Paquete NuGet: `MSMDotNet.Result` (versión `1.0.0`)
- Namespaces: `Results` (tipos) y `Results.Extensions` (métodos `Match`)
- Repositorio fuente: https://github.com/msmdotnet/result
- Licencia: MIT
 
## Instalación
 
```xml
<PackageReference Include="MSMDotNet.Result" Version="1.0.0" />
```
 
```csharp
using Results;
using Results.Extensions; // necesario para usar Match(...)
```
 
## Tipos disponibles
 
### `Result` (sin valor de retorno)
 
Representa el resultado de una operación que no devuelve datos, solo éxito o fallo.
 
```csharp
public sealed class Result : ResultBase
{
    public static Result Ok();
    public static Result Fail(params Error[] errors);
    public static Result Fail(string errorMessage);
}
```
 
Miembros heredados de `ResultBase`:
 
| Miembro        | Tipo       | Descripción                                     |
|----------------|------------|-------------------------------------------------|
| `IsSuccess`    | `bool`     | `true` si no hay errores.                       |
| `IsFailure`    | `bool`     | `true` si hay al menos un error.                |
| `Errors`       | `Error[]`  | Lista completa de errores (vacía si es éxito).  |
| `Error`        | `Error?`   | Primer error, o `null` si es éxito.             |
| `ErrorMessage` | `string?`  | Mensaje del primer error, o `null` si es éxito. |
 
### `Result<TSuccess>` (con valor de retorno en éxito)
 
```csharp
public sealed class Result<TSuccess> : ResultBase
{
    public TSuccess Value { get; } // válido solo si IsSuccess
 
    public static Result<TSuccess> Ok(TSuccess value);
    public static Result<TSuccess> Fail(params Error[] errors);
    public static Result<TSuccess> Fail(string errorMessage);
}
```
 
⚠️ `Value` no lanza excepción explícita por parte del tipo si se accede en fallo (queda
en su valor por defecto/último asignado internamente); **siempre verificar `IsSuccess`
antes de leer `Value`**, o usar `Match` (ver abajo).
 
### `Result<TSuccess, TError>` (éxito y error fuertemente tipados)
 
Útil cuando el error no es un simple mensaje sino un objeto de dominio.
 
```csharp
public sealed class Result<TSuccess, TError>
{
    public TSuccess Value { get; } // lanza InvalidOperationException si IsFailure
    public TError Error { get; }   // lanza InvalidOperationException si IsSuccess
    public bool IsSuccess { get; }
    public bool IsFailure { get; }
 
    public static Result<TSuccess, TError> Ok(TSuccess successValue);
    public static Result<TSuccess, TError> Fail(TError errorValue);
}
```
 
A diferencia de `Result` y `Result<TSuccess>`, aquí acceder a `Value` en fallo (o a
`Error` en éxito) **sí lanza `InvalidOperationException`**. Usar `IsSuccess`/`IsFailure`
o `Match` para acceder de forma segura.
 
### `Error`
 
```csharp
public class Error
{
    public string? Code { get; }
    public string Message { get; }
 
    public Error(string code, string message);
    public Error(string message); // Code queda en null
}
```
 
## Métodos `Match` (namespace `Results.Extensions`)
 
Permiten manejar éxito/error de forma funcional, evitando `if (result.IsSuccess) ...`.
 
```csharp
// Result -> TResult
TResult Match<TResult>(this Result result, Func<TResult> onSuccess, Func<Result, TResult> onError);
 
// Result -> void
void Match(this Result result, Action onSuccess, Action<Result> onError);
 
// Result<T> -> TResult
TResult Match<T, TResult>(this Result<T> result, Func<T, TResult> onSuccess, Func<Result<T>, TResult> onError);
 
// Result<T> -> void
void Match<T>(this Result<T> result, Action<T> onSuccess, Action<Result<T>> onError);
```
 
> No existe `Match` para `Result<TSuccess, TError>` en la versión `1.0.0`: para ese tipo
> se debe usar `IsSuccess`/`IsFailure` explícitamente.
 
## Ejemplos de uso
 
### Validación con `Result<T>` y consumo con `Match`
 
```csharp
public Result<Pizza> RegisterPizza(string name, string description, decimal price, int sizeInCentimeters)
{
    if (string.IsNullOrWhiteSpace(name))
        return Result<Pizza>.Fail(new Error("PIZZA_NAME_REQUIRED", "El nombre de la pizza no puede estar vacío."));
 
    if (price <= 0)
        return Result<Pizza>.Fail(new Error("PIZZA_PRICE_INVALID", "El precio debe ser mayor a cero."));
 
    var pizza = new Pizza { /* ... */ };
    return Result<Pizza>.Ok(pizza);
}
```
 
```csharp
var result = _pizzaService.RegisterPizza(name, description, price, size);
 
result.Match(
    onSuccess: pizza => _console.WriteLine($"Pizza registrada. Id: {pizza.Id}"),
    onError: failure => _console.WriteLine($"Error al registrar la pizza: {failure.ErrorMessage}")
);
```
 
### `Result` sin valor (operaciones tipo `void`)
 
```csharp
public Result Add(Pizza pizza)
{
    try
    {
        // persistir...
        return Result.Ok();
    }
    catch (IOException ex)
    {
        return Result.Fail(new Error("PIZZA_SAVE_FAILED", $"No se pudo guardar la pizza: {ex.Message}"));
    }
}
```