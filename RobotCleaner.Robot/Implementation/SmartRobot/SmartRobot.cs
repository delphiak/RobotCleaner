using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RobotCleaner.Robot
{
    /// <summary>
    /// Smart robot implementation
    /// Takes much more time to process every move, but memory consumtion is at O(moves)
    /// </summary>
    public class SmartRobot : IRobot
    {
        public Point Origin { get; }
        public Point Position { get; private set; }
        public IEnumerable<RobotMove> Moves => moves;
        public int CleanedSpaces { get; private set; }


        private List<PathSection> sections = new List<PathSection>();
        private List<RobotMove> moves = new List<RobotMove>();

        public SmartRobot(Point origin)
        {
            CleanedSpaces = 1; // robot has to start somewhere, and we count this place as cleaned
            Origin = origin;
            Position = origin;

            sections.Add(new PathSection(origin, origin));            
        }


        /// <summary>
        /// Move robot and pre-calculate cleaned spaces
        /// Pre-calculation is done for efficiency for crosstesting the solution
        /// </summary>
        public void Move(RobotMove move)
        {
            moves.Add(move);

            // calculate path section and move the robot to new position
            var section = GetSection(Position, move);
            Position = section.BottomLeft == Position ? section.TopRight : section.BottomLeft;

            // get list of previous moves that this move intersects with
            var intersections = new List<PathSection>();
            foreach (var s in sections)
            {
                var common = GetIntersection(section, s);
                if (common != null)
                {
                    intersections.Add(common);
                }
            }

            // squash intersections to prevent for double counting of them
            var squash = Squash(intersections).ToList();

            // calculate new number of cleaned spaces with this move
            CleanedSpaces += section.Length - squash.Sum(x => x.Length);

            // store this path section to consider it in the future
            sections.Add(section);
        }

        /// <summary>
        /// Get intersection of two path sections
        /// </summary>
        /// <returns>Intersection, if exists, null otherwise</returns>
        private PathSection GetIntersection(PathSection a, PathSection b)
        {
            if (a.Horizontal == b.Horizontal)
            {
                // sections are parallel to each other
                // there can be [0, Min(a.Length, b.Length)] places in common

                if (a.Horizontal)
                {
                    // horizontal lines
                    if (a.BottomLeft.Y == b.BottomLeft.Y)
                    {
                        var bl = Math.Max(a.BottomLeft.X, b.BottomLeft.X);
                        var tr = Math.Min(a.TopRight.X, b.TopRight.X);

                        // return intersection if exists
                        return bl <= tr ? new PathSection(new Point(bl, a.BottomLeft.Y), new Point(tr, a.BottomLeft.Y)) : null;
                    }
                    else
                    {
                        // lines are in different Y axis, there cannot be any intersection
                        return null;
                    }
                }
                else
                {
                    // vertical lines
                    if (a.BottomLeft.X == b.BottomLeft.X)
                    {
                        var bl = Math.Max(a.BottomLeft.Y, b.BottomLeft.Y);
                        var tr = Math.Min(a.TopRight.Y, b.TopRight.Y);

                        // return intersection if exists
                        return bl <= tr ? new PathSection(new Point(a.BottomLeft.X, bl), new Point(a.BottomLeft.X, tr)) : null;
                    }
                    else
                    {
                        // lines are in different X axis, there cannot be any intersection
                        return null;
                    }
                }
            }
            else
            {
                // sections are perpendicular to each other
                // there can be [0, 1] places in common

                var h = a.Horizontal ? a : b;
                var v = a.Horizontal ? b : a;

                var c1 = (h.BottomLeft.X <= v.BottomLeft.X) && (v.BottomLeft.X <= h.TopRight.X);
                var c2 = (v.BottomLeft.Y <= h.BottomLeft.Y) && (h.BottomLeft.Y <= v.TopRight.Y);
                var ip = new Point(v.BottomLeft.X, h.BottomLeft.Y);
                return c1 && c2 ? new PathSection(ip, ip) : null;
            }
        }

        /// <summary>
        /// Squash path sections on one axis that share common ground
        /// </summary>
        private IEnumerable<PathSection> Squash(IEnumerable<PathSection> sections)
        {
            var orderedSections = sections
                .OrderBy(x => x.BottomLeft.X)
                .ThenBy(x => x.BottomLeft.Y)
                .ToList();

            var squashed = new List<PathSection>();
            var current = orderedSections.First();
            foreach (var s in orderedSections)
            {
                if (s.BottomLeft.X > current.TopRight.X || s.BottomLeft.Y > current.TopRight.Y)
                {
                    squashed.Add(current);
                    current = s;
                }
                else
                {
                    var mx = Math.Max(current.TopRight.X, s.TopRight.X);
                    var my = Math.Max(current.TopRight.Y, s.TopRight.Y);
                    current = new PathSection(current.BottomLeft, new Point(mx, my));
                }
            }

            if (current != null)
            {
                squashed.Add(current);
            }


            return squashed;
        }

        /// <summary>
        /// Translate move of the robot from postion into a section on the grid
        /// </summary>
        /// <param name="p">Position of the robot</param>
        /// <param name="move">Robot's move</param>
        /// <returns>Path section on the grid</returns>
        private PathSection GetSection(Point p, RobotMove move)
        {
            return (move.Direction) switch
            {
                RobotMoveDirection.E => new PathSection(p, new Point(p.X + move.Steps, p.Y)),
                RobotMoveDirection.W => new PathSection(new Point(p.X - move.Steps, p.Y), p),
                RobotMoveDirection.N => new PathSection(p, new Point(p.X, p.Y + move.Steps)),
                RobotMoveDirection.S => new PathSection(new Point(p.X, p.Y - move.Steps), p),
                _ => throw new ArgumentException($"Unkown value of direction paramater: {move.Direction}")
            };
        }

    }
}
