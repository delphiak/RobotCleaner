using System;
using System.Collections.Generic;
using System.Drawing;

namespace RobotCleaner.Robot
{
    /// <summary>
    /// Naive robot implementation
    /// Highly memory consumptive, due to large space of possible places to visit
    /// It just marks if the place was visit or not and returns number of visitied, unique places
    /// </summary>
    public class NaiveRobot : IRobot
    {
        public Point Origin { get; }
        public Point Position { get; private set; }

        public IEnumerable<RobotMove> Moves => moves;

        public int CleanedSpaces => visited.Count;

        private List<RobotMove> moves = new List<RobotMove>();
        private HashSet<Point> visited = new HashSet<Point>();

        public NaiveRobot(Point origin)
        {
            Origin = origin;
            Position = origin;
            visited.Add(origin);
        }

        public void Move(RobotMove move)
        {
            moves.Add(move);

            var newPosition = GetPosition(Position, move);
            Visit(Position, newPosition);
            Position = newPosition;
        }

        private void Visit(Point from, Point to)
        {
            var current = new Point(from.X, from.Y);
            if (from.X == to.X)
            {
                while (current.Y != to.Y)
                {
                    current = new Point(from.X, from.Y < to.Y ? current.Y + 1 : current.Y - 1);
                    visited.Add(current);
                }
            }
            else
            {
                while (current.X != to.X)
                {
                    current = new Point(from.X < to.X ? current.X + 1 : current.X - 1, from.Y);
                    visited.Add(current);
                }
            }
        }


        private Point GetPosition(Point position, RobotMove move)
        {
            return (move.Direction) switch
            {
                RobotMoveDirection.E => new Point(position.X - move.Steps, position.Y),
                RobotMoveDirection.W => new Point(position.X + move.Steps, position.Y),
                RobotMoveDirection.N => new Point(position.X, position.Y + move.Steps),
                RobotMoveDirection.S => new Point(position.X, position.Y - move.Steps),
                _ => throw new ArgumentException("Unkown value of direction paramater")
            };
        }

    }
}
