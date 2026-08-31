---
name: Plan de validación de datos de las pizzas
version: 1.0
source: validar-datos-de-las-pizzas.md
---

# Plan: Validar datos de las pizzas

Ajustar el validador existente para que una pizza solo se pueda registrar con nombre no vacío ni compuesto únicamente de espacios, precio positivo y un tamaño de 20, 30 o 40 cm. La validación seguirá ocurriendo en la capa actual antes del repositorio, por lo que los datos rechazados no se persistirán y la consulta ya implementada podrá confirmar el resultado.

## Pasos

1. Revisar `HotPizza/Validators/PizzaValidator.cs` y sustituir la regla genérica de tamaño mayor que cero por una comprobación explícita que acepte únicamente `20`, `30` o `40`. Mantener la comprobación existente `string.IsNullOrWhiteSpace` para el nombre y la regla de precio mayor que cero. Mantener la validación existente de descripción para no relajar la conducta previa no solicitada.
2. Conservar `HotPizza/Services/PizzaService.cs` como responsable de ejecutar `IValidator<Pizza>.Validate` antes de llamar a `IPizzaRepository.AddAsync`. Solo adaptar esta capa si fuese necesario por el mensaje de validación; no duplicar reglas en `ConsoleUI` ni trasladarlas a `Program.cs`.
3. Actualizar `HotPizza.Tests/Validators/PizzaValidatorTests.cs` conforme a las reglas finales: conservar los casos de nombre vacío, precio cero y precio negativo; agregar un caso para nombre con solo espacios; reemplazar las pruebas de tamaño negativo/cero por casos representativos de valores no permitidos, como `25`, y añadir o mantener casos de tamaños válidos `20`, `30` y `40`.
4. Ampliar `HotPizza.Tests/Services/PizzaServiceTests.cs` con una prueba de integración de servicio para datos inválidos que use el validador real y compruebe que `AddAsync` no es invocado. Conservar los mocks existentes y no modificar pruebas para ocultar fallos.
5. No modificar el repositorio JSON, las entidades, los contratos del catálogo ni `ConsoleUI`, salvo que la presentación actual no muestre el mensaje de error que ya devuelve el servicio. La UI debe seguir mostrando en español los errores de validación retornados.
6. Ejecutar la compilación, todas las pruebas y los escenarios interactivos indicados en la verificación.

## Archivos relevantes

- `HotPizza/Validators/PizzaValidator.cs`: punto propietario de las reglas de nombre, precio y tamaño; cambiar únicamente la regla de tamaño.
- `HotPizza/Validators/IValidator.cs` y `HotPizza/Validators/ValidationResult.cs`: contratos existentes que deben reutilizarse sin cambios.
- `HotPizza/Services/PizzaService.cs`: barrera existente que evita la llamada al repositorio cuando el resultado de validación no es válido.
- `HotPizza/UI/ConsoleUI.cs`: consumidor de `OperationResult`; confirmar que imprime los errores de validación en español.
- `HotPizza.Tests/Validators/PizzaValidatorTests.cs`: cobertura principal de cada regla individual.
- `HotPizza.Tests/Services/PizzaServiceTests.cs`: prueba de que un resultado inválido no persiste mediante `IPizzaRepository.AddAsync`.
- `HotPizza/Repositories/JsonPizzaRepository.cs`: no modificar; su comportamiento solo se verificará indirectamente mediante la consulta del catálogo.

## Verificación

1. Ejecutar `dotnet build HotPizza.slnx`.
2. Ejecutar `dotnet test HotPizza.slnx`; debe pasar toda la batería existente y los nuevos casos de nombre con espacios, tamaños inválidos y tamaños permitidos.
3. Ejecutar `dotnet run --project HotPizza/HotPizza.csproj` y usar la opción de registro para intentar: nombre vacío, nombre con espacios, precio `0`, precio negativo y tamaño `25`. Cada intento debe informar el error y no registrar una pizza.
4. Registrar una pizza válida con nombre no vacío, precio positivo y tamaño `20`, `30` o `40`; confirmar el mensaje de éxito.
5. Consultar el catálogo con la opción 2 y verificar que no aparezcan las pizzas rechazadas, que aparezca la válida con sus cinco campos y que registro y consulta sigan regresando al menú.

## Decisiones

- Se conserva la separación actual: reglas en `PizzaValidator`, orquestación y no persistencia en `PizzaService`, interacción en `ConsoleUI` y persistencia en el repositorio JSON.
- La regla actual de descripción no vacía se conserva porque ya forma parte del comportamiento de registro y el encargo no pide eliminarla.
- No se añadirán operaciones de edición o eliminación, cambios de DTO/formato JSON ni abstracciones nuevas para la consola.
