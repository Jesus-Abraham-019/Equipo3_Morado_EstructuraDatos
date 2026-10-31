using System;

namespace EscapeEstructuras
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ESC4P3?… - PRUEBAS DE ESTRUCTURAS (PAQUETE 4) ===");

            // 1. Probar el Árbol Binario de Búsqueda (BST)
            ArbolBinarioBusqueda arbol = new ArbolBinarioBusqueda();
            arbol.Insertar(120, "Intento 1 - Atrapado en pasillo");
            arbol.Insertar(360, "Intento 2 - ESCAPE EXITOSO");
            arbol.Insertar(45, "Intento 3 - Jumpscare cámara");

            arbol.MostrarEnOrden();

            Console.WriteLine($"¿Existe récord de 360 segundos?: {arbol.Buscar(360)}");
            Console.WriteLine($"¿Existe récord de 200 segundos?: {arbol.Buscar(200)}");

            // 2. Probar HashSet (Coleccionables únicos)
            ModuloPaquete4.ProbarHashSet();

            // 3. Probar PriorityQueue (Monstruos por urgencia)
            ModuloPaquete4.ProbarPriorityQueue();

            Console.WriteLine("\nPresiona cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}
