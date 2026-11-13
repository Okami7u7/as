
using System;

namespace DonJuanMarket
{
    // Administra los productos mediante un arbol AVL.
    public class ArbolAVL
    {
        private NodoAVL? raiz;

        public ArbolAVL()
        {
            raiz = null;
        }

        // Devuelve la altura de un nodo.
        private int ObtenerAltura(NodoAVL? nodo)
        {
            return nodo == null ? 0 : nodo.Altura;
        }

        // Calcula el factor de balance del nodo.
        private int ObtenerBalance(NodoAVL? nodo)
        {
            return nodo == null
                ? 0
                : ObtenerAltura(nodo.Izquierdo)
                  - ObtenerAltura(nodo.Derecho);
        }

        // Actualiza la altura segun sus hijos.
        private void ActualizarAltura(NodoAVL nodo)
        {
            nodo.Altura = 1 + Math.Max(
                ObtenerAltura(nodo.Izquierdo),
                ObtenerAltura(nodo.Derecho));
        }

        // Rotacion simple hacia la derecha.
        private NodoAVL RotarDerecha(NodoAVL y)
        {
            NodoAVL x = y.Izquierdo!;
            NodoAVL? temporal = x.Derecho;

            x.Derecho = y;
            y.Izquierdo = temporal;

            ActualizarAltura(y);
            ActualizarAltura(x);

            return x;
        }

        // Rotacion simple hacia la izquierda.
        private NodoAVL RotarIzquierda(NodoAVL x)
        {
            NodoAVL y = x.Derecho!;
            NodoAVL? temporal = y.Izquierdo;

            y.Izquierdo = x;
            x.Derecho = temporal;

            ActualizarAltura(x);
            ActualizarAltura(y);

            return y;
        }

        // Inserta un producto y mantiene el arbol equilibrado.
        public bool Insertar(Producto producto)
        {
            if (producto == null)
                return false;

            if (Buscar(producto.Codigo) != null)
                return false;

            raiz = InsertarRecursivo(raiz, producto);
            return true;
        }

        private NodoAVL InsertarRecursivo(
            NodoAVL? nodo, Producto producto)
        {
            if (nodo == null)
                return new NodoAVL(producto);

            int comparacion = string.Compare(
                producto.Codigo,
                nodo.Producto.Codigo,
                StringComparison.OrdinalIgnoreCase);

            if (comparacion < 0)
                nodo.Izquierdo = InsertarRecursivo(
                    nodo.Izquierdo, producto);
            else
                nodo.Derecho = InsertarRecursivo(
                    nodo.Derecho, producto);

            ActualizarAltura(nodo);

            int balance = ObtenerBalance(nodo);

            // Caso izquierda-izquierda.
            if (balance > 1 &&
                string.Compare(producto.Codigo,
                    nodo.Izquierdo!.Producto.Codigo,
                    StringComparison.OrdinalIgnoreCase) < 0)
                return RotarDerecha(nodo);

            // Caso derecha-derecha.
            if (balance < -1 &&
                string.Compare(producto.Codigo,
                    nodo.Derecho!.Producto.Codigo,
                    StringComparison.OrdinalIgnoreCase) > 0)
                return RotarIzquierda(nodo);

            // Caso izquierda-derecha.
            if (balance > 1 &&
                string.Compare(producto.Codigo,
                    nodo.Izquierdo!.Producto.Codigo,
                    StringComparison.OrdinalIgnoreCase) > 0)
            {
                nodo.Izquierdo =
                    RotarIzquierda(nodo.Izquierdo!);

                return RotarDerecha(nodo);
            }

            // Caso derecha-izquierda.
            if (balance < -1 &&
                string.Compare(producto.Codigo,
                    nodo.Derecho!.Producto.Codigo,
                    StringComparison.OrdinalIgnoreCase) < 0)
            {
                nodo.Derecho =
                    RotarDerecha(nodo.Derecho!);

                return RotarIzquierda(nodo);
            }

            return nodo;
        }

        // Busca un producto por su codigo.
        public Producto? Buscar(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return null;

            NodoAVL? actual = raiz;
            codigo = codigo.Trim();

            while (actual != null)
            {
                int comparacion = string.Compare(
                    codigo,
                    actual.Producto.Codigo,
                    StringComparison.OrdinalIgnoreCase);

                if (comparacion == 0)
                    return actual.Producto;

                if (comparacion < 0)
                    actual = actual.Izquierdo;
                else
                    actual = actual.Derecho;
            }

            return null;
        }

        // Actualiza los datos sin cambiar el codigo.
        public bool Actualizar(
            string codigo,
            string nombre,
            string categoria,
            double precio,
            int stock)
        {
            Producto? producto = Buscar(codigo);

            if (producto == null ||
                string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(categoria) ||
                precio <= 0 ||
                double.IsNaN(precio) ||
                double.IsInfinity(precio) ||
                stock < 0)
                return false;

            producto.Nombre = nombre.Trim();
            producto.Categoria = categoria.Trim();
            producto.Precio = precio;
            producto.Stock = stock;

            return true;
        }

        // Elimina un producto por su codigo.
        public bool Eliminar(string codigo)
        {
            if (Buscar(codigo) == null)
                return false;

            raiz = EliminarRecursivo(raiz, codigo.Trim());
            return true;
        }

