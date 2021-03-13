using System;

namespace RobotCleaner.Robot
{
    public class RobotMove
    {

        public RobotMoveDirection Direction { get; }
        public int Steps { get; }

        public RobotMove(RobotMoveDirection direction, int steps)
        {
            Direction = direction;
            Steps = steps;
        }

    }
}
