using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WheelOfHangman
{
    public class WheelOfHangmanForm : Form
    {
        // user interface
        private System.Windows.Forms.Label lblPuzzleBoard;
        private System.Windows.Forms.Label lblScoreBoard;
        private System.Windows.Forms.Label lblHangman;
        private System.Windows.Forms.Label lblStatusMessage;

        private TextBox txtLetterInput;
        private Button btnGuess;
        private Button btnBuyVowel;

        private TextBox txtSolveInput;
        private Button btnSolve;

        private Button btnSpin;

        private Button btnPlayAgain;
        private Button btnQuit;

        // main game logic object
        private WheelOfHangmanGame game;
        private bool needsConsonant = false;
        private HashSet<char> usedLetters = new HashSet<char>(); // works like a set from set theory

        // form constructor
        public WheelOfHangmanForm()
        {
            InitializeComponent();
            StartNewGame();
        }

        private void InitializeComponent()
        {
            lblPuzzleBoard = new Label();
            lblScoreBoard = new Label();
            lblHangman = new Label();
            lblStatusMessage = new Label();
            txtLetterInput = new TextBox();
            btnGuess = new Button();
            btnBuyVowel = new Button();
            txtSolveInput = new TextBox();
            btnSolve = new Button();
            btnSpin = new Button();
            btnPlayAgain = new Button();
            btnQuit = new Button();
            SuspendLayout();

            // lblPuzzleBoard
            lblPuzzleBoard.AutoSize = true;
            lblPuzzleBoard.Font = new Font("Consolas", 28F, FontStyle.Bold);
            lblPuzzleBoard.Location = new Point(30, 30);
            lblPuzzleBoard.Name = "lblPuzzleBoard";
            lblPuzzleBoard.Size = new Size(284, 55);
            lblPuzzleBoard.TabIndex = 11;
            lblPuzzleBoard.Text = "LOADING...";

            // lblScoreBoard
            lblScoreBoard.AutoSize = true;
            lblScoreBoard.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold);
            lblScoreBoard.ForeColor = Color.DarkSlateGray;
            lblScoreBoard.Location = new Point(30, 90);
            lblScoreBoard.Name = "lblScoreBoard";
            lblScoreBoard.Size = new Size(276, 29);
            lblScoreBoard.TabIndex = 10;
            lblScoreBoard.Text = "Score: 0 | Strikes: 0 / 6";
            
            // lblHangman
            lblHangman.AutoSize = true;
            lblHangman.Font = new Font("Consolas", 14F);
            lblHangman.Location = new Point(380, 104);
            lblHangman.Name = "lblHangman";
            lblHangman.Size = new Size(129, 196);
            lblHangman.TabIndex = 3;
            lblHangman.Text =
@"  +---+
  |   |
      |
      |
      |
      |
========="; // Need ASCII drawing for design view
            
            // lblStatusMessage
            lblStatusMessage.BackColor = Color.White;
            lblStatusMessage.BorderStyle = BorderStyle.FixedSingle;
            lblStatusMessage.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            lblStatusMessage.ForeColor = Color.DarkBlue;
            lblStatusMessage.Location = new Point(30, 320);
            lblStatusMessage.Name = "lblStatusMessage";
            lblStatusMessage.Size = new Size(680, 60);
            lblStatusMessage.TabIndex = 0;
            lblStatusMessage.Text = "Loading game engine...";
            lblStatusMessage.TextAlign = ContentAlignment.MiddleCenter;

            // txtLetterInput
            txtLetterInput.CharacterCasing = CharacterCasing.Upper;
            txtLetterInput.Font = new Font("Microsoft Sans Serif", 14F);
            txtLetterInput.Location = new Point(30, 150);
            txtLetterInput.MaxLength = 1;
            txtLetterInput.Name = "txtLetterInput";
            txtLetterInput.Size = new Size(40, 34);
            txtLetterInput.TabIndex = 7;
            txtLetterInput.KeyDown += txtLetterInput_KeyDown;
            // 
            // btnGuess
            // 
            btnGuess.Location = new Point(80, 149);
            btnGuess.Name = "btnGuess";
            btnGuess.Size = new Size(140, 31);
            btnGuess.TabIndex = 5;
            btnGuess.Text = "Guess Consonant";
            btnGuess.Click += btnGuess_Click;

            // btnBuyVowel
            btnBuyVowel.Location = new Point(230, 149);
            btnBuyVowel.Name = "btnBuyVowel";
            btnBuyVowel.Size = new Size(140, 31);
            btnBuyVowel.TabIndex = 6;
            btnBuyVowel.Text = "Buy Vowel ($250)";
            btnBuyVowel.Click += btnBuyVowel_Click;

            // txtSolveInput
            txtSolveInput.CharacterCasing = CharacterCasing.Upper;
            txtSolveInput.Font = new Font("Microsoft Sans Serif", 14F);
            txtSolveInput.Location = new Point(30, 200);
            txtSolveInput.Name = "txtSolveInput";
            txtSolveInput.Size = new Size(230, 34);
            txtSolveInput.TabIndex = 8;
            txtSolveInput.KeyDown += txtSolveInput_KeyDown;
            // 
            // btnSolve
            // 
            btnSolve.Location = new Point(270, 199);
            btnSolve.Name = "btnSolve";
            btnSolve.Size = new Size(100, 31);
            btnSolve.TabIndex = 9;
            btnSolve.Text = "Solve Puzzle";
            btnSolve.Click += btnSolve_Click;

            // btnSpin
            btnSpin.BackColor = Color.LightGreen;
            btnSpin.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            btnSpin.Location = new Point(30, 250);
            btnSpin.Name = "btnSpin";
            btnSpin.Size = new Size(340, 50);
            btnSpin.TabIndex = 4;
            btnSpin.Text = "SPIN WHEEL";
            btnSpin.UseVisualStyleBackColor = false;
            btnSpin.Click += btnSpin_Click;

            // btnPlayAgain
            btnPlayAgain.Location = new Point(240, 400);
            btnPlayAgain.Name = "btnPlayAgain";
            btnPlayAgain.Size = new Size(120, 40);
            btnPlayAgain.TabIndex = 1;
            btnPlayAgain.Text = "Play Again";
            btnPlayAgain.Visible = false;
            btnPlayAgain.Click += btnPlayAgain_Click;

            // btnQuit
            btnQuit.Location = new Point(380, 400);
            btnQuit.Name = "btnQuit";
            btnQuit.Size = new Size(120, 40);
            btnQuit.TabIndex = 2;
            btnQuit.Text = "Quit Game";
            btnQuit.Visible = false;
            btnQuit.Click += btnQuit_Click;

            // WheelOfHangmanForm
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(929, 470);
            Controls.Add(lblStatusMessage);
            Controls.Add(btnPlayAgain);
            Controls.Add(btnQuit);
            Controls.Add(lblHangman);
            Controls.Add(btnSpin);
            Controls.Add(btnGuess);
            Controls.Add(btnBuyVowel);
            Controls.Add(txtLetterInput);
            Controls.Add(txtSolveInput);
            Controls.Add(btnSolve);
            Controls.Add(lblScoreBoard);
            Controls.Add(lblPuzzleBoard);
            Name = "WheelOfHangmanForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Wheel of Hangman";
            ResumeLayout(false);
            PerformLayout();
        }

        private async void StartNewGame()
        {
            PuzzleManager manager = new PuzzleManager("puzzles.txt");
            await manager.LoadPuzzlesAsync();

            game = new WheelOfHangmanGame("Player 1", manager);
            game.StartNewRound();

            usedLetters.Clear();
            needsConsonant = false; // Start of turn logic

            lblStatusMessage.ForeColor = Color.DarkBlue;
            UpdateDisplay("New game! Spin the wheel.");
        }

        private void UpdateDisplay(string statusUpdate = "")
        {
            lblPuzzleBoard.Text = game.CurrentPuzzle.CheckHiddenPuzzleState();
            lblScoreBoard.Text = $"Score: {game.CurrentScore} | Strikes: {game.IncorrectGuesses} / {game.MaxAttempts}";
            lblHangman.Text = GetHangmanDrawing(game.IncorrectGuesses);

            if (!string.IsNullOrEmpty(statusUpdate))
            {
                lblStatusMessage.Text = statusUpdate;
            }

            txtLetterInput.Clear();
            txtSolveInput.Clear();
            txtLetterInput.Focus();

            if (game.IsGameOver || game.CurrentPuzzle.IsSolved())
            {
                btnSpin.Enabled = false;
                btnGuess.Enabled = false;
                btnBuyVowel.Enabled = false;
                btnSolve.Enabled = false;

                btnPlayAgain.Visible = true;
                btnQuit.Visible = true;
            }
            else
            {
                if (needsConsonant)
                {
                    btnSpin.Enabled = false;
                    btnSolve.Enabled = false;
                    btnBuyVowel.Enabled = false;
                    btnGuess.Enabled = true;
                    btnSpin.BackColor = Color.LightGray;
                }
                else
                {
                    btnSpin.Enabled = true;
                    btnSolve.Enabled = true;
                    btnBuyVowel.Enabled = game.CurrentScore >= 250;
                    btnGuess.Enabled = false;
                    btnSpin.BackColor = Color.LightGreen; // Ready to spin
                }
            }
        }

        private void CheckWinLoss()
        {
            if (game.IsGameOver)
            {
                lblStatusMessage.ForeColor = Color.DarkRed;
                UpdateDisplay("GAME OVER! You ran out of strikes.");
            }
            else if (game.CurrentPuzzle.IsSolved())
            {
                lblStatusMessage.ForeColor = Color.DarkGreen;
                UpdateDisplay("CONGRATULATIONS! You solved the puzzle!");
            }
        }

        // --- SAFE KEYBOARD EVENTS ---
        private void txtLetterInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (txtLetterInput.Text.Length > 0)
                {
                    char guess = txtLetterInput.Text[0];
                    if ("AEIOU".Contains(guess) && btnBuyVowel.Enabled)
                        btnBuyVowel.PerformClick();
                    else if (!"AEIOU".Contains(guess) && btnGuess.Enabled)
                        btnGuess.PerformClick();
                }
            }
        }

        // can use Enter key instead of having to click button
        private void txtSolveInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && btnSolve.Enabled)
            {
                e.SuppressKeyPress = true;
                btnSolve.PerformClick();
            }
        }

        // button event handlers
        private void btnSpin_Click(object sender, EventArgs e)
        {
            int points = game.SpinWheel();
            needsConsonant = true; // Lock the player into guessing a consonant

            lblStatusMessage.ForeColor = Color.DarkBlue;
            UpdateDisplay($"You spun for {points} points! You MUST guess a consonant.");
        }

        private void btnGuess_Click(object sender, EventArgs e)
        {
            if (txtLetterInput.Text.Length == 0) return;
            char guess = txtLetterInput.Text[0];

            if ("AEIOU".Contains(guess))
            {
                lblStatusMessage.ForeColor = Color.DarkRed;
                UpdateDisplay("That is a vowel! You must guess a consonant after spinning.");
                return;
            }

            if (usedLetters.Contains(guess))
            {
                lblStatusMessage.ForeColor = Color.DarkRed;
                UpdateDisplay($"You already guessed '{guess}'! Pick a different consonant.");
                return;
            }

            usedLetters.Add(guess);
            bool found = game.GuessLetter(guess, false);
            needsConsonant = false; // Unlock standard turn options

            lblStatusMessage.ForeColor = Color.DarkBlue;
            if (found)
            {
                UpdateDisplay($"Good job! '{guess}' is in the puzzle. Spin, solve, or buy a vowel.");
            }
            else
            {
                UpdateDisplay($"Sorry, there is no '{guess}'. Strike {game.IncorrectGuesses}!");
            }

            CheckWinLoss();
        }

        private void btnBuyVowel_Click(object sender, EventArgs e)
        {
            if (txtLetterInput.Text.Length == 0) return;
            char guess = txtLetterInput.Text[0];

            if (!"AEIOU".Contains(guess))
            {
                lblStatusMessage.ForeColor = Color.DarkRed;
                UpdateDisplay("That is a consonant! You can only buy vowels here.");
                return;
            }

            if (usedLetters.Contains(guess))
            {
                lblStatusMessage.ForeColor = Color.DarkRed;
                UpdateDisplay($"You already bought '{guess}'! Pick a different vowel.");
                return;
            }

            usedLetters.Add(guess);
            bool found = game.BuyVowel(guess);

            // can keep buying vowels until out of points (or win)... no need to spin
            lblStatusMessage.ForeColor = Color.DarkBlue;
            if (found)
                UpdateDisplay($"You bought an '{guess}'. Spin, solve, or buy another vowel.");
            else
                UpdateDisplay($"You bought an '{guess}', but it missed. Strike {game.IncorrectGuesses}!");

            CheckWinLoss();
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            if (txtSolveInput.Text.Length == 0) return;

            bool correct = game.SolvePuzzle(txtSolveInput.Text);

            if (correct)
            {
                CheckWinLoss(); // Triggers the victory message
            }
            else
            {
                lblStatusMessage.ForeColor = Color.DarkRed;
                UpdateDisplay($"Incorrect solve attempt. Strike {game.IncorrectGuesses}! Your turn continues.");
                CheckWinLoss();
            }
        }

        // after game is over
        private void btnPlayAgain_Click(object sender, EventArgs e) => StartNewGame();
        private void btnQuit_Click(object sender, EventArgs e) => Application.Exit();

        // hangman states based on strikes
        private string GetHangmanDrawing(int strikes)
        {
            string[] stages = {
@"  +---+
  |   |
      |
      |
      |
      |
=========",
@"  +---+
  |   |
  O   |
      |
      |
      |
=========",
@"  +---+
  |   |
  O   |
  |   |
      |
      |
=========",
@"  +---+
  |   |
  O   |
 /|   |
      |
      |
=========",
@"  +---+
  |   |
  O   |
 /|\  |
      |
      |
=========",
@"  +---+
  |   |
  O   |
 /|\  |
 /    |
      |
=========",
@"  +---+
  |   |
  O   |
 /|\  |
 / \  |
      |
=========" };

            // If strikes are out of bounds, return ERROR
            if (strikes < 0 || strikes >= stages.Length)
            { 
                return "INVALID STRIKES ERROR";
            }
            return stages[strikes];
        }
    }
}
