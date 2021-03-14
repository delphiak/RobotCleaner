using System.Drawing;

namespace RobotCleaner.Robot
{
    public class PathSection
    {
        public Point BottomLeft { get; } 
        public Point TopRight { get; }

        public bool Horizontal => BottomLeft.Y == TopRight.Y;
        public int Coverage => Horizontal ? TopRight.X - BottomLeft.X + 1 : TopRight.Y - BottomLeft.Y + 1;

        public PathSection(Point a, Point b)
        {
            BottomLeft = a;
            TopRight = b;
        }

    }
}
