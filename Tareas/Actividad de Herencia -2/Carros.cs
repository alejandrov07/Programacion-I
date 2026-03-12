using System;

class Vehiculo
{
    public void Encender()
    {
        Console.WriteLine("El vehículo se ha encendido");
    }
}

class Carro : Vehiculo
{
    public void Acelerar()
    {
        Console.WriteLine("El carro está acelerando");
    }
}

class Program
{
    static void Main()
    {
        Carro carro = new Carro();
        carro.Encender();
        carro.Acelerar();
    }
}