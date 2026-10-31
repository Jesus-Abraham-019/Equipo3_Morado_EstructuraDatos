using System;
using System.Collections.Generic;
using System.Linq;

namespace ESC4PE
{
    // ============================================================
    //  1) LISTA ENLAZADA SIMPLE  ->  Registro de eventos de la noche
    // ============================================================
    public class NodoEvento
    {
        public string Hora { get; set; }
        public string Texto { get; set; }
        public NodoEvento Siguiente { get; set; }

        public NodoEvento(string hora, string texto)
        {
            Hora = hora;
            Texto = texto;
            Siguiente = null;
        }
    }

    public class RegistroNoche
    {
        private NodoEvento cabeza;
        private NodoEvento cola;
        public int Total { get; private set; }

        // ---------- INSERTAR ----------
        public void Agregar(string hora, string texto)
        {
            var nuevo = new NodoEvento(hora, texto);
            if (cabeza == null) { cabeza = nuevo; cola = nuevo; }
            else { cola.Siguiente = nuevo; cola = nuevo; }
            Total++;
        }

        // ---------- RECORRER / MOSTRAR ----------
        public void Mostrar()
        {
            Console.WriteLine("\n--- REGISTRO DE LA NOCHE ---");
            if (cabeza == null) { Console.WriteLine("(sin eventos)"); return; }
            var actual = cabeza;
            while (actual != null)
            {
                Console.WriteLine($"  [{actual.Hora}] {actual.Texto}");
                actual = actual.Siguiente;
            }
        }

        public IEnumerable<string> Lineas()
        {
            var actual = cabeza;
            while (actual != null)
            {
                yield return $"[{actual.Hora}] {actual.Texto}";
                actual = actual.Siguiente;
            }
        }

        // ---------- BUSCAR ----------
        public bool Buscar(string texto)
        {
            var actual = cabeza;
            while (actual != null)
            {
                if (actual.Texto.ToLower().Contains(texto.ToLower()))
                    return true;
                actual = actual.Siguiente;
            }
            return false;
        }

        public string BuscarTexto(string texto)
        {
            var actual = cabeza;
            while (actual != null)
            {
                if (actual.Texto.ToLower().Contains(texto.ToLower()))
                    return $"[{actual.Hora}] {actual.Texto}";
                actual = actual.Siguiente;
            }
            return null;
        }

        // ---------- ELIMINAR ----------
        public bool EliminarUltimo()
        {
            if (cabeza == null) return false;
            if (cabeza == cola) { cabeza = null; cola = null; Total--; return true; }

            var actual = cabeza;
            while (actual.Siguiente != cola) actual = actual.Siguiente;
            actual.Siguiente = null;
            cola = actual;
            Total--;
            return true;
        }

        public int EliminarPorTexto(string texto)
        {
            int borrados = 0;
            while (cabeza != null && cabeza.Texto.ToLower().Contains(texto.ToLower()))
            {
                cabeza = cabeza.Siguiente;
                borrados++;
            }
            if (cabeza == null) { cola = null; Total -= borrados; return borrados; }

            var actual = cabeza;
            while (actual.Siguiente != null)
            {
                if (actual.Siguiente.Texto.ToLower().Contains(texto.ToLower()))
                {
                    actual.Siguiente = actual.Siguiente.Siguiente;
                    borrados++;
                }
                else actual = actual.Siguiente;
            }
            cola = actual;
            Total -= borrados;
            return borrados;
        }
    }

    // ============================================================
    //  2) ARREGLO / MATRIZ 2D  ->  Mapa de la oficina
    // ============================================================
    public class Mapa
    {
        public const char VACIO = '.';
        public const char PUERTA_IZQ = 'L';
        public const char PUERTA_DER = 'R';
        public const char CAMARA = 'C';
        public const char JUGADOR = '@';
        public const char MONSTRUO = 'X';

        public int Filas { get; }
        public int Columnas { get; }
        private readonly char[,] celdas;

        public Mapa(int filas, int columnas)
        {
            Filas = filas;
            Columnas = columnas;
            celdas = new char[filas, columnas];
            for (int f = 0; f < filas; f++)
                for (int c = 0; c < columnas; c++)
                    celdas[f, c] = VACIO;
        }

