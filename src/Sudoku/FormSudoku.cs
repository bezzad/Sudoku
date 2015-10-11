/**********************************************************************/
/*   Application Name:            Sudoku 9×9                          */
/*   Modify Info:                 Copyright © Behzadkh 2009           */
/**********************************************************************/

using System;
using System.Drawing;
using System.Windows.Forms;
using Sudoku.Properties;

namespace Sudoku
{
    public partial class FormSudoku : Form
    {
        public FormSudoku()
        {
            InitializeComponent();
        }

        private bool _on = false;
        private string[] _readSample;       // [1054]
        private string[,] _sample;         // [27][45]
        private TextBox[,] _txtSudoku;      // [9][9]
        private static int[,] _sudoku;      // [9][9]
        private bool[,,] _blackList;       // [9][9][10]
        private int[,] _peculiaritiesPhase; // [81][2]
        private int[,,] _coefficient;      // [9][9][10] MAX COEFFICIENT for One Home Equal the Row - Column - Squar
        private bool _timerUndoColor = false;
        private bool _nonNumberEntered = false;
        private bool _blComplete = false;
        private bool _sampleRun = false;
        private int _phaseOfSample = 0;
        private int _time = 0;
        private static bool _sampleReadOnce = false;

        private static int
            _locX = 0, _locY = 0,
            _rx, _ry,
            _cx, _cy,
            _sx, _sy,
            _pec = 0, // Number of Phase
            _backPec = 0;


