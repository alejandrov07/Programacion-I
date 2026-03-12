using System;

class Figura
{
    public void Dibujar()
    {
        Console.WriteLine("Se está dibujando una figura");
    }
}

class Circulo : Figura
{
    public void CalcularArea()
    {
        Console.WriteLine("Calculando el área del círculo");
    }
}

class Program
{
    static void Main()
    {
        Circulo circulo = new Circulo();

        circulo.Dibujar();
        circulo.CalcularArea();
    }
}