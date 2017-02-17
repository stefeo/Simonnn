using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Simonnn
{
    class Program
    {
        public static List<char> simonPattern = new List<char>();
        public static List<char> playerPattern = new List<char>();

        public static int cycleTime = 1000;

        public static int turnNum = 0;
        public static int loopCounter = 0;

        public static bool playerCorrect = true;

        public static Random random = new Random();

        const int CHARACTER_INTERVAL_IN_MILLISECONDS = 30;
        const int PARAGRAPH_INTERVAL_IN_MILLISECONDS = 250;

        private static readonly string[] PARAGRAPHS = {"Welcome to Simon", "Type 'play' to play the game, or type 'exit' to quit"};

        static int Main(string[] args)
        {
            declare();
            foreach (string paragraph in PARAGRAPHS)
            {
                Display(paragraph);
            }

            string s = Console.ReadLine();

            if (s == "exit")
            {
                Environment.Exit(0);
            }
            else if (s == "play")
            {
                gamePlayer(turnNum, simonPattern, loopCounter);
            }
            else
            {

            }
            //Console.ReadLine();
            Console.Clear();
            Main(args);

            return 0;
        }

        public static void declare()
        {
            Console.BackgroundColor = ConsoleColor.Black;

            simonPattern.Clear();
            playerPattern.Clear();
            turnNum = 0;
            loopCounter = 0;
            playerCorrect = true;
        }

        public static void Display(string paragraph)
        {
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

        public static void gamePlayer(int turnNum, List<char> simonPattern, int loopCounter)
        {
            while (loopCounter >= 0)
            {
                if (playerCorrect == true)
                {
                    simonTurn(simonPattern);

                    playerTurn(turnNum, simonPattern, playerPattern);

                    cycleTime -= 50;
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
            Console.ForegroundColor = ConsoleColor.Red;
            simonPattern.Add(GetLetter());
            for (int i = 0; i < simonPattern.Count(); i++)
            {
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

        public static char GetLetter()
        {
            int num = random.Next(0, 26);
            char let = (char)('a' + num);
            return let;
        }

        public static void formatText()
        {
            Console.Clear();
            for (int j = 0; j < 13; j++)
            {
                EndOfLine();
            }
            Console.Write("                                                                             ");
        }

        public static void sFormatText()
        {
            Console.Clear();
            for (int j = 0; j < 13; j++)
            {
                EndOfLine();
            }
            Console.Write("                                            ");
        }

        public static void gameOver()
        {
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
                Console.Write("YOU REACHED LEVEL " + simonPattern.Count() );
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
    }
}