        private NodoAVL? EliminarRecursivo(
            NodoAVL? nodo, string codigo)
        {
            if (nodo == null)
                return null;

            int comparacion = string.Compare(
                codigo,
                nodo.Producto.Codigo,
                StringComparison.OrdinalIgnoreCase);

            if (comparacion < 0)
                nodo.Izquierdo = EliminarRecursivo(
                    nodo.Izquierdo, codigo);
            else if (comparacion > 0)
                nodo.Derecho = EliminarRecursivo(
                    nodo.Derecho, codigo);
            else
            {
                // Caso: el nodo no tiene hijo izquierdo.
                if (nodo.Izquierdo == null)
                    return nodo.Derecho;

                // Caso: el nodo no tiene hijo derecho.
                if (nodo.Derecho == null)
                    return nodo.Izquierdo;

                // Caso: tiene dos hijos.
                NodoAVL sucesor = ObtenerMinimo(nodo.Derecho);

                nodo.Producto = sucesor.Producto;

                nodo.Derecho = EliminarRecursivo(
                    nodo.Derecho, sucesor.Producto.Codigo);
            }

            ActualizarAltura(nodo);

            int balance = ObtenerBalance(nodo);

            // Desequilibrio izquierda-izquierda.
            if (balance > 1 &&
                ObtenerBalance(nodo.Izquierdo) >= 0)
                return RotarDerecha(nodo);

            // Desequilibrio izquierda-derecha.
            if (balance > 1 &&
                ObtenerBalance(nodo.Izquierdo) < 0)
            {
                nodo.Izquierdo =
                    RotarIzquierda(nodo.Izquierdo!);

                return RotarDerecha(nodo);
            }

            // Desequilibrio derecha-derecha.
            if (balance < -1 &&
                ObtenerBalance(nodo.Derecho) <= 0)
                return RotarIzquierda(nodo);

            // Desequilibrio derecha-izquierda.
            if (balance < -1 &&
                ObtenerBalance(nodo.Derecho) > 0)
            {
                nodo.Derecho =
                    RotarDerecha(nodo.Derecho!);

                return RotarIzquierda(nodo);
            }

            return nodo;
        }

        // Encuentra el nodo con el menor codigo.
        private NodoAVL ObtenerMinimo(NodoAVL nodo)
        {
            NodoAVL actual = nodo;

            while (actual.Izquierdo != null)
                actual = actual.Izquierdo;

            return actual;
        }

        // Muestra los productos en una tabla ordenada.
        public void MostrarInOrden()
        {
            if (raiz == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("El inventario esta vacio.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("                         INVENTARIO DE PRODUCTOS");
            Console.WriteLine("+--------------+----------------------+------------------+--------------+--------+");
            Console.WriteLine("| CODIGO       | NOMBRE               | CATEGORIA        |       PRECIO | STOCK  |");
            Console.WriteLine("+--------------+----------------------+------------------+--------------+--------+");
            Console.ResetColor();

            MostrarInOrdenRecursivo(raiz);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("+--------------+----------------------+------------------+--------------+--------+");
            Console.ResetColor();

            ObtenerEstadisticas(
                out int cantidadProductos,
                out int totalUnidades,
                out double valorInventario);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("RESUMEN DEL INVENTARIO");
            Console.ResetColor();

            Console.WriteLine($"Productos registrados: {cantidadProductos}");
            Console.WriteLine($"Unidades disponibles:  {totalUnidades}");
            Console.WriteLine($"Valor del inventario:  Q{valorInventario:F2}");
            Console.WriteLine();
        }

        // Recorre el arbol y muestra cada producto en una fila.
        private void MostrarInOrdenRecursivo(NodoAVL? nodo)
        {
            if (nodo == null)
                return;

            MostrarInOrdenRecursivo(nodo.Izquierdo);

            Producto producto = nodo.Producto;

            Console.WriteLine(
                $"| {AjustarTexto(producto.Codigo, 12)} " +
                $"| {AjustarTexto(producto.Nombre, 20)} " +
                $"| {AjustarTexto(producto.Categoria, 16)} " +
                $"| {("Q" + producto.Precio.ToString("F2")).PadLeft(12)} " +
                $"| {producto.Stock.ToString().PadLeft(6)} |");

            MostrarInOrdenRecursivo(nodo.Derecho);
        }

        // Ajusta el texto para mantener alineadas las columnas.
        private static string AjustarTexto(string texto, int ancho)
        {
            if (texto.Length > ancho)
                return texto.Substring(0, ancho - 3) + "...";

            return texto.PadRight(ancho);
        }

        // Comprueba si todos los nodos mantienen el balance AVL.
        public bool EstaEquilibrado()
        {
            return VerificarEquilibrio(raiz);
        }

        private bool VerificarEquilibrio(NodoAVL? nodo)
        {
            if (nodo == null)
                return true;

            int balance = ObtenerBalance(nodo);

            if (balance < -1 || balance > 1)
                return false;

            return VerificarEquilibrio(nodo.Izquierdo)
                && VerificarEquilibrio(nodo.Derecho);
        }

        // Calcula las estadisticas del inventario sin modificar el arbol.
        public void ObtenerEstadisticas(
            out int cantidadProductos,
            out int totalUnidades,
            out double valorInventario)
        {
            cantidadProductos = 0;
            totalUnidades = 0;
            valorInventario = 0;

            CalcularEstadisticas(
                raiz,
                ref cantidadProductos,
                ref totalUnidades,
                ref valorInventario);
        }

        // Recorre los nodos y acumula los datos de cada producto.
        private void CalcularEstadisticas(
            NodoAVL? nodo,
            ref int cantidadProductos,
            ref int totalUnidades,
            ref double valorInventario)
        {
            if (nodo == null)
                return;

            CalcularEstadisticas(
                nodo.Izquierdo,
                ref cantidadProductos,
                ref totalUnidades,
                ref valorInventario);

            cantidadProductos++;
            totalUnidades += nodo.Producto.Stock;
            valorInventario += nodo.Producto.Precio * nodo.Producto.Stock;

            CalcularEstadisticas(
                nodo.Derecho,
                ref cantidadProductos,
                ref totalUnidades,
                ref valorInventario);
        }
    }
}
