using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Tetris
{
    public class Gamegrid
    {
        private readonly int[,] grid;
        public int Rows { get; }
        public int Columns { get; }

        public int this[int r, int c]
        {
            get => grid[r, c];
            set => grid[r, c] = value;
        }

        public Gamegrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];
        }

        public bool Isinside(int r, int c)
        {
            return r >= 0 && r < Rows && c >= 0 && c < Columns;
        }

        public bool Isempty(int r, int c)
        {
            return Isinside(r, c) && grid[r, c] == 0;
        }

        public bool Isrowfull(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[r, c] == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool Isrowempty(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[r, c] != 0)
                {
                    return false;
                }

            }
            return true;
        }
        
        private void Clearrow(int r)
        {
            for(int c = 0; c < Columns; c++)
            {
                grid[r, c] = 0;
            }
        }

        private void Moverowdown(int r, int numRows)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r + numRows, c] = grid[r, c];
                grid[r, c] = 0;
            }
        }

        public int Clearfullrow()
        {
            int cleared = 0;
            for (int r = Rows-1; r >= 0; r--)
            {
                Console.WriteLine(r);
                if (Isrowfull(r))
                {
                    Clearrow(r);
                    cleared++;
                }
                else if (cleared > 0)
                {
                    Moverowdown(r, cleared);
                }
            }
            return cleared;
        }

    }
}