
// Code I wrote from homework assignment
// I will modify to match UML and objectives for my game

using System;
using static System.Console;
using System.Globalization;
using System.Threading.Channels;

class Word
{
	private string word;
	private char[] hiddenWordState;

	public Word(string word)
	{
		this.word = word;
		hiddenWordState = new char[word.Length];
		Array.Fill(hiddenWordState, '*');
	}

	public string CheckHiddenWordState()
	{
		return new string(hiddenWordState);
	}

	public bool CheckLetter(char letter)
	{
		bool result = false;

		for (int i = 0; i < word.Length; i++)
		{
			if (letter == word[i])
			{
				result = true;
				hiddenWordState[i] = letter;
			}
		}

		return result;
	}

	public bool IsSolved()
	{
		int totalStars = 0;

		for (int i = 0; i < hiddenWordState.Length; i++)
		{
			if (hiddenWordState[i] == '*')
			{
				totalStars++;
			}
		}

		if (totalStars == 0)
		{
			return true;
		}
		return false;
	}
}
class GuessAWord
{
	static void Main()
	{
		// Write your code here
		string[] words = {"mathematics", "physics", "computer", "science", "software", "development", "engineering", "automobile"};
		
		Random RandomClass = new Random();
		int randomNumber;
		randomNumber = RandomClass.Next(0, words.Length);

		string word = words[randomNumber];

		Word gameWord = new Word(word);

		string guess = "";
		bool correct = false;

		while (!gameWord.IsSolved())
		{
			WriteLine($"Word: {gameWord.CheckHiddenWordState()}");
			Write("Guess a letter >> ");
			guess = ReadLine();
			char guessChar = guess.ToCharArray()[0];
			correct = gameWord.CheckLetter(guessChar);
			if (correct)
			{
				WriteLine($"Yes! {guess} is in the word");
			}
			else
			{
				WriteLine($"Sorry. {guess} is not in the word");
			}
		}
		WriteLine($"Word: {gameWord.CheckHiddenWordState()}");
		WriteLine("Success! You found the hidden word!");
	}
}