using System;
using System.Collections.Generic;

namespace EscapeEstructuras
{
    // 1. ESTRUCTURA CON NODOS PROPIA: ÁRBOL BINARIO DE BÚSQUEDA (BST)
    // Aplicación: Registro de récords de tiempo en el limbo (segundos sobrevividos)
    public class NodoArbol
    {
        public int SegundosSobrevividos;
        public string NombreIntento;
        public NodoArbol Izquierdo;
        public NodoArbol Derecho;

        public NodoArbol(int segundos, string intento)
        {
            SegundosSobrevividos = segundos;
            NombreIntento = intento;
            Izquierdo = null;
            Derecho = null;
        }
    }

    public class ArbolBinarioBusqueda
    {
        private NodoArbol raiz;

        // Operación 1: Insertar récord
        public void Insertar(int segundos, string intento)
        {
            raiz = InsertarRecursivo(raiz, segundos, intento);
            Console.WriteLine($"[BST] Récord guardado: {intento} ({segundos}s)");
        }

        private NodoArbol InsertarRecursivo(NodoArbol actual, int segundos, string intento)
        {
            if (actual == null)
                return new NodoArbol(segundos, intento);

            if (segundos < actual.SegundosSobrevividos)
                actual.Izquierdo = InsertarRecursivo(actual.Izquierdo, segundos, intento);
            else if (segundos > actual.SegundosSobrevividos)
                actual.Derecho = InsertarRecursivo(actual.Derecho, segundos, intento);

            return actual;
        }

        // Operación 2: Recorrer / Mostrar en orden (de menor a mayor tiempo)
        public void MostrarEnOrden()
        {
            Console.WriteLine("\n--- Historial de Tiempos de Supervivencia (Menor a Mayor) ---");
            RecorridoInOrden(raiz);
            Console.WriteLine("----------------------------------------------------------\n");
        }

        private void RecorridoInOrden(NodoArbol nodo)
        {
            if (nodo != null)
            {
                RecorridoInOrden(nodo.Izquierdo);
                Console.WriteLine($"- {nodo.NombreIntento}: {nodo.SegundosSobrevividos} segundos");
                RecorridoInOrden(nodo.Derecho);
            }
        }

        // Operación 3: Buscar un récord por tiempo exacto
        public bool Buscar(int segundos)
        {
            return BuscarRecursivo(raiz, segundos);
        }

        private bool BuscarRecursivo(NodoArbol actual, int segundos)
        {
            if (actual == null) return false;
            if (actual.SegundosSobrevividos == segundos) return true;

            return segundos < actual.SegundosSobrevividos
                ? BuscarRecursivo(actual.Izquierdo, segundos)
                : BuscarRecursivo(actual.Derecho, segundos);
        }
    }
    
    // 2 y 3. ESTRUCTURAS CON COLECCIONES DE C# (HashSet y PriorityQueue)
    public static class ModuloPaquete4
    {
        // 2. HashSet: Recuerdos/Coleccionables únicos encontrados en la casa
        private static HashSet<string> recuerdosEncontrados = new HashSet<string>();

        public static void ProbarHashSet()
        {
            Console.WriteLine("\n=== PRUEBA DE HASHSET (Coleccionables Únicos) ===");
            
            // Operación 1: Insertar (Add)
            recuerdosEncontrados.Add("Llave Vieja del Desván");
            recuerdosEncontrados.Add("Fotografía Quemada");
            bool repetido = recuerdosEncontrados.Add("Llave Vieja del Desván"); // No se agregará
            Console.WriteLine($"Intento de duplicar 'Llave Vieja': {(repetido ? "Agregado" : "Rechazado (ya existe)")}");

            // Operación 2: Mostrar todos
            Console.WriteLine("\nColeccionables actuales:");
            foreach (var item in recuerdosEncontrados)
                Console.WriteLine($"* {item}");

            // Operación 3: Buscar (Contains)
            bool tieneFoto = recuerdosEncontrados.Contains("Fotografía Quemada");
            Console.WriteLine($"\n¿Tiene 'Fotografía Quemada'?: {tieneFoto}");
        }

        // 3. PriorityQueue: Manejo de amenazas de monstruos por prioridad de atención
        public static void ProbarPriorityQueue()
        {
            Console.WriteLine("\n=== PRUEBA DE PRIORITYQUEUE (Amenazas de Monstruos) ===");
            // Prioridad menor = Más urgente (1 = crítico, 3 = bajo)
            PriorityQueue<string, int> colaAmenazas = new PriorityQueue<string, int>();

            // Operación 1: Encolar con prioridad (Enqueue)
            colaAmenazas.Enqueue("El Acechador Dual (en cámaras)", 2);
            colaAmenazas.Enqueue("El Invasor del Generador (apagón inminente)", 1);
            colaAmenazas.Enqueue("La Presencia Aracnoide (esquina)", 3);

            // Operación 2 y 3: Mostrar y Desencolar según urgencia (Dequeue)
            Console.WriteLine("Atendiendo amenazas en orden de peligro:");
            while (colaAmenazas.Count > 0)
            {
                colaAmenazas.TryDequeue(out string monstruo, out int prioridad);
                Console.WriteLine($"[Nivel {prioridad}] Atendiendo a: {monstruo}");
            }
        }
    }
}