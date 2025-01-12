using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tetris
{
    public abstract class Block
    {
        protected abstract Position[][] Tiles { get; }
        protected abstract Position Startoffset { get; }
        public abstract int Id { get; }

        private int rotationstatus;
        private Position offset;

        public Block()
        {
            offset = new Position(Startoffset.Row, Startoffset.Column);
        }

        public IEnumerable<Position> Tileposition()
        {
            foreach ( Position p in Tiles[rotationstatus])
            {
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }

        }

        public void RotateCW()
        {
            rotationstatus = (rotationstatus +1) % Tiles.Length;
        }

        public void RotateCCW()
        {
            if (rotationstatus == 0)
            {
                rotationstatus = Tiles.Length - 1;
            }
            else
            {
                rotationstatus--;
            }
        }

        public void Move(int rows, int columns)
        {
            offset.Row += rows;
            offset.Column += columns;
        }

        public void Reset()
        {
            rotationstatus = 0;
            offset.Row = Startoffset.Row;
            offset.Column = Startoffset.Column;
        }

        public int GetRaw()
        {
            return offset.Row;
        }
    }
}
