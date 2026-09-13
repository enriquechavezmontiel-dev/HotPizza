# Caso de uso: Add Pizza (Registrar pizza)
 
## Historia de usuario
Como administrador de la empresa, deseo poder registrar los datos de pizzas para construir el catálogo de la empresa.
 
## Input Port y DTOs
 
### DTOs de entrada
 
#### `AddPizzaRequest`
 
| Nombre | Tipo | Restricciones | Descripción |
|--------|------|---------------|-------------|
| Name   | string | Requerido, no vacío | Nombre de la pizza |
| Description | string | Requerido, no vacío | Descripción de la pizza |
| Price | decimal | Mayor que cero | Precio de la pizza. |
| Size | int | Valores permitidos: 20, 30 y 40 | Tamaño de la pizza. |
 
### DTOs de Salida
 
#### `AddPizzaResult`
 
| Nombre | Tipo | Descripción |
|--------|------|---------------|-------------|
| Id   | int |  Identificador de la pizza asignado por el medio de persistencia |
 
### Input Port `IAddPizzaInputPort`
 
| Método | Datos de entrada | Datos de salida |
|--------|------------------|-----------------|
| AddPizzaAsync | AddPizzaRequest | AddPizzaResult |
 
- **Comportamiento con exito:** se devuelve el identificador asignado a la pizza a través de AddPizzaResult
- **Comportamiento en error:** se devuelven los errores encontrados de la operación.
 
## Domain Models
 
### Entidad `Pizza`
 
| Nombre | Tipo | Dato de entrada | Comentario |
|--------|------|---------------|-------------|
| Id   | int | | Identificador asignado por el medio de persistencia |
| Name   | string | Name | Sigue las reglas de validación de los datos de entrada |
| Description | string | Description | Sigue las reglas de validación de los datos de entrada  |
| Price | decimal | Price | Sigue las reglas de validación de los datos de entrada  |
| Size | int | Size | Sigue las reglas de validación de los datos de entrada  |
 
## Output Ports
 
### Repositorio `IAddPizzaRepository`
 
| Método | Datos de entrada | Datos de salida |
|--------|------------------|-----------------|
| AddPizzaAsync | Pizza | int |
 
- **Comportamiento con exito:** se devuelve el identificador asignado a la pizza.
- **Comportamiento en error:** se devuelven los errores encontrados de la operación.
 
 
## Flujo del caso de uso
 
### Camino feliz
 
1. El sistema recibe la solicitud con los datos de entrada.
2. El sistema crea un instancia de Pizza con datos válidos.
3. El sistema usa el repositorio para persistir los datos de la pizza.
4. El sistema devuelve el identificador de la pizza.
 
### Flujo alterno: Datos de entrada no válidos
1. Al intentar crear la instancia de Pizza, la validación falla y se devuelve un resultado con la lista de errores.
2. Los datos no son persistidos.
3. El proceso finaliza.
 
### Flujo alterno: Error al persistir los datos
1. Devolver el resultado de error de persistencia.
2. Los datos no son persistidos.
3. El proceso finaliza.
 
## Criterios de aceptación / pruebas
 
1. **Exito:** dado un `AddPizzaRequest` válido, el servicio registra la pizza y devuelve un `AddPizzaResult` con un `Id` asignado.
2. **Error de validación:** el servicio no registra y devuelve la lista de errores.
 
## Fuera de alcance
 
- Consultar, listar, modificar o eliminar pizzas.
- Cualquier funcionalidad no descrita en este documento.