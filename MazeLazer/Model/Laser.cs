using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLaser.Model
{
    public enum Direction { Up, Down, Left, Right }
    class Laser
    {
        public Room Room { get; set; }
        public Direction Direction { get; set; }

        public string Orientation
        {
            get
            {
                return (Direction == Direction.Up || Direction == Direction.Down) ? "vertical" : "horizontal";
            }
        }

        public Laser() { }
        public Laser(Room room, Direction direction)
        {
            Room = room;
            Direction = direction;
        }

        //public bool Move()
        //{


        //    return true;
        //}
    }
}
