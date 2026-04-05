using System;
using System.Collections.Generic;
using System.Text;

namespace CifradorHill
{
    // clase con metodos de apoyo para operaciones matematicas y de texto
    class Utilidades
    {
        // limpia el texto, quita acentos, caracteres especiales y pasa a mayusculas
        public static string NormalizarTexto(string texto)
        {
            var reemplazos = new Dictionary<char, char>
            {
                {'Á','A'}, {'É','E'}, {'Í','I'}, {'Ó','O'}, {'Ú','U'},
                {'Ü','U'}, {'Ñ','N'},
                {'á','A'}, {'é','E'}, {'í','I'}, {'ó','O'}, {'ú','U'},
                {'ü','U'}, {'ñ','N'}
            };

            var sb = new StringBuilder();
            foreach (char c in texto.ToUpper())
            {
                if (reemplazos.ContainsKey(c))
                    sb.Append(reemplazos[c]);
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        // transforma las letras en indices numericos (a=0, b=1, etc.)
        public static List<int> TextoANumeros(string texto)
        {
            var numeros = new List<int>();
            foreach (char c in texto)
            {
                // el guion bajo se trata como el indice 26
                if (c == '_')
                    numeros.Add(26);
                else
                    numeros.Add(c - 'A');
            }
            return numeros;
        }

        // transforma los indices numericos de vuelta a caracteres
        public static string NumerosATexto(List<int> numeros)
        {
            var sb = new StringBuilder();
            foreach (int n in numeros)
            {
                if (n == 26)
                    sb.Append('_');
                else
                    sb.Append((char)(n + 'A'));
            }
            return sb.ToString();
        }

        // calcula el maximo comun divisor usando el algoritmo de euclides
        public static int MCD(int a, int b)
        {
            while (b != 0)
            {
                int t = b;
                b = a % b;
                a = t;
            }
            return Math.Abs(a);
        }

        // comprueba si la matriz tiene inversa valida en el modulo dado
        public static bool EsInvertibleMod(int[,] matriz, int mod)
        {
            int det = (int)Math.Round(Determinante(matriz));
            // debe ser coprimo con el modulo para ser invertible
            return MCD(((det % mod) + mod) % mod, mod) == 1;
        }

        // resuelve el determinante para matrices de tamano 2 o 3
        public static double Determinante(int[,] matriz)
        {
            int n = matriz.GetLength(0);
            if (n == 2)
            {
                return matriz[0, 0] * matriz[1, 1] - matriz[0, 1] * matriz[1, 0];
            }
            else if (n == 3)
            {
                return matriz[0, 0] * (matriz[1, 1] * matriz[2, 2] - matriz[1, 2] * matriz[2, 1])
                     - matriz[0, 1] * (matriz[1, 0] * matriz[2, 2] - matriz[1, 2] * matriz[2, 0])
                     + matriz[0, 2] * (matriz[1, 0] * matriz[2, 1] - matriz[1, 1] * matriz[2, 0]);
            }
            else
            {
                throw new NotSupportedException("solo se soportan matrices 2x2 y 3x3.");
            }
        }

        // obtiene la matriz adjunta necesaria para calcular la inversa
        public static int[,] MatrizAdjunta(int[,] m)
        {
            int n = m.GetLength(0);
            int[,] adj = new int[n, n];

            if (n == 2)
            {
                adj[0, 0] =  m[1, 1];
                adj[0, 1] = -m[0, 1];
                adj[1, 0] = -m[1, 0];
                adj[1, 1] =  m[0, 0];
            }
            else if (n == 3)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        int[,] sub = SubMatriz(m, i, j);
                        // calculo de cofactores y transposicion simultanea
                        double cofactor = Math.Pow(-1, i + j) * Determinante(sub);
                        adj[j, i] = (int)Math.Round(cofactor); 
                    }
                }
            }
            return adj;
        }

        // genera una matriz menor eliminando una fila y una columna especificas
        public static int[,] SubMatriz(int[,] m, int fila, int col)
        {
            int n = m.GetLength(0);
            int[,] sub = new int[n - 1, n - 1];
            int ri = 0;
            for (int i = 0; i < n; i++)
            {
                if (i == fila) continue;
                int rj = 0;
                for (int j = 0; j < n; j++)
                {
                    if (j == col) continue;
                    sub[ri, rj] = m[i, j];
                    rj++;
                }
                ri++;
            }
            return sub;
        }
        
