# Nexo — Frontend de inventario

Interfaz de gestión de productos y categorías desarrollada con Angular 22 standalone. Consume la API ASP.NET Core de `../apiinicial-main`.

## Requisitos

- Node.js compatible con Angular 22
- API backend ejecutándose en `https://localhost:7080`
- Certificado HTTPS de desarrollo confiable (`dotnet dev-certs https --trust`)

## Ejecutar en desarrollo

Desde esta carpeta:

```powershell
npm install
npm start
```

Abre `http://localhost:4200`. La URL de la API se configura en `src/environments/environment.ts`. El backend debe permitir CORS para ese origen.

## Funcionalidades

- Listado de productos con búsqueda por nombre/categoría y filtros.
- Alta, edición y eliminación de productos.
- Creación y filtrado por categoría.
- Indicadores de inventario, stock bajo, productos activos y valor en stock.
- Formularios reactivos con validación y mensajes de error.
- Componentes standalone, servicios HTTP, directiva de stock bajo y pipe de estado.

## Compilar

```powershell
npm run build
```

Los archivos de producción se generan en `dist/frontend/browser`.
