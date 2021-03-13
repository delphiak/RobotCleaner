using System;
using System.Drawing;
using Xunit;

namespace RobotCleaner.Robot.Tests
{

    public class RobotCleanedSpacesTests
    {

        private IRobot CreateRobot(Point origin) => new SmartRobot(origin);

        [Fact]
        public void TaskExample()
        {
            var systemUnderTest = CreateRobot(new Point(0, 0));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.E, 2));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.N, 1));
            Assert.Equal(4, systemUnderTest.CleanedSpaces);
        }

        [Fact]
        public void RobotNotMoved()
        {
            var systemUnderTest = CreateRobot(new Point(0, 0));
            Assert.Equal(1, systemUnderTest.CleanedSpaces);
        }

        [Fact]
        public void AllDirectionsNoIntersection()
        {
            var systemUnderTest = CreateRobot(new Point(0, 0));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.S, 1));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.E, 2));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.N, 3));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.W, 4));
            Assert.Equal(11, systemUnderTest.CleanedSpaces);
        }

        [Fact]
        public void FullVerticalParallelIntersection()
        {
            var systemUnderTest = CreateRobot(new Point(0, 0));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.S, 3));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.N, 3));
            Assert.Equal(4, systemUnderTest.CleanedSpaces);
        }

        [Fact]
        public void PartialVerticalParallelIntersection()
        {
            var systemUnderTest = CreateRobot(new Point(0, 0));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.S, 3));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.N, 6));
            Assert.Equal(7, systemUnderTest.CleanedSpaces);
        }

        [Fact]
        public void FullHorizontalParallelIntersection()
        {
            var systemUnderTest = CreateRobot(new Point(0, 0));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.W, 3));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.E, 3));
            Assert.Equal(4, systemUnderTest.CleanedSpaces);
        }

        [Fact]
        public void PartialHorizontalParallelIntersection()
        {
            var systemUnderTest = CreateRobot(new Point(0, 0));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.W, 3));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.E, 6));
            Assert.Equal(7, systemUnderTest.CleanedSpaces);
        }

        [Fact]
        public void SingleIntersection()
        {
            var systemUnderTest = CreateRobot(new Point(0, 0));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.N, 3));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.W, 3));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.S, 1));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.E, 6));
            Assert.Equal(13, systemUnderTest.CleanedSpaces);
        }

        [Fact]
        public void TripleIntersection()
        {
            var systemUnderTest = CreateRobot(new Point(0, 0));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.N, 3));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.E, 1));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.S, 3));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.E, 2));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.N, 3));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.E, 3));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.S, 1));
            systemUnderTest.Move(new RobotMove(RobotMoveDirection.W, 9));
            Assert.Equal(23, systemUnderTest.CleanedSpaces);
        }

        [Fact]
        public void OriginShouldNotAffectCleanedSpaces()
        {
            var moves = new[]
            {
                new RobotMove(RobotMoveDirection.N, 3),
                new RobotMove(RobotMoveDirection.E, 1),
                new RobotMove(RobotMoveDirection.S, 3),
                new RobotMove(RobotMoveDirection.E, 2),
                new RobotMove(RobotMoveDirection.N, 3),
                new RobotMove(RobotMoveDirection.E, 3),
                new RobotMove(RobotMoveDirection.S, 1),
                new RobotMove(RobotMoveDirection.W, 9),
            };

            var originRobot = CreateRobot(new Point(0, 0));
            var systemUnderTest = CreateRobot(new Point(0, 1));

            foreach (var m in moves)
            {
                originRobot.Move(m);
                systemUnderTest.Move(m);
            }

            Assert.Equal(originRobot.CleanedSpaces, systemUnderTest.CleanedSpaces);
        }

    }
}
