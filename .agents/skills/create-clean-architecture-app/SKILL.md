---
name: create-clean-architecture-app
description: Crea la estructura base de una nueva aplicación utilizando el script New-CleanArchApp.ps1. Utiliza este SKILL cuando sea necesario crear una nueva aplicación y su estructura inicial implementando clean architecture Hexagonal u Onion.
---

# Crear aplicación con Clean Architecture
 
## Alcance
 
Este Skill se limita a solicitar al script `New-CleanArchApp.ps1` la creación de la estructura inicial de una aplicación y comunicar el resultado de la operación.
 
La creación y validación de la estructura generada son responsabilidad del script. El Skill no debe realizar verificaciones adicionales sobre los archivos, directorios, proyectos o referencias generados.
 
La implementación de funcionalidades, casos de uso, reglas de negocio y pruebas queda fuera del alcance de este Skill.
 
## Restricciones
 
- NO ejecutes comandos ni herramientas de inspección (`Test-Path`, `list_dir`, `view_file`, etc.) antes o después de ejecutar el script.
- Si el usuario solicita un plan, el plan debe limitarse exclusivamente a los pasos de este procedimiento, sin
  añadir fases de validación previa o posterior.
 
## Procedimiento
 
Cuando sea necesario crear una nueva aplicación implementando Clean Architecture, Hexagonal u Onion, utiliza el script `New-CleanArchApp.ps1` para generar su estructura base.
 
Sigue estos pasos:
 
1. Determina el nombre de la aplicación a partir de la solicitud del usuario.
2. Ejecuta el script `New-CleanArchApp.ps1` utilizando el nombre de la aplicación proporcionado por el usuario:
 
   ```powershell
   .\.agents\scripts\New-CleanArchApp.ps1 -AppName "<nombre-de-la-aplicación>"
   ```
 
3. Permite que el script cree la solución, los proyectos, los directorios y los archivos iniciales definidos por la arquitectura del proyecto.
4. Determina si la operación fue exitosa basándote EXCLUSIVAMENTE en el código de salida y la consola del script, sin inspeccionar el sistema de archivos.
5. Informa al usuario del resultado, incluyendo la ubicación de la aplicación generada cuando la operación haya sido exitosa.
 
## Resultado
 
* Si el script finaliza correctamente, informa al usuario que la aplicación fue creada correctamente e indica su ubicación.
* Si el script informa un error o no puede completar la operación, comunica el problema al usuario.