        // ---------- INSERTAR / ACCESO ----------
        public char this[int f, int c]
        {
            get => celdas[f, c];
            set => celdas[f, c] = value;
        }

        public bool Dentro(int f, int c) =>
            f >= 0 && f < Filas && c >= 0 && c < Columnas;

        // ---------- RECORRER / MOSTRAR ----------
        public void Dibujar(string titulo)
        {
            Console.WriteLine($"\n{titulo}");
            Console.Write("    ");
            for (int c = 0; c < Columnas; c++) Console.Write(c + " ");
            Console.WriteLine();
            for (int f = 0; f < Filas; f++)
            {
                Console.Write($"  {f} ");
                for (int c = 0; c < Columnas; c++)
                    Console.Write(celdas[f, c] + " ");
                Console.WriteLine();
            }
        }

        // ---------- BUSCAR ----------
        public List<(int fila, int columna)> Buscar(char simbolo)
        {
            var resultado = new List<(int, int)>();
            for (int f = 0; f < Filas; f++)
                for (int c = 0; c < Columnas; c++)
                    if (celdas[f, c] == simbolo)
                        resultado.Add((f, c));
            return resultado;
        }

        public int Contar(char simbolo)
        {
            int total = 0;
            for (int f = 0; f < Filas; f++)
                for (int c = 0; c < Columnas; c++)
                    if (celdas[f, c] == simbolo) total++;
            return total;
        }

        // ---------- ELIMINAR ----------
        public bool BorrarCelda(int f, int c)
        {
            if (!Dentro(f, c)) return false;
            celdas[f, c] = VACIO;
            return true;
        }

        public void Limpiar()
        {
            for (int f = 0; f < Filas; f++)
                for (int c = 0; c < Columnas; c++)
                    celdas[f, c] = VACIO;
        }
    }

    // ============================================================
    //  MONSTRUO
    // ============================================================
    public class Monstruo
    {
        public string Nombre { get; }
        public int Fila { get; set; }
        public int Columna { get; set; }
        public int Progreso { get; set; }   // 0 = lejos, 100 = en tu puerta
        public int Velocidad { get; }
        public Stack<string> Rastro { get; } = new Stack<string>();

        public Monstruo(string nombre, int fila, int columna, int velocidad)
        {
            Nombre = nombre;
            Fila = fila;
            Columna = columna;
            Velocidad = velocidad;
        }

        public void Registrar(string paso) => Rastro.Push(paso);

        public string UltimoPaso() => Rastro.Count > 0 ? Rastro.Peek() : "(nada)";
    }

    // ============================================================
    //  JUEGO
    // ============================================================
    public class Juego
    {
        private readonly Mapa mapa = new Mapa(7, 9);
        private readonly RegistroNoche registro = new RegistroNoche();
        private readonly Stack<string> historial = new Stack<string>();

        private readonly List<Monstruo> monstruos = new List<Monstruo>
        {
            new Monstruo("UMBRAL", 0, 4, 12),
            new Monstruo("ACEHANTE", 0, 1, 8),
            new Monstruo("ESCUCHA", 0, 7, 6)
        };

        private int hora = 0;          // 0 = medianoche
        private int energia = 100;
        private int turno = 0;
        private bool puertaIzq = false;
        private bool puertaDer = false;
        private bool camaraAbierta = false;
        private bool luzIzq = false;
        private bool luzDer = false;
        private bool acechanteVisible = false;

        public void Jugar()
        {
            Presentacion();
            while (hora < 6 && energia > 0)
            {
                DibujarEstado();
                string accion = PedirAccion();
                if (accion == null) continue;
                Ejecutar(accion);
                if (hora >= 6) break;
                if (energia <= 0) break;
                TurnoMonstruos();
                if (RevisarCaptura()) return;
                turno++;
                if (turno % 3 == 0) hora++;
            }
            Final();
        }

