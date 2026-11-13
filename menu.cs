
using System;
using System.Globalization;

namespace DonJuanMarket
{
    // Administra los menus y la interaccion con el usuario.
    public class Menu
    {
        private readonly Supermercado supermercado;

        public Menu()
        {
            supermercado = new Supermercado();
        }

        public void Iniciar()
        {
            int opcion;

            do
            {
                Console.Clear();
                Titulo("DON JUAN MARKET");
                Subtitulo("SISTEMA DE GESTION DE SUPERMERCADO");

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("  MENU PRINCIPAL");
                Separador();

                MostrarOpcion("1", "Gestionar inventario");
                MostrarOpcion("2", "Atencion de clientes");
                MostrarOpcion("3", "Mostrar inventario ordenado");
                MostrarOpcion("4", "Mostrar clientes en espera");
                MostrarOpcion("5", "Panel de resumen");
                MostrarOpcion("0", "Salir");

                Separador();
                opcion = LeerEntero("  Seleccione una opcion: ", 0, 5);

                switch (opcion)
                {
                    case 1:
                        MenuInventario();
                        break;

                    case 2:
                        MenuClientes();
                        break;

                    case 3:
                        Console.Clear();
                        Titulo("INVENTARIO DE PRODUCTOS");
                        MostrarInventarioConBalance();
                        Pausar();
                        break;

                    case 4:
                        Console.Clear();
                        Titulo("CLIENTES EN ESPERA");
                        supermercado.MostrarClientesEnEspera();
                        Pausar();
                        break;

                    case 5:
                        Console.Clear();
                        MostrarPanelResumen();
                        Pausar();
                        break;

                    case 0:
                        Console.Clear();
                        Titulo("DON JUAN MARKET");
                        MensajeExito("Gracias por utilizar Don Juan Market.");
                        Pausar();
                        break;
                }

            } while (opcion != 0);
        }

        private void MenuInventario()
        {
            int opcion;

            do
            {
                Console.Clear();
                Titulo("GESTION DE INVENTARIO");

                MostrarOpcion("1", "Registrar producto");
                MostrarOpcion("2", "Buscar producto");
                MostrarOpcion("3", "Actualizar producto");
                MostrarOpcion("4", "Eliminar producto");
                MostrarOpcion("5", "Mostrar inventario ordenado");
                MostrarOpcion("0", "Volver al menu principal");

                Separador();
                opcion = LeerEntero("  Seleccione una opcion: ", 0, 5);

                switch (opcion)
                {
                    case 1:
                        RegistrarProducto();
                        break;

                    case 2:
                        BuscarProducto();
                        break;

                    case 3:
                        ActualizarProducto();
                        break;

                    case 4:
                        EliminarProducto();
                        break;

                    case 5:
                        Console.Clear();
                        Titulo("INVENTARIO DE PRODUCTOS");
                        MostrarInventarioConBalance();
                        Pausar();
                        break;
                }

            } while (opcion != 0);
        }

        // Muestra el inventario y comprueba el equilibrio del arbol AVL.
        private void MostrarInventarioConBalance()
        {
            supermercado.MostrarInventario();

            Console.WriteLine();
            Separador();

            if (supermercado.EstaEquilibrado())
                MensajeExito("El arbol AVL esta equilibrado correctamente.");
            else
                MensajeAdvertencia("El arbol AVL presenta un desequilibrio.");
        }

        // Muestra el resumen general del supermercado.
        private void MostrarPanelResumen()
        {
            Titulo("PANEL DE RESUMEN");

            int cantidadProductos;
            int totalUnidades;
            double valorInventario;

            supermercado.ObtenerEstadisticasInventario(
                out cantidadProductos,
                out totalUnidades,
                out valorInventario);

            int clientesEnEspera =
                supermercado.TotalClientesEnEspera();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  RESUMEN DEL INVENTARIO");
            Console.ResetColor();

            Separador();
            Console.WriteLine($"  Productos registrados : {cantidadProductos}");
            Console.WriteLine($"  Unidades disponibles  : {totalUnidades}");
            Console.WriteLine($"  Valor del inventario  : Q{valorInventario:N2}");

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  RESUMEN DE ATENCION");
            Console.ResetColor();

            Separador();
            Console.WriteLine($"  Clientes en espera    : {clientesEnEspera}");

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  ESTADO DEL SISTEMA");
            Console.ResetColor();

            Separador();

            if (supermercado.EstaEquilibrado())
                MensajeExito("Arbol AVL: Equilibrado correctamente.");
            else
                MensajeAdvertencia("Arbol AVL: Se detecto un desequilibrio.");

            if (cantidadProductos == 0)
                MensajeAdvertencia("El inventario se encuentra vacio.");

            Separador();
        }

