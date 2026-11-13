
using System;
using System.Collections.Generic;

namespace DonJuanMarket
{
    // Administra la fila de clientes mediante un Min Heap.
    // La prioridad menor se atiende primero.
    public class ColaPrioridad
    {
        private readonly List<Cliente> heap;
        private long siguienteOrden;

        public ColaPrioridad()
        {
            heap = new List<Cliente>();
            siguienteOrden = 0;
        }

        public int Cantidad
        {
            get { return heap.Count; }
        }

        public bool EstaVacia
        {
            get { return heap.Count == 0; }
        }

        // Compara dos clientes:
        // primero por prioridad y luego por orden de llegada.
        private bool TienePrioridad(Cliente a, Cliente b)
        {
            int prioridadA = a.ObtenerPrioridad();
            int prioridadB = b.ObtenerPrioridad();

            if (prioridadA != prioridadB)
                return prioridadA < prioridadB;

            return a.OrdenLlegada < b.OrdenLlegada;
        }

        // Agrega un cliente al final y lo sube hasta
        // recuperar la propiedad del Min Heap.
        public void Agregar(Cliente cliente)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente));

            cliente.OrdenLlegada = siguienteOrden++;
            heap.Add(cliente);

            Subir(heap.Count - 1);
        }

        // Bubble Up: compara el cliente con su padre.
        private void Subir(int indice)
        {
            while (indice > 0)
            {
                int padre = (indice - 1) / 2;

                if (!TienePrioridad(heap[indice], heap[padre]))
                    break;

                Intercambiar(indice, padre);
                indice = padre;
            }
        }

        // Retira la raíz, que es el cliente que debe
        // atenderse primero, y reorganiza el Heap.
        public Cliente? Extraer()
        {
            if (EstaVacia)
                return null;

            Cliente atendido = heap[0];
            int ultimo = heap.Count - 1;

            if (ultimo == 0)
            {
                heap.RemoveAt(0);
                return atendido;
            }

            // Coloca el último elemento en la raíz.
            heap[0] = heap[ultimo];
            heap.RemoveAt(ultimo);

            // Recupera el orden del Min Heap.
            Bajar(0);

            return atendido;
        }

        // Bubble Down: mueve el elemento hacia abajo
        // intercambiándolo con el hijo de mayor prioridad.
        private void Bajar(int indice)
        {
            while (true)
            {
                int izquierdo = 2 * indice + 1;
                int derecho = 2 * indice + 2;
                int mejor = indice;

                if (izquierdo < heap.Count &&
                    TienePrioridad(heap[izquierdo], heap[mejor]))
                {
                    mejor = izquierdo;
                }

                if (derecho < heap.Count &&
                    TienePrioridad(heap[derecho], heap[mejor]))
                {
                    mejor = derecho;
                }

                if (mejor == indice)
                    break;

                Intercambiar(indice, mejor);
                indice = mejor;
            }
        }

        // Intercambia dos posiciones de la lista.
        private void Intercambiar(int a, int b)
        {
            Cliente temporal = heap[a];
            heap[a] = heap[b];
            heap[b] = temporal;
        }

        // Consulta al siguiente cliente sin retirarlo.
        public Cliente? ConsultarSiguiente()
        {
            if (EstaVacia)
                return null;

            return heap[0];
        }

        // Muestra la fila ordenada usando una copia del Heap.
        // La cola original no se modifica.
        public void MostrarEspera()
        {
            if (EstaVacia)
            {
                Console.WriteLine("No hay clientes en espera.");
                return;
            }

            ColaPrioridad copia = new ColaPrioridad();

            // Copiamos los clientes sin cambiar su orden
            // original de llegada ni modificar la cola real.
            foreach (Cliente cliente in heap)
            {
                copia.heap.Add(cliente);
            }

            copia.siguienteOrden = siguienteOrden;

            // Reconstruimos el Heap de la copia.
            for (int i = copia.heap.Count / 2 - 1; i >= 0; i--)
            {
                copia.Bajar(i);
            }

            Console.WriteLine("CLIENTES EN ORDEN DE ATENCION");
            Console.WriteLine();

            while (!copia.EstaVacia)
            {
                Cliente? cliente = copia.Extraer();

                if (cliente != null)
                    cliente.MostrarDatos();
            }

            Console.WriteLine(
                $"Total de clientes en espera: {heap.Count}");
        }
    }
}
