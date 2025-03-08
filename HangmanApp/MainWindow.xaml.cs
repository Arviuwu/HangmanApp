using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HangmanApp;

public partial class MainWindow : Window
{
    bool open = false; //button visibility switch
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
    "Morning", "Pebble", "Castle", "Guitar", "Universe",
    "Meadow", "Jungle", "Lantern", "Diamond", "Picture",
    "Parrot", "Balloon", "Mailbox", "Harbor", "Planet",
    "Fountain", "Coconut", "Whistle", "Compass", "Festival",
    "Library", "Tornado", "Sunset", "Luggage", "Carousel",
    "Voyage", "Gateway", "Fortune", "Meadow", "Painting",
    "Chimney", "Octopus", "Firefly", "Cottage", "Snowflake",
    "Windmill", "Backyard", "Treasure", "Icicle", "Blossom",
    "Lantern", "Seagull", "Volcano", "Cabbage", "Sparrow",
    "Marbles", "Velvet", "Picnic", "Cushion", "Courage",
    "Jigsaw", "Bracelet", "Trophy", "Postcard", "Lullaby"
};
    string guess = "";
    char letterGuess = '\0';
    int lives = 7;
    Random random = new Random(); //random init
    string word;
    string oldWord;
    string[] guessDisplay;
    char[] wordArray;
    string dialogueMessage;
    int stage = 0;
    int streak = 0;

    public MainWindow()
    {
        InitializeComponent();
        InitializeGame();
        DrawHangman(stage);
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
        stage = 0;
        dialogueMessage = "";
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
        Guesshere.Visibility = Visibility.Visible;
        tbguessedLetters.Text = "Guessed letters: ";
        DrawHangman(stage);
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
            if(guess == "CHEATSON")
            {
                tbWord.Visibility = Visibility.Visible;
                tbxGuessInput.Clear();
                return;
            }
            else if(guess == "CHEATSOFF")
            {
                tbWord.Visibility = Visibility.Hidden;
                tbxGuessInput.Clear();
                return;
            }
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
                        stage++;
                        tblives.Text = $"Lives remaining: {lives}";
                        tbDialogue.Text = dialogueMessage;
                        DrawHangman(stage);
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
                Guesshere.Visibility = Visibility.Hidden;
                streak++;
                StreakHandler(streak);
                Replay();
            }
            else if (lives == 0)
            {
                tbDialogue.Text = $"You lost. The word was {oldWord}.";
                tbxGuessInput.Visibility = Visibility.Hidden;
                Guesshere.Visibility = Visibility.Hidden;
                streak = 0;
                StreakHandler(streak);
                Replay();
            }
        }
    }
    private void Replay()
    {
        
        if (open == true)
        {
           
            btnReset.IsEnabled = true;
            
        }
        else
        {
            
            btnReset.IsEnabled = false;
            
            
        }
        open = !open;
    }
    private void btnReset_Click(object sender, RoutedEventArgs e)
    {
        InitializeGame();
    }
    private void btnExit_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
    private void DrawHangman(int stage) //gpt code for dynamic drawing
    {
        HangmanCanvas.Children.Clear();

        double canvasWidth = HangmanCanvas.ActualWidth;
        double canvasHeight = HangmanCanvas.ActualHeight;

        // Define dynamic positions based on canvas size
        double baseX = canvasWidth * 0.2;
        double baseY = canvasHeight * 0.9;
        double standHeight = canvasHeight * 0.6;
        double topWidth = canvasWidth * 0.3;

        // Draw the stand
        Line baseLine = new Line { X1 = baseX - 30, Y1 = baseY, X2 = baseX + 100, Y2 = baseY, Stroke = Brushes.Black, StrokeThickness = 5 };
        Line pole = new Line { X1 = baseX, Y1 = baseY - standHeight, X2 = baseX, Y2 = baseY, Stroke = Brushes.Black, StrokeThickness = 5 };
        Line top = new Line { X1 = baseX, Y1 = baseY - standHeight, X2 = baseX + topWidth, Y2 = baseY - standHeight, Stroke = Brushes.Black, StrokeThickness = 5 };
        Line rope = new Line { X1 = baseX + topWidth, Y1 = baseY - standHeight, X2 = baseX + topWidth, Y2 = baseY - standHeight + (canvasHeight * 0.1), Stroke = Brushes.Black, StrokeThickness = 3 };

        HangmanCanvas.Children.Add(baseLine);
        HangmanCanvas.Children.Add(pole);
        HangmanCanvas.Children.Add(top);
        
        if(stage > 0)
        {
            HangmanCanvas.Children.Add(rope);
        }
        if (stage > 1) // Head
        {
            double headSize = canvasHeight * 0.08;
            Ellipse head = new Ellipse { Width = headSize, Height = headSize, Stroke = Brushes.Black, StrokeThickness = 3 };
            Canvas.SetLeft(head, baseX + topWidth - (headSize / 2));
            Canvas.SetTop(head, baseY - standHeight + (canvasHeight * 0.1));
            HangmanCanvas.Children.Add(head);
        }

        if (stage > 2) // Body
        {
            Line body = new Line { X1 = baseX + topWidth, Y1 = baseY - standHeight + (canvasHeight * 0.18), X2 = baseX + topWidth, Y2 = baseY - standHeight + (canvasHeight * 0.35), Stroke = Brushes.Black, StrokeThickness = 3 };
            HangmanCanvas.Children.Add(body);
        }

        if (stage > 3) // Left Arm
        {
            Line leftArm = new Line { X1 = baseX + topWidth, Y1 = baseY - standHeight + (canvasHeight * 0.2), X2 = baseX + topWidth - (canvasWidth * 0.08), Y2 = baseY - standHeight + (canvasHeight * 0.28), Stroke = Brushes.Black, StrokeThickness = 3 };
            HangmanCanvas.Children.Add(leftArm);
        }

        if (stage > 4) // Right Arm
        {
            Line rightArm = new Line { X1 = baseX + topWidth, Y1 = baseY - standHeight + (canvasHeight * 0.2), X2 = baseX + topWidth + (canvasWidth * 0.08), Y2 = baseY - standHeight + (canvasHeight * 0.28), Stroke = Brushes.Black, StrokeThickness = 3 };
            HangmanCanvas.Children.Add(rightArm);
        }

        if (stage > 5) // Left Leg
        {
            Line leftLeg = new Line { X1 = baseX + topWidth, Y1 = baseY - standHeight + (canvasHeight * 0.35), X2 = baseX + topWidth - (canvasWidth * 0.06), Y2 = baseY - standHeight + (canvasHeight * 0.45), Stroke = Brushes.Black, StrokeThickness = 3 };
            HangmanCanvas.Children.Add(leftLeg);
        }

        if (stage > 6) // Right Leg
        {
            Line rightLeg = new Line { X1 = baseX + topWidth, Y1 = baseY - standHeight + (canvasHeight * 0.35), X2 = baseX + topWidth + (canvasWidth * 0.06), Y2 = baseY - standHeight + (canvasHeight * 0.45), Stroke = Brushes.Black, StrokeThickness = 3 };
            HangmanCanvas.Children.Add(rightLeg);
        }
    }

    private void CanvasSizeChanged(object sender, RoutedEventArgs e)
    {
        DrawHangman(stage);
    }

    private void tbxGuessInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!String.IsNullOrEmpty(tbxGuessInput.Text))
        {
            Guesshere.Text = "";
        }
        else
        {
            Guesshere.Text = "Guess here!";
        }
    }

    private void StreakHandler(int streak)
    {
        tbStreak.Text = $"Streak: {streak}";
        if (streak == 0)
        {
            tbStreak.Visibility = Visibility.Hidden;
        }
        else if (streak > 1)
        {
            tbStreak.Visibility = Visibility.Visible;
        }
    }
}