        // ---------- presentación ----------
        private void Presentacion()
        {
            Console.Clear();
            Console.WriteLine("=====================================");
            Console.WriteLine("            E S C 4 P E");
            Console.WriteLine("=====================================");
            Console.WriteLine("Sobrevive de medianoche a 6:00 AM.");
            Console.WriteLine("Tres criaturas te buscan. Una corre,");
            Console.WriteLine("una acecha y una te escucha.");
            Console.WriteLine("=====================================\n");
            registro.Agregar("00:00", "Inicio del turno. Oficina sellada.");
            Console.WriteLine("Presiona ENTER para comenzar...");
            Console.ReadLine();
        }

        // ---------- estado ----------
        private void DibujarEstado()
        {
            Console.Clear();
            Console.WriteLine($"=== ESC4PE === Hora: {hora}:00 AM | Energia: {energia}% | Turno: {turno}");
            Console.WriteLine($"Puerta izq: {(puertaIzq ? "CERRADA" : "abierta")} | " +
                              $"Puerta der: {(puertaDer ? "CERRADA" : "abierta")} | " +
                              $"Camaras: {(camaraAbierta ? "ON" : "OFF")}");

            mapa.Limpiar();
            mapa[3, 4] = Mapa.JUGADOR;
            mapa[3, 0] = Mapa.PUERTA_IZQ;
            mapa[3, 8] = Mapa.PUERTA_DER;
            mapa[1, 4] = Mapa.CAMARA;

            foreach (var m in monstruos)
            {
                if (m.Nombre == "ACEHANTE" && !acechanteVisible) continue;
                if (mapa.Dentro(m.Fila, m.Columna) && mapa[m.Fila, m.Columna] == Mapa.VACIO)
                    mapa[m.Fila, m.Columna] = Mapa.MONSTRUO;
            }

            mapa.Dibujar("Mapa de la oficina  (@ tu, L/R puertas, C camara, X monstruo)");

            Console.WriteLine("\nCriaturas:");
            foreach (var m in monstruos)
            {
                string estado = m.Progreso >= 100 ? "EN TU PUERTA"
                              : m.Progreso >= 60 ? "cerca"
                              : m.Progreso >= 30 ? "a media distancia"
                              : "lejos";
                Console.WriteLine($"  {m.Nombre,-9} -> {estado,-18} (ultimo paso: {m.UltimoPaso()})");
            }
        }

        // ---------- entrada ----------
        private string PedirAccion()
        {
            Console.WriteLine("\nAcciones:");
            Console.WriteLine(" 1) Cerrar/abrir puerta izquierda   2) Cerrar/abrir puerta derecha");
            Console.WriteLine(" 3) Encender/apagar camaras         4) Revisar camaras (gasta energia)");
            Console.WriteLine(" 5) Luz izquierda                   6) Luz derecha");
            Console.WriteLine(" 7) Deshacer ultima accion          8) Ver registro de la noche");
            Console.WriteLine(" 9) Ver historial completo de acciones");
            Console.WriteLine("10) Buscar en el historial");
            Console.WriteLine("11) Coordenadas de un monstruo en el mapa");
            Console.WriteLine("12) Buscar en el registro de la noche");
            Console.WriteLine("13) Borrar ultimo evento del registro");
            Console.Write("> ");
            return Console.ReadLine()?.Trim();
        }