        private void RegistrarProducto()
        {
            Console.Clear();
            Titulo("REGISTRAR PRODUCTO");

            string codigo = LeerTexto("  Codigo: ");
            string nombre = LeerTexto("  Nombre: ");
            string categoria = LeerTexto("  Categoria: ");
            double precio = LeerDouble("  Precio: ", 0.01);
            int stock = LeerEntero("  Cantidad en stock: ", 0, int.MaxValue);

            try
            {
                Producto producto = new Producto(
                    codigo, nombre, categoria, precio, stock);

                if (supermercado.RegistrarProducto(producto))
                    MensajeExito("Producto registrado correctamente.");
                else
                    MensajeAdvertencia("Ya existe un producto con ese codigo.");
            }
            catch (ArgumentException ex)
            {
                MensajeError($"No se pudo registrar: {ex.Message}");
            }

            Pausar();
        }

        private void BuscarProducto()
        {
            Console.Clear();
            Titulo("BUSCAR PRODUCTO");

            string codigo = LeerTexto("  Ingrese el codigo: ");
            Producto? producto = supermercado.BuscarProducto(codigo);

            if (producto == null)
            {
                MensajeAdvertencia("Producto no encontrado.");
            }
            else
            {
                Console.WriteLine();
                producto.MostrarDatos();
            }

            Pausar();
        }

        private void ActualizarProducto()
        {
            Console.Clear();
            Titulo("ACTUALIZAR PRODUCTO");

            string codigo = LeerTexto("  Codigo del producto: ");
            Producto? existente = supermercado.BuscarProducto(codigo);

            if (existente == null)
            {
                MensajeAdvertencia("Producto no encontrado.");
                Pausar();
                return;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  DATOS ACTUALES");
            Console.ResetColor();

            existente.MostrarDatos();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  INGRESE LOS NUEVOS DATOS");
            Console.ResetColor();

            string nombre = LeerTexto("  Nuevo nombre: ");
            string categoria = LeerTexto("  Nueva categoria: ");
            double precio = LeerDouble("  Nuevo precio: ", 0.01);
            int stock = LeerEntero("  Nuevo stock: ", 0, int.MaxValue);

            if (supermercado.ActualizarProducto(
                codigo, nombre, categoria, precio, stock))
            {
                MensajeExito("Producto actualizado correctamente.");
            }
            else
            {
                MensajeError("No se pudo actualizar el producto.");
            }

            Pausar();
        }

        private void EliminarProducto()
        {
            Console.Clear();
            Titulo("ELIMINAR PRODUCTO");

            string codigo = LeerTexto("  Codigo del producto: ");

            Console.WriteLine();
            Console.Write("  Confirma la eliminacion? (S/N): ");
            string? confirmacion = Console.ReadLine();

            if (!string.Equals(
                confirmacion?.Trim(), "S",
                StringComparison.OrdinalIgnoreCase))
            {
                MensajeAdvertencia("Operacion cancelada.");
            }
            else if (supermercado.EliminarProducto(codigo))
            {
                MensajeExito("Producto eliminado correctamente.");
            }
            else
            {
                MensajeError("Producto no encontrado.");
            }

            Pausar();
        }

        private void MenuClientes()
        {
            int opcion;

            do
            {
                Console.Clear();
                Titulo("ATENCION DE CLIENTES");

                MostrarOpcion("1", "Registrar cliente y generar ticket");
                MostrarOpcion("2", "Atender siguiente cliente");
                MostrarOpcion("3", "Consultar siguiente cliente");
                MostrarOpcion("4", "Mostrar clientes en espera");
                MostrarOpcion("0", "Volver al menu principal");

                Separador();
                opcion = LeerEntero("  Seleccione una opcion: ", 0, 4);

                switch (opcion)
                {
                    case 1:
                        RegistrarCliente();
                        break;

                    case 2:
                        AtenderCliente();
                        break;

                    case 3:
                        ConsultarSiguiente();
                        break;

                    case 4:
                        Console.Clear();
                        Titulo("CLIENTES EN ESPERA");
                        supermercado.MostrarClientesEnEspera();
                        Pausar();
                        break;
                }

            } while (opcion != 0);
        }

        private void RegistrarCliente()
        {
            Console.Clear();
            Titulo("REGISTRO DE CLIENTE");

            string nombre = LeerTexto("  Nombre del cliente: ");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  TIPO DE CLIENTE");
            Console.ResetColor();

            MostrarOpcion("1", "VIP");
            MostrarOpcion("2", "Adulto mayor");
            MostrarOpcion("3", "Regular");

            int tipoSeleccionado = LeerEntero("  Seleccione el tipo: ", 1, 3);

            TipoCliente tipo;

            switch (tipoSeleccionado)
            {
                case 1:
                    tipo = TipoCliente.VIP;
                    break;

                case 2:
                    tipo = TipoCliente.ADULTO_MAYOR;
                    break;

                default:
                    tipo = TipoCliente.REGULAR;
                    break;
            }

            int cantidad = LeerEntero(
                "  Cantidad de productos que comprara: ", 0, int.MaxValue);

            Cliente cliente = supermercado.RegistrarCliente(
                nombre, tipo, cantidad);

            Console.WriteLine();
            Titulo("TICKET DE TURNO");

            Separador();
            Console.WriteLine($"  Turno      : {cliente.NumeroTurno}");
            Console.WriteLine($"  Nombre     : {cliente.Nombre}");
            Console.WriteLine($"  Categoria  : {cliente.Tipo}");
            Console.WriteLine($"  Productos  : {cliente.CantidadProductos}");
            Console.WriteLine($"  Hora       : {cliente.HoraLlegada:HH:mm:ss}");
            Console.WriteLine($"  Prioridad  : {cliente.ObtenerPrioridad()}");
            Separador();

            MensajeExito("Conserve su numero de turno.");

            Pausar();
        }

        private void AtenderCliente()
        {
            Console.Clear();
            Titulo("ATENCION EN CAJA");

            Cliente? cliente = supermercado.AtenderCliente();

            if (cliente == null)
            {
                MensajeAdvertencia("No hay clientes pendientes.");
            }
            else
            {
                Console.WriteLine("  SIGUIENTE CLIENTE");
                Separador();

                cliente.MostrarDatos();

                MensajeExito("Cliente enviado a caja correctamente.");
            }

            Pausar();
        }

        private void ConsultarSiguiente()
        {
            Console.Clear();
            Titulo("SIGUIENTE CLIENTE");

            Cliente? cliente = supermercado.ConsultarSiguienteCliente();

            if (cliente == null)
                MensajeAdvertencia("No hay clientes en espera.");
            else
                cliente.MostrarDatos();

            Pausar();
        }

        // Presentacion visual de los titulos.
        private static void Titulo(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"                 {texto}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            Console.ResetColor();
            Console.WriteLine();
        }

        private static void Subtitulo(string texto)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"       {texto}");
            Console.ResetColor();
        }

