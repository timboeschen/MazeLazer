using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLaser.Model
{
    class Room
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Mirror Mirror { get; set; }

        public Room() { }
        public Room(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            var room = obj as Room;

            if (room == null)
                return false;

            return X == room.X && Y == room.Y;
        }
        public override int GetHashCode() => new { X, Y }.GetHashCode();
    }
}