        private void Ejecutar(string accion)
        {
            switch (accion)
            {
                case "1":
                    puertaIzq = !puertaIzq;
                    historial.Push($"Puerta izq {(puertaIzq ? "cerrada" : "abierta")}");
                    energia -= 3;
                    registro.Agregar(HoraTexto(), $"Puerta izquierda {(puertaIzq ? "cerrada" : "abierta")}");
                    break;
                case "2":
                    puertaDer = !puertaDer;
                    historial.Push($"Puerta der {(puertaDer ? "cerrada" : "abierta")}");
                    energia -= 3;
                    registro.Agregar(HoraTexto(), $"Puerta derecha {(puertaDer ? "cerrada" : "abierta")}");
                    break;
                case "3":
                    camaraAbierta = !camaraAbierta;
                    historial.Push($"Camaras {(camaraAbierta ? "ON" : "OFF")}");
                    energia -= 2;
                    registro.Agregar(HoraTexto(), $"Camaras {(camaraAbierta ? "encendidas" : "apagadas")}");
                    break;
                case "4":
                    energia -= 5;
                    acechanteVisible = true;
                    historial.Push("Revisar camaras");
                    registro.Agregar(HoraTexto(), "Revisaste las camaras.");
                    Console.WriteLine("\nRevisando...");
                    System.Threading.Thread.Sleep(600);
                    break;
                case "5":
                    luzIzq = !luzIzq;
                    energia -= 4;
                    historial.Push($"Luz izq {(luzIzq ? "ON" : "OFF")}");
                    registro.Agregar(HoraTexto(), $"Luz izquierda {(luzIzq ? "encendida" : "apagada")}");
                    break;
                case "6":
                    luzDer = !luzDer;
                    energia -= 4;
                    historial.Push($"Luz der {(luzDer ? "ON" : "OFF")}");
                    registro.Agregar(HoraTexto(), $"Luz derecha {(luzDer ? "encendida" : "apagada")}");
                    break;
                case "7":
                    if (historial.Count > 0)
                    {
                        string ultima = historial.Pop();
                        registro.Agregar(HoraTexto(), $"Deshecho: {ultima}");
                        Console.WriteLine($"\nDeshecho: {ultima}");
                        System.Threading.Thread.Sleep(500);
                    }
                    else
                    {
                        Console.WriteLine("\nNo hay acciones para deshacer.");
                        System.Threading.Thread.Sleep(600);
                    }
                    break;
                case "8":
                    registro.Mostrar();
                    Console.WriteLine("\nENTER para continuar...");
                    Console.ReadLine();
                    break;
                case "9":
                    MostrarHistorial();
                    Console.WriteLine("\nENTER para continuar...");
                    Console.ReadLine();
                    break;
                case "10":
                    Console.Write("\nQue accion buscas? ");
                    string buscar = Console.ReadLine() ?? "";
                    Console.WriteLine(HistorialContiene(buscar)
                        ? $"\nSi esta. La hiciste {ContarAccion(buscar)} vez/veces."
                        : "\nNo aparece en el historial.");
                    Console.WriteLine("\nENTER para continuar...");
                    Console.ReadLine();
                    break;
                case "11":
                    Console.Write("\nNombre del monstruo (UMBRAL / ACEHANTE / ESCUCHA): ");
                    string nombre = (Console.ReadLine() ?? "").ToUpper();
                    var m = monstruos.FirstOrDefault(x => x.Nombre == nombre);
                    if (m == null) Console.WriteLine("\nNo existe ese monstruo.");
                    else
                    {
                        Console.WriteLine($"\n{m.Nombre} esta en fila {m.Fila}, columna {m.Columna}.");
                        MostrarRastro(m);
                    }
                    Console.WriteLine("\nENTER para continuar...");
                    Console.ReadLine();
                    break;
                case "12":
                    Console.Write("\nBuscar en el registro: ");
                    string texto = Console.ReadLine() ?? "";
                    string encontrado = registro.BuscarTexto(texto);
                    Console.WriteLine(encontrado != null ? $"\nEncontrado: {encontrado}" : "\nSin coincidencias.");
                    Console.WriteLine("\nENTER para continuar...");
                    Console.ReadLine();
                    break;
                case "13":
                    Console.WriteLine(registro.EliminarUltimo()
                        ? "\nUltimo evento eliminado."
                        : "\nEl registro esta vacio.");
                    System.Threading.Thread.Sleep(700);
                    break;
                default:
                    Console.WriteLine("\nAccion no valida.");
                    System.Threading.Thread.Sleep(500);
                    break;
            }
            if (energia < 0) energia = 0;
        }

        // ---------- pila: recorrer y buscar ----------
        private void MostrarHistorial()
        {
            Console.WriteLine("\n--- HISTORIAL DE ACCIONES (tope arriba) ---");
            if (historial.Count == 0) { Console.WriteLine("(vacio)"); return; }
            int i = 1;
            foreach (var accion in historial)
                Console.WriteLine($"  {i++}. {accion}");
        }

        private bool HistorialContiene(string texto) =>
            historial.Any(a => a.ToLower().Contains(texto.ToLower()));

