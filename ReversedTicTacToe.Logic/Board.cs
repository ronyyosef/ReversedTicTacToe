using System;
using System.Collections.Generic;

namespace ReversedTicTacToe.Logic
{
    public class Board
    {
        public int Size { get; }

        public Cell this[int i_Row, int i_Col]
        {
            get
            {
                if (((i_Row >= 0) && (i_Row < Size)) && ((i_Col >= 0) && (i_Col < Size)))
                {
                    return r_Matrix[i_Row, i_Col];
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }

            set
            {
                if (((i_Row >= 0) && (i_Row < Size)) && ((i_Col >= 0) && (i_Col < Size)))
                {
                    r_Matrix[i_Row, i_Col] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public Cell this[Pos i_Pos]
        {
            get
            {
                return this[i_Pos.Row, i_Pos.Col];
            }

            set
            {
                this[i_Pos.Row, i_Pos.Col] = value;
            }
        }

        public char[,] ToCharMatrix()
        {
            char[,] result = new char[Size, Size];
            for (int i = 0; i <= r_Matrix.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= r_Matrix.GetUpperBound(1); j++)
                {
                    switch (r_Matrix[i, j].Value)
                    {
                        case GameLogic.eSign.X:
                            result[i, j] = 'X';
                            break;

                        case GameLogic.eSign.O:
                            result[i, j] = 'O';
                            break;

                        case GameLogic.eSign.Empty:
                            result[i, j] = ' ';
                            break;
                        default:
                            break;
                    }
                }
            }

            return result;
        }

        public Board(int i_Size)
        {
            Size = i_Size;
            r_Matrix = new Cell[i_Size, i_Size];
            for (int i = 0; i <= r_Matrix.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= r_Matrix.GetUpperBound(1); j++)
                {
                    r_Matrix[i, j] = new Cell(GameLogic.eSign.Empty);
                }
            }
        }

        public bool IsInBounds(Pos i_Pos)
        {
            return ((i_Pos.Row >= 0) && (i_Pos.Row < Size)) && ((i_Pos.Col >= 0) && (i_Pos.Col < Size));
        }

        public bool CheckForDiagonalSequenceSameSign()
        {
            Cell startCell = this[0, 0];
            int counter1 = 1;
            for (int i = 1; i < Size; i++)
            {
                if (this[i, i].Value == startCell.Value && !startCell.Available)
                {
                    counter1++;
                }
            }

            startCell = this[0, Size - 1];
            int counter2 = 1;
            for (int i = 1; i < Size; i++)
            {
                if (this[i, Size - 1 - i].Value == startCell.Value && !startCell.Available)
                {
                    counter2++;
                }
            }

            return counter1 == Size || counter2 == Size;
        }

        public bool CheckForVerticalSequenceSameSign()
        {
            bool isSequence = false;
            for (int j = 0; j < Size; j++)
            {
                Cell currentCell = this[0, j];
                int counter = 0;
                for (int i = 0; i < Size; i++)
                {
                    if (this[i, j].Value == currentCell.Value && !currentCell.Available)
                    {
                        counter++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (counter == Size)
                {
                    isSequence = true;
                    break;
                }
            }

            return isSequence;
        }

        public bool CheckForHorizontalSequenceSameSign()
        {
            bool isSequence = false;
            for (int j = 0; j < Size; j++)
            {
                Cell currentCell = this[j, 0];
                int counter = 0;
                for (int i = 0; i < Size; i++)
                {
                    if (this[j, i].Value == currentCell.Value && !currentCell.Available)
                    {
                        counter++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (counter == Size)
                {
                    isSequence = true;
                    break;
                }
            }

            return isSequence;
        }

        public bool IsFull()
        {
            int counter = 0;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (!this[i, j].Available)
                    {
                        counter++;
                    }
                }
            }

            return counter == Size * Size;
        }

        public void Reset()
        {
            for (int i = 0; i <= r_Matrix.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= r_Matrix.GetUpperBound(1); j++)
                {
                    r_Matrix[i, j].Value = GameLogic.eSign.Empty;
                }
            }
        }

        public List<Pos> AllEmptyCells()
        {
            List<Pos> emptyCellsList = new List<Pos>();
            for (int i = 0; i <= r_Matrix.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= r_Matrix.GetUpperBound(1); j++)
                {
                    if (r_Matrix[i, j].Available)
                    {
                        emptyCellsList.Add(new Pos(i, j));
                    }
                }
            }

            return emptyCellsList;
        }

        public int GetSequenceCounter(int i_SequenceSize, GameLogic.eSign i_Sign)
        {
            int counter = 0;
            for (int i = 0; i <= r_Matrix.GetUpperBound(0) + 1 - i_SequenceSize; i++)
            {
                for (int j = 0; j <= r_Matrix.GetUpperBound(1) + 1 - i_SequenceSize; j++)
                {
                    Cell currentCell = this[i, j];
                    if (currentCell.Value == GameLogic.eSign.Empty || currentCell.Value != i_Sign)
                    {
                        continue;
                    }

                    if (i_SequenceSize == 1)
                    {
                        if (currentCell.Value == i_Sign)
                        {
                            counter++;
                        }
                    }
                    else
                    {
                        int tempCounter = 1;
                        for (int k = 1; k < i_SequenceSize; k++)
                        {
                            if (this[i, j + k].Value == i_Sign)
                            {
                                tempCounter++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (tempCounter == i_SequenceSize)
                        {
                            counter++;
                        }

                        tempCounter = 1;
                        for (int k = 1; k < i_SequenceSize; k++)
                        {
                            if (this[i + k, j].Value == i_Sign)
                            {
                                tempCounter++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (tempCounter == i_SequenceSize)
                        {
                            counter++;
                        }

                        tempCounter = 1;
                        for (int k = 1; k < i_SequenceSize; k++)
                        {
                            if (this[i + k, j + k].Value == i_Sign)
                            {
                                tempCounter++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (tempCounter == i_SequenceSize)
                        {
                            counter++;
                        }
                    }
                }
            }

            return counter;
        }

        private readonly Cell[,] r_Matrix;
    }
}