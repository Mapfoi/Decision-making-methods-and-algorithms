# Maximin Algorithm — Self-Learning Classification

A detailed description of the program with code explanations.

---

## 1. Program Purpose

The program implements the **maximin algorithm** for self-learning classification of objects in a multidimensional feature space. The algorithm automatically determines the number of classes and their centers (kernels) without prior specification of the number of clusters.

---

## 2. Input Data

### 2.1 Object Set

Objects are defined as a set of **N** vectors **X = {X₁, X₂, ..., Xₙ}**, each of dimension **d** (number of features).

- **N** — from 100 to 100,000 objects;
- **d** — fixed dimension **2** (two-dimensional feature space).

### 2.2 Random Object Generation

Features are generated randomly using `System.Random` within the specified range `[minValue, maxValue]`:

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

- **count** — number of objects N;
- **dimension** — dimension d;
- **minValue, maxValue** — range of feature values (e.g., 0–100 or −10–10);
- **seed** — random generator seed (`null` or −1 for random sequence).

---

## 3. Maximin Algorithm

### 3.1 General Scheme

The algorithm iteratively builds a set of kernels (class centers) and assigns objects to the nearest kernels. On each iteration, a criterion is checked: if the farthest object from its kernel exceeds threshold T, a new kernel is created.

### 3.2 Step 1: First Kernel Initialization

The first kernel coincides with the first object in the sample:

```csharp
// Step 1: First kernel — first object
kernels.Add((double[])_objects[0].Clone());
```

### 3.3 Step 2: Assigning Objects to Nearest Kernels

For each object, the Euclidean distance to all kernels is computed. The object is assigned to the class with the nearest kernel:

```csharp
// Step 2: Assign each object to the nearest kernel
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

Euclidean distance between vectors **a** and **b**:

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

### 3.4 Step 3: Maximum Distances δₖᵢ

For each class **k**, the maximum distance from objects of that class to their kernel is found — this is **δₖᵢ**:

```csharp
// Step 3: Maximum distances δₖᵢ for each class
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

### 3.5 Step 4: Global Maximum δₖₚ

Among all δₖᵢ, the maximum value — **δₖₚ** — and the corresponding object are selected:

```csharp
// Step 4: Global maximum δₖₚ
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

### 3.6 Step 5: Threshold T

Threshold **T** is half the arithmetic mean of distances between all current kernels:

$$T = \frac{1}{2} \cdot \frac{1}{C_m^2} \sum_{i<j} \rho(N_i, N_j)$$

where **ρ** is Euclidean distance and **m** is the number of kernels.

```csharp
// Step 5: Threshold T = half of average distance between kernels
if (kernels.Count == 1)
{
    // For one kernel: T = 0, to create second kernel for any δₖₚ > 0
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

With one kernel there are no pairs, so T = 0 — any positive δₖₚ leads to creation of the second kernel.

### 3.7 Step 6: Stopping Criterion and New Kernel Creation

- If **δₖₚ ≤ T** — the algorithm terminates (all objects are sufficiently close to their kernels).
- If **δₖₚ > T** — a new kernel is created at the object with maximum distance.

```csharp
// Step 6: Stopping criterion — δₖₚ ≤ T or max number of kernels reached
if (globalMax <= threshold || kernels.Count >= _objects.Length)
{
    isTerminated = true;
}
else
{
    // Create new kernel at the object with maximum distance
    kernels.Add((double[])_objects[globalMaxObj].Clone());
    newKernelCreated = true;
}
```

---

## 4. Output Data (for Each Iteration)

### 4.1 MaximinIteration Structure

The state of one iteration is stored in the `MaximinIteration` class:

```csharp
public class MaximinIteration
{
    public int IterationNumber { get; set; }           // Iteration number
    public IReadOnlyList<double[]> Kernels { get; set; }  // Kernels {N₁, N₂, ..., Nₘ}
    public IReadOnlyList<int> ObjectAssignments { get; set; }  // Object assignments
    public IReadOnlyList<double> MaxDistancesPerClass { get; set; }  // δₖᵢ per class
    public double GlobalMaxDistance { get; set; }      // δₖₚ
    public int GlobalMaxObjectIndex { get; set; }      // Index of object with δₖₚ
    public double Threshold { get; set; }             // Threshold T
    public bool NewKernelCreated { get; set; }        // New kernel created?
    public bool IsTerminated { get; set; }             // Algorithm terminated?
}
```

### 4.2 Interface Display

The "Iteration Details" panel shows:

- list of kernels with coordinates;
- maximum distances δₖᵢ per class;
- global maximum δₖₚ and object number;
- threshold T;
- decision (new kernel created / algorithm terminated);
- assignment of first 50 objects to classes.

```csharp
sb.AppendLine("--- Kernels (class centers) ---");
for (int k = 0; k < iter.Kernels.Count; k++)
{
    var coords = string.Join(", ", iter.Kernels[k].Select(x => x.ToString("F2")));
    sb.AppendLine($"  N{k + 1}: ({coords})");
}
sb.AppendLine($"Global maximum δₖₚ = {iter.GlobalMaxDistance:F4}");
sb.AppendLine($"Threshold T = {iter.Threshold:F4}");
```

---

## 5. Graphical Interface

### 5.1 Parameters Panel

- **Number of objects N** — 100–100,000;
- **Dimension** — fixed (d = 2);
- **Min./max. value** — feature generation range;
- **Seed** — −1 for random sequence;
- **Generate** — creates objects;
- **Run Maximin** — executes the algorithm.

### 5.2 Iteration Viewer

- **Prev** and **Next** buttons — switch between iterations;
- **Slider** — quick jump to desired iteration;
- Displays iteration number and current kernel count.

### 5.3 Visualization

A scatter plot is built for two features (X₁, X₂):

```csharp
// Determine bounds from first two features
double minX = _objects.Min(o => o[0]);
double maxX = _objects.Max(o => o[0]);
double minY = _objects.Min(o => o[1]);
double maxY = _objects.Max(o => o[1]);
// ... scale to pixels ...
float ToX(double x) => pad + (float)((x - minX) / rangeX * w);
float ToY(double y) => panelPlot.Height - pad - (float)((y - minY) / rangeY * h);
```

- Objects are colored by class (different colors);
- Kernels are shown as black circles with labels N₁, N₂, …;
- For N > 5000, a sample of points is displayed for faster rendering.

---

## 6. Workflow

1. Set parameters (N, range, seed).
2. Click **Generate** — random objects are created.
3. Click **Run Maximin** — the algorithm executes.
4. Switch iterations with buttons or slider.
5. Analyze the plot and text details for each iteration.

---

## 7. Project Files

| File | Purpose |
|------|----------|
| `MaximinAlgorithm.cs` | `MaximinIteration` model — iteration state |
| `MaximinEngine.cs` | Algorithm logic and object generation |
| `Form1.cs` | Event handlers and visualization |
| `Form1.Designer.cs` | UI layout |

---

## 8. Running

```bash
cd "DM Lab1 Maximin"
dotnet run
```

Or open the solution in Visual Studio and run the project.
