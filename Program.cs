


namespace Sudoku
{
    class Program
    {
        static int RandomNumGamemode()
        {
            Random random = new Random();
            int rndNum = random.Next(1, 101);
            return rndNum;
        }

        static int RandomNum(int[] numbers)
        {          
            int rndNum; bool numFound = false;
            do
            {
                //Insures that the number given is always different:
                Random random = new Random();
                rndNum = random.Next(1, 10);
                for (int i = 0; i < numbers.Length; i++)
                {
                    if (numbers[i] == rndNum)
                    {
                        numbers[i] = 0;
                        numFound = true;
                    }
                }
            } while (!numFound);
            
            return rndNum;
        }

        static void GenerateField(int[][,] mainMatrix, int[][,] helperMatrix, int gamemode)
        {
            //Random numbers filter:
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int r = 0, c = 0; //Rows and Columns
            int rndNum, rndNumCounter = 0;
            for (int i = 0; i < 9; i++)
            {
                rndNum = RandomNum(numbers);
                //Rows rules:
                if (rndNumCounter == 3)
                {
                    r = 1;
                }
                else if (rndNumCounter == 6)
                { 
                    r = 2;
                }
                //Columns rules:
                if (rndNumCounter == 3 || rndNumCounter == 6)
                {
                    c = 0;
                }
                else if (rndNumCounter == 1 || rndNumCounter == 4 || rndNumCounter == 7)
                {
                    c = 1;
                }
                else if (rndNumCounter == 2 || rndNumCounter == 5 || rndNumCounter == 8)
                {
                    c = 2;
                }
                //Go trough boxes:
                for (int p = 0; p < mainMatrix.Length; p++)
                {
                    //Try to place num:
                    if (RandomNumGamemode() >= gamemode)
                    {
                    mainMatrix[p][r, c] = rndNum;
                    }
                    //Always place num in helperMatrix:
                    helperMatrix[p][r, c] = rndNum;
                    //Rows loop:
                    r++;
                    if (r == 3) //Failsafe
                    {
                        r = 0;
                    }
                    //Columns loop:
                    if ((p+1) % 3 == 0)
                    {
                        c++;
                        if (c == 3)
                        {
                            c = 0;
                        }
                    }               
                }
                rndNumCounter++;
            }
        }

        static bool PlaceNum(int[][,] mainMatrix, int[][,] helperMatrix,int blockNum ,int blcPosNum, int playerNum, ref int mistakes)
        {
            blockNum--; //Because computers start at 0, but humans start at 1.
            //Block position number exchange into matrix coordinates:
            int[,] legendMatrix = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            int r = 0, c = 0; // Rows and Columns
            bool shouldBreak = false;
            for (r = 0; r < 3; r++)
            {
                for (c = 0; c < 3; c++)
                {
                    if (legendMatrix[r,c] == blcPosNum)
                    {
                        shouldBreak = true;
                        break;                
                    }
                }
                if (shouldBreak)
                {
                    break;
                }         
            }
            if (mainMatrix[blockNum][r,c] == 0)
            {
                if (!GameRulesBroken(helperMatrix, blockNum, r, c, playerNum))
                {
                    //If game rules are NOT broken, place number:
                    mainMatrix[blockNum][r, c] = playerNum;
                    Console.Clear();
                }
                else
                {
                    Console.Clear();
                    mistakes--; Console.WriteLine("You made a mistake. :(");
                }
                    
                
                return false; //Spot not taken.
            }
            else
            {
                Console.Clear(); 
                Console.WriteLine("Spot already taken!");
                Console.WriteLine("Please choose a different spot.");
                Console.WriteLine($"Mistakes: {mistakes}\n");
                DisplayField(mainMatrix);
                return true; //Spot taken.
            }  
        }

        static int Gamemode(int gamemodeNum)
        {
            int gamemode = 0;
            Console.Clear();
            switch (gamemodeNum)
            {
                case 1:
                    gamemode = 25;  //Easy
                    Console.WriteLine("Easy mode selected!\n");
                    break;
                case 2:
                    gamemode = 50;  //Medium
                    Console.WriteLine("Medium mode selected!\n");
                    break;
                case 3:
                    gamemode = 75;  //Hard
                    Console.WriteLine("Hard mode selected!\n");
                    break;                    
            }

            return gamemode;
        }

