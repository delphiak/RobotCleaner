using System.Collections.Generic;
using System.Drawing;

namespace RobotCleaner.Robot
{
    public interface IRobot
    {
        Point Origin { get; }
        Point Position { get; }
        IEnumerable<RobotMove> Moves { get; }
        void Move(RobotMove move);
        int CleanedSpaces { get; }
    }
}
