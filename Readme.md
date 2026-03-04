# Sistema de Gestión de Productos

### INF-512 – Unidad 3 | C# / List\<T\>

---

## Descripción

Sistema de consola desarrollado en C# que permite gestionar un inventario de productos. Implementa los principios de **Programación Orientada a Objetos** con una relación de composición entre las clases `Almacen` y `Producto`.

---

## Clases del sistema

| Clase      | Responsabilidad                                            |
| ---------- | ---------------------------------------------------------- |
| `Producto` | Representa un producto con código, nombre, precio y stock  |
| `Almacen`  | Gestiona la colección de productos usando `List<Producto>` |
| `Program`  | Interfaz de consola con menú interactivo                   |

---

## Funcionalidades

- ✅ Registrar productos (evita códigos duplicados)
- ✅ Buscar productos por código o nombre
- ✅ Eliminar productos con confirmación
- ✅ Listar todos los productos
- ✅ Validación de entradas del usuario
- ✅ Manejo de errores sin detener el programa

---

## Contenedor utilizado

Se utilizó **`List<Producto>`** por su tamaño dinámico, facilidad de uso con LINQ y menor complejidad de código comparado con un array convencional.

---

## Requisitos

- .NET 8.0 o superior
- Visual Studio 2022 o VS Code

---

## Cómo ejecutar

```bash
dotnet run
```

---

## Estructura del proyecto

```
SistemaProductos/
├── Program.cs          # Clases Producto, Almacen y Program
├── Ejercicio_3.2.csproj
└── README.md
```
