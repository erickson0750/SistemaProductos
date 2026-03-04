using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaProductos
{
    //clase producto
    class Producto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public int Stock { get; set; }

        public Producto(string codigo, string nombre, double precio, int stock)
        {
            Codigo = codigo;
            Nombre = nombre;
            Precio = precio;
            Stock = stock;
        }

        public override string ToString()
        {
            return $"[{Codigo}] {Nombre,-20} | Precio: ${Precio,8:F2} | Stock: {Stock,4} unidades";
        }
    }

    //clase almacen
    class Almacen
    {
        private List<Producto> _productos;
        private readonly string _nombre;

        public string Nombre => _nombre;

        public Almacen(string nombre)
        {
            _nombre = nombre;
            _productos = new List<Producto>();
        }

        //agregar un producto evitando codigos duplicados
        public bool AgregarProducto(Producto producto)
        {
            if (producto == null)
                throw new ArgumentNullException(nameof(producto), "El producto no puede ser nulo.");

            if (_productos.Any(p => p.Codigo.Equals(producto.Codigo, StringComparison.OrdinalIgnoreCase)))
                return false;

            _productos.Add(producto);
            return true;
        }

        //buscar por codigo o nombre(sin distincion de mayusculas)
        public List<Producto> BuscarProductos(string criterio)
        {
            if (string.IsNullOrWhiteSpace(criterio))
                return new List<Producto>();

            string criterioLower = criterio.ToLower();
            return _productos
                .Where(p => p.Codigo.ToLower().Contains(criterioLower) ||
                             p.Nombre.ToLower().Contains(criterioLower))
                .ToList();
        }

        //eliminar un producto por su codigo exacto
        public bool EliminarProducto(string codigo)
        {
            var producto = _productos.FirstOrDefault(p =>
                p.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));

            if (producto == null)
                return false;

            _productos.Remove(producto);
            return true;
        }

        //Rotrnar una copia de la lista
        public List<Producto> ObtenerProductos()
        {
            return new List<Producto>(_productos);
        }

        //Retorna la cantidad total de productos
        public int ObtenerCantidad()
        {
            return _productos.Count;
        }
    }


    // menu clase program
    class Program
    {
        static Almacen almacen = new Almacen("Almacen Central");

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            MostrarBienvenida();

            bool salir = false;
            while (!salir)
            {
                MostrarMenu();
                string opcion = Console.ReadLine()?.Trim() ?? "";

                switch (opcion)
                {
                    case "1": RegistrarProducto(); break;
                    case "2": BuscarProducto(); break;
                    case "3": EliminarProducto(); break;
                    case "4": ListarProductos(); break;
                    case "5":
                        salir = true;
                        Console.WriteLine("\n Gracias por usar el sistema.\n");
                        break;
                    default:
                        MostrarError("Opcion no valida. Ingrese un numero del 1 al 5.");
                        break;
                }
            }
        }

        // funcionalidades del menu
        static void RegistrarProducto()
        {
            Console.Clear();
            MostrarBienvenida();
            Console.WriteLine("     REGISTRAR NUEVO PRODUCTO \n");

            try
            {
                string codigo = LeerTexto(" Codigo del producto: ");
                string nombre = LeerTexto(" Nombre del producto: ");
                double precio = LeerDouble(" Precio (ej. 15.99): ");
                int stock = LeerEntero(" Stock inicial: ");

                var producto = new Producto(codigo, nombre, precio, stock);

                if (almacen.AgregarProducto(producto))
                    MostrarExito($"Producto '{nombre}' registrado exitosamente.");
                else
                    MostrarError($"Ya existe un producto con el codigo '{codigo}'. Registro cancelado.");
            }
            catch (Exception ex)
            {
                MostrarError($"Error inesperado: {ex.Message}");
            }
            Pausa();
        }

        static void BuscarProducto()
        {
            Console.Clear();
            MostrarBienvenida();
            Console.WriteLine("     Buscar Producto\n");

            string criterio = LeerTexto("  Codigo o nombre a buscar: ");
            List<Producto> resultados = almacen.BuscarProductos(criterio);

            if (resultados.Count == 0)
            {
                MostrarInfo($"No se encontraron productos con el criterio '{criterio}'.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n Se encontraron {resultados.Count} resultado(s):\n");
                Console.ResetColor();
                ImprimirTabla(resultados);
            }

            Pausa();
        }

        static void EliminarProducto()
        {
            Console.Clear();
            MostrarBienvenida();
            Console.WriteLine("     ELIMINAR PRODUCTO\n");

            string codigo = LeerTexto(" Codigo del producto a eliminar: ");

            Console.Write($"\n Confirma eliminar '{codigo}'? (s/n):");
            string confimacion = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (confimacion == "s")
            {
                if (almacen.EliminarProducto(codigo))
                    MostrarExito($"Producto '{codigo}' eliminado correctamente.");
                else

                    MostrarError($"No se encontro ningun producto con el codigo '{codigo}'.");
            }
            else
            {
                MostrarInfo("Operacion cancelada.");
            }
            Pausa();
        }



        static void ListarProductos()
        {
            Console.Clear();
            MostrarBienvenida();
            Console.WriteLine("     LISTADO DE PRODUCTO\n");

            List<Producto> productos = almacen.ObtenerProductos();

            if (productos.Count == 0)
            {
                MostrarInfo("El almacen no tiene productos registrados.");
            }
            else
            {
                Console.WriteLine($"  Total de productos: {almacen.ObtenerCantidad()}\n");
                ImprimirTabla(productos);
            }

            Pausa();
        }

        //Utilidades de consola
        static void MostrarBienvenida()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║    SISTEMA DE GESTION DE PRODUCTOS       ║");
            Console.WriteLine("║           Almacen Central                ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();
        }

        static void MostrarMenu()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ┌─────────────────────────────┐");
            Console.WriteLine("  │       MENU PRINCIPAL        │");
            Console.WriteLine("  ├─────────────────────────────┤");
            Console.WriteLine("  │  1. Registrar producto      │");
            Console.WriteLine("  │  2. Buscar producto         │");
            Console.WriteLine("  │  3. Eliminar producto       │");
            Console.WriteLine("  │  4. Listar productos        │");
            Console.WriteLine("  │  5. Salir                   │");
            Console.WriteLine("  └─────────────────────────────┘");
            Console.ResetColor();
            Console.Write("  Seleccione una opcion: ");
        }

        static void ImprimirTabla(List<Producto> productos)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  {"CODIGO",-10} {"NOMBRE",-22} {"PRECIO",10} {"STOCK",8}");
            Console.WriteLine($"  {"──────",-10} {"──────",-22} {"──────",10} {"──────",8}");
            Console.ResetColor();
            foreach (var p in productos)
                Console.WriteLine($"  {p.Codigo,-10} {p.Nombre,-22} ${p.Precio,9:F2} {p.Stock,7} uds");
        }

        static void MostrarExito(string msg) { Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine($"\n  {msg}"); Console.ResetColor(); }
        static void MostrarError(string msg) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"\n  {msg}"); Console.ResetColor(); }
        static void MostrarInfo(string msg) { Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine($"\n  {msg}"); Console.ResetColor(); }
        static void Pausa() { Console.WriteLine($"\n  Presione ENTER para continuar..."); Console.ReadLine(); }

        //Ayudante de entrada valida

        static string LeerTexto(string etiqueta)
        {
            string valor;
            do
            {
                Console.WriteLine(etiqueta);
                valor = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrEmpty(valor))
                    MostrarError("Este campo no puede estar vacio.");
            } while (string.IsNullOrEmpty(valor));
            return valor;
        }


        static double LeerDouble(string etiqueta)
        {
            double valor;
            while (true)
            {
                Console.Write(etiqueta);
                string entrada = Console.ReadLine()?.Trim() ?? "";
                if (double.TryParse(entrada, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out valor) && valor >= 0)
                    return valor;
                MostrarError("Ingrese un numero decimal positivo (ej. 12.50).");
            }
        }


        static int LeerEntero(string etiqueta)
        {
            int valor;
            while (true)
            {
                Console.Write(etiqueta);
                string entrada = Console.ReadLine()?.Trim() ?? "";
                if (int.TryParse(entrada, out valor) && valor >= 0)
                    return valor;
                MostrarError("Ingrese un numero entero no negativo.");
            }
        }
    }
}
