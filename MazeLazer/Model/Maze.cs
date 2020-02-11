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

        public Room ShootLaser(Laser laser)
        {
            var lastRoom = new Room();
            int x = laser.Room.X;
            int y = laser.Room.Y;

            while (laser.Room != null)
            {
                lastRoom = laser.Room;
                if (laser.Room.Mirror != null)
                {
                    if (laser.Room.Mirror.Lean == Lean.Right)
                    {
                        switch (laser.Direction)
                        {
                            case Direction.Up:
                                switch (laser.Room.Mirror.ReflectiveSide)
                                {
                                    case ReflectiveSide.Right:
                                    case ReflectiveSide.Both:
                                        laser.Direction = Direction.Right;
                                        break;
                                }
                                break;
                            case Direction.Down:
                                switch (laser.Room.Mirror.ReflectiveSide)
                                {
                                    case ReflectiveSide.Left:
                                    case ReflectiveSide.Both:
                                        laser.Direction = Direction.Left;
                                        break;
                                }
                                break;
                            case Direction.Left:
                                switch (laser.Room.Mirror.ReflectiveSide)
                                {
                                    case ReflectiveSide.Right:
                                    case ReflectiveSide.Both:
                                        laser.Direction = Direction.Down;
                                        break;
                                }
                                break;
                            case Direction.Right:
                                switch (laser.Room.Mirror.ReflectiveSide)
                                {
                                    case ReflectiveSide.Left:
                                    case ReflectiveSide.Both:
                                        laser.Direction = Direction.Up;
                                        break;
                                }
                                break;
                        }
                    }
                    else if (laser.Room.Mirror.Lean == Lean.Left)
                    {
                        switch (laser.Direction)
                        {
                            case Direction.Up:
                                switch (laser.Room.Mirror.ReflectiveSide)
                                {
                                    case ReflectiveSide.Left:
                                    case ReflectiveSide.Both:
                                        laser.Direction = Direction.Left;
                                        break;
                                }
                                break;
                            case Direction.Down:
                                switch (laser.Room.Mirror.ReflectiveSide)
                                {
                                    case ReflectiveSide.Right:
                                    case ReflectiveSide.Both:
                                        laser.Direction = Direction.Right;
                                        break;
                                }
                                break;
                            case Direction.Left:
                                switch (laser.Room.Mirror.ReflectiveSide)
                                {
                                    case ReflectiveSide.Right:
                                    case ReflectiveSide.Both:
                                        laser.Direction = Direction.Up;
                                        break;
                                }
                                break;
                            case Direction.Right:
                                switch (laser.Room.Mirror.ReflectiveSide)
                                {
                                    case ReflectiveSide.Left:
                                    case ReflectiveSide.Both:
                                        laser.Direction = Direction.Down;
                                        break;
                                }
                                break;
                        }
                    }
                }

                switch (laser.Direction)
                {
                    case Direction.Up:
                        y++;
                        break;
                    case Direction.Down:
                        y--;
                        break;
                    case Direction.Left:
                        x--;
                        break;
                    case Direction.Right:
                        x++;
                        break;
                }

                laser.Room = Rooms.SingleOrDefault(r => r.X == x && r.Y == y);
            }

            return lastRoom;
        }

    }
}