        private static void MostrarOpcion(string numero, string descripcion)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"  [{numero}] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(descripcion);
            Console.ResetColor();
        }

        private static void Separador()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("--------------------------------------------------");
            Console.ResetColor();
        }

        private static void MensajeExito(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  [OK] {mensaje}");
            Console.ResetColor();
        }

        private static void MensajeAdvertencia(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  [AVISO] {mensaje}");
            Console.ResetColor();
        }

        private static void MensajeError(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  [ERROR] {mensaje}");
            Console.ResetColor();
        }

        private static string LeerTexto(string mensaje)
        {
            string? valor;

            do
            {
                Console.Write(mensaje);
                valor = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(valor))
                    MensajeAdvertencia("El dato no puede estar vacio.");

            } while (string.IsNullOrWhiteSpace(valor));

            return valor.Trim();
        }

        private static int LeerEntero(
            string mensaje, int minimo, int maximo)
        {
            int valor;

            while (true)
            {
                Console.Write(mensaje);
                string? entrada = Console.ReadLine();

                if (int.TryParse(entrada, out valor) &&
                    valor >= minimo && valor <= maximo)
                    return valor;

                MensajeAdvertencia(
                    $"Ingrese un numero entero entre {minimo} y {maximo}.");
            }
        }

        private static double LeerDouble(
            string mensaje, double minimo)
        {
            double valor;

            while (true)
            {
                Console.Write(mensaje);
                string? entrada = Console.ReadLine();

                bool valido = double.TryParse(
                    entrada,
                    NumberStyles.Float,
                    CultureInfo.CurrentCulture,
                    out valor);

                if (!valido)
                {
                    valido = double.TryParse(
                        entrada,
                        NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out valor);
                }

                if (valido &&
                    valor >= minimo &&
                    !double.IsNaN(valor) &&
                    !double.IsInfinity(valor))
                    return valor;

                MensajeAdvertencia(
                    $"Ingrese un precio valido mayor o igual a {minimo}.");
            }
        }

        private static void Pausar()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  Presione una tecla para continuar...");
            Console.ResetColor();
            Console.ReadKey(true);
        }
    }
}
