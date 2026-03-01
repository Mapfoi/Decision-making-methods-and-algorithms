namespace DM_Lab1_Maximin;

public partial class Form1 : Form
{
    private double[][]? _objects;
    private MaximinEngine? _engine;
    private List<MaximinIteration> _iterations = [];
    private int _currentIterIndex;

    public Form1()
    {
        InitializeComponent();
        UpdateIterationControls(false);
    }

    private void BtnGenerate_Click(object? sender, EventArgs e)
    {
        int count = (int)numObjectCount.Value;
        const int dim = 2;
        double minVal = (double)numMinValue.Value;
        double maxVal = (double)numMaxValue.Value;
        int? seed = numSeed.Value >= 0 ? (int)numSeed.Value : null;

        if (minVal >= maxVal)
        {
            MessageBox.Show("Минимальное значение должно быть меньше максимального.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _objects = MaximinEngine.GenerateRandomObjects(count, dim, minVal, maxVal, seed);
            _engine = new MaximinEngine(_objects);
            _iterations = [];
            _currentIterIndex = 0;
            UpdateIterationControls(false);
            txtDetails.Clear();
            panelPlot.Invalidate();
            lblIterNum.Text = "Итерация: — / —";
            lblIterInfo.Text = "Ядер: 0";
            MessageBox.Show($"Сгенерировано {count} объектов (размерность 2).", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnRunMaximin_Click(object? sender, EventArgs e)
    {
        if (_engine == null || _objects == null)
        {
            MessageBox.Show("Сначала сгенерируйте объекты.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnRunMaximin.Enabled = false;
        btnGenerate.Enabled = false;
        try
        {
            _iterations = await Task.Run(() => _engine.Run());
            _currentIterIndex = Math.Max(0, _iterations.Count - 1);
            UpdateIterationControls(true);
            trackIteration.Maximum = Math.Max(0, _iterations.Count - 1);
            trackIteration.Value = _currentIterIndex;
            ShowIteration(_currentIterIndex);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnRunMaximin.Enabled = true;
            btnGenerate.Enabled = true;
        }
    }

    private void BtnPrev_Click(object? sender, EventArgs e)
    {
        if (_iterations.Count == 0) return;
        _currentIterIndex = Math.Max(0, _currentIterIndex - 1);
        trackIteration.Value = _currentIterIndex;
        ShowIteration(_currentIterIndex);
    }

    private void BtnNext_Click(object? sender, EventArgs e)
    {
        if (_iterations.Count == 0) return;
        _currentIterIndex = Math.Min(_iterations.Count - 1, _currentIterIndex + 1);
        trackIteration.Value = _currentIterIndex;
        ShowIteration(_currentIterIndex);
    }

    private void TrackIteration_Scroll(object? sender, EventArgs e)
    {
        if (_iterations.Count == 0) return;
        _currentIterIndex = trackIteration.Value;
        ShowIteration(_currentIterIndex);
    }

    private void UpdateIterationControls(bool hasIterations)
    {
        bool enabled = hasIterations && _iterations.Count > 0;
        btnPrev.Enabled = enabled;
        btnNext.Enabled = enabled;
        trackIteration.Enabled = enabled;
    }

    private void ShowIteration(int index)
    {
        if (index < 0 || index >= _iterations.Count) return;

        var iter = _iterations[index];
        lblIterNum.Text = $"Итерация: {iter.IterationNumber} / {_iterations.Count}";
        lblIterInfo.Text = $"Ядер: {iter.Kernels.Count}";

        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"=== Итерация {iter.IterationNumber} ===");
        sb.AppendLine();
        sb.AppendLine($"Количество ядер: {iter.Kernels.Count}");
        sb.AppendLine();

        sb.AppendLine("--- Ядра (центры классов) ---");
        for (int k = 0; k < iter.Kernels.Count; k++)
        {
            var coords = string.Join(", ", iter.Kernels[k].Select(x => x.ToString("F2")));
            sb.AppendLine($"  N{k + 1}: ({coords})");
        }
        sb.AppendLine();

        sb.AppendLine("--- Максимальные расстояния δₖᵢ ---");
        for (int k = 0; k < iter.MaxDistancesPerClass.Count; k++)
        {
            sb.AppendLine($"  Класс {k + 1}: δ = {iter.MaxDistancesPerClass[k]:F4}");
        }
        sb.AppendLine();
        sb.AppendLine($"Глобальный максимум δₖₚ = {iter.GlobalMaxDistance:F4}");
        sb.AppendLine($"  (объект #{iter.GlobalMaxObjectIndex + 1})");
        sb.AppendLine();
        sb.AppendLine($"Порог T = {iter.Threshold:F4}");
        sb.AppendLine();
        sb.AppendLine(iter.IsTerminated
            ? "Решение: Алгоритм завершён."
            : iter.NewKernelCreated
                ? "Решение: Создано новое ядро."
                : "Решение: Продолжение...");

        sb.AppendLine();
        sb.AppendLine("--- Принадлежность объектов (первые 50) ---");
        int showCount = Math.Min(50, iter.ObjectAssignments.Count);
        for (int i = 0; i < showCount; i++)
        {
            sb.AppendLine($"  Объект {i + 1} → Класс {iter.ObjectAssignments[i] + 1}");
        }
        if (iter.ObjectAssignments.Count > 50)
            sb.AppendLine($"  ... и ещё {iter.ObjectAssignments.Count - 50} объектов");

        txtDetails.Text = sb.ToString();
        panelPlot.Invalidate();
    }

    private void PanelPlot_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(Color.White);

        if (_objects == null || _iterations.Count == 0)
        {
            g.DrawString("Сгенерируйте объекты и запустите алгоритм максимина.",
                Font, Brushes.Gray, 20, 20);
            return;
        }

        var iter = _iterations[_currentIterIndex];
        int n = _objects.Length;

        // Определяем границы по первым двум признакам
        double minX = _objects.Min(o => o[0]);
        double maxX = _objects.Max(o => o[0]);
        double minY = _objects.Min(o => o[1]);
        double maxY = _objects.Max(o => o[1]);

        double rangeX = maxX - minX;
        double rangeY = maxY - minY;
        if (rangeX < 1e-9) rangeX = 1;
        if (rangeY < 1e-9) rangeY = 1;

        int pad = 50;
        int w = panelPlot.Width - 2 * pad;
        int h = panelPlot.Height - 2 * pad;

        float ToX(double x) => pad + (float)((x - minX) / rangeX * w);
        float ToY(double y) => panelPlot.Height - pad - (float)((y - minY) / rangeY * h);

        // Цвета для классов
        var colors = new[]
        {
            Color.FromArgb(65, 105, 225),   // RoyalBlue
            Color.FromArgb(220, 20, 60),    // Crimson
            Color.FromArgb(34, 139, 34),    // ForestGreen
            Color.FromArgb(255, 140, 0),    // DarkOrange
            Color.FromArgb(138, 43, 226),   // BlueViolet
            Color.FromArgb(0, 191, 255),    // DeepSkyBlue
            Color.FromArgb(255, 69, 0),     // OrangeRed
            Color.FromArgb(50, 205, 50),    // LimeGreen
            Color.FromArgb(255, 215, 0),    // Gold
            Color.FromArgb(255, 20, 147),   // DeepPink
            Color.FromArgb(75, 0, 130),     // Indigo
            Color.FromArgb(255, 165, 0),    // Orange
            Color.FromArgb(218, 112, 214),  // Orchid
            Color.FromArgb(64, 224, 208),   // Turquoise
            Color.FromArgb(210, 105, 30),   // Chocolate
            Color.FromArgb(255, 105, 180)   // HotPink
        };

        // Ограничиваем количество отображаемых точек для производительности
        int maxPoints = 10000;
        int step = n <= maxPoints ? 1 : n / maxPoints;

        using (var penKernel = new Pen(Color.Black, 3))
        {
            for (int i = 0; i < n; i += step)
            {
                int k = iter.ObjectAssignments[i];
                var c = colors[k % colors.Length];
                using (var brush = new SolidBrush(Color.FromArgb(180, c)))
                {
                    float px = ToX(_objects[i][0]);
                    float py = ToY(_objects[i][1]);
                    g.FillEllipse(brush, px - 2, py - 2, 4, 4);
                }
            }

            // Ядра — крупные чёрные круги
            for (int k = 0; k < iter.Kernels.Count; k++)
            {
                var kernel = iter.Kernels[k];
                float kx = ToX(kernel[0]);
                float ky = ToY(kernel[1]);
                g.DrawEllipse(penKernel, kx - 6, ky - 6, 12, 12);
                g.FillEllipse(Brushes.Black, kx - 4, ky - 4, 8, 8);
                g.DrawString($"N{k + 1}", Font, Brushes.Black, kx + 8, ky - 6);
            }
        }
    }
}
