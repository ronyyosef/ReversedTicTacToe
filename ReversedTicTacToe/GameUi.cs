using System;
using System.Text;

namespace ReversedTicTacToe
{
    public class GameUi
    {
        private static void quitOptionMessage()
        {
            Console.WriteLine("Enter Q to quit");
        }

        private static void getUserGameModeInput(out int i_UserGameModeInput)
        {
            i_UserGameModeInput = 0;
            Console.WriteLine("please select the game mode of the game(1 for computer, 2 for two players)");
            bool gameModeInputValid = false;

            while (gameModeInputValid == false)
            {
                string userInput = Console.ReadLine();
                if (int.TryParse(userInput, out i_UserGameModeInput) == false)
                {
                    Console.WriteLine("The input is not in the correct format");
                    continue;
                }

                bool correctInput = i_UserGameModeInput == 1 || i_UserGameModeInput == 2;
                if (correctInput == false)
                {
                    Console.WriteLine("The number need to be in 1 or 2");
                    continue;
                }

                gameModeInputValid = true;
            }
        }

        private static void getUserSizeInput(out int i_UserSizeInput)
        {
            i_UserSizeInput = 0;
            Console.WriteLine("please enter the size of the matrix(3-9)");
            bool sizeInputValid = false;
            while (sizeInputValid == false)
            {
                string userInput = Console.ReadLine();
                if (int.TryParse(userInput, out i_UserSizeInput) == false)
                {
                    Console.WriteLine("The input is not in the correct format");
                    continue;
                }

                bool sizeIsInRange = i_UserSizeInput >= 3 && i_UserSizeInput <= 9;
                if (sizeIsInRange == false)
                {
                    Console.WriteLine("The number need to be in the range 3-9");
                    continue;
                }

                sizeInputValid = true;
            }
        }

        public GameUi()
        {
            GetUserInitInput(out int userSizeInput, out int userGameModeInput);
            r_GameLogic = new Logic.GameLogic(userSizeInput, userGameModeInput);
            m_ToQuit = false;
            m_PlayAnotherGame = true;
        }

        public void Run()
        {
            while (m_PlayAnotherGame == true)
            {
                while (r_GameLogic.GameOver == false && r_GameLogic.GameIsTie == false && m_ToQuit == false)
                {
                    if(r_GameLogic.CurrentPlayerIsComputer == false)
                    {
                        DrawBoard(r_GameLogic.Board);
                        quitOptionMessage();
                    }

                    PlayTurn();
                }

                DrawBoard(r_GameLogic.Board);
                ShowGameScoreView();
                m_PlayAnotherGame = PlayAnotherGame();
                if (m_PlayAnotherGame == true)
                {
                    r_GameLogic.RestartGame();
                }

                restart();
            }
        }

        public void PlayTurn()
        {
            if (r_GameLogic.CurrentPlayerIsComputer == true)
            {
                r_GameLogic.ComputerPlayTurn();
            }
            else
            {
                bool playerPositionValid = false;
                int row = 0, col = 0;
                PleaseSelectPosition(r_GameLogic.CurrentPlayerSign);
                while (playerPositionValid == false)
                {
                    GetPlayerPositionSelection(out row, out col);
                    if (m_ToQuit == true)
                    {
                        return;
                    }

                    if (r_GameLogic.PlayerPositionSelectionInBounds(row, col) == true)
                    {
                        if (r_GameLogic.PlayerPositionSelectionTaken(row, col) == false)
                        {
                            playerPositionValid = true;
                        }
                        else
                        {
                            PositionTaken(row, col);
                        }
                    }
                    else
                    {
                        PositionOutOfBounds(row, col);
                    }
                }

                r_GameLogic.HumanPlayTurn(row, col);
            }
        }

        public void GetUserInitInput(out int i_UserSizeInput, out int i_UserGameModeInput)
        {
            i_UserSizeInput = 0;
            i_UserGameModeInput = 0;
            getUserSizeInput(out i_UserSizeInput);
            getUserGameModeInput(out i_UserGameModeInput);
        }