        static bool GameRulesBroken(int[][,] helperMatrix,int blockNum, int r, int c, int playerNum)
        {
            int rows = 0, columns = 0;
            int blockNumHelper = blockNum;
            //Check rows (up and down):
            switch (blockNum)
            {
                case <= 2:
                    blockNum = blockNum + 3;
                    while (blockNum < 9)
                    {                     
                        while (rows < 3)
                        {
                            if (helperMatrix[blockNum][rows, c] == playerNum)
                            {
                                return true;
                            }
                            rows++;
                        }
                        rows = 0;
                        blockNum = blockNum + 3;
                    }
                    break;
                case <= 5:
                    blockNum = blockNum + 3;
                    do
                    {
                        
                        while (rows < 3)
                        {
                            if (helperMatrix[blockNum][rows, c] == playerNum)
                            {
                                return true;
                            }
                            rows++;
                        }
                        rows = 0;
                        blockNum = blockNum - 6;
                    } while (blockNum >= 0);
                    break;
                case <= 8:
                    blockNum = blockNum - 3;
                    while (blockNum > -1)
                    {
                        while (rows < 3)
                        {
                            if (helperMatrix[blockNum][rows, c] == playerNum)
                            {
                                return true;
                            }
                            rows++;
                        }
                        rows = 0;
                        blockNum = blockNum - 3;
                    }
                    break;
            }
            //Check columns (left and right):
            blockNum = blockNumHelper;
            blockNumHelper = 0;
            if (blockNum == 0 || blockNum == 3 || blockNum == 6)
            {
                blockNum = blockNum + 1;
                while (blockNumHelper < 2)
                {
                    while (columns < 3)
                    {
                        if (helperMatrix[blockNum][r, columns] == playerNum)
                        {
                            return true;
                        }
                        columns++;
                    }
                    columns = 0;
                    blockNum = blockNum + 1;
                    blockNumHelper++;
                }
            }
            else if (blockNum == 1 || blockNum == 4 || blockNum == 7)
            {
                blockNum = blockNum + 1;
                while (blockNumHelper < 2)
                {
                    while (columns < 3)
                    {
                        if (helperMatrix[blockNum][r, columns] == playerNum)
                        {
                            return true;
                        }
                        columns++;
                    }
                    columns = 0;
                    blockNum = blockNum - 2;
                    blockNumHelper++;
                }
            }
            else if (blockNum == 2 || blockNum == 5 || blockNum == 8)
            {
                blockNum = blockNum - 1;
                while (blockNumHelper < 2)
                {
                    while (columns < 3)
                    {
                        if (helperMatrix[blockNum][r, columns] == playerNum)
                        {
                            return true;
                        }
                        columns++;
                    }
                    columns = 0;
                    blockNum = blockNum - 1;
                    blockNumHelper++;
                }
            }
            //If no gamerules were brocken return false:
            return false;
        }

        static void DisplayField(int[][,] mainMatrix)
        {
            int r = 0; // Rows
            for (int i = 0; i < mainMatrix.Length; i++) //Go trough mainMatrix
            {   
                for (int c = 0; c < 3; c++) //Go trough columns
                {
                    Console.Write($"{mainMatrix[i][r,c]}");
                    if (c != 2)
                    {
                        Console.Write(" ");
                    }
                    if (c == 2 && i != 2 && i != 5 && i != 8)
                    {
                        Console.Write('|');
                    }
                }
                //New line at end of Blocksection:
                if ((i + 1) % 3 == 0)
                {
                    Console.WriteLine();
                    if ((i + 1) % 3 == 0 && r == 2 && i+1 != 9)
                    {
                        Console.Write("-----------------");
                        Console.WriteLine();
                    }
                }
                //Conditions for displaying 1 line from 3 blocks at a time:
                if (i == 2 && r != 2)
                {
                    i = -1; r++;
                }
                else if(i == 2 && r == 2)
                { 
                    i = 2; r = 0;
                }
                else if (i == 5 && r != 2)
                {
                    i = 2; r++;
                }
                else if (i == 5 && r == 2)
                {
                    i = 5; r = 0;
                }
                else if (i == 8 && r != 2)
                {
                    i = 5; r++;
                }
            }
        }

