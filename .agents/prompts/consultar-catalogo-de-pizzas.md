---
name: Consultar catálogo de pizzas
version: 1.0
---
 
# Objetivo
 
Extiende la aplicación de consola en C# con .NET existente para permitir consultar las pizzas registradas en el catálogo de la pizzería.
 
## Contexto
 
La aplicación ya implementa la funcionalidad para registrar pizzas y persistirlas en un archivo JSON local.
Antes de realizar cambios, analiza la estructura de la solución y el código existente para comprender cómo está organizada la aplicación.
 
## Funcionalidad
 
Implementa únicamente la funcionalidad necesaria para consultar las pizzas existentes en el catálogo.
La aplicación debe mostrar un menú que permita seleccionar entre las siguientes opciones:
 
1. Registrar una pizza
2. Consultar las pizzas registradas.
3. Salir.
 
Después de ejecutar una operación, la aplicación debe volver a mostrar el menú hasta que el usuario seleccione la opción para salir.
 
La aplicación debe permitir mostrar las pizzas registradas, incluyendo para cada pizza:
 
- Identificador
- Nombre
- Descripción
- Precio
- Tamaño
 
Utiliza la información que ya se encuentra almacenada en el archivo JSON local.
Si no existen pizzas registradas, la aplicación debe mostrar un mensaje indicando que el catálogo está vacío.
 
## Restricciones
 
No modifiques ni elimines la funcionalidad existente para registrar pizzas.
No implementes funcionalidades para odificar o eliminar pizzas.
No elimines ni remplaces los dtos existentes en el archivo JSON.
Conserva la estructura y organización existentes de la solución, salvo que sea necesario realizar algún cambio para implementar la funcionalidad solicitada.
 
## Organización del código
 
Integra la nueva funcionalidad respetando la separación existente entre la interacción con el usuario, la lógica de negocio, la persistencia y las entidades.
 
Reutiliza las clases y servicios existentes cuando sea apropiado.
 
Evita duplicar lógica que ya exista en la aplicación.
 
No concentres la nueva funcionalidad en `Program.cs`.
 
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
 
Utiliza el proyecto `HotPizza.tests` existente.
 
Agrega pruebas unitarias para verificar el comportamiento de la funcionalidad de consulta.
 
Conserva las pruebas existentes.
 
No modifiques las pruebas existentes para ocultar o evitar errores.
 
## Verificación
 
Al finalizar:
 
1. Compila la solución.
2. Ejecuta todas las pruebas automatizadas.
3. Ejecuta la aplicación de consola.
4. Verifica que sea posible registrar correctamente una pizza.
5. Consulta el catálogo y verifica que la pizza registrada se muestre correctamente.
6. Verifica que la información mostrada incluya el identificador, nombre, descripción, precio y tamaño de pizza.
 
No agregues funcionalidades que no hayan sido solicitadas.