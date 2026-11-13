
using System;

namespace DonJuanMarket
{
    // Representa un producto registrado en el inventario.
    public class Producto
    {
        public string Codigo { get; private set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public double Precio { get; set; }
        public int Stock { get; set; }

        public Producto(
            string codigo,
            string nombre,
            string categoria,
            double precio,
            int stock)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("El codigo es obligatorio.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(categoria))
                throw new ArgumentException("La categoria es obligatoria.");

            if (precio <= 0 || double.IsNaN(precio)
                || double.IsInfinity(precio))
                throw new ArgumentException("El precio debe ser mayor que cero.");

            if (stock < 0)
                throw new ArgumentException("El stock no puede ser negativo.");

            Codigo = codigo.Trim().ToUpperInvariant();
            Nombre = nombre.Trim();
            Categoria = categoria.Trim();
            Precio = precio;
            Stock = stock;
        }

        public void MostrarDatos()
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"Codigo:    {Codigo}");
            Console.WriteLine($"Nombre:    {Nombre}");
            Console.WriteLine($"Categoria: {Categoria}");
            Console.WriteLine($"Precio:    Q{Precio:F2}");
            Console.WriteLine($"Stock:     {Stock} unidades");
            Console.WriteLine("----------------------------------");
        }
    }
}
