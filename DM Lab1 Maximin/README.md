# Алгоритм максимина — самообучающаяся классификация

Подробное описание работы программы с пояснениями к коду.

---

## 1. Назначение программы

Программа реализует **алгоритм максимина** для самообучающейся классификации объектов в многомерном пространстве признаков. Алгоритм автоматически определяет количество классов и их центры (ядра) без предварительного задания числа кластеров.

---

## 2. Входные данные

### 2.1 Множество объектов

Объекты задаются как набор из **N** векторов **X = {X₁, X₂, ..., Xₙ}**, каждый размерности **d** (количество признаков).

- **N** — от 100 до 100 000 объектов;
- **d** — фиксированная размерность **2** (двумерное пространство признаков).

### 2.2 Генерация случайных объектов

Признаки генерируются случайно с помощью `System.Random` в заданном диапазоне `[minValue, maxValue]`:

```csharp
public static double[][] GenerateRandomObjects(int count, int dimension, double minValue, double maxValue, int? seed = null)
{
    var rnd = seed.HasValue ? new Random(seed.Value) : new Random();
    var objects = new double[count][];
    double range = maxValue - minValue;
    for (int i = 0; i < count; i++)
    {
        objects[i] = new double[dimension];
        for (int j = 0; j < dimension; j++)
            objects[i][j] = minValue + rnd.NextDouble() * range;
    }
    return objects;
}
```

- **count** — количество объектов N;
- **dimension** — размерность d;
- **minValue, maxValue** — диапазон значений признаков (например, 0–100 или −10–10);
- **seed** — зерно генератора (`null` или −1 — случайная последовательность).

---

## 3. Алгоритм максимина

### 3.1 Общая схема

Алгоритм итеративно строит множество ядер (центров классов) и назначает объекты ближайшим ядрам. На каждой итерации проверяется критерий: если самый удалённый объект от своего ядра превышает порог T, создаётся новое ядро.

### 3.2 Шаг 1: Инициализация первого ядра

Первое ядро совпадает с первым объектом выборки:

```csharp
// Шаг 1: Первое ядро — первый объект
kernels.Add((double[])_objects[0].Clone());
```

### 3.3 Шаг 2: Назначение объектов ближайшим ядрам

Для каждого объекта вычисляется расстояние до всех ядер по формуле Евклида. Объект относится к классу с ближайшим ядром:

```csharp
// Шаг 2: Назначить каждый объект ближайшему ядру
for (int i = 0; i < _objects.Length; i++)
{
    double minDist = double.MaxValue;
    int bestK = 0;
    for (int k = 0; k < kernels.Count; k++)
    {
        double d = EuclideanDistance(_objects[i], kernels[k]);
        if (d < minDist)
        {
            minDist = d;
            bestK = k;
        }
    }
    assignments[i] = bestK;
}
```

Расстояние Евклида между векторами **a** и **b**:

```csharp
private static double EuclideanDistance(double[] a, double[] b)
{
    double sum = 0;
    for (int i = 0; i < a.Length; i++)
    {
        double d = a[i] - b[i];
        sum += d * d;
    }
    return Math.Sqrt(sum);
}
```

### 3.4 Шаг 3: Максимальные расстояния δₖᵢ

Для каждого класса **k** находится максимальное расстояние от объектов этого класса до их ядра — это **δₖᵢ**:

```csharp
// Шаг 3: Максимальные расстояния δₖᵢ для каждого класса
var maxDistPerClass = new double[kernels.Count];
var maxObjPerClass = new int[kernels.Count];
Array.Fill(maxDistPerClass, -1);

for (int i = 0; i < _objects.Length; i++)
{
    int k = assignments[i];
    double d = EuclideanDistance(_objects[i], kernels[k]);
    if (d > maxDistPerClass[k])
    {
        maxDistPerClass[k] = d;
        maxObjPerClass[k] = i;
    }
}
```

### 3.5 Шаг 4: Глобальный максимум δₖₚ

Среди всех δₖᵢ выбирается максимальное значение — **δₖₚ** — и объект, которому оно соответствует:

```csharp
// Шаг 4: Глобальный максимум δₖₚ
double globalMax = 0;
int globalMaxObj = 0;
for (int k = 0; k < kernels.Count; k++)
{
    if (maxDistPerClass[k] > globalMax)
    {
        globalMax = maxDistPerClass[k];
        globalMaxObj = maxObjPerClass[k];
    }
}
```

### 3.6 Шаг 5: Порог T

Порог **T** — половина среднего арифметического расстояния между всеми текущими ядрами.

```csharp
// Шаг 5: Порог T = половина среднего расстояния между ядрами
if (kernels.Count == 1)
{
    // Для одного ядра: T = 0, чтобы создать второе ядро при любом δₖₚ > 0
    threshold = 0;
}
else
{
    double sumDist = 0;
    int pairCount = 0;
    for (int i = 0; i < kernels.Count; i++)
    {
        for (int j = i + 1; j < kernels.Count; j++)
        {
            sumDist += EuclideanDistance(kernels[i], kernels[j]);
            pairCount++;
        }
    }
    threshold = pairCount > 0 ? 0.5 * (sumDist / pairCount) : 0;
}
```

