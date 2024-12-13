using System.Dynamic;
using System.Threading;

namespace Tetris
{
    public class Gamestatus
    {
        private Block currentBlock;

        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset();
            }
        }

        public Gamegrid Gamegrid { get; }
        public Queue Queue { get; }
        public bool Gameover { get; private set; }

        public Gamestatus()
        {
            Gamegrid = new Gamegrid(22, 10);
            Queue = new Queue();
            currentBlock = Queue.Getndupdate();
        }

        private bool Blockfits()
        {
            foreach (Position p in currentBlock.Tileposition())
            {
                if (!Gamegrid.Isempty(p.Row, p.Column))
                {
                    return false;
                }
            }
            return true;
        }

        public void RotateCW()
        {
            currentBlock.RotateCW();
            if (!Blockfits())
            {
                currentBlock.RotateCW();
            }
        }

        public void RotateCCW()
        {
            currentBlock.RotateCCW();
            if (!Blockfits())
            {
                currentBlock.RotateCCW();
            }
        }

        public void Moveleft()
        {
            currentBlock.Move(0, -1);
            if (!Blockfits())
            {
                currentBlock.Move(0, 1);    
            }
        }

        public void Moveright()
        {
            currentBlock.Move(0, 1);
            if (!Blockfits())
            {
                currentBlock.Move(0, -1);
            }
        }

        private bool Gameisover()
        {
            return !(Gamegrid.Isrowempty(0) && Gamegrid.Isrowempty(1));
        }

        private void Placeblock()
        {
            foreach (Position p in currentBlock.Tileposition())
            {
                Gamegrid[p.Row, p.Column] = currentBlock.Id;
            }

            Gamegrid.Clearfullrow();

            if (Gameisover())
            {
                Gameover = true;
            }
            else
            {
                CurrentBlock = Queue.Getndupdate();
            }
        }

        public void Movedown()
        {
            currentBlock.Move(1, 0);
            if (!Blockfits())
            {
                currentBlock.Move(-1, 0);
                Placeblock();
            }
        }
    }
}