        private void FormSudoku_Load(object sender, EventArgs e)
        {
            _sample = new string[27, 45];


            _readSample = Resources.SampleFile.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            _sudoku = new int[9, 9];
            _txtSudoku = new TextBox[9, 9];
            _blackList = new bool[9, 9, 10];
            _coefficient = new int[9, 9, 10];
            _peculiaritiesPhase = new int[81, 2]; // 81 Phase and 0 = X , 1 = Y
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    this._txtSudoku[i, j] = new System.Windows.Forms.TextBox();
                    this._txtSudoku[i, j].BorderStyle = System.Windows.Forms.BorderStyle.None;
                    this._txtSudoku[i, j].Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this._txtSudoku[i, j].ForeColor = System.Drawing.Color.Blue;
                    this._txtSudoku[i, j].BackColor = Color.White;
                    this._txtSudoku[i, j].Location = new System.Drawing.Point(20 + (i * 39), 85 + (j * 39));
                    this._txtSudoku[i, j].MaxLength = 1;
                    this._txtSudoku[i, j].Multiline = true;
                    this._txtSudoku[i, j].Name = "txtSudoku";
                    this._txtSudoku[i, j].Size = new System.Drawing.Size(28, 26);
                    this._txtSudoku[i, j].TabIndex = (5 + (j * 9) + i);
                    this._txtSudoku[i, j].TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                    this._txtSudoku[i, j].Text = string.Empty;
                    this._txtSudoku[i, j].TextChanged += new System.EventHandler(this.txtSudoku_TextChanged);
                    this._txtSudoku[i, j].KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSudoku_KeyDown);
                    this._txtSudoku[i, j].KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSudoku_KeyPress);
                    this._txtSudoku[i, j].KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSudoku_KeyUp);
                    this.Controls.Add(this._txtSudoku[i, j]);
                    this._txtSudoku[i, j].BringToFront();
                    // Must All Sudoku 2D-Matris be zero 0; 
                    _sudoku[i, j] = 0;
                    for (int k = 0; k < 10; k++)
                    {
                        _blackList[i, j, k] = true;
                        _coefficient[i, j, k] = 0;
                    }
                }
            for (int i = 0; i < 81; i++)
            {
                _peculiaritiesPhase[i, 0] = -1;
                _peculiaritiesPhase[i, 1] = -1;
            }

            _txtSudoku[0, 0].Focus();
            _txtSudoku[0, 0].Select();
        }

        private void txtSudoku_TextChanged(object sender, EventArgs e)
        {
            string Spil = this.ActiveControl.Text.ToString();
            if (Spil != "1" && Spil != "2" && Spil != "3" && Spil != "4" && Spil != "5" &&
                Spil != "6" && Spil != "7" && Spil != "8" && Spil != "9" && Spil != string.Empty)
            {
                this.ActiveControl.Text = string.Empty;
                MessageBox.Show("Please Enter Just Number 1~9. This TextBox don't Accept Other Key!" +
                    "\nلطفاً فقط اعداد 1 الی 9 را وارد کنید. این فیلد کلید دیگری را قبول نمی کند ", "Enter InCorrect Key"
                    , MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.ActiveControl.Text = string.Empty;
            }
        }

        private void txtSudoku_KeyDown(object sender, KeyEventArgs e)
        {
            // Initialize the flag to false.
            _nonNumberEntered = false;

            // Determine whether the keystroke is a number from the top of the keyboard.
            if (e.KeyCode < Keys.D1 || e.KeyCode > Keys.D9)
            {
                // Determine whether the keystroke is a number from the keypad.
                if (e.KeyCode < Keys.NumPad1 || e.KeyCode > Keys.NumPad9)
                {
                    // Determine whether the keystroke is a backspace.
                    if (e.KeyCode != Keys.Back)
                    {
                        // A non-numerical keystroke was pressed.
                        // Set the flag to true and evaluate in KeyPress event.
                        _nonNumberEntered = true;
                    }
                }
            }
            if (this.ActiveControl.TabIndex > 4) // if the ActiveControl was TextBox Then TabIndex No. > 4
            {
                if (e.KeyCode == Keys.Left)
                {
                    if (this.ActiveControl.TabIndex > 5)
                        ActiveControl = GetNextControl(ActiveControl, false);
                    else
                        _txtSudoku[8, 8].Select();
                }
                else if (e.KeyCode == Keys.Right)
                {
                    if (this.ActiveControl.TabIndex < 85)
                        ActiveControl = GetNextControl(ActiveControl, true);
                    else
                        _txtSudoku[0, 0].Select();
                }
                else if (e.KeyCode == Keys.Up)
                {
                    FindPeculiarities();
                    if (_locY > 0) _locY--;
                    else if (_locX < 8)
                    {
                        _locX++;
                        _locY = 8;
                    }
                    else
                    {
                        _locX = 0;
                        _locY = 8;
                    }
                    ActiveControl = this._txtSudoku[_locX, _locY];  // == txtSudoku[LocX, LocY].Select();
                }
                else if (e.KeyCode == Keys.Down)
                {
                    FindPeculiarities();
                    if (_locY < 8) _locY++;
                    else if (_locX > 0)
                    {
                        _locX--;
                        _locY = 0;
                    }
                    else
                    {
                        _locX = 8;
                        _locY = 0;
                    }
                    _txtSudoku[_locX, _locY].Select(); // == ActiveControl = this.txtSudoku[LocX, LocY];
                }
            }
        }

        private void txtSudoku_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check for the flag being set in the KeyDown event.
            if (_nonNumberEntered == true || _timerUndoColor == true)
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
            }
        }

        private void FindPeculiarities()
        {
            _locX = _locY = 0;
            int X = this.ActiveControl.Location.X - 20;
            int Y = this.ActiveControl.Location.Y - 85;
            while (X != 0)
            {
                X -= 39;
                _locX++;
            }
            while (Y != 0)
            {
                Y -= 39;
                _locY++;
            }
        }

        private void timerUndoColorRow_Tick(object sender, EventArgs e) // this timer for Error in Row's
        {
            _txtSudoku[_rx, _ry].BackColor = Color.White;
            _sudoku[_locX, _locY] = 0;
            _timerUndoColor = false;
            timerUndoColorRow.Enabled = false;
        }
        private void timerUndoColorColumn_Tick(object sender, EventArgs e) // this timer for Error in Column's
        {
            _txtSudoku[_cx, _cy].BackColor = Color.White;
            _sudoku[_locX, _locY] = 0;
            _timerUndoColor = false;
            timerUndoColorColumn.Enabled = false;
        }
        private void timerUndoColorSquar_Tick(object sender, EventArgs e) // this timer for Error in Squar's 3×3
        {
            _txtSudoku[_sx, _sy].BackColor = Color.White;
            _sudoku[_locX, _locY] = 0;
            _timerUndoColor = false;
            timerUndoColorSquar.Enabled = false;
        }
        private void txtSudoku_KeyUp(object sender, KeyEventArgs e)
        {
            FindPeculiarities();
            bool Row = false, Column = false, Squar = false;
            if (this.ActiveControl.Text.ToString() == string.Empty)
                _sudoku[_locX, _locY] = 0;
            else
            {
                _sudoku[_locX, _locY] = int.Parse(this.ActiveControl.Text.ToString());
                for (int i = 0; i < 9; i++)
                {
                    // Check Row
                    if (i != _locX && (_sudoku[_locX, _locY] == _sudoku[i, _locY]))
                    {
                        _timerUndoColor = true;
                        _txtSudoku[i, _locY].BackColor = Color.Red;
                        _txtSudoku[_locX, _locY].Text = string.Empty;
                        _rx = i;
                        _ry = _locY;
                        timerUndoColorRow.Enabled = true;
                        Row = true;
                    }
                    // Check Column
                    if (i != _locY && (_sudoku[_locX, _locY] == _sudoku[_locX, i]))
                    {
                        _timerUndoColor = true;
                        _txtSudoku[_locX, i].BackColor = Color.Red;
                        _txtSudoku[_locX, _locY].Text = string.Empty;
                        _cx = _locX;
                        _cy = i;
                        timerUndoColorColumn.Enabled = true;
                        Column = true;
                    }
                }
                // Check All Squre 3×3  
                if (_locX >= 0 && _locX < 3)
                    if (_locY >= 0 && _locY < 3)
                    {   // Check Squre 3×3 Down  
                        //   _ _ _ _ _ _ _ _ _
                        // 0|*|*|*|_|_|_|_|_|_| 
                        // 1|*|*|*|_|_|_|_|_|_|
                        // 2|*|*|*|_|_|_|_|_|_|
                        // 3|_|_|_|_|_|_|_|_|_|
                        // 4|_|_|_|_|_|_|_|_|_|
                        // 5|_|_|_|_|_|_|_|_|_|
                        // 6|_|_|_|_|_|_|_|_|_|
                        // 7|_|_|_|_|_|_|_|_|_|
                        // 8|_|_|_|_|_|_|_|_|_|
                        //   0 1 2 3 4 5 6 7 8   
                        for (int i = 0; i < 3; i++)
                            for (int j = 0; j < 3; j++)
                                if (i != _locX || j != _locY)
                                    if (_sudoku[_locX, _locY] == _sudoku[i, j])
                                    {
                                        _timerUndoColor = true;
                                        _txtSudoku[i, j].BackColor = Color.Red;
                                        _txtSudoku[_locX, _locY].Text = string.Empty;
                                        _sx = i;
                                        _sy = j;
                                        timerUndoColorSquar.Enabled = true;
                                        Squar = true;
                                    }
                    }
                    else if (_locY >= 3 && _locY < 6)
                    {   // Check Squre 3×3 Down  
                        //   _ _ _ _ _ _ _ _ _
                        // 0|_|_|_|_|_|_|_|_|_| 
                        // 1|_|_|_|_|_|_|_|_|_|
                        // 2|_|_|_|_|_|_|_|_|_|
                        // 3|*|*|*|_|_|_|_|_|_|
                        // 4|*|*|*|_|_|_|_|_|_|
                        // 5|*|*|*|_|_|_|_|_|_|
                        // 6|_|_|_|_|_|_|_|_|_|
                        // 7|_|_|_|_|_|_|_|_|_|
                        // 8|_|_|_|_|_|_|_|_|_|
                        //   0 1 2 3 4 5 6 7 8
                        for (int i = 0; i < 3; i++)
                            for (int j = 3; j < 6; j++)
                                if (i != _locX || j != _locY)
                                    if (_sudoku[_locX, _locY] == _sudoku[i, j])
                                    {
                                        _timerUndoColor = true;
                                        _txtSudoku[i, j].BackColor = Color.Red;
                                        _txtSudoku[_locX, _locY].Text = string.Empty;
                                        _sx = i;
                                        _sy = j;
                                        timerUndoColorSquar.Enabled = true;
                                        Squar = true;
                                    }
                    }
                    else
                    {   // Check Squre 3×3 Down  
                        //   _ _ _ _ _ _ _ _ _
                        // 0|_|_|_|_|_|_|_|_|_| 
                        // 1|_|_|_|_|_|_|_|_|_|
                        // 2|_|_|_|_|_|_|_|_|_|
                        // 3|_|_|_|_|_|_|_|_|_|
                        // 4|_|_|_|_|_|_|_|_|_|
                        // 5|_|_|_|_|_|_|_|_|_|
                        // 6|*|*|*|_|_|_|_|_|_|
                        // 7|*|*|*|_|_|_|_|_|_|
                        // 8|*|*|*|_|_|_|_|_|_|
                        //   0 1 2 3 4 5 6 7 8
                        for (int i = 0; i < 3; i++)
                            for (int j = 6; j < 9; j++)
                                if (i != _locX || j != _locY)
                                    if (_sudoku[_locX, _locY] == _sudoku[i, j])
                                    {
                                        _timerUndoColor = true;
                                        _txtSudoku[i, j].BackColor = Color.Red;
                                        _txtSudoku[_locX, _locY].Text = string.Empty;
                                        _sx = i;
                                        _sy = j;
                                        timerUndoColorSquar.Enabled = true;
                                        Squar = true;
                                    }
                    }
                else if (_locX >= 3 && _locX < 6)
                    if (_locY >= 0 && _locY < 3)
                    {   // Check Squre 3×3 Down  
                        //   _ _ _ _ _ _ _ _ _
                        // 0|_|_|_|*|*|*|_|_|_| 
                        // 1|_|_|_|*|*|*|_|_|_|
                        // 2|_|_|_|*|*|*|_|_|_|
                        // 3|_|_|_|_|_|_|_|_|_|
                        // 4|_|_|_|_|_|_|_|_|_|
                        // 5|_|_|_|_|_|_|_|_|_|
                        // 6|_|_|_|_|_|_|_|_|_|
                        // 7|_|_|_|_|_|_|_|_|_|
                        // 8|_|_|_|_|_|_|_|_|_|
                        //   0 1 2 3 4 5 6 7 8
                        for (int i = 3; i < 6; i++)
                            for (int j = 0; j < 3; j++)
                                if (i != _locX || j != _locY)
                                    if (_sudoku[_locX, _locY] == _sudoku[i, j])
                                    {
                                        _timerUndoColor = true;
                                        _txtSudoku[i, j].BackColor = Color.Red;
                                        _txtSudoku[_locX, _locY].Text = string.Empty;
                                        _sx = i;
                                        _sy = j;
                                        timerUndoColorSquar.Enabled = true;
                                        Squar = true;
                                    }
                    }
                    else if (_locY >= 3 && _locY < 6)
                    {   // Check Squre 3×3 Down  
                        //   _ _ _ _ _ _ _ _ _
                        // 0|_|_|_|_|_|_|_|_|_| 
                        // 1|_|_|_|_|_|_|_|_|_|
                        // 2|_|_|_|_|_|_|_|_|_|
                        // 3|_|_|_|*|*|*|_|_|_|
                        // 4|_|_|_|*|*|*|_|_|_|
                        // 5|_|_|_|*|*|*|_|_|_|
                        // 6|_|_|_|_|_|_|_|_|_|
                        // 7|_|_|_|_|_|_|_|_|_|
                        // 8|_|_|_|_|_|_|_|_|_|
                        //   0 1 2 3 4 5 6 7 8
                        for (int i = 3; i < 6; i++)
                            for (int j = 3; j < 6; j++)
                                if (i != _locX || j != _locY)
                                    if (_sudoku[_locX, _locY] == _sudoku[i, j])
                                    {
                                        _timerUndoColor = true;
                                        _txtSudoku[i, j].BackColor = Color.Red;
                                        _txtSudoku[_locX, _locY].Text = string.Empty;
                                        _sx = i;
                                        _sy = j;
                                        timerUndoColorSquar.Enabled = true;
                                        Squar = true;
                                    }
                    }
                    else
                    {   // Check Squre 3×3 Down  
                        //   _ _ _ _ _ _ _ _ _
                        // 0|_|_|_|_|_|_|_|_|_| 
                        // 1|_|_|_|_|_|_|_|_|_|
                        // 2|_|_|_|_|_|_|_|_|_|
                        // 3|_|_|_|_|_|_|_|_|_|
                        // 4|_|_|_|_|_|_|_|_|_|
                        // 5|_|_|_|_|_|_|_|_|_|
                        // 6|_|_|_|*|*|*|_|_|_|
                        // 7|_|_|_|*|*|*|_|_|_|
                        // 8|_|_|_|*|*|*|_|_|_|
                        //   0 1 2 3 4 5 6 7 8
                        for (int i = 3; i < 6; i++)
                            for (int j = 6; j < 9; j++)
                                if (i != _locX || j != _locY)
                                    if (_sudoku[_locX, _locY] == _sudoku[i, j])
                                    {
                                        _timerUndoColor = true;
                                        _txtSudoku[i, j].BackColor = Color.Red;
                                        _txtSudoku[_locX, _locY].Text = string.Empty;
                                        _sx = i;
                                        _sy = j;
                                        timerUndoColorSquar.Enabled = true;
                                        Squar = true;
                                    }
                    }
                else
                    if (_locY >= 0 && _locY < 3)
                {   // Check Squre 3×3 Down  
                    //   _ _ _ _ _ _ _ _ _
                    // 0|_|_|_|_|_|_|*|*|*| 
                    // 1|_|_|_|_|_|_|*|*|*|
                    // 2|_|_|_|_|_|_|*|*|*|
                    // 3|_|_|_|_|_|_|_|_|_|
                    // 4|_|_|_|_|_|_|_|_|_|
                    // 5|_|_|_|_|_|_|_|_|_|
                    // 6|_|_|_|_|_|_|_|_|_|
                    // 7|_|_|_|_|_|_|_|_|_|
                    // 8|_|_|_|_|_|_|_|_|_|
                    //   0 1 2 3 4 5 6 7 8
                    for (int i = 6; i < 9; i++)
                        for (int j = 0; j < 3; j++)
                            if (i != _locX || j != _locY)
                                if (_sudoku[_locX, _locY] == _sudoku[i, j])
                                {
                                    _timerUndoColor = true;
                                    _txtSudoku[i, j].BackColor = Color.Red;
                                    _txtSudoku[_locX, _locY].Text = string.Empty;
                                    _sx = i;
                                    _sy = j;
                                    timerUndoColorSquar.Enabled = true;
                                    Squar = true;
                                }
                }
                else if (_locY >= 3 && _locY < 6)
                {   // Check Squre 3×3 Down  
                    //   _ _ _ _ _ _ _ _ _
                    // 0|_|_|_|_|_|_|_|_|_| 
                    // 1|_|_|_|_|_|_|_|_|_|
                    // 2|_|_|_|_|_|_|_|_|_|
                    // 3|_|_|_|_|_|_|*|*|*|
                    // 4|_|_|_|_|_|_|*|*|*|
                    // 5|_|_|_|_|_|_|*|*|*|
                    // 6|_|_|_|_|_|_|_|_|_|
                    // 7|_|_|_|_|_|_|_|_|_|
                    // 8|_|_|_|_|_|_|_|_|_|
                    //   0 1 2 3 4 5 6 7 8
                    for (int i = 6; i < 9; i++)
                        for (int j = 3; j < 6; j++)
                            if (i != _locX || j != _locY)
                                if (_sudoku[_locX, _locY] == _sudoku[i, j])
                                {
                                    _timerUndoColor = true;
                                    _txtSudoku[i, j].BackColor = Color.Red;
                                    _txtSudoku[_locX, _locY].Text = string.Empty;
                                    _sx = i;
                                    _sy = j;
                                    timerUndoColorSquar.Enabled = true;
                                    Squar = true;
                                }
                }
                else
                {   // Check Squre 3×3 Down  
                    //   _ _ _ _ _ _ _ _ _
                    // 0|_|_|_|_|_|_|_|_|_| 
                    // 1|_|_|_|_|_|_|_|_|_|
                    // 2|_|_|_|_|_|_|_|_|_|
                    // 3|_|_|_|_|_|_|_|_|_|
                    // 4|_|_|_|_|_|_|_|_|_|
                    // 5|_|_|_|_|_|_|_|_|_|
                    // 6|_|_|_|_|_|_|*|*|*|
                    // 7|_|_|_|_|_|_|*|*|*|
                    // 8|_|_|_|_|_|_|*|*|*|
                    //   0 1 2 3 4 5 6 7 8
                    for (int i = 6; i < 9; i++)
                        for (int j = 6; j < 9; j++)
                            if (i != _locX || j != _locY)
                                if (_sudoku[_locX, _locY] == _sudoku[i, j])
                                {
                                    _timerUndoColor = true;
                                    _txtSudoku[i, j].BackColor = Color.Red;
                                    _txtSudoku[_locX, _locY].Text = string.Empty;
                                    _sx = i;
                                    _sy = j;
                                    timerUndoColorSquar.Enabled = true;
                                    Squar = true;
                                }
                }
                if (e.KeyCode != Keys.Up && e.KeyCode != Keys.Down && e.KeyCode != Keys.Left && _blComplete == false && ActiveControl.TabIndex < 85
                    && e.KeyCode != Keys.Right && e.KeyCode != Keys.Tab && Row == false && Column == false && Squar == false)
                {

                    ActiveControl = GetNextControl(ActiveControl, true);
                    while (ActiveControl.Enabled == false)
                        if (ActiveControl.TabIndex < 85)
                            ActiveControl = GetNextControl(ActiveControl, true);
                        else break;

                }
            }
            if (_sampleRun == true)
            {
                if (timerSampleSolve.Enabled == false)
                    timerSampleSolve.Enabled = true;
                Complete();
            }
        }
        private void Complete()
        {
            bool FindEmpty = false;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    if (_sudoku[i, j] == 0)
                        FindEmpty = true;
            if (FindEmpty == false && _blComplete == false)
            {
                timerSampleSolve.Enabled = false;
                int Min = 0;
                int Sec = 0;
                int Hour = 0;
                if (_time > 59)
                {
                    if (_time > 3599)
                    {
                        Hour = _time / 3600;
                        Min = (_time % 3600) / 60;
                        Sec = (_time % 3600) % 60;
                        MessageBox.Show("Congratulations! \nYour time was " + Hour.ToString() + "\nHour. and " + Min.ToString()
                            + " Min. and " + Sec.ToString() + " Sec.", "Sudoku Completed!");
                        if (Hour > 9)
                        {
                            if (Min > 9)
                                if (Sec > 9)
                                    MessageBox.Show("    " + Hour.ToString() + ":" + Min.ToString() + ":" + Sec.ToString());
                                else
                                    MessageBox.Show("    " + Hour.ToString() + ":" + Min.ToString() + ":0" + Sec.ToString());
                            else
                                if (Sec > 9)
                                MessageBox.Show("    " + Hour.ToString() + ":0" + Min.ToString() + ":" + Sec.ToString());
                            else
                                MessageBox.Show("    " + Hour.ToString() + ":0" + Min.ToString() + ":0" + Sec.ToString());
                        }
                        else
                        {
                            if (Min > 9)
                                if (Sec > 9)
                                    MessageBox.Show("    0" + Hour.ToString() + ":" + Min.ToString() + ":" + Sec.ToString());
                                else
                                    MessageBox.Show("    0" + Hour.ToString() + ":" + Min.ToString() + ":0" + Sec.ToString());
                            else
                                if (Sec > 9)
                                MessageBox.Show("    0" + Hour.ToString() + ":0" + Min.ToString() + ":" + Sec.ToString());
                            else
                                MessageBox.Show("    0" + Hour.ToString() + ":0" + Min.ToString() + ":0" + Sec.ToString());
                        }
                        _blComplete = true;
                        return;
                    }
                    else
                    {
                        Min = _time / 60;
                        Sec = _time % 60;
                        MessageBox.Show("Congratulations! \nYour time was " + Min.ToString() + "\nMin. and " + Sec.ToString()
                            + " Sec.", "Sudoku Completed!");
                        if (Min > 9)
                            if (Sec > 9)
                                MessageBox.Show("    00:" + Min.ToString() + ":" + Sec.ToString());
                            else
                                MessageBox.Show("    00:" + Min.ToString() + ":0" + Sec.ToString());
                        else
                            if (Sec > 9)
                            MessageBox.Show("    00:0" + Min.ToString() + ":" + Sec.ToString());
                        else
                            MessageBox.Show("    00:0" + Min.ToString() + ":0" + Sec.ToString());
                        _blComplete = true;
                        return;
                    }
                }
                else
                {
                    Sec = _time;
                    MessageBox.Show("Congratulations! \nYour time was " + Sec.ToString() + " Sec.", "Sudoku Completed!");
                    if (Sec > 9)
                        MessageBox.Show("    00:00:" + Sec.ToString());
                    else
                        MessageBox.Show("    00:00:0" + Sec.ToString());
                    _blComplete = true;
                    return;
                }
            }
        }
        private void btnAbout_Click(object sender, EventArgs e)
        {
            FormAbout About = new FormAbout();
            About.ShowDialog();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            timerSampleSolve.Enabled = false;
            _timerUndoColor = false; // Handler False
            _locX = 0;
            _locY = 0;
            _pec = 0;
            _blComplete = false;
            _backPec = 0;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    _txtSudoku[i, j].ForeColor = System.Drawing.Color.Blue;
                    _txtSudoku[i, j].Text = string.Empty;
                    _sudoku[i, j] = 0;
                    _sudoku[i, j] = 0;
                    _txtSudoku[i, j].Enabled = true;
                    for (int k = 0; k < 10; k++)
                    {
                        _coefficient[i, j, k] = 0;
                        _blackList[i, j, k] = true;
                    }
                }
            for (int i = 0; i < 81; i++)
            {
                _peculiaritiesPhase[i, 0] = -1;
                _peculiaritiesPhase[i, 1] = -1;
            }
            btnSolve.Enabled = true;
            _time = 0;
            _sampleRun = false;
        }

        // Solver CODE
        private void btnSolve_Click(object sender, EventArgs e)
        {
            timerSampleSolve.Enabled = false;
            _sampleRun = false;
            _timerUndoColor = true; // Handler True
            btnSolve.Enabled = false;
            for (int j = 0; j < 9; j++)
                for (int i = 0; i < 9; i++)
                    if (_sudoku[i, j] != 0)
                    {
                        _blackList[i, j, _sudoku[i, j]] = false;
                        FillByOne(i, j, _sudoku[i, j]);
                    }
                    else
                    {
                        _peculiaritiesPhase[_pec, 0] = i;
                        _peculiaritiesPhase[_pec, 1] = j;
                        _pec++;
                    }
            // Check The Other Number by Computer
            bool Finded = false;
            while (_backPec < _pec)
            {
                if (_backPec < 0) break;
                for (int R = 1; R < 10; R++)
                    if (_blackList[_peculiaritiesPhase[_backPec, 0], _peculiaritiesPhase[_backPec, 1], R] == true)
                    {
                        _blackList[_peculiaritiesPhase[_backPec, 0], _peculiaritiesPhase[_backPec, 1], R] = false;
                        _coefficient[_peculiaritiesPhase[_backPec, 0], _peculiaritiesPhase[_backPec, 1], R]++;
                        _sudoku[_peculiaritiesPhase[_backPec, 0], _peculiaritiesPhase[_backPec, 1]] = R;
                        FillByOne(_peculiaritiesPhase[_backPec, 0], _peculiaritiesPhase[_backPec, 1], R);
                        Finded = true;
                        break;
                    }
                if (Finded == false)
                {
                    FreePhase(_backPec - 1);
                }
                else
                {
                    _backPec++;
                    Finded = false;
                    continue;
                }
            }
            // Print in Screen
            if (_backPec >= 0)
            {
                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                        if (_sudoku[i, j] != 0 && _txtSudoku[i, j].Text == string.Empty)
                        {
                            _txtSudoku[i, j].ForeColor = System.Drawing.Color.Red;
                            _txtSudoku[i, j].Text = _sudoku[i, j].ToString();
                        }
            }
            else btnClear_Click(sender, e);
        }
        private void FreePhase(int p)
        {
            int i = _peculiaritiesPhase[p, 0];
            int j = _peculiaritiesPhase[p, 1];

            for (int c = 0; c < 9; c++)
            {
                // Free All Row's Number ===================================
                if (c != i)
                {
                    _coefficient[c, j, _sudoku[i, j]]--;
                    if (_coefficient[c, j, _sudoku[i, j]] == 0)
                        _blackList[c, j, _sudoku[i, j]] = true;
                }
                //==========================================================
                // Free All Column's Number ================================
                if (c != j)
                {
                    _coefficient[i, c, _sudoku[i, j]]--;
                    if (_coefficient[i, c, _sudoku[i, j]] == 0)
                        _blackList[i, c, _sudoku[i, j]] = true;
                }
                // =========================================================
            }
            //-------------------------------------- Free Squar's Number -----------------------------------
            //----------------------------------------------------------------------------------------------
            if (i >= 0 && i < 3)                 // Squar 3×3 Left
            {
                if (j >= 0 && j < 3)                    // North
                {
                    for (int c1 = 0; c1 < 3; c1++)
                        for (int c2 = 0; c2 < 3; c2++)
                        {
                            if (c1 != i && c2 != j)
                            {
                                _coefficient[c1, c2, _sudoku[i, j]]--;
                                if (_coefficient[c1, c2, _sudoku[i, j]] == 0)
                                    _blackList[c1, c2, _sudoku[i, j]] = true;
                            }
                        }
                }
                else if (j >= 3 && j < 6)               // Bitween
                {
                    for (int c1 = 0; c1 < 3; c1++)
                        for (int c2 = 3; c2 < 6; c2++)
                        {
                            if (c1 != i && c2 != j)
                            {
                                _coefficient[c1, c2, _sudoku[i, j]]--;
                                if (_coefficient[c1, c2, _sudoku[i, j]] == 0)
                                    _blackList[c1, c2, _sudoku[i, j]] = true;
                            }
                        }
                }
                else                                    //  South
                {
                    for (int c1 = 0; c1 < 3; c1++)
                        for (int c2 = 6; c2 < 9; c2++)
                        {
                            if (c1 != i && c2 != j)
                            {
                                _coefficient[c1, c2, _sudoku[i, j]]--;
                                if (_coefficient[c1, c2, _sudoku[i, j]] == 0)
                                    _blackList[c1, c2, _sudoku[i, j]] = true;
                            }
                        }
                }
            }
            else if (i >= 3 && i < 6)            // Squar 3×3 Bitween
            {
                if (j >= 0 && j < 3)                    // North
                {
                    for (int c1 = 3; c1 < 6; c1++)
                        for (int c2 = 0; c2 < 3; c2++)
                        {
                            if (c1 != i && c2 != j)
                            {
                                _coefficient[c1, c2, _sudoku[i, j]]--;
                                if (_coefficient[c1, c2, _sudoku[i, j]] == 0)
                                    _blackList[c1, c2, _sudoku[i, j]] = true;
                            }
                        }
                }
                else if (j >= 3 && j < 6)               // Bitween
                {
                    for (int c1 = 3; c1 < 6; c1++)
                        for (int c2 = 3; c2 < 6; c2++)
                        {
                            if (c1 != i && c2 != j)
                            {
                                _coefficient[c1, c2, _sudoku[i, j]]--;
                                if (_coefficient[c1, c2, _sudoku[i, j]] == 0)
                                    _blackList[c1, c2, _sudoku[i, j]] = true;
                            }
                        }
                }
                else                                    // South
                {
                    for (int c1 = 3; c1 < 6; c1++)
                        for (int c2 = 6; c2 < 9; c2++)
                        {
                            if (c1 != i && c2 != j)
                            {
                                _coefficient[c1, c2, _sudoku[i, j]]--;
                                if (_coefficient[c1, c2, _sudoku[i, j]] == 0)
                                    _blackList[c1, c2, _sudoku[i, j]] = true;
                            }
                        }
                }
            }
            else                                 // Squar 3×3 Right
            {
                if (j >= 0 && j < 3)                    // North
                {
                    for (int c1 = 6; c1 < 9; c1++)
                        for (int c2 = 0; c2 < 3; c2++)
                        {
                            if (c1 != i && c2 != j)
                            {
                                _coefficient[c1, c2, _sudoku[i, j]]--;
                                if (_coefficient[c1, c2, _sudoku[i, j]] == 0)
                                    _blackList[c1, c2, _sudoku[i, j]] = true;
                            }
                        }
                }
                else if (j >= 3 && j < 6)               // Bitween
                {
                    for (int c1 = 6; c1 < 9; c1++)
                        for (int c2 = 3; c2 < 6; c2++)
                        {
                            if (c1 != i && c2 != j)
                            {
                                _coefficient[c1, c2, _sudoku[i, j]]--;
                                if (_coefficient[c1, c2, _sudoku[i, j]] == 0)
                                    _blackList[c1, c2, _sudoku[i, j]] = true;
                            }
                        }
                }
                else                                    // South
                {
                    for (int c1 = 6; c1 < 9; c1++)
                        for (int c2 = 6; c2 < 9; c2++)
                        {
                            if (c1 != i && c2 != j)
                            {
                                _coefficient[c1, c2, _sudoku[i, j]]--;
                                if (_coefficient[c1, c2, _sudoku[i, j]] == 0)
                                    _blackList[c1, c2, _sudoku[i, j]] = true;
                            }
                        }
                }
            }
            //----------------------------------------------------------------------------------------------
            _coefficient[i, j, _sudoku[i, j]] = 0;
            int Plus = _sudoku[i, j] + 1;
            _blackList[i, j, _sudoku[i, j]] = true;
            // Return New Number
            bool Finded = false;
            for (; Plus < 10; Plus++)
            {
                if (_blackList[i, j, Plus] == true)
                {
                    _blackList[i, j, Plus] = false;
                    _coefficient[i, j, Plus]++;
                    _sudoku[i, j] = Plus;
                    FillByOne(i, j, Plus);
                    Finded = true;
                    break;
                }
            }
            if (Finded == false)
            {
                if (p != 0)
                {
                    FreePhase(p - 1);
                    _backPec--;
                }
                else
                {
                    MessageBox.Show("Your Entered Numbers is not correct or is more than Normaly"
                        + "\nPlease Change your Number's", "Warning Incorrect Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _timerUndoColor = false; // Handler False
                    _locX = 0;
                    _locY = 0;
                    _pec = 0;
                    _backPec = 0;
                    btnSolve.Enabled = true;
                    return;
                }
            }
            else Finded = false;
        }
        public void FillByOne(int i, int j, int no)
        {
            for (int c = 0; c < 9; c++)
            {
                // Fill All Row's Number of Operator No. ===================
                if (c != i)
                {
                    _blackList[c, j, no] = false;
                    _coefficient[c, j, no]++;
                }
                //==========================================================
                // Fill All Column's Number of Operator No. ================
                if (c != j)
                {
                    _blackList[i, c, no] = false;
                    _coefficient[i, c, no]++;
                }
                // =========================================================
            }
            //-------------------------------------- Fill Squar's of the Operator No. ----------------------
            //----------------------------------------------------------------------------------------------
            if (i >= 0 && i < 3)                 // Squar 3×3 Left
            {
                if (j >= 0 && j < 3)                    // North
                {
                    for (int c1 = 0; c1 < 3; c1++)
                        for (int c2 = 0; c2 < 3; c2++)
                        {
                            if (c1 != i && c2 != j)
                            {
                                _blackList[c1, c2, no] = false;
                                _coefficient[c1, c2, no]++;
                            }
                        }
                }
                else if (j >= 3 && j < 6)               // Bitween
                {
                    for (int c1 = 0; c1 < 3; c1++)
                        for (int c2 = 3; c2 < 6; c2++)
                        {
                            if (c1 != i && c2 != j)
                            {
                                _blackList[c1, c2, no] = false;
                                _coefficient[c1, c2, no]++;
                            }
                        }
                }
                else                                    //  South
                {
                    for (int c1 = 0; c1 < 3; c1++)
                        for (int c2 = 6; c2 < 9; c2++)
                        {
                            if (c1 != i && c2 != j)
                            {
                                _blackList[c1, c2, no] = false;
                                _coefficient[c1, c2, no]++;
                            }
                        }
                }
            }
            else if (i >= 3 && i < 6)            // Squar 3×3 Bitween
            {
                if (j >= 0 && j < 3)                    // North
                {
                    for (int c1 = 3; c1 < 6; c1++)
                        for (int c2 = 0; c2 < 3; c2++)
                        {
                            if (c1 != i && c2 != j)
                            {
                                _blackList[c1, c2, no] = false;
                                _coefficient[c1, c2, no]++;
                            }
                        }
                }
                else if (j >= 3 && j < 6)               // Bitween
                {
                    for (int c1 = 3; c1 < 6; c1++)
                        for (int c2 = 3; c2 < 6; c2++)
                        {
                            if (c1 != i && c2 != j)
                            {
                                _blackList[c1, c2, no] = false;
                                _coefficient[c1, c2, no]++;
                            }
                        }
                }
                else                                    // South
                {
                    for (int c1 = 3; c1 < 6; c1++)
                        for (int c2 = 6; c2 < 9; c2++)
                        {
                            if (c1 != i && c2 != j)
                            {
                                _blackList[c1, c2, no] = false;
                                _coefficient[c1, c2, no]++;
                            }
                        }
                }
            }
            else                                 // Squar 3×3 Right
            {
                if (j >= 0 && j < 3)                    // North
                {
                    for (int c1 = 6; c1 < 9; c1++)
                        for (int c2 = 0; c2 < 3; c2++)
                        {
                            if (c1 != i && c2 != j)
                            {
                                _blackList[c1, c2, no] = false;
                                _coefficient[c1, c2, no]++;
                            }
                        }
                }
                else if (j >= 3 && j < 6)               // Bitween
                {
                    for (int c1 = 6; c1 < 9; c1++)
                        for (int c2 = 3; c2 < 6; c2++)
                        {
                            if (c1 != i && c2 != j)
                            {
                                _blackList[c1, c2, no] = false;
                                _coefficient[c1, c2, no]++;
                            }
                        }
                }
                else                                    // South
                {
                    for (int c1 = 6; c1 < 9; c1++)
                        for (int c2 = 6; c2 < 9; c2++)
                        {
                            if (c1 != i && c2 != j)
                            {
                                _blackList[c1, c2, no] = false;
                                _coefficient[c1, c2, no]++;
                            }
                        }
                }
            }
            //----------------------------------------------------------------------------------------------
        }


        private void btnSample_Click(object sender, EventArgs e)
        {
            btnClear_Click(sender, e);
            _sampleRun = true;
            if (_sampleReadOnce == false)
            {
                ReadAllFile();
                RunSample();
                _sampleReadOnce = true;
            }
            else
                RunSample();


        }
        void ReadAllFile()
        {
            for (int i = 0; i < 27; i++)
                for (int j = 0; j < 45; j++)
                    _sample[i, j] = string.Empty;
            int index = 0, index2 = 0;
            for (int i = 0; i < 1053; i++)
            {
                if (_readSample[i] != "-----")
                {
                    _sample[index, index2] = _readSample[i];
                    index2++;
                }
                else
                {
                    index++;
                    index2 = 0;
                }
            }
        }
        void RunSample()
        {
            if (_phaseOfSample > 26)
                _phaseOfSample = 0;
            for (int i = 0; i < 45; i++)
            {
                if (_sample[_phaseOfSample, i] != string.Empty)
                {
                    char[] strPec = _sample[_phaseOfSample, i].ToCharArray();
                    int X = int.Parse(strPec[0].ToString());
                    int Y = int.Parse(strPec[2].ToString());
                    int No = int.Parse(strPec[4].ToString());
                    _sudoku[X, Y] = No;
                    _txtSudoku[X, Y].ForeColor = Color.Black;
                    _txtSudoku[X, Y].Text = No.ToString();
                    _txtSudoku[X, Y].Enabled = false;
                }
                else break;
            }
            _phaseOfSample++;
        }


        private void timerSampleSolve_Tick(object sender, EventArgs e)
        {
            _time++;
        }
    }
}