При одном ядре пар нет, поэтому T = 0 — любое положительное δₖₚ приводит к созданию второго ядра.

### 3.7 Шаг 6: Критерий остановки и создание нового ядра

- Если **δₖₚ ≤ T** — алгоритм завершается (все объекты достаточно близки к своим ядрам).
- Если **δₖₚ > T** — создаётся новое ядро в точке объекта с максимальным расстоянием.

```csharp
// Шаг 6: Критерий остановки — δₖₚ ≤ T или достигнуто макс. число ядер
if (globalMax <= threshold || kernels.Count >= _objects.Length)
{
    isTerminated = true;
}
else
{
    // Создать новое ядро в точке объекта с максимальным расстоянием
    kernels.Add((double[])_objects[globalMaxObj].Clone());
    newKernelCreated = true;
}
```

---

## 4. Выходные данные (для каждой итерации)

### 4.1 Структура MaximinIteration

Состояние одной итерации хранится в классе `MaximinIteration`:

```csharp
public class MaximinIteration
{
    public int IterationNumber { get; set; }           // Номер итерации
    public IReadOnlyList<double[]> Kernels { get; set; }  // Ядра {N₁, N₂, ..., Nₘ}
    public IReadOnlyList<int> ObjectAssignments { get; set; }  // Принадлежность объектов
    public IReadOnlyList<double> MaxDistancesPerClass { get; set; }  // δₖᵢ по классам
    public double GlobalMaxDistance { get; set; }      // δₖₚ
    public int GlobalMaxObjectIndex { get; set; }      // Индекс объекта с δₖₚ
    public double Threshold { get; set; }             // Порог T
    public bool NewKernelCreated { get; set; }        // Создано новое ядро?
    public bool IsTerminated { get; set; }             // Алгоритм завершён?
}
```

### 4.2 Отображение в интерфейсе

В панели «Детали итерации» выводятся:

- список ядер с координатами;
- максимальные расстояния δₖᵢ по классам;
- глобальный максимум δₖₚ и номер объекта;
- порог T;
- решение (создано новое ядро / алгоритм завершён);
- принадлежность первых 50 объектов к классам.

```csharp
sb.AppendLine("--- Ядра (центры классов) ---");
for (int k = 0; k < iter.Kernels.Count; k++)
{
    var coords = string.Join(", ", iter.Kernels[k].Select(x => x.ToString("F2")));
    sb.AppendLine($"  N{k + 1}: ({coords})");
}
sb.AppendLine($"Глобальный максимум δₖₚ = {iter.GlobalMaxDistance:F4}");
sb.AppendLine($"Порог T = {iter.Threshold:F4}");
```

---

## 5. Графический интерфейс

### 5.1 Панель параметров

- **Количество объектов N** — 100–100 000;
- **Размерность** — фиксирована (d = 2);
- **Мин./макс. значение** — диапазон генерации признаков;
- **Зерно** — −1 для случайной последовательности;
- **Сгенерировать** — создаёт объекты;
- **Запустить максимин** — выполняет алгоритм.

### 5.2 Просмотр итераций

- Кнопки **«Пред»** и **«След»** — переключение между итерациями;
- **Ползунок** — быстрый переход к нужной итерации;
- Отображаются номер итерации и текущее количество ядер.

### 5.3 Визуализация

Строится scatter-plot по двум признакам (X₁, X₂):

```csharp
// Определяем границы по первым двум признакам
double minX = _objects.Min(o => o[0]);
double maxX = _objects.Max(o => o[0]);
double minY = _objects.Min(o => o[1]);
double maxY = _objects.Max(o => o[1]);
// ... масштабирование в пиксели ...
float ToX(double x) => pad + (float)((x - minX) / rangeX * w);
float ToY(double y) => panelPlot.Height - pad - (float)((y - minY) / rangeY * h);
```

- Объекты окрашены по классам (разные цвета);
- Ядра отображаются чёрными кругами с подписями N₁, N₂, …;
- При N > 5000 отображается выборка точек для ускорения отрисовки.

---

## 6. Последовательность работы

1. Задать параметры (N, d, диапазон, зерно).
2. Нажать **«Сгенерировать»** — создаются случайные объекты.
3. Нажать **«Запустить максимин»** — выполняется алгоритм.
4. Переключать итерации кнопками или ползунком.
5. Анализировать график и текстовые детали для каждой итерации.

---

## 7. Файлы проекта

| Файл | Назначение |
|------|------------|
| `MaximinAlgorithm.cs` | Модель `MaximinIteration` — состояние итерации |
| `MaximinEngine.cs` | Логика алгоритма и генерация объектов |
| `Form1.cs` | Обработчики событий и визуализация |
| `Form1.Designer.cs` | Разметка элементов интерфейса |

---

## 8. Запуск

```bash
cd "DM Lab1 Maximin"
dotnet run
```

Или открыть решение в Visual Studio и запустить проект.
