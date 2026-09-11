<#
.SYNOPSIS
	Genera una solución con arquitectura hexagonal / puertos y adaptadores para el AppName indicado.
 
.DESCRIPTION
    Crea la estructura de directorios y proyectos de una arquitectura de dependencias
    hacia el dominio (compatible con Hexagonal / Onion / Clean Architecture):
    DomainModels en el núcleo (sin dependencias), puertos de entrada/salida
    (InputPorts/OutputPorts), capas de servicios (DomainServices/ApplicationServices),
    el composition root (IoC) y un proyecto de pruebas xUnit con NSubstitute.
 
.PARAMETER AppName
	Nombre de la aplicación / solución a crear. Se usa como prefijo de todos
	los proyectos y como nombre de la carpeta raíz.
 
.EXAMPLE
	.\New-CleanArchApp.ps1 -AppName ColdBurger
#>
[CmdletBinding()]
param(
	[Parameter(Mandatory = $true)]
	[ValidateNotNullOrEmpty()]
	[string]$AppName
)
 
$ErrorActionPreference = 'Stop'
 
function Write-Step {
	param([string]$Message)
	Write-Host "[STEP] $Message"
}
 
# --------------------------------------------------------------------------
# 0. Validaciones
# --------------------------------------------------------------------------
$rootDir = Join-Path (Get-Location) $AppName
 
Write-Step "Verificando que el directorio '$AppName' no exista..."
if (Test-Path -LiteralPath $rootDir) {
	Write-Error "El directorio '$rootDir' ya existe. Abortando la ejecución."
	exit 1
}
 
$srcDir   = Join-Path $rootDir 'src'
$testsDir = Join-Path $rootDir 'tests'
 
# --------------------------------------------------------------------------
# 1. Estructura de directorios
# --------------------------------------------------------------------------
Write-Step "Creando la estructura de directorios base..."
$null = New-Item -ItemType Directory -Path $rootDir
$null = New-Item -ItemType Directory -Path $srcDir
$null = New-Item -ItemType Directory -Path $testsDir
Write-Host "        Creado: $rootDir"
Write-Host "        Creado: $srcDir"
Write-Host "        Creado: $testsDir"
 
# --------------------------------------------------------------------------
# 2. Definición de proyectos
# --------------------------------------------------------------------------
$libProjects = @(
	'DomainModels'
	'InputPorts'
	'OutputPorts'
	'DomainServices'
	'ApplicationServices'
	'IoC'
)
 
$targetFramework = 'net10.0'
 
