
using System;

namespace DonJuanMarket
{
    // Integra el inventario AVL y la cola de prioridad.
    public class Supermercado
    {
        private readonly ArbolAVL inventario;
        private readonly ColaPrioridad colaClientes;

        private int siguienteTurno;

        public Supermercado()
        {
            inventario = new ArbolAVL();
            colaClientes = new ColaPrioridad();
            siguienteTurno = 1;
        }

        // Registra un producto en el inventario.
        public bool RegistrarProducto(Producto producto)
        {
            return inventario.Insertar(producto);
        }

        // Busca un producto por codigo.
        public Producto? BuscarProducto(string codigo)
        {
            return inventario.Buscar(codigo);
        }

        // Actualiza los datos de un producto existente.
        public bool ActualizarProducto(
            string codigo,
            string nombre,
            string categoria,
            double precio,
            int stock)
        {
            return inventario.Actualizar(
                codigo, nombre, categoria, precio, stock);
        }

        // Elimina un producto del inventario.
        public bool EliminarProducto(string codigo)
        {
            return inventario.Eliminar(codigo);
        }

        // Muestra el inventario ordenado por codigo.
        public void MostrarInventario()
        {
            inventario.MostrarInOrden();
        }

        // Obtiene las estadisticas generales del inventario.
        public void ObtenerEstadisticasInventario(
            out int cantidadProductos,
            out int totalUnidades,
            out double valorInventario)
        {
            inventario.ObtenerEstadisticas(
                out cantidadProductos,
                out totalUnidades,
                out valorInventario);
        }

        // Comprueba si el arbol AVL esta equilibrado.
        public bool EstaEquilibrado()
        {
            return inventario.EstaEquilibrado();
        }

        // Registra un cliente y genera su numero de turno.
        public Cliente RegistrarCliente(
            string nombre,
            TipoCliente tipo,
            int cantidadProductos)
        {
            Cliente cliente = new Cliente(
                nombre,
                tipo,
                cantidadProductos,
                siguienteTurno);

            colaClientes.Agregar(cliente);
            siguienteTurno++;

            return cliente;
        }

        // Atiende al cliente con mayor prioridad.
        public Cliente? AtenderCliente()
        {
            return colaClientes.Extraer();
        }

        // Muestra los clientes pendientes sin retirarlos.
        public void MostrarClientesEnEspera()
        {
            colaClientes.MostrarEspera();
        }

        // Consulta al proximo cliente sin atenderlo.
        public Cliente? ConsultarSiguienteCliente()
        {
            return colaClientes.ConsultarSiguiente();
        }

        public int TotalClientesEnEspera()
        {
            return colaClientes.Cantidad;
        }

        public bool HayClientesEnEspera()
        {
            return !colaClientes.EstaVacia;
        }
    }
}
