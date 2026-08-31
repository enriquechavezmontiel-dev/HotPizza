---
name: Plan de validación inmediata de datos
version: 1.0
source: user-input-data-validations
---

# Plan: Validación inmediata de registro

Validar cada campo de registro cuando la persona pulsa Enter y repetir únicamente el campo inválido. Las reglas se centralizarán en validadores de campo que reutilizará `PizzaValidator`; `PizzaService` conservará la validación completa previa a la persistencia como barrera final.

## Pasos

1. Crear `HotPizza/Validators/IFieldValidator.cs`, un contrato genérico que devuelva el `ValidationResult` existente para un valor individual. Crear validadores concretos para nombre, descripción, precio y tamaño; cada uno devuelve los mensajes en español ya utilizados: nombre y descripción no vacíos ni solo espacios, precio mayor que cero y tamaño de 20, 30 o 40 cm.
2. Refactorizar `HotPizza/Validators/PizzaValidator.cs` para recibir esos cuatro validadores y componer sus resultados al validar una entidad `Pizza`. Esto elimina reglas duplicadas y conserva su contrato público `IValidator<Pizza>` para `PizzaService`. Este paso depende del anterior.
3. Crear un adaptador de consola en `HotPizza/UI/` que encapsule `ReadLine`, `Write` y `WriteLine`, junto con su interfaz. Registrar la implementación real y los validadores de campo en `HotPizza/Program.cs`. El adaptador permite probar las repeticiones de entrada sin usar `System.Console` directamente. Puede ejecutarse en paralelo con el paso 2.
4. Refactorizar `HotPizza/UI/ConsoleUI.cs` para inyectar el adaptador y los validadores de campo. Extraer lectores reutilizables por tipo: texto requerido para nombre y descripción, decimal positivo para precio e entero permitido para tamaño. Cada lector mostrará el prompt, validará la entrada al pulsar Enter, escribirá el error en español y continuará hasta devolver un valor correcto; errores de formato numérico también repetirán el mismo campo. `RegisterPizzaAsync` solo llamará al servicio tras obtener los cuatro valores correctos.
5. Mantener `HotPizza/Services/PizzaService.cs` como la segunda barrera de validación y no modificar repositorios, formato JSON, consulta de catálogo ni el menú salvo el uso indirecto de la UI refactorizada.
6. Ampliar pruebas: validar individualmente las cuatro clases de campo; ajustar las pruebas de `PizzaValidator` para inyectar los validadores reales y confirmar que la entidad compone sus errores; añadir pruebas de `ConsoleUI` con una entrada simulada que prueben reintentos por texto vacío, formato numérico inválido, precio no positivo y tamaño no permitido, además de que solo se llama a `RegisterPizzaAsync` tras la secuencia válida. Conservar las pruebas existentes de no persistencia en `PizzaService`.
7. Compilar y ejecutar la solución; realizar una sesión manual de registro con entradas inválidas y una pizza válida, seguida de consulta de catálogo para confirmar que únicamente la pizza válida aparece.

## Archivos relevantes

- `HotPizza/Validators/PizzaValidator.cs`: pasará a componer los validadores por campo y mantiene la validación final de entidad.
- `HotPizza/Validators/IValidator.cs` y `HotPizza/Validators/ValidationResult.cs`: contratos existentes que se reutilizan.
- `HotPizza/Validators/IFieldValidator.cs` y los cuatro validadores concretos: nuevos propietarios de las reglas individuales compartidas por UI y validador de entidad.
- `HotPizza/UI/ConsoleUI.cs`: puntos de captura y bucles de reintento por campo.
- `HotPizza/UI/IConsoleAdapter.cs` y `HotPizza/UI/ConsoleAdapter.cs`: nueva abstracción mínima de I/O para mantener las pruebas aisladas.
- `HotPizza/Program.cs`: registro de dependencias de validadores y adaptador de consola.
- `HotPizza/Services/PizzaService.cs`: no modificar su flujo; verificar que conserva la barrera antes de `IPizzaRepository.AddAsync`.
- `HotPizza.Tests/Validators/PizzaValidatorTests.cs` y nuevas pruebas de validadores de campo: reglas individuales y composición.
- `HotPizza.Tests/UI/ConsoleUITests.cs`: reintento y mensajes de validación de la interacción.

## Verificación

1. Ejecutar `dotnet build HotPizza.slnx` y `dotnet test HotPizza.slnx`.
2. En pruebas de UI, comprobar que cada entrada inválida genera el mensaje esperado y vuelve a mostrar solo su prompt; comprobar que el servicio no se llama hasta contar con los cuatro campos válidos.
3. Ejecutar `dotnet run --project HotPizza/HotPizza.csproj`; probar nombre y descripción vacíos o con espacios, precio no numérico, cero y negativo, tamaño no numérico y `25`, seguidos de entradas válidas. Cada campo debe volver a solicitarse inmediatamente.
4. Registrar una pizza correcta y consultarla con la opción 2; verificar que se muestra correctamente y que ningún intento rechazado llega a `pizzas.json`.

## Decisiones

- "Conforme escribe" significa después de confirmar cada campo con Enter, no captura por pulsación de tecla.
- La descripción también se valida inmediatamente como requerida para preservar la regla de negocio existente.
- Las reglas se implementan una vez en validadores de campo y se reutilizan desde UI y `PizzaValidator`; no se duplican condiciones en `ConsoleUI`.
- No se añadirán edición o eliminación, cambios a DTO o JSON, ni cambios funcionales a la consulta del catálogo.
