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

                for (int i = 0; i < 2; i++)
                {
                    currentBlock.Move(1, 0);

                    if (!Blockfits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }
            }
        }

        public Gamegrid Gamegrid { get; }
        public Queue Queue { get; }
        public bool Gameover { get; private set; }

        public int Score { get; private set; }

        public Block HeldBlock { get; set; }
        public bool CanHold { get; private set; }

        public Gamestatus()
        {
            Gamegrid = new Gamegrid(22, 10);
            Queue = new Queue();
            currentBlock = Queue.Getndupdate();
            Gameover = false;
            CanHold = true;
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

        public void HoldBlock()
        {
            if (!CanHold)
            {
                return;
            }

            if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = Queue.Getndupdate();
            } else
            {
                Block tmp = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = tmp;
            }
            
            CanHold = false;
        }

        public void RotateCW()
        {
            currentBlock.RotateCW();
            if (!Blockfits())
            {
                currentBlock.RotateCCW();
            }
        }

        public void RotateCCW()
        {
            currentBlock.RotateCCW();
            if (!Blockfits())
            {
                currentBlock.RotateCW();
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

            Score += Gamegrid.Clearfullrow();

            if (Gameisover())
            {
                Gameover = true;
            }
            else
            {
                CurrentBlock = Queue.Getndupdate();
                CanHold = true;
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

        private int TileDropDistance(Position p)
        {
            int drop = 0;

            while (Gamegrid.Isempty(p.Row + drop +1, p.Column))
            {
                drop++;
            }

            return drop;
        }

        public int BlockDropDistance()
        {
            int drop = Gamegrid.Rows;

            foreach (Position p in currentBlock.Tileposition())
            {
                drop = System.Math.Min(drop, TileDropDistance(p));
            }

            return drop;
        }

        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            Placeblock();
        }
    }
}

