using System;
using System.Drawing;
using Xunit;

namespace RobotCleaner.Robot.Tests
{

    public class CrossVerificationTests
    {

        /// <summary>
        /// Test verifies smart robot implementation (that actually satisfies the task's constraints) with the naive implementation that is easy to reason for it's correctness
        /// </summary>
        [Fact]
        public void CrossTest()
        {
            var runs = 100;
            var moves = 1000;
            

            for (var t = 0; t < runs; t++)
            {
                var randomizer = new Random();
                var origin = new Point(randomizer.Next(-1000, 1000), randomizer.Next(-1000, 1000));
                var naiveRobot = new NaiveRobot(origin);
                var smartRobot = new SmartRobot(origin);

                for (var i = 0; i < moves; i++)
                {

                    var move = GetRandomMove(randomizer);

                    naiveRobot.Move(move);
                    smartRobot.Move(move);

                    Assert.Equal(naiveRobot.CleanedSpaces, smartRobot.CleanedSpaces);
                }
            }
        }

        private RobotMove GetRandomMove(Random randomizer)
        {
            return new RobotMove(GetRandomDirection(randomizer), randomizer.Next(1, 100));
        }

        private RobotMoveDirection GetRandomDirection(Random randomizer)
        {
            return (randomizer.Next(1, 4)) switch
            {
                1 => RobotMoveDirection.E,
                2 => RobotMoveDirection.S,
                3 => RobotMoveDirection.N,
                4 => RobotMoveDirection.W,
                _ => throw new ArgumentOutOfRangeException("Unkown direction to follow"),
            };
        }

    }
}
