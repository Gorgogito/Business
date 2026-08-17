# Business ERP

Sistema ERP empresarial desarrollado con .NET 10, ASP.NET Core, Blazor y SQL Server.

## Arquitectura

```
Business/
├── src/
│   ├── Backend/
│   │   ├── Business.Domain          # Entidades, interfaces
│   │   ├── Business.Application     # DTOs, servicios, validaciones
│   │   ├── Business.Infrastructure  # JWT, autenticación
│   │   ├── Business.Persistence     # DbContext, repositorios, migraciones
│   │   └── Business.API             # Controladores REST
│   ├── Frontend/
│   │   └── Business.Web             # Blazor Server + MudBlazor
│   └── Tests/
│       └── Business.Tests           # Tests unitarios xUnit
```

## Tecnologías

- **.NET 10** / **ASP.NET Core 10**
- **Entity Framework Core 10** + SQL Server
- **JWT Authentication** (Swashbuckle 6.9)
- **FluentValidation 12**
- **BCrypt.Net** para hash de contraseñas
- **Blazor Server** + **MudBlazor 9.5**
- **xUnit** + **Moq** + **FluentAssertions**

## Módulos

| Módulo | Funcionalidades |
|--------|----------------|
| Seguridad | Usuarios, Roles, Permisos, Menús dinámicos, JWT |
| Configuración | Empresas, Sucursales, Parámetros |
| Maestros | Clientes, Proveedores, Productos, Categorías |
| Inventario | Almacenes, Stock, Movimientos |
| Ventas | Cotizaciones, Pedidos, Facturas |
| Compras | Órdenes de Compra, Recepciones |
| Dashboard | KPIs y métricas en tiempo real |

## Configuración

Los secretos (cadena de conexión y clave JWT) **no se versionan**: se cargan desde
*user-secrets* (en desarrollo) o variables de entorno (en producción). `appsettings.json`
solo contiene la estructura con valores vacíos.

### Configurar secretos (desarrollo)
Desde `src/Backend/Business.API`:
```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Data Source=SERVIDOR;Initial Catalog=BusinessERP;User ID=usuario;Password=clave;Encrypt=True;TrustServerCertificate=True"
dotnet user-secrets set "JwtSettings:Secret" "<clave-aleatoria-de-al-menos-32-caracteres>"
```
Ver los secretos configurados: `dotnet user-secrets list`.

### Producción
Definir los mismos valores como variables de entorno:
```
ConnectionStrings__DefaultConnection=...
JwtSettings__Secret=...
```

> Seguridad: la clave JWT anterior y la contraseña de `sa` estaban expuestas en el
> repositorio. La clave JWT ya fue rotada; se recomienda **rotar también la contraseña
> de la base de datos** y usar una cuenta con permisos mínimos en lugar de `sa`.

Resto de parámetros JWT (no sensibles) en `appsettings.json`:
```json
"JwtSettings": { "Issuer": "BusinessERP", "Audience": "BusinessERP-Client", "ExpiryMinutes": "60" }
```

## Ejecución

### Backend (API)
```bash
cd src/Backend/Business.API
dotnet run
```
La API se ejecuta en `http://localhost:5000`
Swagger UI: `http://localhost:5000` (raíz)

### Frontend
```bash
cd src/Frontend/Business.Web
dotnet run
```
La aplicación se ejecuta en `http://localhost:5037`

## Migraciones

La migración inicial ya está creada. La base de datos se crea automáticamente al iniciar la API.

Para crear una nueva migración:
```bash
dotnet ef migrations add NombreMigracion \
  --project src/Backend/Business.Persistence \
  --startup-project src/Backend/Business.API
```

## Tests

```bash
dotnet test src/Tests/Business.Tests
```

## Credenciales iniciales

| Campo | Valor |
|-------|-------|
| Usuario | admin |
| Contraseña | Admin123! |

## API Endpoints

### Autenticación
- `POST /api/auth/login` - Iniciar sesión
- `POST /api/auth/refresh` - Renovar token
- `POST /api/auth/logout` - Cerrar sesión

### Módulos (requieren Bearer token)
- `GET/POST/PUT/DELETE /api/clientes`
- `GET/POST/PUT/DELETE /api/proveedores`
- `GET/POST/PUT/DELETE /api/productos`
- `GET/POST/PUT/DELETE /api/categorias`
- `GET/POST/PUT/DELETE /api/almacenes`
- `GET/POST /api/stock/movimientos`
- `GET/POST /api/cotizaciones`
- `GET/POST /api/pedidos`
- `GET/POST /api/facturas`
- `GET/POST /api/ordenes-compra`
- `GET/POST /api/recepciones`
- `GET /api/dashboard`
