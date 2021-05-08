﻿namespace ReversedTicTacToe.Logic
{
    public struct Pos
    {
        public int Row { get; set; }

        public int Col { get; set; }

        public Pos(int i_Row, int i_Col)
        {
            Row = i_Row;
            Col = i_Col;
        }
    }
}