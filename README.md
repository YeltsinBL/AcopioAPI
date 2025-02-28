# AcopioAPI

APIs para el sistema web Acopio

## Package NuGet

- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- System.Data.SqlClient
- Dapper
- Microsoft.AspNetCore.Authentication.JwtBearer
- CloudinaryDotNet

## MODEL

Uso de Scaffold-DbContext Command para la creación del Modelo desde una base de datos existente

Abrir la Consola de Administrador de Paquetes Nuget y copiar el siguiente comando:

```sh
Scaffold-DbContext "Data Source=[SERVER_NAME];Initial Catalog=[DATABASE_NAME]; Integrated Security=True; Trusted_Connection=True; TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
```

> Nota: Si ya se había hecho el Scaffold-DbContext y luego se actualizó la Base de datos, agregar al final del comando anterior `-force` para actualizar el fichero creado.
