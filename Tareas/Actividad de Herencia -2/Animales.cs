using System;

class Animal
{
    public void Comer()
    {
        Console.WriteLine("El animal está comiendo");
    }
}

class Perro : Animal
{
    public void Ladrar()
    {
        Console.WriteLine("El perro está ladrando");
    }
}

class Program
{
    static void Main()
    {
        Perro perro = new Perro();
        perro.Comer();
        perro.Ladrar();
    }
}