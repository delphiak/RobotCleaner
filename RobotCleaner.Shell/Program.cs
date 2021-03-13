using System;
using System.Drawing;
using System.Linq;
using RobotCleaner.Robot;

namespace RobotCleaner.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            var originRaw = Console.ReadLine().Split(' ').Select(x => int.Parse(x)).ToList();
            var origin = new Point(originRaw.First(), originRaw.Last());

            var robot = new SmartRobot(origin);

            for (var i = 0; i < n; i++)
            {
                var moveRaw = Console.ReadLine().Split(' ').ToList();
                var direction = Enum.Parse<RobotMoveDirection>(moveRaw.First());
                var steps = int.Parse(moveRaw.Last());

                robot.Move(new RobotMove(direction, steps));
            }

            Console.WriteLine($"=> Cleaned: {robot.CleanedSpaces}");
        }
    }
}
