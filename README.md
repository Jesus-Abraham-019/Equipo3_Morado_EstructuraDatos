# Equipo3_Morado_EstructuraDatos
## Proyecto: ESC4P3?… - Sistema de Estructuras de Datos

### Integrantes y Responsabilidades
- **Paquete 4:** Implementación de Árbol Binario de Búsqueda (BST con nodos propios), Conjunto (`HashSet<T>`) y Cola de Prioridad (`PriorityQueue<T, P>`).

---

### Tabla Comparativa de Estructuras de Datos (Paquete 4)

| Estructura | Memoria | Acceso | Inserción / Eliminación | ¿Cuándo conviene usarla? | Ejemplo en ESC4P3?… |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Árbol Binario de Búsqueda (BST)** | Dinámica (por nodos enlazados) | $O(\log n)$ promedio, $O(n)$ peor caso | $O(\log n)$ promedio | Cuando se requiere mantener datos ordenados continuamente y buscar rápido por clave. | Registro y ranking de tiempos de supervivencia en segundos. |
| **Conjunto (`HashSet<T>`)** | Dinámica (tabla hash) | $O(1)$ promedio | $O(1)$ promedio | Cuando se necesita verificar existencia instantánea y evitar cualquier elemento duplicado. | Inventario de recuerdos y coleccionables únicos recolectados en la casa. |
| **Cola de Prioridad (`PriorityQueue<T, P>`)** | Dinámica (montículo binario / heap) | $O(1)$ lectura del elemento más urgente | $O(\log n)$ | Cuando los eventos o tareas no se atienden por orden de llegada, sino por nivel de urgencia o peligro. | Sistema de IA para atender al monstruo o amenaza más crítica primero. |

---

### Cómo ejecutar el proyecto
1. Clonar el repositorio.
2. Abrir la terminal en la raíz del proyecto.
3. Ejecutar:
```bash
dotnet run