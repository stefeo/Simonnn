using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

/// <summary>
/// Stefan Andrekovic
/// Started February 13th 2017
/// Completed February 17th 2017
/// 
/// ICS4U @ SCSS
/// </summary>
/// 
namespace Simonnn
{
    class Program
    {
        #region  Declaring lists, vars, and randoms.
        public static List<char> simonPattern = new List<char>();
        public static List<char> playerPattern = new List<char>();

        public static int cycleTime = 1000;

        public static int turnNum = 0;
        public static int loopCounter = 0;

        public static bool playerCorrect = true;

        public static Random random = new Random();

        const int CHARACTER_INTERVAL_IN_MILLISECONDS = 30;
        const int PARAGRAPH_INTERVAL_IN_MILLISECONDS = 250;

        private static string[] PARAGRAPHS = {"Welcome to Simon", "Type 'play' to play the game, 'help' to learn how to play the game, or type 'exit' to quit"};
        #endregion

        static int Main(string[] args)
        {
            declare();
            //title screen text
            foreach (string paragraph in PARAGRAPHS)
            {
                Display(paragraph);
            }

            //waiting for further input
            string s = Console.ReadLine();

            if (s == "exit")
            {
                Environment.Exit(0);
            }
            else if (s == "play")
            {
                gamePlayer(turnNum, simonPattern, loopCounter);
            }
            else if (s == "help")
            {
                helpScreen();
            }

            Console.Clear();

            //re make menu screen
            Main(args);

            return 0;
        }

        public static void declare()
        {
            //declaring and resetting all values.
            Console.BackgroundColor = ConsoleColor.Black;

            PARAGRAPHS[0] = "Welcome to Simon";
            PARAGRAPHS[1] = "Type 'play' to play the game, 'help' to learn how to play the game, or type 'exit' to quit";

            simonPattern.Clear();
            playerPattern.Clear();
            turnNum = 0;
            loopCounter = 0;
            playerCorrect = true;
            cycleTime = 1000;
        }

        #region flowing text code, and text formatting
        public static void Display(string paragraph)
        {
            //flowing text from PARAGRAPHS
            EndOfLine();
            foreach (char ch in paragraph)
            {
                Thread.Sleep(CHARACTER_INTERVAL_IN_MILLISECONDS);
                DisplayChar(ch);
            }
            EndOfLine();
            Thread.Sleep(PARAGRAPH_INTERVAL_IN_MILLISECONDS);
            return;
        }

        private static void DisplayChar(char ch)
        {
            ConsumeInput();
            Console.Out.Write(ch);
            Console.Out.Flush();
            return;
        }

        private static void EndOfLine()
        {
            ConsumeInput();
            Console.Out.WriteLine();
            Console.Out.Flush();
            return;
        }

        private static void ConsumeInput()
        {
            const bool NO_ECHO = true;

            // consume any pending keystrokes w/o echoing the character
            while (Console.KeyAvailable)
            {
                Console.ReadKey(NO_ECHO);
            }

            return;
        }

        public static void formatText()
        {
            //formatting for text showing up on player turn
            Console.Clear();
            for (int j = 0; j < 13; j++)
            {
                EndOfLine();
            }
            Console.Write("                                                                             ");
        }

        public static void sFormatText()
        {
            //formatting for text showing on simon turn
            Console.Clear();
            for (int j = 0; j < 13; j++)
            {
                EndOfLine();
            }
            Console.Write("                                            ");
        }
        #endregion

        #region game loops, and turns code
        public static void gamePlayer(int turnNum, List<char> simonPattern, int loopCounter)
        {
            //infinite game loop until you lose.
            while (loopCounter >= 0)
            {
                if (playerCorrect == true)
                {
                    simonTurn(simonPattern);

                    playerTurn(turnNum, simonPattern, playerPattern);
                    
                    //faster every round
                    cycleTime -= 75;
                }

                else
                {
                    gameOver();
                    return;
                }
            }
            return;
        }

        public static void simonTurn(List<char> simonPattern)
        {
            //getting random letter, adding to list, and showing list procedurally with random colours.
            simonPattern.Add(GetLetter());
            for (int i = 0; i < simonPattern.Count(); i++)
            {
                Console.ForegroundColor = GetRandomConsoleColor();
                sFormatText();
                Console.Write(simonPattern[i]);
                Console.Beep();
                Thread.Sleep(cycleTime);
                Console.Clear();
                Thread.Sleep(cycleTime / 4);
                turnNum++;
            }
            Console.ResetColor();
        }

        public static void playerTurn(int turnNum, List<char> simonPattern, List<char> playerPattern)
        {
            //fetching player letters, checking with simon pattern, and adding to player pattern
            playerPattern.Clear();
            for (int i = 0; i < simonPattern.Count(); i++)
            {
                playerPattern.Add(Console.ReadKey().KeyChar);
                Console.Beep();
                Console.Clear();
                formatText();
                Console.Write(playerPattern[i]);
                Thread.Sleep(cycleTime);
                Console.Clear();
                Thread.Sleep(cycleTime / 4);
                if (playerPattern[i] != simonPattern[i])
                {
                    playerCorrect = false;
                    return;
                }
            }

            return;
        }
        #endregion

        #region screens


        public static void gameOver()
        {
            //showing game over screen with score until player presses space

            Console.BackgroundColor = ConsoleColor.Red;

            for (int i = 0; i >= 0; i++)
            {
                Console.Clear();
                sFormatText();
                Console.Write("            ");
                Console.Write("GAME OVER");
                for (int j = 0; j < 4; j++)
                {
                    EndOfLine();
                }

                Console.Write("                                                 ");
                Console.Write("PRESS SPACE TO CONTINUE");
                for (int j = 0; j < 4; j++)
                {
                    EndOfLine();
                }
                Console.Write("                                                   ");
                Console.Write("YOU REACHED LEVEL " + simonPattern.Count());
                Console.Beep();
                char c = Console.ReadKey().KeyChar;
                if (c == ' ')
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                    break;
                }
            }
        }

        public static void helpScreen()
        {
            //changing flowing text to help text, and then flowing it onto page in green.

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            PARAGRAPHS[0] = "HOW TO PLAY SIMON:";
            PARAGRAPHS[1] = "Simon will press a letter every turn, then it is your job to copy the letter he has pressed. Each consecutive turn you  correctly copy Simon, he will add another letter to the pattern. See how long you can keep up with Simon!";
            foreach (string paragraph in PARAGRAPHS)
            {
                Display(paragraph);
            }
            Console.ResetColor();
            Console.ReadKey();
        }
        #endregion

        #region random gens
        public static ConsoleColor GetRandomConsoleColor()
        {
            //getting random colour, excluding 0 - 7 (too dark), using the colour Enum to convert to console colour.

            var consoleColors = Enum.GetValues(typeof(ConsoleColor));
            return (ConsoleColor)consoleColors.GetValue(random.Next(7, 15));
        }

        public static char GetLetter()
        {
            //fetching random letter for simon's pattern

            int num = random.Next(0, 26);
            char let = (char)('a' + num);
            return let;
        }
        #endregion
    }
}
