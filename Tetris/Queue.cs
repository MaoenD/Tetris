using System;

namespace Tetris
{
    public class Queue
    {
        private readonly Block[] blocks = new Block[]
        {
            new Iblock(),
            new Jblock(),
            new Lblock(),
            new Oblock(),
            new Sblock(),
            new Tblock(),
            new Zblock()
        };

        private readonly Random rand = new Random();
        public Block Nextblock { get; private set; }

        public Queue()
        {
            Nextblock = Randomblock();
        }
        private Block Randomblock()
        {
            return blocks[rand.Next(blocks.Length)];
        }

        public Block Getndupdate()
        {
            Block block = Nextblock;

            do
            {
                Nextblock = Randomblock();
            }
            while (block.Id == Nextblock.Id);

            return block;
        }
    }
}
