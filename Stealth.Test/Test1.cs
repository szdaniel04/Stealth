using Stealth.Model;
using Stealth.Model.Character;
using Stealth.Model.Utils;
using Stealth.Persistence;
namespace Stealth.Test
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void Load()
        {
            try
            {
                new Game("../../../../maps/testmap.txt");
            }
            catch (FileManagerException)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void LoadFromBadFile()
        {
            try
            {
                new Game("non_existing_file.txt");
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void LoadBadFormat()
        {
            try
            {
                new Game("../../../../../maps/badformat.txt");
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }


        [TestMethod]
        public void CanThePlayerExit()
        {
            Game game = new Game("../../../../maps/testmap.txt");
            game.MovePlayer(Direction.UP);
            Assert.AreEqual(true, game.IsWon);
        }

        [TestMethod]
        public void GetCaughtTest()
        {
            Game game = new Game("../../../../maps/testmap.txt", new Player(1, 1), new List<Guard> { new Guard(3, 3) });
            Assert.AreEqual(true, game.IsLost);
        }
        [TestMethod]
        public void GuardMoveTest()
        {
            Game game = new Game("../../../../maps/testmap.txt", new Player(1, 1), new List<Guard> { new Guard(3, 3, Direction.RIGHT) });
            game.MoveAllGuards();
            Assert.AreEqual(3, game.Guards[0].X);
            Assert.AreEqual(3, game.Guards[0].Y);
            game = new Game("../../../../maps/testmap.txt", new Player(1, 1), new List<Guard> { new Guard(3, 3, Direction.UP) });
            game.MoveAllGuards();
            Assert.AreEqual(3, game.Guards[0].X);
            Assert.AreEqual(2, game.Guards[0].Y);

        }

        [TestMethod]
        public void PlayerMoveTest()
        {
            Game game = new Game("../../../../maps/testmap.txt", new Player(1, 2), new List<Guard> { new Guard(3, 3, Direction.RIGHT) });
            game.player.Move(Direction.UP); //wall
            Assert.AreEqual(1, game.player.Y);
            game.player.Move(Direction.DOWN); //wall
            Assert.AreEqual(2, game.player.Y);

        }
    }
}
