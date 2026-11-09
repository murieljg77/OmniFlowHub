# OmniFlowHub API
Microservicio backend desarrollado en **.NET 9** utilizando los principios de **Clean Architecture**, diseñado para ser escalable, desacoplado y de alto rendimiento.

## Tecnologías y Herramientas:
* **Lenguaje & Framework:** C# / .NET 9
* **Arquitectura:** Clean Architecture (Domain, Application, Infrastructure, Api, Worker)
* **Persistencia de Datos:** MySQL con Dapper (`MySqlConnector`)
* **Almacenamiento:** Amazon Web Services S3 SDK (`AWSSDK.S3`)
* **Documentación de API:** Scalar (`Scalar.AspNetCore`) con OpenAPI nativo de .NET 9

## Características Principales

* **Desacoplamiento:** Patrón Factory para la gestión transparente de conexiones a la base de datos MySQL.
* **Integración Cloud:** Servicio dedicado para el manejo e interacción con buckets en AWS S3.
* **Documentación Interactiva:** Interfaz moderna de testing de endpoints con Scalar integrada en el entorno de desarrollo.
* **Procesamiento en Segundo Plano:** Proyecto Worker desacoplado listo para consumir tareas y eventos de fondo.

## Configuración del Entorno Local:
1. Clonar el repositorio: `git clone https://github.com/murieljg77/OmniFlowHub.git`
2. Configurar las cadenas de conexión y credenciales de AWS S3 en el archivo `appsettings.Development.json` dentro de `OmniFlowHub.Api`.
3. Ejecutar la API: `dotnet run --project OmniFlowHub.Api`
4. Acceder a la documentación en el navegador en la ruta `/scalar/v1`.