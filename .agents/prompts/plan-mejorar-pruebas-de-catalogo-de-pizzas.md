---
name: Plan de mejora de pruebas del catálogo de pizzas
version: 1.0
source: mejorar-pruebas-del-catalogo-de-pizzas.md
---

# Plan: Mejorar pruebas del catálogo de pizzas

Ampliar únicamente las pruebas que faltan para demostrar el flujo completo de registro, validación, consulta y persistencia. La implementación de producción no se modificará: las pruebas deben revelar cualquier comportamiento incorrecto y conservar todas las pruebas actuales.

## Pasos

1. Ejecutar la suite actual como línea base y revisar las pruebas existentes antes de añadir casos. No duplicar la cobertura ya presente: `PizzaServiceTests` ya cubre registro correcto, consulta de repositorio vacía y con pizzas, y que un tamaño inválido no llega a `AddAsync`; `PizzaValidatorTests` ya cubre nombre vacío o con espacios, precio cero o negativo, tamaño no permitido y tamaños válidos; `JsonPizzaRepositoryTests` ya cubre persistencia JSON.
2. Extender `HotPizza.Tests/Validators/FieldValidatorTests.cs` con los dos límites aún ausentes de validadores individuales: descripción compuesta solo por espacios y precio negativo. Mantener los mensajes de error en español y el estilo xUnit existente. Puede ejecutarse en paralelo con el paso 3.
3. Extender `HotPizza.Tests/UI/ConsoleUITests.cs`, reutilizando `TestConsoleAdapter` y Moq: añadir una prueba de la opción 2 con catálogo vacío, una de catálogo con pizza que compruebe identificador, nombre, descripción, precio y tamaño, y una de fallo de `GetAllPizzasAsync` que compruebe el mensaje de error. Cada prueba debe terminar con la opción 3 para completar el bucle de menú. Puede ejecutarse en paralelo con el paso 2.
4. Añadir una prueba específica de UI para un fallo devuelto por `RegisterPizzaAsync` después de que todos los datos de entrada sean válidos. Comprobar que muestra el encabezado de error y el mensaje del servicio, sin asumir ni alterar el comportamiento de persistencia en producción. Este paso depende del patrón reutilizado en el paso 3.
5. Evaluar la necesidad de una prueba de integración UI a JSON solo después de los pasos 2 a 4. No añadirla si repite `ConsoleUITests`, `PizzaServiceTests` y `JsonPizzaRepositoryTests`; si se incorpora, debe usar un archivo temporal aislado y limpiar sus recursos, sin tocar `pizzas.json` del repositorio.
6. Ejecutar `dotnet build HotPizza.slnx` y `dotnet test HotPizza.slnx`. Generar la cobertura con `dotnet test HotPizza.slnx --collect:"XPlat Code Coverage"`, localizar `coverage.cobertura.xml` en el resultado de pruebas y reportar el porcentaje global de cobertura de líneas que contiene el resumen Cobertura. Si se requiere un informe HTML, generar uno con ReportGenerator sin editar producción.

## Archivos relevantes

- `HotPizza.Tests/Validators/FieldValidatorTests.cs`: ampliar únicamente descripción con espacios y precio negativo.
- `HotPizza.Tests/Validators/PizzaValidatorTests.cs`: referencia para los casos ya cubiertos; no duplicarlos ni modificarlos.
- `HotPizza.Tests/Services/PizzaServiceTests.cs`: referencia de registro, consulta y no persistencia ya cubiertos; no duplicarlos.
- `HotPizza.Tests/Repositories/JsonPizzaRepositoryTests.cs`: referencia de persistencia JSON ya cubierta.
- `HotPizza.Tests/UI/ConsoleUITests.cs`: ampliar la cobertura de consulta, catálogo vacío, error de consulta y error de registro de la UI.
- `HotPizza/UI/IConsoleAdapter.cs`: contrato reutilizable para la entrada y salida simulada de UI.
- `HotPizza/UI/ConsoleUI.cs`: sistema bajo prueba; no modificar.
- `HotPizza/Validators/DescriptionFieldValidator.cs` y `HotPizza/Validators/PriceFieldValidator.cs`: sistemas bajo prueba; no modificar.

## Verificación

1. Compilar con `dotnet build HotPizza.slnx` y ejecutar todas las pruebas mediante `dotnet test HotPizza.slnx`; toda prueba existente y nueva debe ejecutarse correctamente.
2. Confirmar que la batería nueva cubre la descripción con espacios, precio negativo, catálogo vacío, presentación completa de una pizza, error de consulta y error de registro mostrado por la UI.
3. Ejecutar `dotnet test HotPizza.slnx --collect:"XPlat Code Coverage"` y extraer de `coverage.cobertura.xml` el porcentaje de cobertura de líneas global para informarlo.
4. Si una prueba nueva falla por un defecto de producción, no modificar la aplicación: informar el método, comportamiento observado y prueba que lo detectó.

## Decisiones

- El alcance es exclusivamente pruebas; no se modificará la aplicación para lograr resultados verdes.
- No se duplicarán los casos de validación, servicio y repositorio ya presentes.
- La prioridad es cerrar la falta de cobertura de la consulta en la interfaz de consola y sus mensajes de error.
- No se agregará una prueba UI a JSON que modifique el archivo de catálogo del repositorio; una integración aislada solo se añade si, tras revisar duplicación, aporta una garantía distinta.
