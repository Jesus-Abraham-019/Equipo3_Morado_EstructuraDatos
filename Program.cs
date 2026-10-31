using System;
using System.Collections.Generic;

// ===== LISTA DOBLEMENTE ENLAZADA: nodo =====
class NodoDoble
{
    public string Dato;          // nombre de la habitación
    public NodoDoble Anterior;   // apunta hacia atrás
    public NodoDoble Siguiente;  // apunta hacia adelante

    public NodoDoble(string dato)
    {
        Dato = dato;
        Anterior = null;
        Siguiente = null;
    }
}

// ===== LISTA DOBLEMENTE ENLAZADA: plano de la casa =====
class ListaDoble
{
    public NodoDoble Cabeza;
    public NodoDoble Cola;
    public int Cantidad;

    public void AgregarAlFinal(string dato)
    {
        NodoDoble nuevo = new NodoDoble(dato);
        if (Cabeza == null)
        {
            Cabeza = nuevo;
            Cola = nuevo;
        }
        else
        {
            nuevo.Anterior = Cola;
            Cola.Siguiente = nuevo;
            Cola = nuevo;
        }
        Cantidad++;
    }

    public void AgregarAlInicio(string dato)
    {
        Console.WriteLine("hola nns, aqui se veran lo que es una lista enlazada doble ");
        
        NodoDoble nuevo = new NodoDoble(dato);
        if (Cabeza == null)
        {
            Cabeza = nuevo;
            Cola = nuevo;
        }
        else
        {
            nuevo.Siguiente = Cabeza;
            Cabeza.Anterior = nuevo;
            Cabeza = nuevo;
        }
        Cantidad++;
    }

    // Inserta una habitación nueva justo después de otra existente
    public void InsertarDespuesDe(string referencia, string dato)
    {
        NodoDoble actual = Buscar(referencia);
        if (actual == null) return;

        if (actual == Cola)
        {
            AgregarAlFinal(dato);
            return;
        }

        NodoDoble nuevo = new NodoDoble(dato);
        nuevo.Anterior = actual;
        nuevo.Siguiente = actual.Siguiente;
        actual.Siguiente.Anterior = nuevo;
        actual.Siguiente = nuevo;
        Cantidad++;
    }

    // Elimina una habitación (por ejemplo, una puerta que queda bloqueada)
    public bool Eliminar(string dato)
    {
        NodoDoble nodo = Buscar(dato);
        if (nodo == null) return false;

        if (nodo.Anterior != null) nodo.Anterior.Siguiente = nodo.Siguiente;
        else Cabeza = nodo.Siguiente;

        if (nodo.Siguiente != null) nodo.Siguiente.Anterior = nodo.Anterior;
        else Cola = nodo.Anterior;

        Cantidad--;
        return true;
    }

    public NodoDoble Buscar(string dato)
    {
        NodoDoble actual = Cabeza;
        while (actual != null)
        {
            if (actual.Dato == dato) return actual;
            actual = actual.Siguiente;
        }
        return null;
    }

    public void MostrarAdelante()
    {
        NodoDoble actual = Cabeza;
        while (actual != null)
        {
            Console.Write(actual.Dato);
            if (actual.Siguiente != null) Console.Write(" <-> ");
            actual = actual.Siguiente;
        }
        Console.WriteLine();
    }

    public void MostrarAtras()
    {
        NodoDoble actual = Cola;
        while (actual != null)
        {
            Console.Write(actual.Dato);
            if (actual.Anterior != null) Console.Write(" <-> ");
            actual = actual.Anterior;
        }
        Console.WriteLine();
    }
}

