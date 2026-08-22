---
name: Crear catálogo de pizzas
Description: Este prompt contiene indicaciones para la creación del catálogo de pizzas
version: 1.0
---
 
# Objetivo
 
Desarrolla una aplicación de consola en C# con .NET 8.0 para administrar el catálogo de una pizzeria.
 
## Funcionalidad
 
Implementa únicamente la funcionalidad para registrar nueva pizzas.
 
Una pizza debe tener los siguientes datos:
 
- Nombre
- Descripción
- Precio
- Tamaño, expresado en centímetros.
 
Al registrar una pizza correctamente, la aplicación debe mostrar el identificador asignado a la pizza.
 
Si ocurre un error durante el registro, la aplicación debe mostrar un mensaje indicando el error.
 
Los datos deben persistirse en un archivo JSON local.
 
No implementes funcionalidades para consultar, modificar o eliminar pizzas.
 
## Estructura de la solución
 
El directorio actual `HotPizza` debe ser el directorio raíz de la solución.
 
Crea una solución llamada `HotPizza` utilizando el formato de solución `.slnx`.
 
Dentro del directorio raíz crea un proyecto de consola llamado `HotPizza`.
 
La estructura debe quedar de la siguiente manera:
 
```text
Hotpizza/
|-- Hotpizza.slnx
|-- HotPizza/
|   |-- HotPizza.csproj
|   |-- Program.cs
|   |-- ...
|-- HotPizza.Tests/
    |-- HotPizza.Tests.csproj
    |-- ...
```
 
El proyecto `HotPizza` y el proyecto `HotPizza.Tests` deben estar incluidos en `Hotpizza.slnx`
 
 
## Organización del código
 
No concentres toda la implementación en `Program.cs`.
 
`Program.cs` debe contener únicamente el código necesario para iniciar la aplicación y coordinar la interacción con el usuario.
 
Separa la lógica de negocio, la persistencia y las entidades en clases y archivos independientes.
 
Utiliza una estructura de directorios clara dentro del proyecto `HotPizza`.
 
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
 
Utiliza xUnit
Crea el proyecto `HotPizza.Tests` para las pruebas automatizadas.

 
Agrega pruebas unitarias para verificar el comportamiento de la funcionalidad de registro de pizzas.
 
## Verificación
 
Al finalizar:
 
1. Compila la solución.
2. Ejecuta las pruebas automatizadas.
3. Ejecuta la aplicación de consola.
4. Verifica que sea posible registrar correctamente una pizza.
 
No agregues funcionalidades que no hayan sido solicitadas.