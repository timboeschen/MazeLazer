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
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Please type in or paste the path to your maze definition file.");
                var defenitionFile = Console.ReadLine();

                var maze = GetMaze(defenitionFile);
                var laser = new Laser(maze.EntryRoom, maze.GetDirection());

                Console.WriteLine($"Maze is {maze.Rooms.Max(r => r.X) + 1} rooms by {maze.Rooms.Max(r => r.Y) + 1} rooms");
                Console.WriteLine($"Laser enters the maze traveling with a {laser.Orientation} orientation into Room({maze.EntryRoom.X},{maze.EntryRoom.Y}).");

                var exitRoom = maze.ShootLaser(laser);
                Console.WriteLine($"Laser exits the maze traveling with a {laser.Orientation} orientation out of Room({exitRoom.X},{exitRoom.Y}).");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.WriteLine();
                Console.WriteLine("Hit any key to exit the application.");
                Console.ReadKey();
            }
        }

        static Maze GetMaze(string fileName)
        {
            const string RECORD_DELIMITER = "-1";

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.Default))
            {
                var dimensions = GetDimensions(sr.ReadLine());
                var maze = new Maze(dimensions.First(), dimensions.Last());

                if (sr.ReadLine() != RECORD_DELIMITER)
                    throw new Exception("Invalid Definition File Format!!");

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
                var entryRoomMatch = entryRoomRegex.Match(line);
                var entryRoomDetails = entryRoomRegex.Match(line).Value;
                var entryRoomDimensions = GetDimensions(line.Substring(0, entryRoomMatch.Index));

                maze.EntryRoom = new Room(entryRoomDimensions.First(), entryRoomDimensions.Last());
                maze.LazerOrientation = entryRoomDetails == "V" ? LaserOrientation.Vertical : LaserOrientation.Horizontal;

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
    }
}
