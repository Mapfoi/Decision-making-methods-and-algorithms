namespace DM_Lab1_Maximin
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            grpParams = new GroupBox();
            lblSeed = new Label();
            numSeed = new NumericUpDown();
            lblMaxVal = new Label();
            numMaxValue = new NumericUpDown();
            lblMinVal = new Label();
            numMinValue = new NumericUpDown();
            lblCount = new Label();
            numObjectCount = new NumericUpDown();
            btnGenerate = new Button();
            btnRunMaximin = new Button();
            grpIteration = new GroupBox();
            lblIterInfo = new Label();
            trackIteration = new TrackBar();
            lblIterNum = new Label();
            btnPrev = new Button();
            btnNext = new Button();
            grpDetails = new GroupBox();
            txtDetails = new TextBox();
            panelPlot = new Panel();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            grpParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numSeed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMaxValue).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMinValue).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numObjectCount).BeginInit();
            grpIteration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackIteration).BeginInit();
            grpDetails.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(3, 4, 3, 4);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(grpParams);
            splitContainer1.Panel1.Controls.Add(btnGenerate);
            splitContainer1.Panel1.Controls.Add(btnRunMaximin);
            splitContainer1.Panel1.Controls.Add(grpIteration);
            splitContainer1.Panel1.Controls.Add(grpDetails);
            splitContainer1.Panel1MinSize = 280;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panelPlot);
            splitContainer1.Size = new Size(1257, 867);
            splitContainer1.SplitterDistance = 366;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 0;
            // 
            // grpParams
            // 
            grpParams.Controls.Add(lblSeed);
            grpParams.Controls.Add(numSeed);
            grpParams.Controls.Add(lblMaxVal);
            grpParams.Controls.Add(numMaxValue);
            grpParams.Controls.Add(lblMinVal);
            grpParams.Controls.Add(numMinValue);
            grpParams.Controls.Add(lblCount);
            grpParams.Controls.Add(numObjectCount);
            grpParams.Location = new Point(14, 16);
            grpParams.Margin = new Padding(3, 4, 3, 4);
            grpParams.Name = "grpParams";
            grpParams.Padding = new Padding(3, 4, 3, 4);
            grpParams.Size = new Size(338, 195);
            grpParams.TabIndex = 0;
            grpParams.TabStop = false;
            grpParams.Text = "Входные данные";
            // 
            // lblSeed
            // 
            lblSeed.AutoSize = true;
            lblSeed.Location = new Point(14, 155);
            lblSeed.Name = "lblSeed";
            lblSeed.Size = new Size(170, 20);
            lblSeed.TabIndex = 8;
            lblSeed.Text = "Зерно (−1 = случайно):";
            // 
            // numSeed
            // 
            numSeed.Location = new Point(214, 153);
            numSeed.Margin = new Padding(3, 4, 3, 4);
            numSeed.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numSeed.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
            numSeed.Name = "numSeed";
            numSeed.Size = new Size(114, 27);
            numSeed.TabIndex = 9;
            numSeed.Value = new decimal(new int[] { 1, 0, 0, int.MinValue });
            // 
            // lblMaxVal
            // 
            lblMaxVal.AutoSize = true;
            lblMaxVal.Location = new Point(14, 115);
            lblMaxVal.Name = "lblMaxVal";
            lblMaxVal.Size = new Size(120, 20);
            lblMaxVal.TabIndex = 6;
            lblMaxVal.Text = "Макс. значение:";
            // 
            // numMaxValue
            // 
            numMaxValue.DecimalPlaces = 1;
            numMaxValue.Location = new Point(214, 113);
            numMaxValue.Margin = new Padding(3, 4, 3, 4);
            numMaxValue.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numMaxValue.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            numMaxValue.Name = "numMaxValue";
            numMaxValue.Size = new Size(114, 27);
            numMaxValue.TabIndex = 7;
            numMaxValue.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // lblMinVal
            // 
            lblMinVal.AutoSize = true;
            lblMinVal.Location = new Point(14, 75);
            lblMinVal.Name = "lblMinVal";
            lblMinVal.Size = new Size(116, 20);
            lblMinVal.TabIndex = 4;
            lblMinVal.Text = "Мин. значение:";
            // 
            // numMinValue
            // 
            numMinValue.DecimalPlaces = 1;
            numMinValue.Location = new Point(214, 73);
            numMinValue.Margin = new Padding(3, 4, 3, 4);
            numMinValue.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numMinValue.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            numMinValue.Name = "numMinValue";
            numMinValue.Size = new Size(114, 27);
            numMinValue.TabIndex = 5;
            // 
            // lblCount
            // 
            lblCount.AutoSize = true;
            lblCount.Location = new Point(14, 37);
            lblCount.Name = "lblCount";
            lblCount.Size = new Size(177, 20);
            lblCount.TabIndex = 0;
            lblCount.Text = "Количество объектов N:";
            // 
            // numObjectCount
            // 
            numObjectCount.Location = new Point(214, 35);
            numObjectCount.Margin = new Padding(3, 4, 3, 4);
            numObjectCount.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numObjectCount.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numObjectCount.Name = "numObjectCount";
            numObjectCount.Size = new Size(114, 27);
            numObjectCount.TabIndex = 1;
            numObjectCount.Value = new decimal(new int[] { 2000, 0, 0, 0 });
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(14, 219);
            btnGenerate.Margin = new Padding(3, 4, 3, 4);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(160, 43);
            btnGenerate.TabIndex = 1;
            btnGenerate.Text = "Сгенерировать";
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += BtnGenerate_Click;
            // 
            // btnRunMaximin
            // 
            btnRunMaximin.Location = new Point(192, 219);
            btnRunMaximin.Margin = new Padding(3, 4, 3, 4);
            btnRunMaximin.Name = "btnRunMaximin";
            btnRunMaximin.Size = new Size(160, 43);
            btnRunMaximin.TabIndex = 2;
            btnRunMaximin.Text = "Запустить максимин";
            btnRunMaximin.UseVisualStyleBackColor = true;
            btnRunMaximin.Click += BtnRunMaximin_Click;
            // 
            // grpIteration
            // 
            grpIteration.Controls.Add(lblIterInfo);
            grpIteration.Controls.Add(trackIteration);
            grpIteration.Controls.Add(lblIterNum);
            grpIteration.Controls.Add(btnPrev);
            grpIteration.Controls.Add(btnNext);
            grpIteration.Location = new Point(14, 270);
            grpIteration.Margin = new Padding(3, 4, 3, 4);
            grpIteration.Name = "grpIteration";
            grpIteration.Padding = new Padding(3, 4, 3, 4);
            grpIteration.Size = new Size(338, 160);
            grpIteration.TabIndex = 3;
            grpIteration.TabStop = false;
            grpIteration.Text = "Просмотр итераций";
            // 
            // lblIterInfo
            // 
            lblIterInfo.Location = new Point(210, 67);
            lblIterInfo.Name = "lblIterInfo";
            lblIterInfo.Size = new Size(114, 37);
            lblIterInfo.TabIndex = 4;
            lblIterInfo.Text = "Ядер: 0";
            lblIterInfo.TextAlign = ContentAlignment.MiddleRight;
            // 
            // trackIteration
            // 
            trackIteration.Location = new Point(14, 112);
            trackIteration.Margin = new Padding(3, 4, 3, 4);
            trackIteration.Name = "trackIteration";
            trackIteration.Size = new Size(311, 56);
            trackIteration.TabIndex = 3;
            trackIteration.Scroll += TrackIteration_Scroll;
            // 
            // lblIterNum
            // 
            lblIterNum.AutoSize = true;
            lblIterNum.Location = new Point(14, 33);
            lblIterNum.Name = "lblIterNum";
            lblIterNum.Size = new Size(114, 20);
            lblIterNum.TabIndex = 0;
            lblIterNum.Text = "Итерация: 0 / 0";
            // 
            // btnPrev
            // 
            btnPrev.Location = new Point(14, 67);
            btnPrev.Margin = new Padding(3, 4, 3, 4);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(91, 37);
            btnPrev.TabIndex = 1;
            btnPrev.Text = "← Пред";
            btnPrev.UseVisualStyleBackColor = true;
            btnPrev.Click += BtnPrev_Click;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(112, 67);
            btnNext.Margin = new Padding(3, 4, 3, 4);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(91, 37);
            btnNext.TabIndex = 2;
            btnNext.Text = "След →";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += BtnNext_Click;
            // 
            // grpDetails
            // 
            grpDetails.Controls.Add(txtDetails);
            grpDetails.Location = new Point(14, 438);
            grpDetails.Margin = new Padding(3, 4, 3, 4);
            grpDetails.Name = "grpDetails";
            grpDetails.Padding = new Padding(3, 4, 3, 4);
            grpDetails.Size = new Size(338, 413);
            grpDetails.TabIndex = 4;
            grpDetails.TabStop = false;
            grpDetails.Text = "Детали итерации";
            // 
            // txtDetails
            // 
            txtDetails.BackColor = Color.White;
            txtDetails.Dock = DockStyle.Fill;
            txtDetails.Font = new Font("Consolas", 9F);
            txtDetails.Location = new Point(3, 24);
            txtDetails.Margin = new Padding(3, 4, 3, 4);
            txtDetails.Multiline = true;
            txtDetails.Name = "txtDetails";
            txtDetails.ReadOnly = true;
            txtDetails.ScrollBars = ScrollBars.Vertical;
            txtDetails.Size = new Size(332, 385);
            txtDetails.TabIndex = 0;
            txtDetails.WordWrap = false;
            // 
            // panelPlot
            // 
            panelPlot.BackColor = Color.Transparent;
            panelPlot.Dock = DockStyle.Fill;
            panelPlot.Location = new Point(0, 0);
            panelPlot.Margin = new Padding(3, 4, 3, 4);
            panelPlot.Name = "panelPlot";
            panelPlot.Size = new Size(886, 867);
            panelPlot.TabIndex = 0;
            panelPlot.Paint += PanelPlot_Paint;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLightLight;
            ClientSize = new Size(1257, 867);
            Controls.Add(splitContainer1);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(1026, 651);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Алгоритм максимина — самообучающаяся классификация";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            grpParams.ResumeLayout(false);
            grpParams.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numSeed).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMaxValue).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMinValue).EndInit();
            ((System.ComponentModel.ISupportInitialize)numObjectCount).EndInit();
            grpIteration.ResumeLayout(false);
            grpIteration.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackIteration).EndInit();
            grpDetails.ResumeLayout(false);
            grpDetails.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private GroupBox grpParams;
        private Label lblCount;
        private NumericUpDown numObjectCount;
        private Label lblMinVal;
        private NumericUpDown numMinValue;
        private Label lblMaxVal;
        private NumericUpDown numMaxValue;
        private Label lblSeed;
        private NumericUpDown numSeed;
        private Button btnGenerate;
        private Button btnRunMaximin;
        private GroupBox grpIteration;
        private Label lblIterNum;
        private Button btnPrev;
        private Button btnNext;
        private TrackBar trackIteration;
        private Label lblIterInfo;
        private GroupBox grpDetails;
        private TextBox txtDetails;
        private Panel panelPlot;
    }
}
