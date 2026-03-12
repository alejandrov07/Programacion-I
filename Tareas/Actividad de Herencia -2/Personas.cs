using System;

class Persona
{
    public string Nombre;

    public void Saludar()
    {
        Console.WriteLine("Hola, mi nombre es " + Nombre);
    }
}

class Estudiante : Persona
{
    public void Estudiar()
    {
        Console.WriteLine(Nombre + " está estudiando");
    }
}

class Program
{
    static void Main()
    {
        Estudiante estudiante = new Estudiante();
        estudiante.Nombre = "Carlos";

        estudiante.Saludar();
        estudiante.Estudiar();
    }
}