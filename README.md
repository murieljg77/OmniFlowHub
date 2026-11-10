# OmniFlowHub API
Microservicio backend desarrollado en **.NET 9** utilizando los principios de **Clean Architecture**, diseñado para ser escalable, desacoplado y de alto rendimiento.

## Tecnologías y Herramientas:
* **Lenguaje & Framework:** C# / .NET 9
* **Arquitectura:** Clean Architecture (Domain, Application, Infrastructure, Api, Worker, UnitTests)
* **Persistencia de Datos:** MySQL con Dapper (`MySqlConnector`)
* **Almacenamiento:** Amazon Web Services S3 SDK (`AWSSDK.S3`)/ LocalStack
* **Testing:** xUnit y NSubstitute para pruebas unitarias
* **Documentación de API:** Scalar (`Scalar.AspNetCore`) con OpenAPI nativo de .NET 9

## Características Principales

* **Desacoplamiento:** Patrón Factory para la gestión transparente de conexiones a la base de datos MySQL.
* **Procesamiento en Segundo Plano (Background Worker):** Servicio resiliente en segundo plano para el procesamiento por lotes de documentos pendientes, con manejo robusto de excepciones individuales y soporte de `CancellationToken` para un apagado limpio.
* **Integración Cloud:** Servicio dedicado para el manejo e interacción con buckets en AWS S3 (compatible con LocalStack).
* **Pruebas Unitarias:** Pruebas con xUnit y mocks dinámicos mediante NSubstitute para validar la lógica del servicio de procesamiento y su comportamiento ante fallos.
* **Documentación Interactiva:** Interfaz moderna de testing de endpoints con Scalar integrada en el entorno de desarrollo.

## Configuración del Entorno Local:
1. Clonar el repositorio: `git clone https://github.com/murieljg77/OmniFlowHub.git`
2. Configurar las cadenas de conexión y credenciales de AWS S3 en el archivo `appsettings.Development.json` dentro de `OmniFlowHub.Api` y `OmniFlowHub.Worker`.
3. Ejecutar las pruebas unitarias para verificar el sistema: `dotnet test`
4. Ejecutar la API: `dotnet run --project OmniFlowHub.Api`
5. Acceder a la documentación en el navegador en la ruta `/scalar/v1`.