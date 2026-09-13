---
name: implement-onion-applicationcore
description: Procedimiento genérico para implementar el Application Core de Onion Architecture de un caso de uso a partir de un archivo de especificación. Aplica cuando se pide implementar el caso de uso con Onion Architecture.
---
 
# Implementar el Application Core de Onion Architecture de un caso de uso
 
Procedimiento para implementar el `Application Core` de `Onion Architecture` para un caso de uso a partir de un archivo de especificación.
 
## Entradas
 
- Nombre de la aplicación: `AppName`. 
- Nombre del caso de uso a implementar: `UseCaseName`. 
- La ruta del archivo de la especificación del caso de uso tiene la sintaxis `specs/{{AppName}}/usecases/{{UseCaseName}}.spec.md`
- El directorio `{{AppName}}` contiene el archivo de solución `{{AppName}}.slnx` y la estructura de proyectos con las siguientes referencias entre proyectos:
  | Proyecto | Referencia directa | Referencias transitivas |
  |----------|--------------------|-------------------------|
  | {{AppName}}.DomainModels | Ninguna | Ninguna |
  | {{AppName}}.OutputPorts  | {{AppName}}.DomainModels | Ninguna |
  | {{AppName}}.InputPorts   | {{AppName}}.DomainModels | Ninguna |
  | {{AppName}}.DomainServices | {{AppName}}.OutputPorts |  {{AppName}}.DomainModels |
  | {{AppName}}.ApplicationServices | {{AppName}}.InputPorts,  {{AppName}}.DomainServices|  {{AppName}}.DomainModels, {{AppName}}.OutputPorts |
  | {{AppName}}.IoC | {{AppName}}.ApplicationServices | {{AppName}}.InputPorts,  {{AppName}}.DomainServices, {{AppName}}.DomainModels, {{AppName}}.OutputPorts |
  | {{AppName}}.Tests | {{AppName}}.IoC | {{AppName}}.ApplicationServices, {{AppName}}.InputPorts,  {{AppName}}.DomainServices, {{AppName}}.DomainModels, {{AppName}}.OutputPorts |
 
- Se encuentran disponibles los siguientes archivos de referencias ubicados en el direcctorio `.agents/references/`: 
  | Archivo | Descripción |
  |---------|-------------|
  | onion-architecture-applicationcore-reference.md | Guía para ubicar artefactos al implementar el `Application Core` de un caso de uso |
  | onion-architecture-validation-reference.md | Guía para implementar validación de los datos de entrada de un caso de uso. |
  | onion-architecture-dependency-injection-reference.md | Guía para el registro de dependencias. | 
  | MSMDotNet.Result.md | Guía para implementar el patron Result |
 
 
## Verificación antes de iniciar la implementación
 
Verificar las siguientes condiciones, en caso de que alguna no se cumpla, informar al usuario y terminar el proceso:
- Debe haberse proporcionado el nombre de la solución.
- Debe haberse proporcionado el nombre del caso de uso a implementar.
- Debe existir el archivo de especificación del caso de uso.
- Deben existir los archivos de referencias.
 
## Convenciones transversales
 
### Resultado de operaciones (patrón Result)
El tipo de retorno de cada método lo define **la firma declarada en la especificación**. Cuando la spec indique que un método devuelve una **lista de errores** o un **valor de éxito** (o describa flujos alternos con errores), ese método devuelve un `Result` según el patrón definido en `.agents/references/MSMDotNet.Result.md`. Consultar ese documento para su API. **Si la especificación no indica `Result`** (p. ej. devuelve directamente un DTO, un valor o `void`), respeta la firma de la spec y **no** lo envuelvas en `Result`. No inventar tipos de resultado propios.
 
### Asincronía
Si el Input Port u Output Port declara métodos `...Async`, propagar `async`/`await` de extremo a extremo, respetando el tipo de retorno declarado (`Task<T>`, `Task<Result<...>>` o `Task`, según la spec).
 
### Validación 
Cuando se requiera implementar validación de los datos de entrada (DTOs de entrada del Input Port), utilizar el archivo `onion-architecture-validation-reference.md` como guia de implementación de validación. Los artefactos creados para la validación siguen las mismas reglas de ubicación de artefactos y de implementación del patrón `Result` cuando sea necesario.
 
### Ubicación y visibilidad de artefactos
- Seguir siempre `.agents/references/onion-architecture-applicationcore-reference.md` para determinar el tipo de artefacto a crear (class, enum, interface) así como la accesibilidad (public, internal) y el proyecto en el que debe crearse. 
- Las referencias entre proyectos ya existen de forma directa o transitiva, no deben agregarse referencias directas cuando ya contengan las referencias de forma transitiva.
 
