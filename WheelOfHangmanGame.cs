
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

// Run tests of classes
Tests.RunAllTests();

// Modified code I wrote from homework assignment
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

// Base class for game
public abstract class GameBase
{
    private string playerName;
    private int currentScore;
    private bool isGameOver;

    public GameBase(string name)
    {
        playerName = name;
        currentScore = 0;
        isGameOver = false;
    }

    public abstract void EndGame();

    public string PlayerName
    {
        get {return playerName;}
        set {playerName = value;}
    }
    public int CurrentScore
    {
        get {return currentScore;}
        protected set {currentScore = value;}
    }
    public bool IsGameOver
    {
        get {return isGameOver;}
        protected set {isGameOver = value;}
    }
}

// Handles puzzles and files
public class PuzzleManager
{
    private string filePath;
    private List<string> availablePhrases;

    public PuzzleManager(string path)
    {
        filePath = path;
        availablePhrases = new List<string>();
    }

    public async Task LoadPuzzlesAsync()
    {
        if (File.Exists(filePath))
        {
            string[] lines = await File.ReadAllLinesAsync(filePath);
            availablePhrases.AddRange(lines);
        }
        else
        {
            // Fallback phrases
            availablePhrases.Add("SOFTWARE DEVELOPMENT");
            availablePhrases.Add("OBJECT ORIENTED PROGRAMMING");
            availablePhrases.Add("WHEEL OF HANGMAN");
        }
    }

    public string GetRandomPuzzle()
    {
        if (availablePhrases.Count == 0)
        {
            return "DEFAULT PUZZLE";
        }
            
        Random rnd = new Random();
        int index = rnd.Next(availablePhrases.Count);
        return availablePhrases[index];
    }

    public string FilePath 
    { 
        get {return filePath;} 
        set {filePath = value;} 
    }

    public List<string> AvailablePhrases 
    { 
        get {return availablePhrases;} 
        set {availablePhrases = value;} 
    }
}

// The main class for running the game
public class WheelOfHangmanGame : GameBase
{
    private Puzzle currentPuzzle;
    private int currentPointValue;
    private int maxAttempts;
    private int incorrectGuesses;
    private List<char> guessedLetters;
    private PuzzleManager puzzles;

    public Puzzle CurrentPuzzle 
    { 
        get {return currentPuzzle;} 
        set {currentPuzzle = value;} 
    }
    public int CurrentPointValue 
    { 
        get {return currentPointValue;} 
        set {currentPointValue = value;}
    }
    public int MaxAttempts 
    { 
        get {return maxAttempts;} 
        set {maxAttempts = value;}
    }
    public int IncorrectGuesses 
    { 
        get {return incorrectGuesses;} 
        set {incorrectGuesses = value;}
    }
    public List<char> GuessedLetters 
    { 
        get {return guessedLetters;} 
        set {guessedLetters = value;}
    }
    public PuzzleManager Puzzles 
    { 
        get {return puzzles;} 
        set {puzzles = value;}
    }

    // Constructor
    public WheelOfHangmanGame(string playerName, PuzzleManager puzzleManager) : base(playerName)
    {
        puzzles = puzzleManager;
        maxAttempts = 6; // Default strikes allowed
        incorrectGuesses = 0;
        guessedLetters = new List<char>();
    }

    public void StartNewRound()
    {
        string newPhrase = puzzles.GetRandomPuzzle();
        currentPuzzle = new Puzzle(newPhrase);
        guessedLetters.Clear();
        incorrectGuesses = 0;
    }

    public int SpinWheel()
    {
        Random rnd = new Random();
        // Generate value between 100 and 1000 in increments of 50
        currentPointValue = rnd.Next(2, 21) * 50; 
        return currentPointValue;
    }

    public bool GuessLetter(char letter, bool isVowel)
    {
        letter = char.ToUpper(letter);
        if (guessedLetters.Contains(letter)) return false;

        guessedLetters.Add(letter);
        int matches = currentPuzzle.CheckLetter(letter);

        if (matches > 0)
        {
            if (!isVowel) // Vowels don't award points
            {
                CurrentScore += (currentPointValue * matches);
            }
            return true;
        }
        
        incorrectGuesses++;
        if (incorrectGuesses >= maxAttempts) EndGame();
        return false;
    }

    public bool BuyVowel(char vowel)
    {
        const int VOWEL_COST = 250;
        if (CurrentScore >= VOWEL_COST)
        {
            CurrentScore -= VOWEL_COST;
            return GuessLetter(vowel, true);
        }
        return false; // Not enough points
    }

    public bool SolvePuzzle(string guess)
    {
        bool isCorrect = currentPuzzle.CheckPuzzle(guess);
        if (isCorrect)
        {
            CurrentScore += 1000; // Bonus for solving
            return true;
        }
        
        incorrectGuesses++;
        if (incorrectGuesses >= maxAttempts) EndGame();
        return false;
    }

    public override void EndGame()
    {
        IsGameOver = true;
    }
}


