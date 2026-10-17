// Execute the main application



// Code I wrote from homework assignment
// I will modify to match UML and objectives for my game
// I ended up adding a method and changing the entire name...

using System;
using System.Linq;


// Run tests of classes
Tests.RunAllTests();


// Manage the state of the hidden Puzzle and check guesses
// Change name to Puzzle from Word since it technically will contain phrases and not just Puzzles necessarily
public class Puzzle
{
    private const char BLANKCHAR = '_'; // set character for unknown letters
    private string hiddenPuzzle; // Puzzle to be found
    private char[] hiddenPuzzleState; // game state of hidden Puzzle in play

    // Puzzle constructor
    public Puzzle(string puzzle)
    {
        hiddenPuzzle = puzzle.ToUpper(); // all caps (like Wheel of Fortune)
        hiddenPuzzleState = new char[hiddenPuzzle.Length];

        // Array.Fill(hiddenPuzzleState, BLANKCHAR); // only useful for single Puzzles

        // Hide letters and preserve spaces and punctuation
        for (int i = 0; i < hiddenPuzzle.Length; i++)
        {
            if (char.IsLetter(hiddenPuzzle[i]))
            {
                hiddenPuzzleState[i] = BLANKCHAR;
            }
            else
            {
                hiddenPuzzleState[i] = hiddenPuzzle[i];
            }
        }
    }

    // Display current game state of hidden Puzzle 
    public string CheckHiddenPuzzleState()
    {
        return new string(hiddenPuzzleState);
    }

    // Check if letter is in Puzzle and reveal all instances if so
    // Change from bool to int for purposes of scoring
    public int CheckLetter(char letter)
    {
        int matches = 0;
        letter = char.ToUpper(letter);

        for (int i = 0; i < hiddenPuzzle.Length; i++)
        {
            if (letter == hiddenPuzzle[i])
            {
                matches++;
                hiddenPuzzleState[i] = letter;
            }
        }
        return matches;
    }

    // I forgot about this when making the UML...
    // Allows for guessing the entire Puzzle/phrase
    public bool CheckPuzzle(string guess)
    {
        if (guess.ToUpper() == hiddenPuzzle)
        {
            // Gets the puzzle to solved state if guess is correct
            hiddenPuzzleState = hiddenPuzzle.ToCharArray();
            return true;
        }
        return false;
    }

    // Streamlined it with linq
    // Check if puzzle is solved
    public bool IsSolved()
    {
        return !hiddenPuzzleState.Contains(BLANKCHAR);
    }
}


public static class Tests
{
    public static void RunAllTests()
    {
        Console.WriteLine("Test Puzzle Class\n");
        TestPuzzleClass();
        Console.WriteLine("\nPuzzle Class Test Complete");
        // Console.WriteLine("Test GameBase Class\n");
        // TestGameBase();
        // Console.WriteLine("\nGameBase Class Test Complete");
        // Console.WriteLine("Test PuzzleManager Class\n");
        // TestPuzzleManager();
        // Console.WriteLine("\PuzzleManager Class Test Complete");
        // Console.WriteLine("Test WheelOfHangManGame Class\n");
        // TestWheelOfHangManGameLogic();
        // Console.WriteLine("\nWheelOfHangManGame Class Test Complete");
    }

    private static void TestPuzzleClass()
    {
        int fails = 0;
        int numTests = 4;
        Console.WriteLine("Testing Puzzle.cs...");
        
        Puzzle testPuzzle = new Puzzle("HELLO WORLD!");
        
        // Test 1: Initialization
        if (testPuzzle.CheckHiddenPuzzleState() != "_____ _____!")
        {
            fails++;
            Console.WriteLine("FAIL: Initialization did not format correctly.");
        }
            

        // Test 2: CheckLetter (Correct)
        int lCount = testPuzzle.CheckLetter('L');
        if (lCount != 3 || testPuzzle.CheckHiddenPuzzleState() != "__LL_ ___L_!")
        {
            fails++;
            Console.WriteLine("FAIL: CheckLetter did not return correct count or update state.");
        }
            
        // Test 3: CheckLetter (Incorrect)
        int zCount = testPuzzle.CheckLetter('Z');
        if (zCount != 0)
        {
            fails++;
            Console.WriteLine("FAIL: CheckLetter found a letter that doesn't exist.");
        }
            

        // Test 4: CheckPuzzle (Correct guess)
        bool correctSolve = testPuzzle.CheckPuzzle("HELLO WORLD!");
        if (!correctSolve || !testPuzzle.IsSolved())
        {
            fails++;
            Console.WriteLine("FAIL: CheckPuzzle failed to solve the game.");
        }
            

        if (fails == 0)
        {
            Console.WriteLine("Puzzle.cs Tests: PASSED ALL");
        }
        else
        {
            Console.WriteLine($"Puzzle.cs Tests: FAILED {fails} of {numTests}");
        }
    }
}
