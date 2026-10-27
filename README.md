# ApiInicial

API REST de productos y categorias con ASP.NET Core 10, Entity Framework Core y PostgreSQL.

## Requisitos

- .NET SDK 10
- PostgreSQL accesible en `127.0.0.1:55432`
- Base de datos `apiinicial_db`

## Configurar la conexion

La configuracion de `appsettings.json` no contiene contrasena. Guarda la cadena de conexion localmente con User Secrets:

```powershell
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:CatalogoConection" "Host=127.0.0.1;Port=55432;Database=apiinicial_db;Username=campo_logistica;Password=TU_CONTRASENA"
```

Tambien puedes definir la variable de entorno `ConnectionStrings__CatalogoConection`.

## Restaurar, migrar y ejecutar

Desde la carpeta del proyecto:

```powershell
dotnet restore
dotnet ef database update
dotnet run
```

Las migraciones existentes crean las tablas de categorias y productos. La API usa por defecto `http://localhost:5025` y `https://localhost:7080`.

## Endpoints

| Metodo | Ruta | Funcion |
| --- | --- | --- |
| GET | `/api/categorias` | Listar categorias |
| GET | `/api/categorias/{id}` | Obtener categoria |
| POST | `/api/categorias` | Crear categoria |
| GET | `/api/productos` | Listar productos |
| GET | `/api/productos/{id}` | Obtener producto |
| POST | `/api/productos` | Crear producto |
| PATCH | `/api/productos/{id}` | Actualizar solo los campos enviados |
| DELETE | `/api/productos/{id}` | Eliminar producto |

Las rutas anteriores `/api/v1/productos` se conservan como alias. Los cuerpos y ejemplos de solicitudes estan en `ApiInicial.http`. En PATCH se pueden enviar `stock: 0` y `estado: false`; los campos omitidos no cambian.

## Aplicacion Angular

La interfaz Angular 22 esta en `../frontend`. Para ejecutarla en desarrollo, inicia primero la API con `dotnet run` y despues, desde la carpeta `frontend`, ejecuta:

```powershell
npm install
npm start
```

Abre `http://localhost:4200`. La interfaz consume la API local en `https://localhost:7080/api`; si el certificado HTTPS de desarrollo no esta confiado, ejecuta `dotnet dev-certs https --trust`. La API permite solicitudes CORS desde `http://localhost:4200` para el desarrollo local.
