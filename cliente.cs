  
using System;

namespace DonJuanMarket
{
    // Representa un cliente que espera ser atendido.
    public class Cliente
    {
        public string Nombre { get; private set; }
        public TipoCliente Tipo { get; private set; }
        public int CantidadProductos { get; private set; }
        public DateTime HoraLlegada { get; private set; }
        public int NumeroTurno { get; private set; }

        // Permite conservar el orden de llegada en el Heap.
        public long OrdenLlegada { get; internal set; }

        public Cliente(
            string nombre,
            TipoCliente tipo,
            int cantidadProductos,
            int numeroTurno)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es obligatorio.");

            if (!Enum.IsDefined(typeof(TipoCliente), tipo))
                throw new ArgumentException("El tipo de cliente no es valido.");

            if (cantidadProductos < 0)
                throw new ArgumentException(
                    "La cantidad de productos no puede ser negativa.");

            Nombre = nombre.Trim();
            Tipo = tipo;
            CantidadProductos = cantidadProductos;
            NumeroTurno = numeroTurno;
            HoraLlegada = DateTime.Now;
            OrdenLlegada = 0;
        }

        // Menor numero significa mayor prioridad.
        public int ObtenerPrioridad()
        {
            switch (Tipo)
            {
                case TipoCliente.VIP:
                    return 1;

                case TipoCliente.ADULTO_MAYOR:
                    return 2;

                default:
                    return 3;
            }
        }

        public void MostrarDatos()
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"Turno: {NumeroTurno}");
            Console.WriteLine($"Nombre: {Nombre}");
            Console.WriteLine($"Tipo: {Tipo}");
            Console.WriteLine($"Productos: {CantidadProductos}");
            Console.WriteLine($"Llegada: {HoraLlegada:HH:mm:ss}");
            Console.WriteLine("----------------------------------");
        }
    }

    // Categorias permitidas para los clientes.
    public enum TipoCliente
    {
        VIP,
        ADULTO_MAYOR,
        REGULAR
    }
}
