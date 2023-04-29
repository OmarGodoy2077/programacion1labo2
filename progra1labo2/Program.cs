using System;

class Program
{
    static void Main()
    {





        SonidoFunkyTown();


        // Selecciona la dificultad
        Console.WriteLine("Seleccione la dificultad:");
        Console.WriteLine("1. Fácil (3x3)");
        Console.WriteLine("2. Medio (5x5)");
        Console.WriteLine("3. Difícil (8x8)");
        int tamaño = 0;
        switch (Console.ReadLine())
        {
            case "1":
                tamaño = 3;
                break;
            case "2":
                tamaño = 5;
                break;
            case "3":
                tamaño = 8;
                break;
            default:
                Console.WriteLine("Opción no válida. Se usará la dificultad fácil (3x3) por defecto.");
                tamaño = 3;
                break;
        }

        // Genera el tablero de forma aleatoria
        Random random = new Random();
        int[,] tablero = new int[tamaño, tamaño];
        int totalMinas = 0;
        for (int i = 0; i < tablero.GetLength(0); i++)
        {
            for (int j = 0; j < tablero.GetLength(1); j++)
            {
                tablero[i, j] = random.Next(2);
                if (tablero[i, j] == 1)
                {
                    totalMinas++;
                }
            }
        }


        int vidas = 3;
        int intentos = 0;
        int aciertos = 0;

        MostrarSinMina(tablero);

        // Muestra instrucciones al inicio del juego
        Console.WriteLine("Bienvenido al juego. Encuentra todas las posiciones marcadas con un 1 en el tablero.");
        Console.WriteLine("Tienes 3 vidas para encontrar todas las posiciones. ¡Buena suerte!");
        Console.WriteLine($"Total de minas: {totalMinas}");
        Console.WriteLine("El juego comenzará en 6 segundos...");
        System.Threading.Thread.Sleep(6000);

        // Inicia el temporizador
        DateTime tiempoInicio = DateTime.Now;

        while (vidas > 0)
        {
            Console.Clear();
            Console.WriteLine($"Intentos: {intentos} - Vidas: {vidas} - Minas restantes: {totalMinas - aciertos}");
            DibujarTablero(tablero);

            IngresoCoordenadas(tablero, ref vidas, ref intentos, ref aciertos);

            if (VerificarTablero(tablero))
            {
                TimeSpan tiempoTotal = DateTime.Now - tiempoInicio;
                Console.Clear();
                SonidoVictoria();
                Console.WriteLine("¡Felicidades, has ganado el juego!");
                Console.WriteLine($"Intentos totales: {intentos}");
                Console.WriteLine($"Tiempo total: {tiempoTotal}");
                Console.ReadLine();
                break;
            }
        }

        if (vidas == 0)
        {
            Console.Clear();
            SonidoDerrota();
            Console.WriteLine("¡Lo siento, has perdido el juego!");
            Console.ReadLine();
        }
    }

    static void MostrarSinMina(int[,] tablero)
    {
        Console.Write("¿Desea ver la ubicación donde NO hay una mina antes de comenzar? (s/n): ");
        char respuesta = Console.ReadKey().KeyChar;
        Console.WriteLine();

        if (respuesta == 's' || respuesta == 'S')
        {
            bool sinMinaEncontrada = false;
            for (int i = 0; i < tablero.GetLength(0); i++)
            {
                for (int j = 0; j < tablero.GetLength(1); j++)
                {
                    if (tablero[i, j] == 0)
                    {
                        Console.WriteLine($"No hay una mina en la fila {i} y columna {j}.");
                        sinMinaEncontrada = true;
                        break;
                    }
                }
                if (sinMinaEncontrada)
                {
                    break;
                }
            }
        }
    }

    static void DibujarTablero(int[,] tablero)
    {
        Console.WriteLine("  " + new string('-', tablero.GetLength(1) * 2));
        for (int i = 0; i < tablero.GetLength(0); i++)
        {
            Console.Write($"{i}|");
            for (int j = 0; j < tablero.GetLength(1); j++)
            {
                Console.Write(" ");
                char celda = tablero[i, j] switch
                {
                    0 => ' ',
                    1 => ' ',
                    -1 => '*',
                    -2 => 'X',
                    _ => ' ',
                };
                ConsoleColor colorActual = Console.ForegroundColor;
                if (celda == '*')
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else if (celda == 'X')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.Write(celda);
                Console.ForegroundColor = colorActual;
            }
            Console.Write("|");
            Console.WriteLine();
        }
        Console.WriteLine("  " + new string('-', tablero.GetLength(1) * 2));
        Console.WriteLine();
    }


