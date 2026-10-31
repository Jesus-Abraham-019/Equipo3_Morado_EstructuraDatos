# Equipo3_Morado_EstructuraDatos

# ESC4PE — Estructuras de datos

Juego de supervivencia inspirado en el ciclo de *The Joy of Creation*:
sobrevives de medianoche a 6:00 AM mientras tres criaturas te buscan.
El lore es propio; las mecánicas y reglas siguen el mismo esquema.

## Paquete 1 (líder) — estructuras implementadas

| Estructura | Tipo |
|---|---|
| Lista enlazada simple | Propia (nodos) |
| Arreglo / matriz 2D | Propia (arreglo bidimensional) |
| Pila `Stack<T>` | Colección de C# |

## Operaciones por estructura

| Estructura | Insertar | Recorrer / Mostrar | Buscar | Eliminar |
|---|---|---|---|---|
| Lista enlazada simple | `Agregar()` | `Mostrar()`, `Lineas()` | `Buscar()`, `BuscarTexto()` | `EliminarUltimo()`, `EliminarPorTexto()` |
| Matriz 2D | `this[f,c] = valor` | `Dibujar()` | `Buscar(char)`, `Contar(char)` | `BorrarCelda()`, `Limpiar()` |
| Pila `Stack<T>` | `Push()` | `MostrarHistorial()`, `MostrarRastro()` | `HistorialContiene()`, `ContarAccion()` | `Pop()` (deshacer) |

## Tabla comparativa

| Estructura | Memoria | Acceso | Costo insertar | Costo eliminar | Cuándo conviene | Ejemplo en el juego |
|---|---|---|---|---|---|---|
| Lista enlazada simple | Dinámica | Secuencial, \(O(n)\) | \(O(1)\) al final | \(O(n)\) al final, \(O(1)\) al inicio | Cuando crece sin saber cuánto y solo se recorre en orden | Registro de eventos de la noche |
| Arreglo / matriz 2D | Estática (tamaño fijo) | Directo por índice, \(O(1)\) | \(O(1)\) en una celda | \(O(1)\) en una celda | Cuando hay posiciones fijas en una rejilla | Mapa de la oficina con puertas y monstruos |
| Pila `Stack<T>` | Dinámica | Solo el tope, \(O(1)\) | \(O(1)\) | \(O(1)\) | Cuando lo último es lo primero que importa (LIFO) | Deshacer acciones y rastro de cada monstruo |

## Opciones del menú y estructura que usan

| Opción | Estructura |
|---|---|
| 7) Deshacer última acción | Pila |
| 8) Ver registro de la noche | Lista enlazada |
| 9) Ver historial completo | Pila |
| 10) Buscar en el historial | Pila |
| 11) Coordenadas de un monstruo | Matriz 2D + Pila (rastro) |
| 12) Buscar en el registro | Lista enlazada |
| 13) Borrar último evento | Lista enlazada |

## Quién hizo qué

- **Lista enlazada simple:** [tu nombre] — nodos, inserción al final, recorrido, búsqueda y eliminación.
- **Matriz 2D:** [tu nombre] — rejilla, dibujo, búsqueda de símbolos y borrado de celdas.
- **Pila `Stack<T>`:** [tu nombre] — historial de deshacer, búsqueda en historial y rastro de monstruos.

## Cómo ejecutar

```bash
dotnet new console -n ESC4PE
# copia Program.cs dentro de la carpeta del proyecto
dotnet run

Qué estructura usaríamos en Unity y por qué:

Lista enlazada simple para el registro de eventos: los eventos de la noche llegan en orden y no sabemos cuántos serán.

Matriz 2D para el mapa de la oficina: Unity trabaja bien con rejillas y coordenadas fijas para posicionar puertas, cámaras y criaturas.

Pila para el historial de deshacer y el rastro de cada monstruo: lo último que pasó es lo que el jugador necesita recordar.