using System;
using System.Collections.Generic;
using System.Linq;

namespace ReversedTicTacToe.Logic
{
    internal class Player
    {
        public int Score { get; set; }

        public bool IsComputer { get; }

        public GameLogic.eSign Sign { get; }

        public Player(bool i_IsComputer, GameLogic.eSign i_Sign)
        {
            IsComputer = i_IsComputer;
            Sign = i_Sign;
            Score = 0;
        }

        ////Default values because we want to call that function in a polymorphic way
        public void PlayTurn(Board i_Board, int i_Row = -1, int i_Col = -1)
        {
            if (IsComputer == true)
            {
                computerTurn(i_Board);
            }
            else
            {
                i_Board[i_Row, i_Col].Value = Sign;
            }
        }

        private void computerTurn(Board i_Board)
        {
            Pos bestMove = findBestMove(i_Board, Sign);
            i_Board[bestMove].Value = Sign;
        }

        private Pos findBestMove(Board i_Board, GameLogic.eSign i_PlayerSign)
        {
            List<Pos> possibleMoves = i_Board.AllEmptyCells();
            if (possibleMoves.Count == i_Board.Size * i_Board.Size)
            {
                return new Pos(GameLogic.sr_Rnd.Next(0, i_Board.Size - 1), GameLogic.sr_Rnd.Next(0, i_Board.Size - 1));
            }

            List<int> movesScore = new List<int>(possibleMoves.Count);

            foreach (Pos currentPosition in possibleMoves)
            {
                i_Board[currentPosition].Value = Sign;
                movesScore.Add(getPlayerMoveScore(i_Board));
                i_Board[currentPosition].Value = GameLogic.eSign.Empty;
            }

            int bestScore = movesScore.Min();
            int indexBestScore = movesScore.IndexOf(bestScore);
            Pos result = possibleMoves[indexBestScore];

            ///make sure next move don't make the AI lose
            while (moveMakePlayerLose(i_Board, result) == true)
            {
                if (possibleMoves.Count == 1)
                {
                    break;
                }

                possibleMoves.RemoveAt(indexBestScore);
                movesScore.RemoveAt(indexBestScore);
                bestScore = movesScore.Min();
                indexBestScore = movesScore.IndexOf(bestScore);
                result = possibleMoves[indexBestScore];
            }

            return result;
        }

        private bool moveMakePlayerLose(Board i_Board, Pos i_Result)
        {
            i_Board[i_Result].Value = Sign;
            bool makePlayerLose = i_Board.CheckForDiagonalSequenceSameSign() || i_Board.CheckForHorizontalSequenceSameSign()
                                                                           || i_Board.CheckForVerticalSequenceSameSign();
            i_Board[i_Result].Value = GameLogic.eSign.Empty;
            return makePlayerLose;
        }

        private int getPlayerMoveScore(Board i_Board)
        {
            // alone = 1^0
            // pair = 3^1
            // tree = 3^2
            // ..........
            int result = 0;
            for (int sequenceSize = 0; sequenceSize < i_Board.Size - 1; sequenceSize++)
            {
                int currentPow = sequenceSize;
                result += (int)Math.Pow(3, currentPow) * i_Board.GetSequenceCounter(sequenceSize + 1, Sign);
            }

            return result;
        }
    }
}