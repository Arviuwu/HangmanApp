using System;
using System.Windows;
using System.Windows.Input;

namespace HangmanApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
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

                    tbDialogue.Text = "    Guess a single letter!";

                }
                else if (!Char.IsLetter(letterGuess))
                {

                    tbDialogue.Text = "    You may only guess letters!";

                }
                else if (guessDisplay.Contains(letterGuess + " ")) //Checks blanks if the letter is already correclty guessed
                {
                    tbDialogue.Text = "    You already guessed this letter correctly!";
                }
                else if (guessedLetters.Contains(letterGuess))  // checks guessed letters for duplicate guess
                {

                    tbDialogue.Text = "    You have already guessed that letter!";

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
                    tbDialogue.Text = "    Something went wrong";
                }
                tbxGuessInput.Clear();
            }
            else
            {
                tbDialogue.Text = "Enter a letter";
            }

            if (!guessDisplay.Contains("_ "))
            {
                tbDialogue.Text = "    You won!";
            }
            else if (lives == 0)
            {
                tbDialogue.Text = $"    You lost. The word was {oldWord}.";
                btnReset.Visibility = Visibility.Visible;
                btnReset.IsEnabled = true;
            }
        }
    }
    private void Replay()
    {

    }
    private void btnReset_Click(object sender, RoutedEventArgs e)
    {
        InitializeGame();
    }
}
/*static void Main()
    {
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
        string[] hangmanStages = new string[]
        {

        int hangManDrawingProgress = hangmanStages.Length - lives - 1; // -1 -> zero based array
        bool repeat = true;
        string word;
        string oldWord;
        string[] guessDisplay;
        char[] wordArray;
        bool alive = true;

        while (repeat)
        {

            word = RandomWord(wordList, random); //choosing random word
            guessDisplay = new string[word.Length]; // string array with as many positions as the word is long      
            wordArray = word.ToCharArray(); // word converted to array with characters
            oldWord = word; // store word for loss message

            // create guess display array with blanks
            for (int i = 0; i < wordArray.Length; i++)
            {
                guessDisplay[i] = ("_ ");
            }

            // guess loop
            while (alive)
            {
                UpdateUI(word, guessDisplay, lives, guessedLetters, hangmanStages, hangManDrawingProgress);

                CheckWinLoss(guessDisplay, ref alive, lives);

                // input validation (empty/too many characters/already guessed)
                letterGuess = InputValidation(word, guessDisplay, lives, guessedLetters, hangmanStages, hangManDrawingProgress, guess, letterGuess);

                // adds wrong guesses to list
                if (!word.Contains(letterGuess))
                {
                    guessedLetters.Add(letterGuess);
                }


                // lives deduction on wrong guess
                if (!wordArray.Contains(letterGuess))
                {
                    lives--;
                    hangManDrawingProgress = hangmanStages.Length - lives - 1;
                }

                // fill in correct letters to guess display
                for (int i = 0; i < wordArray.Length; i++)
                {
                    if (wordArray[i].Equals(letterGuess))
                    {
                        guessDisplay[i] = (wordArray[i] + " ");
                    }
                }
            }

            Console.WriteLine();

            // win/lose message
            GameEndMessage(lives, oldWord);

            RepeatGameDialogue(ref lives, ref hangManDrawingProgress, hangmanStages, ref guessedLetters, ref repeat, ref alive);
        }
    }
    static string RandomWord(string[] wordList, Random random)
    {
        int randomWordIndex = random.Next(0, wordList.Length); //random index for list of all possible words
        return wordList[randomWordIndex].ToUpper();  //saving randomly selected word in "word" string
    }

    static void UpdateUI(string word, string[] guessDisplay, int lives, List<char> guessedLetters, string[] hangmanStages, int hangManDrawingProgress)
    {
        Console.Clear();
        Console.WriteLine();
        // print updated guess display
        Console.Write("    ");
        foreach (string c in guessDisplay)
        {
            Console.Write(c);
        }

        // print remaining lives
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine($"    Lives remaining: {lives}");
        Console.WriteLine();

        // displays the list of guessed letters
        Console.Write("    Guessed Letters: ");
        foreach (char i in guessedLetters)
        {
            Console.Write(i + " ");
        }
        Console.WriteLine();
        // drawing hangman depending on wrong guesses
        Console.WriteLine(hangmanStages[hangManDrawingProgress]);
        Console.WriteLine();
    }

    static char InputValidation(string word, string[] guessDisplay, int lives, List<char> guessedLetters, string[] hangmanStages, int hangManDrawingProgress, string guess, char letterGuess)
    {
        string dialogueMessage = "    Take your guess!";

        while (true)
        {
            UpdateUI(word, guessDisplay,lives, guessedLetters, hangmanStages,  hangManDrawingProgress);
            Console.WriteLine(dialogueMessage);
            // Letter guess input
            Console.Write("    ");
            guess = Console.ReadLine().ToUpper();

            // empty input protection
            if (guess == "" || guess == null)
            {

                dialogueMessage = "    Enter a letter!";
                continue;
            }

            letterGuess = guess[0]; //assigns the first character of the input to the letter guess variable

            if (guess.Length != 1)  //checks for multi letter inputs
            {

                dialogueMessage = "    Guess a single letter!";
                continue;
            }
            else if (!Char.IsLetter(letterGuess))
            {

                dialogueMessage = "    You may only guess letters!";
                continue;
            }
            if (guessDisplay.Contains(letterGuess + " ")) //Checks blanks if the letter is already correclty guessed
            {
                dialogueMessage = "    You already guessed this letter correctly!";
                continue;
            }
            else if (guessedLetters.Contains(letterGuess))  // checks guessed letters for duplicate guess
            {

                dialogueMessage = "    You have already guessed that letter!";
                continue;
            }
            else if (!guessedLetters.Contains(letterGuess) && guess.Length == 1) // exits validation loop if the guessed letter has not been guessed before
            {
                break;
            }
            else //unexpected error
            {
                Console.WriteLine("    Something went wrong");
            }
        }
        return letterGuess;
    }

    static void GameEndMessage(int lives, string oldWord)
    {
        if (lives > 0)
        {
            Console.WriteLine("    You won!");
        }
        else if (lives == 0)
        {
            Console.WriteLine($"    You lost. The word was {oldWord}.");
        }
        else
        {
            Console.WriteLine("    Lives somehow negative");
        }
    }

    static void RepeatGameDialogue(ref int lives, ref int hangManDrawingProgress, string[] hangmanStages, ref List<char> guessedLetters, ref bool repeat, ref bool alive)
    {
        while (true)
        {
            Console.WriteLine("    Do you want to play another round? (Y/N)");
            Console.Write("    ");
            string anotherRound = Console.ReadLine().ToUpper();
            if (anotherRound == "Y")
            {
                //reset lives/drawing progress/guessed letter list
                lives = 7;
                alive = true;
                hangManDrawingProgress = hangmanStages.Length - lives - 1;
                guessedLetters.Clear();
                break;
            }
            else if (anotherRound == "N")
            {
                //exit game loop
                repeat = false;
                break;
            }
            else
            {
                //wrong input (y/n)
                Console.WriteLine("    Enter \"Y\" play another round, \"N\" to stop playing.");
            }
        }
    }
    static void CheckWinLoss(string[] guessDisplay, ref bool alive, int lives)
    {
        if (!guessDisplay.Contains("_ "))
        {
            alive = false;
        }
        if (lives == 0)
        {
            alive = false;
        }
    }*/