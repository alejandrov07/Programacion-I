using System;

class Empleado
{
    public double Salario;

    public void MostrarSalario()
    {
        Console.WriteLine("El salario es: " + Salario);
    }
}

class Gerente : Empleado
{
    public void TomarDecision()
    {
        Console.WriteLine("El gerente está tomando decisiones");
    }
}

class Program
{
    static void Main()
    {
        Gerente gerente = new Gerente();
        gerente.Salario = 5000;

        gerente.MostrarSalario();
        gerente.TomarDecision();
    }
}