        public void DrawBoard(char[,] i_Board)
        {
            Ex02.ConsoleUtils.Screen.Clear();
            StringBuilder boardToPrint = new StringBuilder();
            boardToPrint.Append("   ");
            for (int i = 0; i < i_Board.GetLength(0); i++)
            {
                boardToPrint.Append($"{i + 1}   ");
            }

            boardToPrint.AppendLine();
            for (int i = 0; i < i_Board.GetLength(0); i++)
            {
                boardToPrint.Append($"{i + 1}|");
                for (int j = 0; j < i_Board.GetLength(0); j++)
                {
                    boardToPrint.Append($" {i_Board[i, j]} |");
                }

                boardToPrint.AppendLine();
                boardToPrint.Append(" ");
                for (int k = 0; k < i_Board.GetLength(0); k++)
                {
                    boardToPrint.Append("====");
                }

                boardToPrint.Append("=");
                boardToPrint.AppendLine();
            }

            Console.Write(boardToPrint);
        }

        public void ShowWinner(char i_CurrentWinner)
        {
            Console.WriteLine($"The current Winner is '{i_CurrentWinner}' !!!.");
        }

        public void ShowScore(char i_Player1Sign, int i_Player1Score, char i_Player2Sign, int i_Player2Score)
        {
            ////show the 1st player in case he won
            if (i_Player1Score > i_Player2Score)
            {
                Console.WriteLine($"The player '{i_Player1Sign}' got {i_Player1Score} points");
                Console.WriteLine($"The player '{i_Player2Sign}' got {i_Player2Score} points");
            }
            else
            {
                Console.WriteLine($"The player '{i_Player2Sign}' got {i_Player2Score} points");
                Console.WriteLine($"The player '{i_Player1Sign}' got {i_Player1Score} points");
            }
        }

        public bool PlayAnotherGame()
        {
            Console.WriteLine("Do you want to play another game? (yes/no)");
            string answer = Console.ReadLine();
            while (answer != "yes" && answer != "no")
            {
                Console.WriteLine("please answer 'yes' or 'no'");
                answer = Console.ReadLine();
            }

            return answer == "yes";
        }

        public void PositionTaken(int i_Row, int i_Col)
        {
            Console.WriteLine($"The position {i_Row + 1} {i_Col + 1} is already taken");
        }

        public void PositionOutOfBounds(int i_Row, int i_Col)
        {
            Console.WriteLine($"The position ({i_Row + 1},{i_Col + 1}) is out of borders");
        }

        public void PleaseSelectPosition(char i_Sign)
        {
            Console.WriteLine($"player {i_Sign}, please select your next move(row,col)");
        }

        public void GetPlayerPositionSelection(out int i_Row, out int i_Col)
        {
            i_Row = 0;
            i_Col = 0;
            bool validInput = false;

            while (!validInput)
            {
                string playerInput = Console.ReadLine();
                if (isQuit(playerInput))
                {
                    r_GameLogic.OpponentGetAPoint();
                    m_ToQuit = true;
                    break;
                }

                if (playerInput != null)
                {
                    string[] playerPositionSelection = playerInput.Split(' ');
                    if (playerPositionSelection.Length != 2)
                    {
                        Console.WriteLine("input isn't in the correct format");
                        continue;
                    }

                    if (!int.TryParse(playerPositionSelection[0], out i_Row))
                    {
                        Console.WriteLine("first number not in the right format");
                        continue;
                    }

                    if (!int.TryParse(playerPositionSelection[1], out i_Col))
                    {
                        Console.WriteLine("second number not in the right format");
                        continue;
                    }
                }

                validInput = true;
            }

            i_Row--;
            i_Col--;
        }

        public void ShowGameScoreView()
        {
            if (r_GameLogic.GameOver == true)
            {
                ShowWinner(r_GameLogic.Winner);
                ShowScore(r_GameLogic.PlayerOneSign, r_GameLogic.PlayerOneScore, r_GameLogic.PlayerTwoSign, r_GameLogic.PlayerTwoScore);
            }
            else if (r_GameLogic.GameIsTie == true || m_ToQuit == true)
            {
                ShowScore(r_GameLogic.PlayerOneSign, r_GameLogic.PlayerOneScore, r_GameLogic.PlayerTwoSign, r_GameLogic.PlayerTwoScore);
            }
        }

        private static bool isQuit(string i_Text)
        {
            return i_Text == "Q" || i_Text == "q";
        }
        
        private void restart()
        {
            m_ToQuit = false;
        }

        private readonly Logic.GameLogic r_GameLogic;
        private bool m_ToQuit;
        private bool m_PlayAnotherGame;
    }
}