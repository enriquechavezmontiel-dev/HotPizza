---
name: Mejorar pruebas del catálogo de pizzas
version: 1.0
---
 
# Objetivo
Extiende las pruebas automatizadas existentes de la aplicación de consola en C# con .NET para verificar el comportamiento de la funcionalidades implementadas hasta el momento.
 
## Contextos
 
La aplicación ya implementa la funcionalidad para registrar y consultar pizzas, validar los datos al registrar una pizza, así como la persistencia de las pizzas  en un archivo JSON local.
 
El proyecto `HotPizza.tests` ya contiene pruebas automatizadas.
 
Antes de realizar cambios, analiza la estructura de la solución, la implementación de la aplicación y las pruebas existentes para comprender cómo están implementadas las funcionalidades actuales.
 
## Funcionalidad
 
Agrega las pruebas automatizadas necesarias para verificar las funcionalidades implementadas hasta este momento.
 
- El registro correcto de una pizza.
- La consulta de las pizzas registradas.
- El rechazo de nombres vacíos o que contemplen únicamente espacios.
- El rechazo de precios menores o iguales a cero.
- El rechazo de tamaños distintos de 20, 30 o 40 centimetros.
- La persistencia de las pizzas registradas.
 
Verifica tanto los casos válidos como los casos no válidos cuando corresponda.
 
Antes de agregar una prueba, revisa si ya existe una prueba que cubra el mismo comportamiento. Evita crear pruebas duplicadas.
 
## Restricciones
 
No modifiques las funcionalidades de la aplicación para hacer que las pruebas pasen.
 
No elimines ni reemplaces las pruebas existentes.
 
No modifiques las pruebas existentes para ocultar o evitar errores.
 
Si encuentras un comportamiento incorrecto en la aplicación, no lo corrijas como parte de esta tarea. Infoma el problema y especifica qué prueba permite detectarlo.
 
Conserva la estructura y organización existentes de la solución, salvo que sea necesario realizar algún cambio para implementar las pruebas solicitadas.
 
## Organización del código
 
Integra las nuevas pruebas respetando la organización existente del proyecto `HotPizza.tests`.
 
Reutiliza las clase auxiliares, configuraciones y mecanismos de prueba existentes cuando sea apropiado.
 
Evita duplicar código de pruebas que ya exista.
 
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
 
## Verificación
 
Al finalizar:
 
1. Compila la solución.
2. Ejecuta todas las pruebas automatizadas.
3. Verifica que todas las pruebas existentes continúen ejecutandose correctamente.
4. Verifica que se hayan agregado pruebas para los comportamientos solicitados.
5. Si alguna prueba falla debido a un comportamiento incorrecto de la aplicación, no modifiques la aplicación para hacerla pasar. Informa del problema encontrado y de la prueba que lo detecta.
6. Informa cuál es el porcentaje de combertura de la pruebas.
 
No agregues funcionalidades que no hayan sido solicitadas.