        static bool YouWon(int[][,]mainMatrix)
        {
            int emptCounter = 0;
            for (int i = 0; i < mainMatrix.Length; i++)
            {
                for (int r = 0; r < 3; r++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        if (mainMatrix[i][r,c] == 0)
                        {
                            emptCounter++;
                        }
                    }
                }
            }

            if (emptCounter == 0)
            {
                return true;
            }
            else
                return false;
        }

        static void Main(string[] args)
        {
            string input;
            do
            {
                Console.Clear();
                //INTRO:
                Console.WriteLine("WELCOME TO SUDOKU!");
                Console.WriteLine("------------------");
                Console.WriteLine("\nPlease choose a difficulty!");
                Console.WriteLine("\n1. Easy | 2. Medium | 3. Hard");
                //Field declaration:
                int[][,] mainMatrix = new int[][,]
                {
                  new int[3,3],
                  new int[3,3],
                  new int[3,3],
                  new int[3,3],
                  new int[3,3],
                  new int[3,3],
                  new int[3,3],
                  new int[3,3],
                  new int[3,3]
                };
                int[][,] helperMatrix = new int[][,]
                {
                  new int[3,3],
                  new int[3,3],
                  new int[3,3],
                  new int[3,3],
                  new int[3,3],
                  new int[3,3],
                  new int[3,3],
                  new int[3,3],
                  new int[3,3]
                };
                int gamemodeNum;
                bool parsed;
                do
                {
                    Console.Write("\n: ");
                    input = Console.ReadLine();
                    parsed = int.TryParse(input, out gamemodeNum);
                    if (gamemodeNum < 1 || gamemodeNum > 3)
                    {
                        parsed = false;
                    }
                    if (!parsed)
                    {
                        if (input == "easy" || input == "Easy" || input == "e")
                        {
                            gamemodeNum = 1;
                            parsed = true;
                        }
                        else if (input == "medium" || input == "Medium" || input == "m")
                        {
                            gamemodeNum = 2;
                            parsed = true;
                        }
                        else if (input == "hard" || input == "Hard" || input == "h")
                        {
                            gamemodeNum = 3;
                            parsed = true;
                        }
                    }
                    if (!parsed)
                    {
                        Console.WriteLine("Please enter either a number between 1 - 3, or just write the word itself.");
                    }
                } while (!parsed);

                GenerateField(mainMatrix, helperMatrix, Gamemode(gamemodeNum));
                DisplayField(mainMatrix);
                Console.WriteLine("\nPlease enter in which block, on what spot and what number you would like to place.");
                int blockNum, blcPosNum, playerNum, mistakes = 3;
                bool wrongNum = false;
                do
                {
                    do
                    {
                        do
                        {
                            if (wrongNum)
                            {
                                Console.Clear();
                                Console.WriteLine("Please choose numbers between 1 - 9 only!\n");
                                DisplayField(mainMatrix);
                            }
                            Console.Write("\nBlock: ");
                            blockNum = Convert.ToInt32(Console.ReadLine());
                            Console.Write("\nSpot: ");
                            blcPosNum = Convert.ToInt32(Console.ReadLine());
                            Console.Write("\nNumber: ");
                            playerNum = Convert.ToInt32(Console.ReadLine());
                            wrongNum = true;
                        } while (blockNum < 1 || blockNum > 9 || blcPosNum < 1 || blcPosNum > 9 || playerNum < 1 || playerNum > 9);
                        wrongNum = false;
                    } while (PlaceNum(mainMatrix, helperMatrix, blockNum, blcPosNum, playerNum, ref mistakes));
                    Console.WriteLine($"Mistakes left: {mistakes}");
                    DisplayField(mainMatrix);
                } while (!YouWon(mainMatrix) && mistakes > 0);

                if (YouWon(mainMatrix) && mistakes > 0)
                {
                    Console.WriteLine("\nCongradulations! You won!");
                }
                else
                    Console.WriteLine("\nSadly you mamde too many mistakes, you lost :(");

                Console.WriteLine("Play a new game? (y/n)");
                input = Console.ReadLine();
            } while (input == "y");

            Console.WriteLine("Goodbye! :)");
            Console.ReadKey();
        }
    }
}