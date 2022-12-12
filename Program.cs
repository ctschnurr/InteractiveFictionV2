using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interactive_Fiction
{
    internal class Program
    {
        // here we initialize the main components we need for the story engine
        static string[] story;

        static string[] currentPageContents = {"blank"};
        static int currentPageNumber = 1;
        static char[] separators = new char[] {';'};

        static System.Media.SoundPlayer click = new System.Media.SoundPlayer(@"click.wav");

        static bool gameOver = false;
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);

            if (System.IO.File.Exists("story.txt"))
            {
                story = System.IO.File.ReadAllLines(@"story.txt");

                string temp = story[0];
                currentPageContents = temp.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                SplashPage();

                Console.Clear();

                PrintFrame();
                Console.WriteLine();
                Console.WriteLine(" You may press Escape at any time during the story to reach the Main Menu.");
                Console.WriteLine();
                Console.WriteLine(" Please press any key to continue.");
                Console.WriteLine();
                PrintFrame();

                Console.ReadKey(true);
                click.Play();


                // game loop:

                while (gameOver == false)
                {
                    temp = story[currentPageNumber];
                    currentPageContents = temp.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    // if a page has no choices included it is considered an end page and is handled here:

                    if (currentPageContents.Length == 1)
                    {
                        Console.Clear();

                        PrintFrame();

                        PrintPlot(currentPageContents[0]);

                        Console.WriteLine();
                        Console.WriteLine(" You've completed the story!");
                        Console.WriteLine();
                        Console.WriteLine(" (R)estart");
                        Console.WriteLine(" (Q)uit");

                        PrintFrame();

                        ConsoleKeyInfo end = Console.ReadKey(true);
                        click.Play();

                        if (end.Key == ConsoleKey.R)
                        {
                            currentPageNumber = 1;
                        }

                        if (end.Key == ConsoleKey.Q)
                        {
                            gameOver = true;
                        }

                        if (end.Key == ConsoleKey.Escape)
                        {
                            MainMenu();
                        }
                    }

                    // if a page includes two choices and two destinations (a standard page) it is handled here:

                    if (currentPageContents.Length == 5)
                    {
                        Console.Clear();

                        PrintFrame();

                        PrintPlot(currentPageContents[0]);

                        Console.WriteLine();
                        Console.WriteLine(" Make Your Choice:");
                        Console.WriteLine();
                        Console.Write(" (A): ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(currentPageContents[1]);
                        Console.ResetColor();
                        Console.Write(" (B): ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(currentPageContents[2]);
                        Console.ResetColor();

                        PrintFrame();

                        ConsoleKeyInfo choice = Console.ReadKey(true);
                        click.Play();

                        int check;

                        if (choice.Key == ConsoleKey.A)
                        {
                            check = ErrorCheck(currentPageContents[3]);

                            if (check != 0)
                            {
                                currentPageNumber = check;
                            }
                        }

                        if (choice.Key == ConsoleKey.B)
                        {
                            check = ErrorCheck(currentPageContents[4]);

                            if (check != 0)
                            {
                                currentPageNumber = check;
                            }
                        }

                        if (choice.Key == ConsoleKey.Escape)
                        {
                            MainMenu();
                        }
                    }

                    // if a page does not fall into the above cases it is handled here:

                    if (currentPageContents.Length != 1 && currentPageContents.Length != 5)
                    {
                        Console.Clear();

                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" Error: the page we are trying to access is not formatted to fit our engine and can not be displayed. Please contact the story author.");
                        Console.ResetColor();

                        Console.WriteLine(" A: Restart");
                        Console.WriteLine(" B: Quit");
                        Console.WriteLine();

                        ConsoleKeyInfo choice = Console.ReadKey(true);
                        click.Play();

                        if (choice.Key == ConsoleKey.A)
                        {
                            currentPageNumber = 1;
                        }

                        if (choice.Key == ConsoleKey.B)
                        {
                            int nextPage = Int32.Parse(currentPageContents[4]);
                            currentPageNumber = nextPage;
                        }
                    }
                }

                // After the player quits, this is the goodbye message:

                temp = story[0];
                currentPageContents = temp.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                Console.Clear();

                PrintFrame();

                Console.Write(" Thank you for enjoying ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(currentPageContents[0]);
                Console.ResetColor();
                Console.Write(" by ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(currentPageContents[1]);
                Console.ResetColor();
                Console.WriteLine("!");
                Console.WriteLine();
                Console.WriteLine(" Brought to you by the Interactive Fiction Engine, by Christopher Schnurr.");
                Console.WriteLine();
                Console.WriteLine(" Please press any key to end.");

                PrintFrame();

                Console.ReadKey(true);
                click.Play();
            }

            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Error: a 'story.txt' file was not found in the same folder as 'InteractiveFiction.exe'. A story.txt is required.");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine(" Press any key to quit.");
                Console.ReadKey(true);
                click.Play();

                gameOver = true;
            }
        }

        // PrintPlot takes the plot element from the current page and breaks it into lines of around 118 characters, then displays the line:
        static void PrintPlot(string plot)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            if (plot.Length < 118)
            {
                Console.WriteLine(" " + plot);
            }

            if (plot.Length > 118)
            {                
                string newplot = " ";
                string lengthcheck = null;

                string[] plotwords = plot.Split(' ');

                for (int i = 0; i < plotwords.Length; i++)
                {
                    newplot = newplot + plotwords[i];
                    newplot = newplot + " ";

                    if (i + 1 < plotwords.Length)
                    {
                        lengthcheck = newplot + plotwords[i + 1];
                    }
                                            
                    if (lengthcheck.Length > 118)
                    {
                        Console.WriteLine(newplot);
                        newplot = " ";
                        lengthcheck = " ";
                    }
                }

                if (lengthcheck != null)
                {
                    Console.WriteLine(newplot);
                }                
            }

            Console.ResetColor();
        }

        // PrintFrame just draws the frame element
        static void PrintFrame()
        {
            Console.WriteLine();
            Console.WriteLine("╔╗═════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("╚╝═════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

        }

        // ErrorCheck makes sure the destination page numbers in the story are valid. If not, displays an error message to prevent crashing.

        static int ErrorCheck(string pageNum)
        {

            int nextPage;
            bool isNumber = int.TryParse(pageNum, out nextPage);

            if (isNumber == true)
            {
                return nextPage;
            }

            else if (isNumber == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Error: the destination page number is invalid or improperly formatted. Please contact the story author.");
                Console.ResetColor();

                Console.ReadKey(true);
                click.Play();

                return 0;
            }

            return 0;
        }
        static void SplashPage()
        {
            bool startStory = false;

            while (startStory == false)
            {
                Console.Clear();

                PrintFrame();

                Console.Write(" Welcome to the Interactive Fiction Engine v 2.0!");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" (Now with bookmarks and file reading!)");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine(" The interactive story you will be enjoying is titled:");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" " + currentPageContents[0]);
                Console.ResetColor();
                Console.Write(" by ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(currentPageContents[1]);
                Console.ResetColor();
                Console.WriteLine(".");
                Console.WriteLine();
                Console.WriteLine(" (N)ew Game");
                if (System.IO.File.Exists("savegame.txt"))
                {
                    string savePage = System.IO.File.ReadAllText(@"savegame.txt");
                    Console.WriteLine(" (L)oad Bookmark on Page " + savePage);
                }
                Console.WriteLine(" (Q)uit");
                Console.WriteLine();

                PrintFrame();

                ConsoleKeyInfo splash = Console.ReadKey(true);
                click.Play();

                if (splash.Key == ConsoleKey.N)
                {
                    currentPageNumber = 1;
                    startStory = true;
                }

                if (splash.Key == ConsoleKey.L)
                {
                    if (System.IO.File.Exists("savegame.txt"))
                    {
                        string savePage = System.IO.File.ReadAllText(@"savegame.txt");
                        currentPageNumber = int.Parse(savePage);
                        startStory = true;

                        Console.WriteLine("Bookmark Loaded! Press any key to continue.");

                        Console.ReadKey(true);
                        click.Play();
                    }
                }

                if (splash.Key == ConsoleKey.Q)
                {
                    bool splashQuit = false;

                    while (splashQuit == false)
                    {
                        Console.WriteLine("Really Quit? (Y/N)");
                        ConsoleKeyInfo quit = Console.ReadKey(true);
                        click.Play();

                        if(quit.Key == ConsoleKey.Y)
                        {
                            splashQuit = true;
                            gameOver = true;
                            startStory = true;
                        }

                        if(quit.Key == ConsoleKey.N)
                        {
                            splashQuit = true;
                        }
                    }
                }
            }
        }

        static void MainMenu()
        {
            bool startStory = false;
            string savePage = "0";

            while (startStory == false)
            {
                Console.Clear();

                PrintFrame();

                Console.WriteLine(" Main Menu");
                Console.WriteLine();
                Console.WriteLine(" Please choose from the following options:");
                Console.WriteLine();
                Console.WriteLine(" (N)ew Game");
                Console.WriteLine(" (B)ookmark Current Page");
                if (System.IO.File.Exists("savegame.txt"))
                {
                    savePage = System.IO.File.ReadAllText(@"savegame.txt");
                    Console.WriteLine(" (L)oad Bookmark on Page " + savePage);
                }
                Console.WriteLine(" (R)eturn");
                Console.WriteLine(" (Q)uit");

                PrintFrame();

                ConsoleKeyInfo menuChoice = Console.ReadKey(true);
                click.Play();

                if (menuChoice.Key == ConsoleKey.N)
                {
                    bool newGame = false;

                    while (newGame == false)
                    {
                        Console.WriteLine(" Start over from page 1? (Y/N)");
                        ConsoleKeyInfo restart = Console.ReadKey(true);
                        click.Play();

                        if (restart.Key == ConsoleKey.Y)
                        {
                            newGame = true;
                            startStory = true;
                            currentPageNumber = 1;
                        }

                        if (restart.Key == ConsoleKey.N)
                        {
                            newGame = true;
                        }
                    }
                }

                if (menuChoice.Key == ConsoleKey.B)
                {
                    bool overwrite = false;

                    while (overwrite == false)
                    {
                        if (System.IO.File.Exists("savegame.txt"))
                        {
                            string savedPage = System.IO.File.ReadAllText(@"savegame.txt");
                            Console.WriteLine(" Overwrite current bookmark on page " + savedPage + "? (Y/N)");
                            ConsoleKeyInfo save = Console.ReadKey(true);
                            click.Play();

                            if (save.Key == ConsoleKey.Y)
                            {
                                savePage = currentPageNumber.ToString();
                                System.IO.File.WriteAllText(@"savegame.txt", savePage);
                                Console.WriteLine(" Page " + currentPageNumber + " Bookmarked! Press any key to continue.");

                                Console.ReadKey(true);
                                click.Play();
                                overwrite = true;
                            }

                            if (save.Key == ConsoleKey.N)
                            {
                                overwrite = true;
                            }
                        }

                        else
                        {
                            savePage = currentPageNumber.ToString();
                            System.IO.File.Create(@"savegame.txt").Close();
                            System.IO.File.WriteAllText(@"savegame.txt", savePage);

                            Console.WriteLine(" Page " + currentPageNumber + " Bookmarked! Press any key to continue.");

                            Console.ReadKey(true);
                            click.Play();
                            overwrite = true;
                        }
                    }
                }

                if (menuChoice.Key == ConsoleKey.L)
                {
                    if (System.IO.File.Exists("savegame.txt"))
                    {
                        bool loadGame = false;

                        while (loadGame == false)
                        {
                            Console.WriteLine(" Load Bookmark? (Y/N)");
                            ConsoleKeyInfo restart = Console.ReadKey(true);
                            click.Play();

                            if (restart.Key == ConsoleKey.Y)
                            {
                                loadGame = true;
                                string savesPage = System.IO.File.ReadAllText(@"savegame.txt");
                                currentPageNumber = int.Parse(savesPage);
                                startStory = true;

                                Console.WriteLine(" Bookmark Loaded! Press any key to continue.");

                                Console.ReadKey(true);
                                click.Play();
                            }

                            if (restart.Key == ConsoleKey.N)
                            {
                                loadGame = true;
                            }
                        }
                    }
                }

                if (menuChoice.Key == ConsoleKey.R)
                {
                    startStory = true;
                }

                if (menuChoice.Key == ConsoleKey.Q)
                {
                    bool splashQuit = false;

                    while (splashQuit == false)
                    {
                        Console.WriteLine(" Really Quit? (Y/N)");
                        ConsoleKeyInfo quit = Console.ReadKey(true);
                        click.Play();

                        if (quit.Key == ConsoleKey.Y)
                        {
                            splashQuit = true;
                            gameOver = true;
                            startStory = true;
                        }

                        if (quit.Key == ConsoleKey.N)
                        {
                            splashQuit = true;
                        }
                    }
                }
            }
        }

    }
}