class TheJoyOfCreation
{
    static void Main()
    {
        // ===== LISTA DOBLE: plano de la casa =====
        ListaDoble casa = new ListaDoble();
        casa.AgregarAlFinal("Sala de estar");
        casa.AgregarAlFinal("Cocina");
        casa.AgregarAlFinal("Pasillo");
        casa.AgregarAlFinal("Dormitorio");
        casa.AgregarAlFinal("Sótano");

        Console.WriteLine("=== Plano original (hacia adelante) ===");
        casa.MostrarAdelante();
        Console.WriteLine("\n=== Plano original (hacia atrás) ===");
        casa.MostrarAtras();

        // Modificaciones al plano
        casa.AgregarAlInicio("Entrada");
        casa.InsertarDespuesDe("Pasillo", "Baño");
        casa.Eliminar("Cocina"); // la puerta de la cocina quedó bloqueada

        Console.WriteLine("\n=== Plano modificado ===");
        casa.MostrarAdelante();
        Console.WriteLine($"Habitaciones disponibles: {casa.Cantidad}");

        // ===== LISTA: inventario del jugador =====
        List<string> inventario = new List<string>
        {
            "Linterna",
            "Llave del sótano",
            "Nota de William Afton"
        };

        // ===== LISTA: historial de habitaciones por donde pasó Charlotte =====
        List<string> historial = new List<string>();

        // ===== COLA: ruidos que Charlotte irá a investigar (FIFO) =====
        Queue<string> ruidos = new Queue<string>();
        ruidos.Enqueue("Dormitorio");
        ruidos.Enqueue("Sótano");
        ruidos.Enqueue("Entrada");

        string escondite = "Baño"; // donde se esconde el jugador

        Console.WriteLine($"\n=== Comienza la noche. Te escondes en: {escondite} ===");
        Console.WriteLine($"Ruidos pendientes: {ruidos.Count}");

        // Charlotte empieza en la Sala de estar
        NodoDoble charlotte = casa.Buscar("Sala de estar");
        historial.Add(charlotte.Dato);
        bool atrapado = false;

        while (ruidos.Count > 0 && !atrapado)
        {
            string destino = ruidos.Dequeue();
            NodoDoble objetivo = casa.Buscar(destino);

            if (objetivo == null)
            {
                Console.WriteLine($"\nRuido en {destino}, pero esa habitación ya no existe.");
                continue;
            }

            Console.WriteLine($"\nCharlotte escucha un ruido en: {destino}");

            // Decide si debe avanzar o retroceder en la lista
            bool adelante = EstaAdelante(charlotte, objetivo);
            Console.WriteLine(adelante ? "  (Avanza por la casa)" : "  (Retrocede por la casa)");

            while (charlotte != objetivo && !atrapado)
            {
                charlotte = adelante ? charlotte.Siguiente : charlotte.Anterior;
                atrapado = EntrarA(charlotte, escondite, inventario, historial);
            }
        }

        // ===== Resultado final =====
        Console.WriteLine("\n=== Recorrido de Charlotte ===");
        Console.WriteLine(string.Join(" -> ", historial));
        Console.WriteLine($"Habitaciones recorridas: {historial.Count}");
        Console.WriteLine($"Inventario final: {(inventario.Count > 0 ? string.Join(", ", inventario) : "vacío")}");

        if (!atrapado)
            Console.WriteLine("\n¡Sobreviviste la noche!");
    }

    // Revisa si el objetivo está delante de Charlotte (recorriendo con Siguiente)
    static bool EstaAdelante(NodoDoble actual, NodoDoble objetivo)
    {
        for (NodoDoble n = actual; n != null; n = n.Siguiente)
        {
            if (n == objetivo) return true;
        }
        return false;
    }

    // Charlotte entra a una habitación. Devuelve true si atrapa al jugador
    static bool EntrarA(NodoDoble nodo, string escondite, List<string> inventario, List<string> historial)
    {
        historial.Add(nodo.Dato);
        Console.WriteLine($"  Charlotte pasa por: {nodo.Dato}");

        if (nodo.Dato == escondite)
        {
            if (inventario.Contains("Linterna"))
            {
                inventario.Remove("Linterna");
                Console.WriteLine("  ¡Usas la linterna y Charlotte retrocede! (la linterna se quemó)");
                return false;
            }

            Console.WriteLine("  ¡Charlotte te encontró sin linterna! GAME OVER");
            return true;
        }

        return false;
    }
}