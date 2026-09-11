# Guía de ubicación de artefactos en Onion Architecture
 
Este documento presenta una guía para ubicar artefactos al implementar el `Application Core` de un caso de uso bajo `Onion Architecture`.
 
## Ubicación de artefactos principales
 
| Artefacto | ¿En qué proyecto se ubica? | Tipo | Accesibilidad | Comentarios |
|---|---|---|---|---|
| Entity / Value Object / Aggregate / Domain Event / Domain Exception / enum / Factory | `{{App}}.DomainModels` | `class`, `enum`| `public` | Dependiendo del tipo de artefacto puede ser `class` o `enum` |
| Input Port | `{{App}}.InputPorts` | `interface` | `public` | Lo implementa un Application Service; lo usa un Input Adapter |
| DTO de Entrada | `{{App}}.InputPorts` | `class` | `public` | Utilizado como contenedor de los datos de entrada que recibe el Input Port. |
| DTO de Salida | `{{App}}.InputPorts` | `class` | `public` | Utilizado como contenedor del resultado devuelto por el Input Port |
| Output Port | `{{App}}.OutputPorts` | `interface` | `public` | Lo implementa un Output Adapter; lo consume Application Service y/o Domain Service |
| Domain Service | `{{App}}.DomainServices` | `class` | `internal` | Puede consumir Output Ports. Opera sobre Domain Model |
| Application Service | `{{App}}.ApplicationServices` | `class` | `internal` | Orquesta el flujo del caso de uso. Implementa el Input Port. Traduce el DTO de entrada a datos de dominio e invoca el método fábrica de la entidad; no valida formato directamente. Puede consumir Output Ports y Domain Services |
| Model Validator | `{{App}}.DomainModels` | `class` | `internal` | Contiene la lógica de validación fuera de la entidad. Es invocado por el método fábrica de la entidad, que devuelve `Result` (la entidad o la lista de errores). Valida, por ejemplo, datos requeridos, valores válidos, invariantes de la entidad, etc. |
| Domain Service Validator | `{{App}}.DomainServices` | `class` | `internal` | Valida reglas de negocio que coordinan varias entidades o requieren datos externos. Por ejemplo, existencia de un producto, saldo de un cliente, etc. |
 
## Criterios de ubicación de código de validación
 
| Validación | Ubicación | Comentario |
|---|---|---|
| Formato/estructura de datos y reglas de una entidad / Value Object (requeridos, rangos, valores permitidos, p. ej. Nombre requerido, Precio > 0) | `{{App}}.DomainModels` | Se evalúan mediante una clase `Model Validator` invocada por el método fábrica de la entidad, que devuelve `Result` (la entidad o la lista de errores). |
| Reglas de un agregado. Por ejemplo, una orden debe tener al menos un producto solicitado. | `{{App}}.DomainModels` | Se evalúan con sus propios datos. |
| Reglas que coordinan varias entidades o agregados. Por ejemplo, entidades independientes que no son encapsuladas en un agregado. | `{{App}}.DomainServices` | Requieren datos de otras entidades o agregados. |
| Reglas que requieren datos externos a través de Output Ports. Por ejemplo, verificar la existencia de un producto del almacén. | `{{App}}.DomainServices` | Requieren de datos externos. |