---
 
## Orden de implementación (de adentro hacia afuera)
 
Implementar en este orden para que cada paso compile apoyándose solo en lo ya creado:
 
1. **Domain Models** (`{{App}}.DomainModels`)
   - Crear las entidades / value objects que declare la spec.
   - Colocar la **lógica de validación fuera de la entidad**, en una clase `Model Validator` llamada `{{Nombre-del-artefacto-a-validar}}Validator` (por ejemplo, ProductValidator.cs para validar la clase Product.) dedicada, que evalúa todas las reglas de formato/estructura e invariantes (requeridos, rangos, valores permitidos) y **acumula todos los errores** (no se detiene en el primero, no lanza excepciones).
   - Exponer un **método fábrica** en la entidad que invoca al `Model Validator` y devuelve un `Result` (según la firma que declare la especificación): la entidad construida si es válida, o la lista de errores si no lo es. La creación pasa siempre por este método fábrica (patrón *always-valid domain*).
 
2. **Input Port y DTOs** (`{{App}}.InputPorts`)
   - Declarar los DTOs de entrada.
   - Declarar los DTOs de salida.
   - Declarar la interface del Input Port con la firma que indica la especificación.
   - El valor de éxito y/o los errores se comunican mediante el patrón Result **si la especificación lo requiere** (ver Convenciones).
 
3. **Output Ports** (`{{App}}.OutputPorts`)
   - Declarar las interfaces que el núcleo necesita para salir hacia infraestructura (repositorios, gateways), con las firmas de la especificación.
 
4. **Domain Services / Domain Service Validators** (`{{App}}.DomainServices`) — solo si la spec lo requiere
   - Implementar reglas que coordinan varias entidades/agregados o que requieren datos externos (obtenidos SIEMPRE vía un Output Port).
 
5. **Application Service** (`{{App}}.ApplicationServices`)
   - Implementa el flujo del caso de uso mediante una clase que implementa la interface del Input Port. 
   - El flujo concreto lo define el archivo de especificación del caso de uso.
 
6. **Registro en DI** (`{{App}}.IoC`)
   - Registra en DI las dependencias creadas para el caso de uso (Input Port, Domain Services y validadores, si existen) siguiendo **estrictamente** `.agents/references/onion-architecture-dependency-injection-reference.md`.
 
## Creación de pruebas unitarias
- Las pruebas unitarias se crean en el proyecto existente `{{AppName}}.Tests/{{UseCaseName}}` utilizando el framework xUnit.
- Para la creación de objetos simulados (mocks) de los Output Port y dependencias se debe utilizar el paquete **NSubstitute** que también ya se encuentra agregado en el proyecto de pruebas.
- Crear una clase de pruebas pública llamada `{{UseCaseName}}ServiceTests.cs`.
- Las pruebas deben seguir estrictamente el patrón **Arrange-Act-Assert (AAA)**.
- Los métodos de prueba deben seguir la convención de nombres: `[MétodoQueSeEvalua]_[Escenario]_[ResultadoEsperado]`.
- El agente debe leer detenidamente el archivo de especificación del caso de uso e implementar obligatoriamente los siguientes escenarios:
### Flujo Principal (Happy Path)
- Crear una prueba que valide el comportamiento correcto cuando los datos de entrada son válidos.
- Verificar que se devuelva el DTO de respuesta esperado con los datos correctos según la especificación.
- Verificar (usando `Received()` de NSubstitute) que los Output Ports requeridos hayan sido invocados con los parámetros correctos.
 
### Flujos Alternativos y Validaciones (Sad Path)
- Crear una prueba independiente para **cada una** de las restricciones o reglas de validación definidas en la especificación.
- Cada prueba debe forzar la falla de una regla específica y verificar que el caso de uso produzca la respuesta de error esperada según la especificación.
- Cuando el caso de uso tenga validación, verificar (usando `DidNotReceive()` de NSubstitute) que los Output Ports de persistencia o efectos secundarios **no** se hayan invocado si la validación falló.
 
##  Verificación de compilación
- Ejecutar `dotnet build` sobre la solución.
- Si hay errores de compilación, reportarlos, corregirlos y no continuar al paso de confirmación.
- Si la compilación es exitosa, continuar.
 
### Confirmación
Una vez terminado el proceso:
- Confirmar la lista completa de artefactos creados o reemplazados (incluyendo la suite de pruebas unitarias y las modificaciones en DependencyContainer.cs)