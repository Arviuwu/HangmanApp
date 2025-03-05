using System;
using System.Windows;
using System.Windows.Input;

namespace HangmanApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    bool open = false;
    List<char> guessedLetters = new List<char>(); // empty list of wrongly guessed characters
    string[] wordList = new string[] // list of possible words
    {
        "Apple", "Tiger", "River", "Mountain", "Puzzle",
        "Chair", "Cloud", "Bottle", "School", "Flower",
        "Window", "Pencil", "Garden", "Rocket", "Banana",
        "Orange", "House", "Bread", "Table", "Friend",
        "Dog", "Sun", "Tree", "Bridge", "Candle",
        "Laptop", "Forest", "Village", "Thunder", "Rainbow",
        "Notebook", "Backpack", "Sandwich", "Adventure", "Chocolate",
        "Morning", "Pebble", "Castle", "Guitar", "Universe"
    };
    string guess = "";
    char letterGuess = '\0';
    int lives = 7;
    Random random = new Random(); //random init
    bool repeat = true;
    string word;
    string oldWord;
    string[] guessDisplay;
    char[] wordArray;
    bool alive = true;
    string dialogueMessage;

    public MainWindow()
    {
        InitializeComponent();
        InitializeGame();
    }
    public void InitializeGame()
    {
        Replay();
        lives = 7;
        guessedLetters.Clear();
        word = RandomWord(wordList, random); //choosing random word
        guessDisplay = new string[word.Length]; // string array with as many positions as the word is long      
        wordArray = word.ToCharArray(); // word converted to array with characters
        oldWord = word; // store word for loss message

        dialogueMessage = "Take your guess!";
        // create guess display array with blanks
        for (int i = 0; i < wordArray.Length; i++)
        {
            guessDisplay[i] = ("_ ");
        }
        tbguessDisplay.Text = string.Join("", guessDisplay);
        tblives.Text = $"Lives remaining: {lives}";
        tbDialogue.Text = dialogueMessage;
        tbWord.Text = word;
        tbxGuessInput.Visibility = Visibility.Visible;
        tbguessedLetters.Text = "Guessed Letters: ";
    }
    static string RandomWord(string[] wordList, Random random)
    {
        int randomWordIndex = random.Next(0, wordList.Length); //random index for list of all possible words
        return wordList[randomWordIndex].ToUpper();  //saving randomly selected word in "word" string
    }

    private void tbxGuessInput_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            guess = tbxGuessInput.Text.ToUpper();
            if (!String.IsNullOrEmpty(guess))
            {
                letterGuess = guess[0]; //assigns the first character of the input to the letter guess variable

                if (guess.Length != 1)  //checks for multi letter inputs
                {

                    tbDialogue.Text = "Guess a single letter!";

                }
                else if (!Char.IsLetter(letterGuess))
                {

                    tbDialogue.Text = "You may only guess letters!";

                }
                else if (guessDisplay.Contains(letterGuess + " ")) //Checks blanks if the letter is already correclty guessed
                {
                    tbDialogue.Text = "You already guessed this letter correctly!";
                }
                else if (guessedLetters.Contains(letterGuess))  // checks guessed letters for duplicate guess
                {

                    tbDialogue.Text = "You have already guessed that letter!";

                }
                else if (!guessedLetters.Contains(letterGuess) && guess.Length == 1) // exits validation loop if the guessed letter has not been guessed before
                {
                    if (!word.Contains(letterGuess))
                    {
                        guessedLetters.Add(letterGuess);
                        tbguessedLetters.Text = "Guessed letters: " + string.Join(", ", guessedLetters);
                        lives--;
                        tblives.Text = $"Lives remaining: {lives}";
                        tbDialogue.Text = dialogueMessage;
                    }
                    else if (word.Contains(letterGuess))
                    {
                        // fill in correct letters to guess display
                        for (int i = 0; i < wordArray.Length; i++)
                        {
                            if (wordArray[i].Equals(letterGuess))
                            {
                                guessDisplay[i] = (wordArray[i] + " ");
                            }
                        }
                        tbguessDisplay.Text = string.Join("", guessDisplay);
                        tbDialogue.Text = dialogueMessage;
                    }
                }
                else //unexpected error
                {
                    tbDialogue.Text = "Something went wrong";
                }
                tbxGuessInput.Clear();
            }
            else
            {
                tbDialogue.Text = "Enter a letter";
            }

            if (!guessDisplay.Contains("_ "))
            {
                tbDialogue.Text = "You won!";
                tbxGuessInput.Visibility = Visibility.Hidden;
                Replay();
            }
            else if (lives == 0)
            {
                tbDialogue.Text = $"You lost. The word was {oldWord}.";
                tbxGuessInput.Visibility = Visibility.Hidden;
                Replay();
            }
        }
    }
    private void Replay()
    {
        
        if (open == true)
        {
            btnReset.Visibility = Visibility.Visible;
            btnReset.IsEnabled = true;
            btnExit.Visibility = Visibility.Visible;
            btnExit.IsEnabled = true;
            open = false;
        }
        else
        {
            btnReset.Visibility = Visibility.Hidden;
            btnReset.IsEnabled = false;
            btnExit.Visibility = Visibility.Hidden;
            btnExit.IsEnabled = false;
            open = true;
        }
    }
    private void btnReset_Click(object sender, RoutedEventArgs e)
    {
        InitializeGame();
    }
    private void btnExit_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

}
