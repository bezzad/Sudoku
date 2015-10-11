namespace Sudoku
{
    partial class FormSudoku
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSudoku));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnSample = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSolve = new System.Windows.Forms.Button();
            this.timerUndoColorRow = new System.Windows.Forms.Timer(this.components);
            this.timerUndoColorColumn = new System.Windows.Forms.Timer(this.components);
            this.timerUndoColorSquar = new System.Windows.Forms.Timer(this.components);
            this.btnThink = new System.Windows.Forms.Button();
            this.picBackground = new System.Windows.Forms.PictureBox();
            this.timerSampleSolve = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picBackground)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 300;
            // this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // btnAbout
            // 
            this.btnAbout.BackColor = System.Drawing.Color.White;
            this.btnAbout.BackgroundImage = global::Sudoku.Properties.Resources.Padideh;
            this.btnAbout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAbout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAbout.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btnAbout.ForeColor = System.Drawing.Color.Navy;
            this.btnAbout.Location = new System.Drawing.Point(305, 7);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(65, 60);
            this.btnAbout.TabIndex = 4;
            this.toolTip.SetToolTip(this.btnAbout, "About");
            this.btnAbout.UseVisualStyleBackColor = false;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnSample
            // 
            this.btnSample.BackColor = System.Drawing.Color.White;
            this.btnSample.BackgroundImage = global::Sudoku.Properties.Resources.package_games_board;
            this.btnSample.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSample.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSample.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btnSample.ForeColor = System.Drawing.Color.Navy;
            this.btnSample.Location = new System.Drawing.Point(234, 7);
            this.btnSample.Name = "btnSample";
            this.btnSample.Size = new System.Drawing.Size(65, 60);
            this.btnSample.TabIndex = 3;
            this.toolTip.SetToolTip(this.btnSample, "Sample");
            this.btnSample.UseVisualStyleBackColor = false;
            this.btnSample.Click += new System.EventHandler(this.btnSample_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.White;
            this.btnClear.BackgroundImage = global::Sudoku.Properties.Resources.clean;
            this.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btnClear.ForeColor = System.Drawing.Color.Navy;
            this.btnClear.Location = new System.Drawing.Point(83, 7);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(65, 60);
            this.btnClear.TabIndex = 1;
            this.toolTip.SetToolTip(this.btnClear, "Clear");
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSolve
            // 
            this.btnSolve.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnSolve.BackgroundImage = global::Sudoku.Properties.Resources.app;
            this.btnSolve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSolve.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSolve.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btnSolve.ForeColor = System.Drawing.Color.Navy;
            this.btnSolve.Location = new System.Drawing.Point(12, 7);
            this.btnSolve.Name = "btnSolve";
            this.btnSolve.Size = new System.Drawing.Size(65, 60);
            this.btnSolve.TabIndex = 0;
            this.toolTip.SetToolTip(this.btnSolve, "Solve");
            this.btnSolve.UseVisualStyleBackColor = false;
            this.btnSolve.Click += new System.EventHandler(this.btnSolve_Click);
            // 
            // timerUndoColorRow
            // 
            this.timerUndoColorRow.Interval = 1500;
            this.timerUndoColorRow.Tick += new System.EventHandler(this.timerUndoColorRow_Tick);
            // 
            // timerUndoColorColumn
            // 
            this.timerUndoColorColumn.Interval = 1500;
            this.timerUndoColorColumn.Tick += new System.EventHandler(this.timerUndoColorColumn_Tick);
            // 
            // timerUndoColorSquar
            // 
            this.timerUndoColorSquar.Interval = 1500;
            this.timerUndoColorSquar.Tick += new System.EventHandler(this.timerUndoColorSquar_Tick);
            // 
            // btnThink
            // 
            this.btnThink.BackColor = System.Drawing.Color.White;
            this.btnThink.BackgroundImage = global::Sudoku.Properties.Resources.database_off;
            this.btnThink.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnThink.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btnThink.Enabled = false;
            this.btnThink.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btnThink.ForeColor = System.Drawing.Color.Navy;
            this.btnThink.Location = new System.Drawing.Point(154, 2);
            this.btnThink.Name = "btnThink";
            this.btnThink.Size = new System.Drawing.Size(74, 70);
            this.btnThink.TabIndex = 2;
            this.btnThink.UseVisualStyleBackColor = false;
            // 
            // picBackground
            // 
            this.picBackground.Image = global::Sudoku.Properties.Resources.BackGround_Pic;
            this.picBackground.Location = new System.Drawing.Point(12, 73);
            this.picBackground.Name = "picBackground";
            this.picBackground.Size = new System.Drawing.Size(358, 361);
            this.picBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBackground.TabIndex = 2;
            this.picBackground.TabStop = false;
            // 
            // timerSampleSolve
            // 
            this.timerSampleSolve.Interval = 1000;
            this.timerSampleSolve.Tick += new System.EventHandler(this.timerSampleSolve_Tick);
            // 
            // FormSudoku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 445);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.btnSample);
            this.Controls.Add(this.btnThink);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSolve);
            this.Controls.Add(this.picBackground);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormSudoku";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sudoku 9×9 Solver";
            this.Load += new System.EventHandler(this.FormSudoku_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBackground)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnSample;
        private System.Windows.Forms.Button btnThink;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSolve;
        private System.Windows.Forms.PictureBox picBackground;
        private System.Windows.Forms.Timer timerUndoColorRow;
        private System.Windows.Forms.Timer timerUndoColorColumn;
        private System.Windows.Forms.Timer timerUndoColorSquar;
        private System.Windows.Forms.Timer timerSampleSolve;
    }
}

