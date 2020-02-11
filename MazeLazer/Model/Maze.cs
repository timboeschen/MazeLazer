using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLaser.Model
{
    public enum LaserOrientation { Vertical, Horizontal }
    class Maze
    {
        private List<Mirror> Mirrors { get; set; }
        public Room EntryRoom { get; set; }
        public LaserOrientation LazerOrientation { get; set; }
        public List<Room> Rooms { get; set; }

        public Maze(int width, int height, Room entryRoom, LaserOrientation lazerOrientation) : this(width, height)
        {
            EntryRoom = entryRoom;
            LazerOrientation = lazerOrientation;
        }
        public Maze(int width, int height)
        {
            Mirrors = new List<Mirror>();
            Rooms = new List<Room>();
            GenerateRooms(width, height);
        }

        public void AddMirror(Mirror mirror, Room room)
        {
            Rooms.Single(r => r.Equals(room)).Mirror = mirror;
        }
        private void GenerateRooms(int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Rooms.Add(new Room(x, y));
                }
            }
        }

        public Direction GetDirection()
        {
            if (LazerOrientation == LaserOrientation.Vertical)
            {
                if (EntryRoom.Y == 0)
                    return Direction.Up;
                else if (EntryRoom.Y == Rooms.Max(r => r.Y))
                    return Direction.Down;
                else
                    throw new Exception("Invalid Entry Room.");
            }
            else
            {
                if (EntryRoom.X == 0)
                    return Direction.Right;
                else if (EntryRoom.X == Rooms.Max(r => r.X))
                    return Direction.Left;
                else
                    throw new Exception("Invalid Entry Room.");
            }
        }

    }
}
