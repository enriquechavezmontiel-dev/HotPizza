# Guía para el registro de dependencias en una aplicación con Onion Architecture
 
Este documento presenta una guía para el registro de dependencias al implementar una aplicación bajo `Onion Architecture`.
 
- El proyecto `{{AppName}}.IoC` es el lugar donde se crean los métodos de extensión de `IServiceCollection` para facilitar el registro de dependencias.

- Los proyectos del núcleo (`DomainModels`, `DomainServices`, `ApplicationServices`, etc.) ya exponen sus miembros `internal` al proyecto `{{AppName}}.IoC`. Por ello, `IoC` puede registrar implementaciones declaradas como `internal` sin necesidad de hacerlas `public` ni de modificar los archivos de proyecto.
 
## Registro de servicios
 
Los servicios se registran en métodos de extensión de `IServiceCollection` dentro de archivos llamados `DependencyContainer.cs`. Cada archivo se ubica en un subdirectorio **dentro del proyecto `{{AppName}}.IoC`** cuyo nombre corresponde al proyecto que contiene los servicios a registrar, de acuerdo a la siguiente tabla:
 
| Proyecto | Directorio (dentro de `{{AppName}}.IoC`) | Método |
|----------|------------|--------|
| {{AppName}}.DomainModels | DomainModels | AddDomainModelsServices |
| {{AppName}}.DomainServices | DomainServices | AddDomainServices |
| {{AppName}}.ApplicationServices | ApplicationServices | AddApplicationServices |
 
- Cada `AddXxxServices` registra **todos** los servicios de su proyecto (no se crea un método por caso de uso). Así, un servicio compartido por varios casos de uso se registra una sola vez.

- Dependiendo de otros servicios de infraestructura que sean implementados, podrán existir directorios adicionales.

- Las clases `DependencyContainer` ubicadas en los subdirectorios (por proyecto) deberán declararse como **`internal`** para no exponer sus métodos al exterior.

- Cada archivo `DependencyContainer.cs` de un subdirectorio usa el **namespace correspondiente a su directorio** (por ejemplo, `{{AppName}}.IoC.DomainServices`), de modo que aunque las clases se llamen igual (`DependencyContainer`) no colisionan.
 
Al registrar los servicios:

- Deberá verificarse la existencia del archivo `DependencyContainer.cs` en el directorio correspondiente. Si no existe, deberá crearse como una copia de `.agents/templates/DependencyContainer.cs` en el directorio correspondiente, ajustando el namespace, el nombre de la clase y su visibilidad.

- Deberá verificarse que existe el método de extensión de `IServiceCollection` requerido; si no existe, deberá ser creado.

- Los servicios deberán ser registrados con ciclo de vida `Scoped` a no ser que la especificación indique lo contrario.
 
### Registro centralizado

Deberá crearse un archivo `DependencyContainer.cs` ubicado en la **raíz** del proyecto `{{AppName}}.IoC` con una clase declarada como **`public`** (para que el Host pueda invocarla) que tenga un método de extensión llamado `Add{{AppName}}Services` que compone el registro de la aplicación.
 
- `Add{{AppName}}Services` debe invocar **únicamente los métodos que ya existan** (`AddDomainModelsServices`, `AddDomainServices`, `AddApplicationServices`, etc.). La invocación a un método se agrega **en el momento en que dicho método de extensión se define por primera vez** para su proyecto, no de antemano. Así se evita invocar métodos inexistentes que no compilarían.

- La clase de la raíz usa el namespace del proyecto (`{{AppName}}.IoC`), distinto al de los subdirectorios.

- Dependiendo de otros servicios de infraestructura que sean implementados, podrán existir llamadas a métodos adicionales, agregadas también la primera vez que se defina el método correspondiente.

 


 