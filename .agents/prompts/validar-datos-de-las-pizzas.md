---
name: Validar datos de las pizzas
version: 1.0
---
 
# Objetivo
 
Extiende la aplicación de consola en C# con .NET existente para validar los datos de las pizzas al momento de registrarlas.
 
## Contexto
 
La aplicación ya implementa la funcionalidad para registrar y consultar pizzas, así como la persistencia de las pizzas  en un archivo JSON local.
Antes de realizar cambios, analiza la estructura de la solución y el código existente para comprender cómo están implementadas las funcionalidades actuales.
 
## Funcionalidad
 
Agrega validaciones a la funcionalidad existente para registrar una pizza.
 
Los datos de una pizza deben cumplir las siguientes reglas:
 
| Dato   | Regla |
|--------|--------|
| Nombre | No puede estar vacío no contener únicamente espacios. |
| Precio | Debe ser mayor que cero |
| Tamaño | Debe ser de 20, 30 o 40 centimetros. |
 
Cuando todos los datos sean válidos, la pizza debe registrarse normalmente.
 
Cuando alguno de los datos no cumpla con su regla de validación:
 
- La aplicación debe informar al usuario cuál es el error.
- La pizza no debe registrarse.
- Los datos no válidos no deben persistirse en el archivo JSON.
 
Las validaciones deben aplicarse al registrar una pizza antes de persistirla.
 
## Restricciones
 
No modifiques ni elimines la funcionalidad existente para registrar pizzas, excepto para incorporar las validaciones solicitadas.
 
No modifiques ni elimines la funcionalidad existente para consultar pizzas.
 
No implementes funcionalidades para modificar o eliminar pizzas.
 
No elimines ni remplaces los datos existentes en el archivo JSON.
 
Conserva la estructura y organización existentes de la solución, salvo que sea necesario realizar algún cambio para implementar la funcionalidad solicitada.
 
## Organización del código
 
Integra las validaciones respetando la separación existente entre la interacción con el usuario, la lógica de negocio, la persistencia y las entidades.
 
Reutiliza las clases y servicios existentes cuando sea apropiado.
 
Evita duplicar lógica que ya existe en la aplicación.
 
NO concentres las validaciones en `Program.cs` si la estructura existente permite ubicarlas en una capa o clase apropiada.
 
## Convenciones de código
 
Utiliza inglés para todos los identificadores del código fuente, incluyendo:
 
- nombres de clases
- nombres de interfaces
- nombres de métodos
- nombres de propiedades
- nombres de variables
- nombres de parámetros
- nombres de archivos
 
Los mensajes mostrados al usuario durante la ejecución de la aplicación deben estar escritos en español.
 
## Pruebas
 
Utiliza el proyecto `HotPizza.Tests` existente.
 
Agrega pruebas unitarias para verificar cada una de las reglas de validación.
 
Las pruebas deben comprobar como mínimo:
 
- que se rechace un nombre vacío
- que se rechace un nombre que contenga únicamente espacios
- que se rechace un precio igual a cero
- que se rechace un precio menor que cero
- que se rechace un tamaño distinto de 20, 30 o 40 centimetros
- que se registre correctamente una pizza con datos válidos
 
Conserva pruebas existentes.
No modifiques las pruebas existentes para ocultar o evitar errores.
 
## Verificación
 
Al finalizar:
 
1. Compila la solución.
2. Ejecuta todas las pruebas automatizadas.
3. Ejecuta la aplicación de consola.
4. Intenta registrar pizzas con datos no válidos y verifica que sean rechazadas.
5. Intenta registrar una pizza con datos válidos y verifica que sea registrada correctamente.
6. Consulta el catálogo y verifica que las pizzas rechazadas no hayan sido registradas.
7. Consulta el catálogo y verifica que la pizza registrada se muestre correctamente.
8. Verifica que las funcionalidades existentes de registro y consulta continúen funcionando correctamente.
 
No agregues funcionalidades que no hayan sido solicitadas.