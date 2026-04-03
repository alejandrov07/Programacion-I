using System;

class Bus
{
    public string Nombre;
    public int Asientos;
    public int Precio;
    public int Pasajeros;

    public Bus(string nombre, int asientos, int precio, int pasajeros)
    {
        Nombre = nombre;
        Asientos = asientos;
        Precio = precio;
        Pasajeros = pasajeros;
    }

    public int Ventas()
    {
        return Precio * Pasajeros;
    }

    public int Disponibles()
    {
        return Asientos - Pasajeros;
    }

    public void Mostrar()
    {
        Console.WriteLine("Autobus " + Nombre + ", " + Pasajeros + " Pasajeros, Ventas " + Ventas() + ", quedan " + Disponibles() + " asientos disponibles");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Bus bus1 = new Bus("Platinum", 22, 1000, 5);
        Bus bus2 = new Bus("Gold", 9, 1333, 3);
        
        bus1.Mostrar();
        bus2.Mostrar();
    }
}
