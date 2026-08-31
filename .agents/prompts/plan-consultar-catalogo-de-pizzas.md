---
name: Plan de consulta del catálogo de pizzas
version: 1.0
source: consultar-catalogo-de-pizzas.md
---

# Plan: Consultar catálogo de pizzas

Extender la aplicación de consola para listar las pizzas ya persistidas, reutilizando la lectura JSON existente. La consulta se expondrá por el servicio y la UI pasará a un menú repetible de tres opciones; el registro actual se conservará sin cambios funcionales.

## Pasos

1. Extender `HotPizza/Services/IPizzaService.cs` con un método asíncrono de consulta, `GetAllPizzasAsync`, que devuelva `OperationResult<List<Pizza>>`, manteniendo el contrato de resultados utilizado por el registro. Este paso bloquea el siguiente.
2. Implementar `GetAllPizzasAsync` en `HotPizza/Services/PizzaService.cs`: delegar en `IPizzaRepository.GetAllAsync()` y devolver la lista en un resultado correcto; manejar de forma coherente con el patrón actual cualquier fallo de persistencia. No cambiar `IPizzaRepository` ni `JsonPizzaRepository`, porque ya proveen la carga desde `pizzas.json`.
3. Reestructurar `HotPizza/UI/ConsoleUI.cs` para que `RunAsync` muestre un menú y repita hasta la opción 3: la opción 1 conserva el flujo actual de registro; la opción 2 invoca el nuevo método del servicio y presenta cada pizza con identificador, nombre, descripción, precio y tamaño; si la lista está vacía, muestra `El catálogo está vacío.`; las opciones inválidas muestran un mensaje en español y vuelven al menú. Mantener la interacción y la lógica de formato en UI, sin trasladarlas a `Program.cs`.
4. Ampliar `HotPizza.Tests/Services/PizzaServiceTests.cs`, siguiendo sus mocks de Moq y estilo xUnit, con pruebas para una lista vacía y para una lista con pizzas devuelta por el repositorio. Verificar que el resultado sea exitoso, conserve las pizzas y que se invoque `GetAllAsync` una vez.
5. Evaluar el patrón de pruebas disponible para `ConsoleUI`; si la clase ya permite inyectar `TextReader` y `TextWriter`, añadir pruebas que cubran salida de catálogo vacío y salida de una pizza. Si no está preparada para I/O inyectable, limitar las pruebas nuevas al contrato del servicio para evitar introducir una abstracción de consola que no es necesaria para esta funcionalidad.
6. Ejecutar la compilación, las pruebas y la verificación manual indicadas abajo.

## Archivos relevantes

- `HotPizza/Services/IPizzaService.cs`: contrato público de la capa de negocio; añadir `GetAllPizzasAsync`.
- `HotPizza/Services/PizzaService.cs`: delegación entre UI y repositorio; reutilizar `IPizzaRepository.GetAllAsync()`.
- `HotPizza/Repositories/IPizzaRepository.cs`: referencia del método existente `GetAllAsync`; no requiere modificación.
- `HotPizza/Repositories/JsonPizzaRepository.cs`: referencia de la lectura de `pizzas.json`; no requiere modificación.
- `HotPizza/UI/ConsoleUI.cs`: propietario del ciclo de menú, mensajes en español y presentación de las pizzas.
- `HotPizza/Entities/Pizza.cs`: fuente de `Id`, `Name`, `Description`, `Price` y `Size` para la salida.
- `HotPizza.Tests/Services/PizzaServiceTests.cs`: lugar preferido para las pruebas unitarias de la consulta.
- `HotPizza/Program.cs`: verificar que la configuración de DI existente no requiera cambios; no incorporar la lógica del menú aquí.

## Verificación

1. Compilar toda la solución con `dotnet build HotPizza.slnx`.
2. Ejecutar todas las pruebas con `dotnet test HotPizza.slnx`, conservando la cobertura existente de validación, repositorio y registro.
3. Ejecutar la consola con `dotnet run --project HotPizza/HotPizza.csproj` usando un archivo JSON de prueba o una pizza creada mediante la opción 1.
4. Confirmar que la opción 2 muestra `Id`, `Name`, `Description`, `Price` y `Size` de cada pizza, que el catálogo vacío muestra el mensaje requerido y que ambas operaciones retornan al menú hasta seleccionar la opción 3.

## Decisiones

- Se reutiliza la persistencia JSON actual sin modificar sus DTOs ni el formato del archivo.
- No se implementarán edición, eliminación ni nuevas operaciones de catálogo.
- Las pruebas de consulta se mantendrán en la capa de servicio; las pruebas directas de consola solo se incorporarán si la infraestructura existente permite capturar I/O sin una refactorización ajena al alcance.
