using System;
using System.Collections.Generic;
using System.Text;

namespace CifradorHill
{
    class Utilidades
    {
        // convierte letras con acento a su version en mayusculas
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

        // convierte caracteres a indices: a-z son 0-25 y el espacio es 26
        public static List<int> TextoANumeros(string texto)
        {
            var numeros = new List<int>();
            foreach (char c in texto)
            {
                if (c == '_')
                    numeros.Add(26);
                else
                    numeros.Add(c - 'A');
            }
            return numeros;
        }

        // operacion inversa: transforma indices numericos en caracteres legibles
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

        // algoritmo de euclides para hallar el maximo comun divisor
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

        // valida la existencia del inverso modular del determinante
        public static bool EsInvertibleMod(int[,] matriz, int mod)
        {
            int det = (int)Math.Round(Determinante(matriz));
            return MCD(((det % mod) + mod) % mod, mod) == 1;
        }

        // calculo de determinante para matrices cuadradas pequeñas (2x2 y 3x3)
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

        // calcula la transpuesta de la matriz de cofactores
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
                        double cofactor = Math.Pow(-1, i + j) * Determinante(sub);
                        adj[j, i] = (int)Math.Round(cofactor); 
                    }
                }
            }
            return adj;
        }

        // extrae una submatriz eliminando la fila y columna indicadas
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
        
        // multiplica matriz por vector y aplica el modulo al resultado
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
    
    // implementacion de la logica de cifrado y descifrado de hill
    class HillCipher
    {
        private int[,] _matriz;
        private int _n;

        // inicializa la clave y valida que sea apta para el modulo 27
        public HillCipher(int[,] matrizClave)
        {
            if (!Utilidades.EsInvertibleMod(matrizClave, 27))
                throw new ArgumentException("la matriz clave no es invertible modulo 27.");

            _matriz = matrizClave;
            _n = matrizClave.GetLength(0);
        }

        public int Determinante()
        {
            return (int)Math.Round(Utilidades.Determinante(_matriz));
        }

        // busca el inverso multiplicativo en el grupo modular por busqueda lineal
        public int InversoModular(int a, int m)
        {
            a = ((a % m) + m) % m;
            for (int x = 1; x < m; x++)
            {
                if ((a * x) % m == 1)
                    return x;
            }
            throw new Exception("no existe inverso modular.");
        }

        // cifra el mensaje procesandolo en bloques del tamaño de la matriz
        public string Cifrar(string texto)
        {
            string textoNormalizado = Utilidades.NormalizarTexto(texto).Replace(" ", "_");
            List<int> numeros = Utilidades.TextoANumeros(textoNormalizado);

            while (numeros.Count % _n != 0)
                numeros.Add(23);

            var cifrado = new List<int>();

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
        
        // descifra aplicando la matriz inversa modular sobre los bloques
        public string Descifrar(string textoCifrado)
        {
            textoCifrado = Utilidades.NormalizarTexto(textoCifrado);
            List<int> numerosCifrado = Utilidades.TextoANumeros(textoCifrado);

            int det = Determinante();
            int detInv = InversoModular(((det % 27) + 27) % 27, 27);
            int[,] adjunta = Utilidades.MatrizAdjunta(_matriz);

            int[,] claveInversa = new int[_n, _n];
            for (int i = 0; i < _n; i++)
                for (int j = 0; j < _n; j++)
                    claveInversa[i, j] = ((detInv * adjunta[i, j]) % 27 + 27) % 27;

            while (numerosCifrado.Count % _n != 0)
                numerosCifrado.Add(23);

            var descifrado = new List<int>();

            for (int i = 0; i < numerosCifrado.Count; i += _n)
            {
                int[] bloque = new int[_n];
                for (int j = 0; j < _n; j++)
                    bloque[j] = numerosCifrado[i + j];

                int[] bloqueDescifrado = Utilidades.MultiplicarMatrizVector(claveInversa, bloque, 27);
                descifrado.AddRange(bloqueDescifrado);
            }

            string mensajeDescifrado = Utilidades.NumerosATexto(descifrado);
            mensajeDescifrado = mensajeDescifrado.Replace("_", " ");
            mensajeDescifrado = mensajeDescifrado.TrimEnd('X');

            if (mensajeDescifrado.Length > 0)
                mensajeDescifrado = char.ToUpper(mensajeDescifrado[0]) +
                                    mensajeDescifrado.Substring(1).ToLower();

            return mensajeDescifrado;
        }
    }

    // clase de entrada para la ejecucion por consola
    class Program
    {
        static void Main(string[] args)
        {
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
                    Console.WriteLine("opcion no valida.");
                }
            }
        }
    }
}
