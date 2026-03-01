namespace DM_Lab1_Maximin;

/// <summary>
/// Состояние одной итерации алгоритма максимина.
/// </summary>
public class MaximinIteration
{
    /// <summary>Номер итерации (начиная с 1).</summary>
    public int IterationNumber { get; set; }

    /// <summary>Центры классов (ядра).</summary>
    public IReadOnlyList<double[]> Kernels { get; set; } = [];

    /// <summary>Принадлежность объектов: индекс ядра для каждого объекта.</summary>
    public IReadOnlyList<int> ObjectAssignments { get; set; } = [];

    /// <summary>Максимальные расстояния δₖᵢ для каждого класса.</summary>
    public IReadOnlyList<double> MaxDistancesPerClass { get; set; } = [];

    /// <summary>Глобальный максимум δₖₚ.</summary>
    public double GlobalMaxDistance { get; set; }

    /// <summary>Индекс объекта, давшего глобальный максимум.</summary>
    public int GlobalMaxObjectIndex { get; set; }

    /// <summary>Порог T для создания нового ядра.</summary>
    public double Threshold { get; set; }

    /// <summary>Решение: создано новое ядро (true) или алгоритм завершён (false).</summary>
    public bool NewKernelCreated { get; set; }

    /// <summary>Алгоритм завершён.</summary>
    public bool IsTerminated { get; set; }
}
