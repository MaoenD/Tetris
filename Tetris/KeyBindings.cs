using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tetris
{
    public class KeyBindings
    {
        public Key MoveLeft { get; set; } = Key.Left;
        public Key MoveRight { get; set; } = Key.Right;
        public Key MoveDown { get; set; } = Key.Down;
        public Key RotateCW { get; set; } = Key.Up;
        public Key RotateCCW { get; set; } = Key.Z;
        public Key HoldBlock { get; set; } = Key.C;
        public Key DropBlock { get; set; } = Key.Space;
    }

}
