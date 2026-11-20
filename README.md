# CineNova — CRUD de películas y categorías

Proyecto académico de ASP.NET Core MVC con .NET 10, Entity Framework Core y PostgreSQL.

## Requisitos
- Visual Studio Community con la carga de trabajo **Desarrollo de ASP.NET y web**.
- .NET 10 SDK.
- PostgreSQL instalado y en ejecución.
- Una base de datos llamada `cinenova_db`.

## 1. Crear la base de datos
En pgAdmin o en la herramienta que uses para PostgreSQL, crea una base de datos con este nombre:

`cinenova_db`

## 2. Configurar la conexión
Abre `appsettings.json` y cambia `CAMBIA_ESTA_CLAVE` por la contraseña de tu usuario PostgreSQL.

Ejemplo:
`Host=localhost;Port=5432;Database=cinenova_db;Username=postgres;Password=TU_CONTRASEÑA`

No publiques contraseñas reales en GitHub. Para producción configura la conexión mediante variables de entorno o secretos del entorno de despliegue.

## 3. Restaurar paquetes
Abre la terminal en la carpeta del proyecto y ejecuta:

```powershell
dotnet restore
```

## 4. Instalar herramienta de migraciones (si no está instalada)
```powershell
dotnet tool install --global dotnet-ef --version 10.0.0
```

Si ya la tienes instalada:
```powershell
dotnet tool update --global dotnet-ef --version 10.0.0
```

## 5. Crear las tablas
```powershell
dotnet ef migrations add Inicial
dotnet ef database update
```

La migración crea las tablas `Categorias` y `Peliculas` y agrega categorías iniciales.

## 6. Ejecutar
```powershell
dotnet run
```

Abre la URL que aparece en la terminal.

## Funcionalidades
- Inicio con resumen del catálogo.
- CRUD de películas.
- CRUD de categorías.
- Búsqueda por título y filtro por categoría.
- Relación uno a muchos entre categoría y películas.
- Validaciones de datos.
- Protección antifalsificación en formularios POST.

## Estructura principal
- `Controllers`: acciones MVC.
- `Models`: entidades y validaciones.
- `Data`: contexto de Entity Framework Core.
- `Views`: páginas Razor.
- `wwwroot/css`: estilos.

## Nota sobre el contenido audiovisual
El sistema guarda enlaces de portadas y fuentes de video. Utiliza únicamente videos propios, de dominio público o con autorización/licencia para su reproducción. Este proyecto no incluye películas comerciales ni enlaces no autorizados.

## Antes de publicar
- Sustituye la contraseña local por una configuración segura.
- No subas credenciales a GitHub.
- Configura la cadena de conexión de forma segura en IIS/AWS/hosting.
- Comprueba que el proveedor de hosting soporte ASP.NET Core .NET 10 y PostgreSQL.
- Usa HTTPS en producción.
