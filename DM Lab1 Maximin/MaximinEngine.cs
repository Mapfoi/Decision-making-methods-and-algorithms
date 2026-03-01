namespace DM_Lab1_Maximin;

/// <summary>
/// Движок алгоритма максимина для самообучающейся классификации.
/// </summary>
public class MaximinEngine
{
    private readonly double[][] _objects;
    private readonly int _dimension;

    public MaximinEngine(double[][] objects)
    {
        _objects = objects ?? throw new ArgumentNullException(nameof(objects));
        if (_objects.Length == 0)
            throw new ArgumentException("Необходим хотя бы один объект.", nameof(objects));
        _dimension = _objects[0].Length;
        if (_dimension != 2)
            throw new ArgumentException("Программа поддерживает только размерность признаков d = 2.", nameof(objects));
        for (int i = 1; i < _objects.Length; i++)
        {
            if (_objects[i].Length != _dimension)
                throw new ArgumentException($"Объект {i} имеет размерность {_objects[i].Length}, ожидалась {_dimension}.");
        }
    }

    /// <summary>Количество объектов.</summary>
    public int ObjectCount => _objects.Length;

    /// <summary>Размерность признаков.</summary>
    public int Dimension => _dimension;

    /// <summary>Исходные объекты.</summary>
    public IReadOnlyList<double[]> Objects => _objects;

    /// <summary>
    /// Выполняет алгоритм максимина и возвращает все итерации.
    /// </summary>
    public List<MaximinIteration> Run(CancellationToken cancellationToken = default)
    {
        var iterations = new List<MaximinIteration>();
        var kernels = new List<double[]>();
        var assignments = new int[_objects.Length];

        // Шаг 1: Первое ядро — первый объект
        kernels.Add((double[])_objects[0].Clone());
        int iterNum = 0;

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            iterNum++;

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

            // Шаг 3: Максимальные расстояния для каждого класса
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

            // Шаг 4: Глобальный максимум
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

            // Шаг 5: Порог T = половина среднего расстояния между ядрами
            double threshold;
            bool newKernelCreated = false;
            bool isTerminated = false;

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

            iterations.Add(new MaximinIteration
            {
                IterationNumber = iterNum,
                Kernels = kernels.Select(k => (double[])k.Clone()).ToList(),
                ObjectAssignments = assignments.ToArray(),
                MaxDistancesPerClass = maxDistPerClass.ToArray(),
                GlobalMaxDistance = globalMax,
                GlobalMaxObjectIndex = globalMaxObj,
                Threshold = threshold,
                NewKernelCreated = newKernelCreated,
                IsTerminated = isTerminated
            });

            if (isTerminated)
                break;
        }

        return iterations;
    }

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

    /// <summary>
    /// Генерирует случайные объекты.
    /// </summary>
    /// <param name="count">Количество объектов (N).</param>
    /// <param name="dimension">Размерность признаков (d = 2).</param>
    /// <param name="minValue">Минимальное значение признака.</param>
    /// <param name="maxValue">Максимальное значение признака.</param>
    /// <param name="seed">Случайное зерно (null — случайное).</param>
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
}