# Plantilla base de Class Library
function New-ClassLibraryContent {
	param(
		[string[]]$ProjectReferences,
		[string[]]$InternalsVisibleTo
	)
 
	$sb = [System.Text.StringBuilder]::new()
	[void]$sb.AppendLine('<Project Sdk="Microsoft.NET.Sdk">')
	[void]$sb.AppendLine('')
	[void]$sb.AppendLine('  <PropertyGroup>')
	[void]$sb.AppendLine("    <TargetFramework>$targetFramework</TargetFramework>")
	[void]$sb.AppendLine('    <ImplicitUsings>enable</ImplicitUsings>')
	[void]$sb.AppendLine('    <Nullable>enable</Nullable>')
	[void]$sb.AppendLine('  </PropertyGroup>')
 
	if ($ProjectReferences -and $ProjectReferences.Count -gt 0) {
		[void]$sb.AppendLine('')
		[void]$sb.AppendLine('  <ItemGroup>')
		foreach ($ref in $ProjectReferences) {
			[void]$sb.AppendLine("    <ProjectReference Include=`"..\$AppName.$ref\$AppName.$ref.csproj`" />")
		}
		[void]$sb.AppendLine('  </ItemGroup>')
	}
 
	if ($InternalsVisibleTo -and $InternalsVisibleTo.Count -gt 0) {
		[void]$sb.AppendLine('')
		[void]$sb.AppendLine('  <ItemGroup>')
		foreach ($ivt in $InternalsVisibleTo) {
			[void]$sb.AppendLine("    <InternalsVisibleTo Include=`"$AppName.$ivt`" />")
		}
		[void]$sb.AppendLine('  </ItemGroup>')
	}
 
	[void]$sb.AppendLine('')
	[void]$sb.AppendLine('</Project>')
	return $sb.ToString()
}
 
# Referencias entre proyectos (por nombre corto)
$projectReferences = @{
	'DomainModels'        = @()
	'InputPorts'          = @('DomainModels')
	'OutputPorts'         = @('DomainModels')
	'DomainServices'      = @('OutputPorts')
	'ApplicationServices' = @('DomainServices', 'InputPorts')
	'IoC'                 = @('ApplicationServices')
}
 
# InternalsVisibleTo por proyecto
$internalsVisibleTo = @{
	'ApplicationServices' = @('IoC', 'Tests')
	'DomainServices'      = @('IoC', 'Tests')
}
 
# --------------------------------------------------------------------------
# 3. Creación de los Class Libraries
# --------------------------------------------------------------------------
foreach ($proj in $libProjects) {
	$projName = "$AppName.$proj"
	$projDir  = Join-Path $srcDir $projName
	$projFile = Join-Path $projDir "$projName.csproj"
 
	Write-Step "Creando proyecto Class Library: $projName"
	$null = New-Item -ItemType Directory -Path $projDir
 
	$content = New-ClassLibraryContent `
		-ProjectReferences $projectReferences[$proj] `
		-InternalsVisibleTo $internalsVisibleTo[$proj]
 
	Set-Content -LiteralPath $projFile -Value $content -Encoding UTF8
	Write-Host "        Creado: $projFile"
 
	if ($projectReferences[$proj].Count -gt 0) {
		Write-Host "        Referencias -> $($projectReferences[$proj] -join ', ')"
	}
	if ($internalsVisibleTo[$proj]) {
		Write-Host "        InternalsVisibleTo -> $($internalsVisibleTo[$proj] -join ', ')"
	}
}
 
# --------------------------------------------------------------------------
# 4. Creación del proyecto de Tests (xUnit + NSubstitute)
# --------------------------------------------------------------------------
$testProjName = "$AppName.Tests"
$testProjDir  = Join-Path $testsDir $testProjName
$testProjFile = Join-Path $testProjDir "$testProjName.csproj"
 
Write-Step "Creando proyecto de pruebas xUnit: $testProjName"
$null = New-Item -ItemType Directory -Path $testProjDir
 
$testContent = @"
<Project Sdk="Microsoft.NET.Sdk">
 
  <PropertyGroup>
<TargetFramework>$targetFramework</TargetFramework>
<ImplicitUsings>enable</ImplicitUsings>
<Nullable>enable</Nullable>
<IsPackable>false</IsPackable>
</PropertyGroup>
 
  <ItemGroup>
<PackageReference Include="coverlet.collector" Version="10.0.1">
<PrivateAssets>all</PrivateAssets>
<IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
</PackageReference>
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="18.9.0" />
<PackageReference Include="NSubstitute" Version="6.2.0" />
<PackageReference Include="xunit.runner.visualstudio" Version="4.0.0">
<PrivateAssets>all</PrivateAssets>
<IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
</PackageReference>
<PackageReference Include="xunit.v3" Version="4.0.0" />
</ItemGroup>
 
  <ItemGroup>
<ProjectReference Include="..\..\src\$AppName.IoC\$AppName.IoC.csproj" />
</ItemGroup>
 
  <ItemGroup>
<Using Include="Xunit" />
</ItemGroup>
 
</Project>
"@
 
Set-Content -LiteralPath $testProjFile -Value $testContent -Encoding UTF8
Write-Host "        Creado: $testProjFile"
Write-Host "        Referencias -> $AppName.IoC"
 
# --------------------------------------------------------------------------
# 5. Creación del archivo de solución .slnx
# --------------------------------------------------------------------------
$slnxFile = Join-Path $srcDir "$AppName.slnx"
Write-Step "Creando el archivo de solución: $AppName.slnx"
 
$slnxContent = @"
<Solution>
<Folder Name="/Application/" />
<Folder Name="/Application/Application Services/">
<Project Path="$AppName.ApplicationServices/$AppName.ApplicationServices.csproj" />
<Project Path="$AppName.InputPorts/$AppName.InputPorts.csproj" />
</Folder>
<Folder Name="/Application/Domain Models/">
<Project Path="$AppName.DomainModels/$AppName.DomainModels.csproj" />
</Folder>
<Folder Name="/Application/Domain Services/">
<Project Path="$AppName.DomainServices/$AppName.DomainServices.csproj" />
<Project Path="$AppName.OutputPorts/$AppName.OutputPorts.csproj" />
</Folder>
<Folder Name="/Host/">
<Project Path="$AppName.IoC/$AppName.IoC.csproj" />
</Folder>
<Folder Name="/Infrastructure/" />
<Folder Name="/Infrastructure/Adapters/" />
<Folder Name="/Infrastructure/Adapters/Input/" />
<Folder Name="/Infrastructure/Adapters/Output/" />
<Folder Name="/Infrastructure/Libs/" />
<Folder Name="/Tests/">
<Project Path="../tests/$AppName.Tests/$AppName.Tests.csproj" />
</Folder>
<Folder Name="/UI/" />
</Solution>
"@
 
Set-Content -LiteralPath $slnxFile -Value $slnxContent -Encoding UTF8
Write-Host "        Creado: $slnxFile"
Write-Host "        Carpetas de solución vacías: Infrastructure, UI"
 
# --------------------------------------------------------------------------
# 6. Resumen
# --------------------------------------------------------------------------
Write-Step "Estructura de la solución '$AppName' generada correctamente."
Write-Host "        Raíz: $rootDir"
Write-Host "        Solución: $slnxFile"