public static class Tests
{
    public static void RunAllTests()
    {
        Console.WriteLine("Test Puzzle Class\n");
        TestPuzzleClass();
        Console.WriteLine("\nPuzzle Class Test Complete\n");
        Console.WriteLine("Test GameBase Class\n");
        TestGameBase();
        Console.WriteLine("\nGameBase Class Test Complete\n");
        Console.WriteLine("Test PuzzleManager Class\n");
        TestPuzzleManager();
        Console.WriteLine("\nPuzzleManager Class Test Complete\n");
        Console.WriteLine("Test WheelOfHangManGame Class\n");
        TestWheelOfHangManGameLogic();
        Console.WriteLine("\nWheelOfHangManGame Class Test Complete\n");
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

    // Fake game for testing
    private class MockGame : GameBase
    {
        public MockGame(string name) : base(name) { }

        public override void EndGame()
        {
            IsGameOver = true;
        }
    }

    private static void TestGameBase()
    {
        int fails = 0;
        int numTests = 2;
        Console.WriteLine("Testing GameBase.cs...");

        MockGame testGame = new MockGame("Jeremy");

        // Test 1: Initialization
        if (testGame.PlayerName != "Jeremy" || testGame.CurrentScore != 0 || testGame.IsGameOver != false)
        {
            fails++;
            Console.WriteLine("FAIL: GameBase properties did not initialize correctly.");
        }

        // Test 2: EndGame updates state
        testGame.EndGame();
        if (!testGame.IsGameOver)
        {
            fails++;
            Console.WriteLine("FAIL: EndGame did not set IsGameOver to true.");
        }

        if (fails == 0)
        {
            Console.WriteLine("GameBase.cs Tests: PASSED ALL");
        }
        else
        {
            Console.WriteLine($"GameBase.cs Tests: FAILED {fails} of {numTests}");
        }
    }

    private static void TestPuzzleManager()
    {
        int fails = 0;
        int numTests = 3; // Increased to 3 tests
        Console.WriteLine("Testing PuzzleManager.cs...");

        //  Missing file fallback tests
        PuzzleManager fallbackManager = new PuzzleManager("missing_file_test.txt");
        fallbackManager.LoadPuzzlesAsync().Wait(); 

        // Test 1: Fallback Load
        if (fallbackManager.AvailablePhrases.Count == 0)
        {
            fails++;
            Console.WriteLine("FAIL: LoadPuzzlesAsync did not load any fallback phrases.");
        }

        // Test 2: Get Random Puzzle
        string puzzle = fallbackManager.GetRandomPuzzle();
        if (string.IsNullOrEmpty(puzzle) || puzzle == "DEFAULT PUZZLE")
        {
            fails++;
            Console.WriteLine("FAIL: GetRandomPuzzle did not return a valid phrase.");
        }

        // Test 3: Existing File (File Reading Logic) 
        string testFileName = "temppuzzles.txt";
        
        // Create a temporary text file with two specific phrases
        File.WriteAllLines(testFileName, new string[] { "TEST PHRASE", "ANOTHER TEST PHRASE" });

        // Initialize a new manager pointing to this file
        PuzzleManager fileManager = new PuzzleManager(testFileName);
        fileManager.LoadPuzzlesAsync().Wait();

        // Verify it loaded the 2 file phrases instead of the 3 fallback phrases
        if (fileManager.AvailablePhrases.Count != 2 || !fileManager.AvailablePhrases.Contains("TEST PHRASE"))
        {
            fails++;
            Console.WriteLine("FAIL: LoadPuzzlesAsync did not correctly read phrases from puzzles.txt.");
        }

        // Clean up the test file 
        if (File.Exists(testFileName))
        {
            File.Delete(testFileName);
        }

        if (fails == 0)
        {
            Console.WriteLine("PuzzleManager.cs Tests: PASSED ALL");
        }
        else
        {
            Console.WriteLine($"PuzzleManager.cs Tests: FAILED {fails} of {numTests}");
        }
    }

    private static void TestWheelOfHangManGameLogic()
    {
        int fails = 0;
        int numTests = 3;
        Console.WriteLine("Testing WheelOfHangmanGame.cs...");

        PuzzleManager mockManager = new PuzzleManager("missing.txt");
        mockManager.LoadPuzzlesAsync().Wait(); // Load fallbacks

        WheelOfHangmanGame game = new WheelOfHangmanGame("Jeremy", mockManager);
        game.StartNewRound();

        // Test 1: Initialization
        if (game.CurrentPuzzle == null || game.PlayerName != "Jeremy")
        {
            fails++;
            Console.WriteLine("FAIL: Game did not initialize or start round correctly.");
        }

        // Test 2: Spin Wheel
        int spinValue = game.SpinWheel();
        if (spinValue < 100 || spinValue > 1000 || game.CurrentPointValue != spinValue)
        {
            fails++;
            Console.WriteLine("FAIL: SpinWheel did not generate a valid point value.");
        }

        // Test 3: Incorrect Guesses & Game Over state
        game.MaxAttempts = 1; 
        game.GuessLetter('Z', false); // Assuming Z isn't in the default phrase
        if (!game.IsGameOver || game.IncorrectGuesses != 1)
        {
            fails++;
            Console.WriteLine("FAIL: Incorrect guess did not increment counter or trigger EndGame.");
        }

        if (fails == 0)
        {
            Console.WriteLine("WheelOfHangmanGame.cs Tests: PASSED ALL");
        }
        else
        {
            Console.WriteLine($"WheelOfHangmanGame.cs Tests: FAILED {fails} of {numTests}");
        }
    }
}
