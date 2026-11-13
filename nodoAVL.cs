
namespace DonJuanMarket
{
    // Representa un nodo dentro del arbol AVL.
    public class NodoAVL
    {
        public Producto Producto { get; set; }

        public NodoAVL? Izquierdo { get; set; }

        public NodoAVL? Derecho { get; set; }

        public int Altura { get; set; }

        public NodoAVL(Producto producto)
        {
            Producto = producto;
            Izquierdo = null;
            Derecho = null;
            Altura = 1;
        }
    }
}
