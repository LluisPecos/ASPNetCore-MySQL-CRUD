Este proyecto es una API REST desarrollada en .NET Core con Entity Framework para operaciones CRUD básicas.

### Tecnologías utilizadas
- .NET Core
- Entity Framework Core
- MySQL
- Swagger

### Instalación y Configuración
**1. Clonar el repositorio**
```sh
git clone https://github.com/LluisPecos/ASPNetCore-MySQL-CRUD
```

**2. Restaurar las dependencias**
```sh
dotnet restore
```

**3. Configurar la base de datos**

En el archivo `appsettings.json`, actualizar la cadena de conexión:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MiBaseDeDatos;User=root;Password=root;Port=3306"
  }
}
```

**4. Ejecutar el proyecto**
```sh
dotnet run
```

**4. Probar la API**

Se pueden probar los endpoints en `http://localhost:<puerto>/swagger`
