using System;
using System.Collections.Generic;

namespace ChimiMiBarriga
{
    // Clase para representar un ingrediente adicional
    public class Extra
    {
        public string Nombre { get; set; }
        public double Precio { get; set; }

        public Extra(string nombre, double precio)
        {
            Nombre = nombre;
            Precio = precio;
        }
    }

    // 1. Clase Base: Hamburguesa
    public class Hamburguesa
    {
        public string Pan { get; protected set; }
        public string Carne { get; protected set; }
        public double PrecioBase { get; protected set; }
        
        protected List<Extra> Extras = new List<Extra>();
        protected int LimiteExtras;

        public Hamburguesa(string pan, string carne, double precioBase)
        {
            Pan = pan;
            Carne = carne;
            PrecioBase = precioBase;
            LimiteExtras = 4; // Límite por defecto
        }

        // Método virtual para permitir que las clases derivadas cambien la lógica de adición
        public virtual void AgregarExtra(string nombre, double precio)
        {
            if (Extras.Count < LimiteExtras)
            {
                Extras.Add(new Extra(nombre, precio));
                Console.WriteLine($"Agregado: {nombre} (+${precio})");
            }
            else
            {
                Console.WriteLine($"Error: No se pueden agregar más de {LimiteExtras} extras a esta hamburguesa.");
            }
        }

        // Método para calcular el total
        public virtual double CalcularTotal()
        {
            double total = PrecioBase;
            foreach (var extra in Extras)
            {
                total += extra.Precio;
            }
            return total;
        }

        // 1. Método MostrarDetalle()
        public virtual void MostrarDetalle()
        {
            Console.WriteLine("\n--- DESGLOSE DE COMPRA ---");
            Console.WriteLine($"Hamburguesa: {Carne} en pan {Pan}");
            Console.WriteLine($"Precio Base: ${PrecioBase:F2}");
            
            if (Extras.Count > 0)
            {
                Console.WriteLine("Extras:");
                foreach (var extra in Extras)
                {
                    Console.WriteLine($"- {extra.Nombre}: ${extra.Precio:F2}");
                }
            }

            Console.WriteLine($"TOTAL FINAL: ${CalcularTotal():F2}");
            Console.WriteLine("--------------------------");
        }
    }

    // 2. Clase Derivada: HamburguesaSaludable
    public class HamburguesaSaludable : Hamburguesa
    {
        // El constructor fuerza el tipo de pan a "Integral"
        public HamburguesaSaludable(string carne, double precioBase) 
            : base("Integral", carne, precioBase)
        {
            // Capacidad Extendida: hasta 6 ingredientes
            LimiteExtras = 6;
        }
    }

    // 3. Clase Derivada: HamburguesaPremium
    public class HamburguesaPremium : Hamburguesa
    {
        public HamburguesaPremium(string pan, string carne, double precioBase) 
            : base(pan, carne, precioBase)
        {
            // Automatización: Agrega papas y bebida al instanciarse
            // Usamos base.AgregarExtra para saltar la restricción de bloqueo inicial
            base.AgregarExtra("Papas fritas", 2.50);
            base.AgregarExtra("Bebida", 1.50);
        }

        // Restricción Estricta: Bloquea cualquier intento de agregar ingredientes adicionales
        public override void AgregarExtra(string nombre, double precio)
        {
            Console.WriteLine($"AVISO: La Hamburguesa Premium ya incluye sus complementos. No se puede agregar: {nombre}.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Chimi MiBarriga del Sr. Billy Navaja ===\n");

            // Caso 1: Crear una hamburguesa clásica con 3 extras
            Console.WriteLine(">> Preparando Hamburguesa Clásica...");
            Hamburguesa clasica = new Hamburguesa("Blanco", "Res", 5.00);
            clasica.AgregarExtra("Queso", 0.50);
            clasica.AgregarExtra("Tocineta", 1.00);
            clasica.AgregarExtra("Huevo", 0.75);
            clasica.MostrarDetalle();

            // Caso 2: Crear una saludable con 5 extras
            Console.WriteLine("\n>> Preparando Hamburguesa Saludable...");
            HamburguesaSaludable saludable = new HamburguesaSaludable("Pollo Grill", 6.50);
            saludable.AgregarExtra("Lechuga", 0.25);
            saludable.AgregarExtra("Tomate", 0.25);
            saludable.AgregarExtra("Cebolla", 0.25);
            saludable.AgregarExtra("Pepino", 0.25);
            saludable.AgregarExtra("Aguacate", 1.25);
            saludable.MostrarDetalle();

            // Caso 3: Crear una premium e intentar (fallidamente) agregar un extra
            Console.WriteLine("\n>> Preparando Hamburguesa Premium...");
            HamburguesaPremium premium = new HamburguesaPremium("Brioche", "Angus", 10.00);
            // Intento fallido
            premium.AgregarExtra("Extra Queso", 0.50); 
            premium.MostrarDetalle();

            Console.WriteLine("\nPresione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}
