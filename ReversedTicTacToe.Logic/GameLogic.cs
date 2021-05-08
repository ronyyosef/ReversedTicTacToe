using System;

namespace ReversedTicTacToe.Logic
{
    public class GameLogic
    {
        public enum eSign
        {
            X = 'X',
            O = 'O',
            Empty = ' ',
        }

        private enum eGameMode
        {
            ComputerMode = 1,
            TwoPlayerMode = 2,
        }

        public static readonly Random sr_Rnd = new Random();

        public bool GameOver { get; private set; }

        public bool GameIsTie { get; private set; }

        public char[,] Board
        {
            get
            {
                return r_Board.ToCharMatrix();
            }
        }

        public char CurrentPlayerSign
        {
            get
            {
                return (char)m_CurrentPlayerTurn.Sign;
            }
        }

        public bool CurrentPlayerIsComputer
        {
            get
            {
                return m_CurrentPlayerTurn.IsComputer;
            }
        }

        public char Winner
        {
            get
            {
                char returnValue;
                if (GameIsTie == true || GameOver == false)
                {
                    returnValue = (char)eSign.Empty;
                }
                else
                {
                    switchTurn();
                    returnValue = CurrentPlayerSign;
                    switchTurn();
                }

                return returnValue;
            }
        }

        public char PlayerOneSign
        {
            get
            {
                return (char)r_Player1.Sign;
            }
        }

        public int PlayerOneScore
        {
            get
            {
                return r_Player1.Score;
            }
        }

        public char PlayerTwoSign
        {
            get
            {
                return (char)r_Player2.Sign;
            }
        }

        public int PlayerTwoScore
        {
            get
            {
                return r_Player2.Score;
            }
        }

        public GameLogic(int size, int gameMode)
        {
            const bool k_IsComputerPlayer = true;
            r_GameMode = gameMode == 1 ? eGameMode.ComputerMode : eGameMode.TwoPlayerMode;
            r_Player1 = r_GameMode == eGameMode.ComputerMode ? new Player(k_IsComputerPlayer, eSign.X) : new Player(!k_IsComputerPlayer, eSign.X);
            r_Player2 = new Player(!k_IsComputerPlayer, eSign.O);
            r_Board = new Board(size);
            randWhoStart();
            GameOver = false;
            GameIsTie = false;
        }

        public void RestartGame()
        {
            randWhoStart();
            GameOver = false;
            GameIsTie = false;
            r_Board.Reset();
        }

        public void HumanPlayTurn(int row, int col)
        {
            m_CurrentPlayerTurn.PlayTurn(r_Board, row, col);
            updateStatus();
        }

        public bool PlayerPositionSelectionInBounds(int row, int col)
        {
            return r_Board.IsInBounds(new Pos(row, col));
        }

        public bool PlayerPositionSelectionTaken(int i_Row, int i_Col)
        {
            return !r_Board[i_Row, i_Col].Available;
        }

        public void OpponentGetAPoint()
        {
            switchTurn();
            m_CurrentPlayerTurn.Score++;
            switchTurn();
        }

        public void ComputerPlayTurn()
        {
            m_CurrentPlayerTurn.PlayTurn(r_Board);
            updateStatus();
        }

        private void randWhoStart()
        {
            m_CurrentPlayerTurn = sr_Rnd.Next(0, 1) == 0 ? r_Player1 : r_Player2;
        }

        private void switchTurn()
        {
            m_CurrentPlayerTurn = m_CurrentPlayerTurn == r_Player2 ? r_Player1 : r_Player2;
        }

        private void checkForWin()
        {
            GameOver = r_Board.CheckForDiagonalSequenceSameSign() ||
                        r_Board.CheckForVerticalSequenceSameSign() ||
                        r_Board.CheckForHorizontalSequenceSameSign();
            if (GameOver == true)
            {
                switchTurn();
                m_CurrentPlayerTurn.Score++;
                switchTurn();
            }
        }

        ////assuming there wasn't a win
        private void checkForTie()
        {
            GameIsTie = r_Board.IsFull() && GameOver == false;
        }

        private void updateStatus()
        {
            checkForWin();
            checkForTie();
            if (GameOver == false && GameIsTie == false)
            {
                switchTurn();
            }
        }

        private readonly Player r_Player2;
        private readonly Player r_Player1;
        private readonly Board r_Board;
        private readonly eGameMode r_GameMode;
        private Player m_CurrentPlayerTurn;
    }
}