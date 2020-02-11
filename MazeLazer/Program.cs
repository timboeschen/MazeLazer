using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLaser.Model;
using System.IO;
using System.Text.RegularExpressions;

namespace MazeLaser
{
    class Program
    {
        const string RECORD_DELIMITER = "-1";

        static void Main(string[] args)
        {
            try
            {

                Console.WriteLine("Please type in or paste the path to your maze definition file.");
                var defenitionFile = Console.ReadLine();

                //Check Code into 
                var maze = GetMaze(defenitionFile);
                var laser = new Laser(maze.EntryRoom, GetDirection(maze));

                Console.WriteLine($"Maze is {maze.Rooms.Max(r => r.X) + 1} rooms by {maze.Rooms.Max(r => r.Y) + 1} rooms");
                Console.WriteLine($"Laser enters the maze traveling with a {laser.Orientation} orientation into Room({maze.EntryRoom.X},{maze.EntryRoom.Y}).");

                var output = ProcessMaze(maze, laser);
                Console.WriteLine($"Laser exits the maze traveling with a {laser.Orientation} orientation out of Room({output.X},{output.Y}).");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally { Console.ReadKey(); }
        }

        static Maze GetMaze(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.Default))
            {
                var dimensions = GetDimensions(sr.ReadLine());
                var maze = new Maze(dimensions.First(), dimensions.Last());
                VerifyDelimiter(sr.ReadLine());

                string line = sr.ReadLine();
                while (line != RECORD_DELIMITER)
                {
                    var mirrorRegex = new Regex("(?:RR|RL|LR|LL|R|L)");
                    var mirrorMatch = mirrorRegex.Match(line);
                    var mirrorDetails = mirrorRegex.Match(line).Value;

                    Lean lean = mirrorDetails.Substring(0, 1) == "R" ? Lean.Right : Lean.Left;
                    ReflectiveSide side = mirrorDetails.Length == 2 ? mirrorDetails.Substring(1, 1) == "R" ? ReflectiveSide.Right : ReflectiveSide.Left : ReflectiveSide.Both;

                    var mirrorDimensions = GetDimensions(line.Substring(0, mirrorMatch.Index));
                    maze.AddMirror(new Mirror(lean, side), new Room(mirrorDimensions.First(), mirrorDimensions.Last()));
                    
                    line = sr.ReadLine();
                }

                line = sr.ReadLine();
                var entryRoomRegex = new Regex("(?:V|H)");
                var erMatch = entryRoomRegex.Match(line);
                var erDetails = entryRoomRegex.Match(line).Value;
                var erDimensions = GetDimensions(line.Substring(0, erMatch.Index));

                maze.EntryRoom = new Room(erDimensions.First(), erDimensions.Last());
                maze.LazerOrientation = erDetails == "V" ? LaserOrientation.Vertical : LaserOrientation.Horizontal;

                return maze;
            }
        }

        static IEnumerable<int> GetDimensions(string line)
        {
            var dimensions = line.Split(',').Select(d => int.Parse(d));
            if (dimensions.Count() != 2)
                throw new Exception("Invalid Maze Dimensions!!");
            return dimensions;
        }

        static void VerifyDelimiter(string line)
        {
            if (line != RECORD_DELIMITER)
                throw new Exception("Invalid Definition File Format!!");
        }

        static Direction GetDirection(Maze maze)
        {
            if (maze.LazerOrientation == LaserOrientation.Vertical)
            {
                if (maze.EntryRoom.Y == 0)
                    return Direction.Up;
                else if (maze.EntryRoom.Y == maze.Rooms.Max(r => r.Y))
                    return Direction.Down;
                else
                    throw new Exception("Invalid Entry Room.");
            }
            else
            {
                if (maze.EntryRoom.X == 0)
                    return Direction.Right;
                else if (maze.EntryRoom.X == maze.Rooms.Max(r => r.X))
                    return Direction.Left;
                else
                    throw new Exception("Invalid Entry Room.");
            }
        }

        static Room ProcessMaze(Maze map, Laser laser)
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
                                    case ReflectiveSide.Left:
                                    case ReflectiveSide.Both:
                                        laser.Direction = Direction.Up;
                                        break;
                                }
                                break;
                            case Direction.Right:
                                switch (laser.Room.Mirror.ReflectiveSide)
                                {
                                    case ReflectiveSide.Right:
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

                laser.Room = map.Rooms.SingleOrDefault(r => r.X == x && r.Y == y);
            }

            return lastRoom;
        }
    }
}