    static void IngresoCoordenadas(int[,] tablero, ref int vidas, ref int intentos, ref int aciertos)
    {
        int fila = 0, columna = 0;

        // Resalta la palabra "fila" en el mensaje
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Ingrese la ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("fila");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" (0-2): ");
        Console.ForegroundColor = ConsoleColor.White;
        bool filaValida = int.TryParse(Console.ReadLine(), out fila);

        // Resalta la palabra "columna" en el mensaje
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Ingrese la ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("columna");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" (0-2): ");
        Console.ForegroundColor = ConsoleColor.White;
        bool columnaValida = int.TryParse(Console.ReadLine(), out columna);

        if (filaValida && columnaValida && fila >= 0 && fila < tablero.GetLength(0) && columna >= 0 && columna < tablero.GetLength(1))
        {
            if (tablero[fila, columna] == 1)
            {
                Console.Beep();
                tablero[fila, columna] = -1;
                aciertos++;

                // Otorga una vida extra cada 3 aciertos
                if (aciertos % 3 == 0)
                {
                    vidas++;
                    Console.WriteLine("¡Ganaste una vida extra!");
                    System.Threading.Thread.Sleep(1500);
                }
            }
            else if (tablero[fila, columna] == 0)
            {
                tablero[fila, columna] = -2;
                vidas--;
                if (vidas > 0)
                {
                    SonidoDerrota();
                }
            }
            intentos++;
        }

    }

    static bool VerificarTablero(int[,] tablero)
    {
        foreach (int celda in tablero)
        {
            if (celda == 1)
            {
                return false;
            }
        }
        return true;
    }



    static void SonidoVictoria()
    {
        Console.Beep(523, 150);
        Console.Beep(587, 150);
        Console.Beep(659, 150);
        Console.Beep(784, 150);
    }

    static void SonidoDerrota()
    {
        Console.Beep(784, 150);
        Console.Beep(659, 150);
        Console.Beep(587, 150);
        Console.Beep(523, 150);
    }
    static void SonidoGameOverMarioBros()
    {
        int duracion = 200;
        int pausa = 100;

        // G4
        Console.Beep(392, duracion);
        System.Threading.Thread.Sleep(pausa);

        // E4
        Console.Beep(330, duracion);
        System.Threading.Thread.Sleep(pausa);

        // E4
        Console.Beep(330, duracion);
        System.Threading.Thread.Sleep(duracion + pausa);

        // Rest
        System.Threading.Thread.Sleep(duracion);

        // C4
        Console.Beep(262, duracion);
        System.Threading.Thread.Sleep(pausa);

        // G3
        Console.Beep(196, duracion);
        System.Threading.Thread.Sleep(duracion + pausa);
    }

    static void SonidoFunkyTown()
    {





        int notaCorta = 200;
        int notaLarga = 400;

        Console.Beep(587, notaCorta);
        Console.Beep(587, notaCorta);
        Console.Beep(698, notaCorta);
        Console.Beep(698, notaCorta);
        Console.Beep(784, notaLarga);
        System.Threading.Thread.Sleep(200);

        Console.Beep(698, notaCorta);
        Console.Beep(784, notaCorta);
        Console.Beep(880, notaLarga);
        System.Threading.Thread.Sleep(200);

        Console.Beep(698, notaCorta);
        Console.Beep(784, notaCorta);
        Console.Beep(880, notaLarga);
        System.Threading.Thread.Sleep(200);

        Console.Beep(698, notaCorta);
        Console.Beep(784, notaCorta);
        Console.Beep(880, notaLarga);

        string arteASCII = @"                                )___(,
                          _______ / __ / _
                 ___ /===========| ___
____       __[\\]___ / ____________ | __[///]   __
 \   \_____[\\]__ / ___________________________\__[//]___
  \40A                                                  |
   \                                                  /
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~";

        Console.WriteLine(arteASCII);
    }
    static void barco()
    {

    }
}




