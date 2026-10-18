# Wheel of Hangman

A classic word puzzle game built with C# and .NET Windows Forms. This application combines the hidden-phrase solving of Hangman with the point mechanics of Wheel of Fortune (another version of Hangman). 

## Prerequisites
* **Operating System:** Windows (Required for Windows Forms applications)
* **Framework:** .NET 8.0 SDK or higher
* **IDE:** Visual Studio 2022 (Recommended) or VS Code

## How to Build and Run

###  Using Visual Studio
1. Clone or download the repository.
2. Open the WheelOfHangman.sln file in Visual Studio.
3. Ensure the puzzles.txt file is in the same directory as the code.
4. Click the Start button at the top to build and launch the game.

## How to Play

1. **Spin the Wheel:** Start your turn by clicking the SPIN WHEEL button to determine the point value for your next guess.
2. **Guess a Consonant:** After spinning, type a single consonant into the input box and press Enter. 
   * If the letter is in the puzzle, you earn the spun points for each time it appears.
   * If it is not in the puzzle, you receive a strike and the hangman drawing advances.
3. **Buy a Vowel:** If you have at least 250 points, you can type a vowel and click Buy Vowel before spinning the wheel. This will deduct 250 points from your score.
4. **Solve the Puzzle:** At the start of any turn, you can type the entire phrase into the solve box and press Enter.
5. **Win/Loss Conditions:** You win by either solving the puzzle directly or guessing all the hidden letters. You lose if you accumulate 6 strikes (this builds a complete hangman).
