using System;
using System.Collections.Generic;

namespace ProyectoAves
{
    class Program
    {
        static void Main(string[] args)
        {

            // Crear una lista polimórfica de aves
            List<Ave> aves = new List<Ave>();

            // Crear instancias de diferentes tipos de aves
            Aguila aguila = new Aguila("Furia", "Águila Real", 4.5, 5, 240);
            Pinguino pinguino = new Pinguino("Tux", "Pingüino Emperador", 35, 3, 500);
            Loro loro = new Loro("Paco", "Guacamayo", 1.2, 4, "Rojo y Verde", 150);
            Gallina gallina = new Gallina("Cloquis", "Gallina Leghorn", 2.0, 2, 45, true);

            // Agregar todas las aves a la lista
            aves.Add(aguila);
            aves.Add(pinguino);
            aves.Add(loro);
            aves.Add(gallina);


            Console.WriteLine("Información de Cada Ave");

            foreach (Ave ave in aves)
            {
                ave.MostrarInformacion();
                Console.WriteLine();
            }
            
            Console.WriteLine("Método Volar() - Polimórfico");

            foreach (Ave ave in aves)
            {
                ave.Volar();
            }
            
            Console.WriteLine("Método Cantar() - Polimórfico");

            foreach (Ave ave in aves)
            {
                ave.Cantar();
            }
            
            Console.WriteLine("Método Comer() - Polimórfico");

            aguila.Comer("peces");
            pinguino.Comer("krill");
            loro.Comer("frutas");
            gallina.Comer("semillas");
            
            Console.WriteLine("Método ObtenerSonido() - Polimórfico");

            foreach (Ave ave in aves)
            {
                Console.WriteLine($"{ave.Nombre}: {ave.ObtenerSonido()}");
            }
            
            Console.WriteLine("Métodos Específicos de Cada Clase");

            // Métodos específicos del Águila
            Console.WriteLine("--- Métodos del Águila ---");
            aguila.Cazar();
            Console.WriteLine();

            // Métodos específicos del Pingüino
            Console.WriteLine("--- Métodos del Pingüino ---");
            pinguino.Bucear();
            pinguino.NadarEnGrupo();
            Console.WriteLine();

            // Métodos específicos del Loro
            Console.WriteLine("--- Métodos del Loro ---");
            loro.Hablar("¡Hola amigo!");
            loro.Imitar("sonido de lluvia");
            Console.WriteLine();

            // Métodos específicos de la Gallina
            Console.WriteLine("--- Métodos de la Gallina ---");
            gallina.ProducirHuevo();
            gallina.BuscarComida();
            Console.WriteLine();
            
            Console.WriteLine("Acceso a Propiedades Específicas");

            // Verificar y acceder a propiedades específicas
            foreach (Ave ave in aves)
            {
                if (ave is Aguila)
                {
                    Aguila aguilaTemp = (Aguila)ave;
                    Console.WriteLine($"El águila {aguilaTemp.Nombre} vuela a {aguilaTemp.VelocidadVuelo} km/h");
                }
                else if (ave is Pinguino)
                {
                    Pinguino pinguinoTemp = (Pinguino)ave;
                    Console.WriteLine($"El pingüino {pinguinoTemp.Nombre} bucea hasta {pinguinoTemp.ProfundidadBuceo} metros");
                }
                else if (ave is Loro)
                {
                    Loro loroTemp = (Loro)ave;
                    Console.WriteLine($"El loro {loroTemp.Nombre} de color {loroTemp.Color} puede decir {loroTemp.CapacidadPalabras} palabras");
                }
                else if (ave is Gallina)
                {
                    Gallina gallinaTemp = (Gallina)ave;
                    Console.WriteLine($"La gallina {gallinaTemp.Nombre} ha producido {gallinaTemp.HuevosProducidos} huevos");
                }
            }
            
            Console.WriteLine("Método Dormir() - Polimórfico");

            foreach (Ave ave in aves)
            {
                ave.Dormir();
            }
            
            Console.WriteLine("Presiona una tecla para salir.");

            Console.ReadKey();
        }
    }
}