        // realiza la multiplicacion lineal entre la matriz y el vector bajo modulo
        public static int[] MultiplicarMatrizVector(int[,] matriz, int[] vector, int mod)
        {
            int n = matriz.GetLength(0);
            int[] resultado = new int[n];
            for (int i = 0; i < n; i++)
            {
                int suma = 0;
                for (int j = 0; j < n; j++)
                    suma += matriz[i, j] * vector[j];
                resultado[i] = ((suma % mod) + mod) % mod;
            }
            return resultado;
        }
    }
    
    // clase que gestiona la logica principal del cifrado hill
    class HillCipher
    {
        private int[,] _matriz;
        private int _n;

        // constructor que valida la viabilidad de la clave antes de iniciar
        public HillCipher(int[,] matrizClave)
        {
            if (!Utilidades.EsInvertibleMod(matrizClave, 27))
                throw new ArgumentException("la matriz clave no es invertible modulo 27. elija otra.");

            _matriz = matrizClave;
            _n = matrizClave.GetLength(0);
        }

        // devuelve el determinante de la matriz de la instancia
        public int Determinante()
        {
            return (int)Math.Round(Utilidades.Determinante(_matriz));
        }

        // encuentra el numero x tal que (a * x) % m == 1
        public int InversoModular(int a, int m)
        {
            a = ((a % m) + m) % m;
            for (int x = 1; x < m; x++)
            {
                if ((a * x) % m == 1)
                    return x;
            }
            throw new Exception("no existe inverso modular para este valor.");
        }

        // procesa el texto plano para convertirlo en texto cifrado
        public string Cifrar(string texto)
        {
            // prepara el texto y cambia espacios por guiones bajos
            string textoNormalizado = Utilidades.NormalizarTexto(texto).Replace(" ", "_");
            List<int> numeros = Utilidades.TextoANumeros(textoNormalizado);

            // añade caracteres de relleno si el bloque no esta completo
            while (numeros.Count % _n != 0)
                numeros.Add(23); // se usa la x como relleno

            var cifrado = new List<int>();

            // aplica la multiplicacion por bloques de tamano n
            for (int i = 0; i < numeros.Count; i += _n)
            {
                int[] bloque = new int[_n];
                for (int j = 0; j < _n; j++)
                    bloque[j] = numeros[i + j];

                int[] bloqueCifrado = Utilidades.MultiplicarMatrizVector(_matriz, bloque, 27);
                cifrado.AddRange(bloqueCifrado);
            }

            return Utilidades.NumerosATexto(cifrado);
        }
        
        // procesa el texto cifrado para recuperar el mensaje original
        public string Descifrar(string textoCifrado)
        {
            textoCifrado = Utilidades.NormalizarTexto(textoCifrado);
            List<int> numerosCifrado = Utilidades.TextoANumeros(textoCifrado);

            // calcula la matriz inversa modular para el descifrado
            int det = Determinante();
            int detInv = InversoModular(((det % 27) + 27) % 27, 27);
            int[,] adjunta = Utilidades.MatrizAdjunta(_matriz);

            int[,] claveInversa = new int[_n, _n];
            for (int i = 0; i < _n; i++)
                for (int j = 0; j < _n; j++)
                    claveInversa[i, j] = ((detInv * adjunta[i, j]) % 27 + 27) % 27;

            // asegura que la longitud sea multiplo de la dimension de la matriz
            while (numerosCifrado.Count % _n != 0)
                numerosCifrado.Add(23);

            var descifrado = new List<int>();

            // multiplica cada bloque cifrado por la matriz inversa
            for (int i = 0; i < numerosCifrado.Count; i += _n)
            {
                int[] bloque = new int[_n];
                for (int j = 0; j < _n; j++)
                    bloque[j] = numerosCifrado[i + j];

                int[] bloqueDescifrado = Utilidades.MultiplicarMatrizVector(claveInversa, bloque, 27);
                descifrado.AddRange(bloqueDescifrado);
            }

            // limpieza final del mensaje: quita rellenos y restaura espacios
            string mensajeDescifrado = Utilidades.NumerosATexto(descifrado);
            mensajeDescifrado = mensajeDescifrado.Replace("_", " ");
            mensajeDescifrado = mensajeDescifrado.TrimEnd('X');

            // ajusta el formato de salida a tipo oracion
            if (mensajeDescifrado.Length > 0)
                mensajeDescifrado = char.ToUpper(mensajeDescifrado[0]) +
                                    mensajeDescifrado.Substring(1).ToLower();

            return mensajeDescifrado;
        }
    }

    // punto de entrada de la aplicacion de consola
    class Program
    {
        static void Main(string[] args)
        {
            // definicion de la matriz de seguridad para el cifrado
            int[,] matrizClave = {
                { 1, 2, 3 },
                { 0, 1, 4 },
                { 5, 6, 0 }
            };

            HillCipher hill;
            try
            {
                hill = new HillCipher(matrizClave);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("error: " + e.Message);
                return;
            }

            // bucle de interaccion con el usuario
            while (true)
            {
                Console.WriteLine("\nseleccione una opcion:");
                Console.WriteLine("1. cifrar un mensaje");
                Console.WriteLine("2. descifrar un mensaje");
                Console.WriteLine("3. salir");
                Console.Write("ingrese su eleccion (1-3): ");

                string opcion = Console.ReadLine()?.Trim();

                if (opcion == "1")
                {
                    Console.Write("ingrese el mensaje a cifrar: ");
                    string mensaje = Console.ReadLine();
                    string cifrado = hill.Cifrar(mensaje);
                    Console.WriteLine($"\nmensaje cifrado: {cifrado}");
                }
                else if (opcion == "2")
                {
                    Console.Write("ingrese el mensaje cifrado: ");
                    string mensajeCifrado = Console.ReadLine();
                    string descifrado = hill.Descifrar(mensajeCifrado);
                    Console.WriteLine($"\nmensaje descifrado: {descifrado}");
                }
                else if (opcion == "3")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("opcion no valida. intente nuevamente.");
                }
            }
        }
    }
}