        private int ContarAccion(string texto) =>
            historial.Count(a => a.ToLower().Contains(texto.ToLower()));

        private void MostrarRastro(Monstruo m)
        {
            Console.WriteLine($"\n--- RASTRO DE {m.Nombre} ---");
            if (m.Rastro.Count == 0) { Console.WriteLine("(sin pasos)"); return; }
            foreach (var paso in m.Rastro)
                Console.WriteLine($"  {paso}");
        }

        // ---------- turno de los monstruos ----------
        private void TurnoMonstruos()
        {
            // UMBRAL: corre siempre hacia ti
            var umbral = monstruos[0];
            umbral.Progreso += umbral.Velocidad;
            MoverHacia(umbral, 3, 4);
            umbral.Registrar($"Turno {turno}: progreso {umbral.Progreso}");

            // ACEHANTE: solo avanza si la miras por camara
            var acechante = monstruos[1];
            if (camaraAbierta || acechanteVisible)
            {
                acechante.Progreso += acechante.Velocidad;
                MoverHacia(acechante, 3, 4);
                acechante.Registrar($"Turno {turno}: te vio por camara");
                registro.Agregar(HoraTexto(), "La que acecha se acerco mientras mirabas.");
            }
            else
            {
                acechante.Registrar($"Turno {turno}: se quedo quieta");
            }

            // ESCUCHA: el ruido la atrae
            var escucha = monstruos[2];
            int ruido = 0;
            if (camaraAbierta) ruido += 4;
            if (puertaIzq || puertaDer) ruido += 3;
            if (luzIzq || luzDer) ruido += 2;
            escucha.Progreso += escucha.Velocidad + ruido;
            MoverHacia(escucha, 3, 4);
            escucha.Registrar($"Turno {turno}: ruido {ruido}");

            acechanteVisible = false;
            luzIzq = false;
            luzDer = false;
        }

        private void MoverHacia(Monstruo m, int objetivoF, int objetivoC)
        {
            if (m.Fila < objetivoF) m.Fila++;
            else if (m.Fila > objetivoF) m.Fila--;
            if (m.Columna < objetivoC) m.Columna++;
            else if (m.Columna > objetivoC) m.Columna--;
        }

        // ---------- captura ----------
        private bool RevisarCaptura()
        {
            foreach (var m in monstruos)
            {
                if (m.Progreso < 100) continue;

                bool puertaCerrada = m.Columna <= 4 ? puertaIzq : puertaDer;
                if (puertaCerrada)
                {
                    m.Progreso = 55;
                    m.Columna = m.Columna <= 4 ? 1 : 7;
                    m.Fila = 0;
                    registro.Agregar(HoraTexto(), $"{m.Nombre} choco con la puerta y retrocedio.");
                    continue;
                }

                Console.Clear();
                Console.WriteLine($"\n{m.Nombre} entro a la oficina.");
                Console.WriteLine("No sobreviviste la noche.");
                registro.Agregar(HoraTexto(), $"{m.Nombre} te atrapo. Fin de la partida.");
                registro.Mostrar();
                return true;
            }
            return false;
        }

        // ---------- final ----------
        private void Final()
        {
            Console.Clear();
            if (hora >= 6)
            {
                Console.WriteLine("\n6:00 AM. Sobreviviste la noche en ESC4PE.");
                registro.Agregar("06:00", "Turno completado. Sobreviviste.");
            }
            else
            {
                Console.WriteLine("\nTe quedaste sin energia. Las puertas se abrieron solas.");
                registro.Agregar(HoraTexto(), "Sin energia. Fin de la partida.");
            }
            registro.Mostrar();
            Console.WriteLine("\nPresiona ENTER para salir.");
            Console.ReadLine();
        }

        private string HoraTexto() => $"{hora}:00";
    }

    // ============================================================
    //  PROGRAMA PRINCIPAL
    // ============================================================
    public static class Program
    {
        public static void Main()
        {
            bool seguir = true;
            while (seguir)
            {
                new Juego().Jugar();
                Console.Write("\nJugar otra noche? (s/n): ");
                seguir = Console.ReadLine()?.Trim().ToLower() == "s";
            }
        }
    }
}
