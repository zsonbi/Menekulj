﻿namespace Menekulj.Model.Tests
{
    [TestClass()]
    public class GameModelTests
    {
        [TestMethod()]
        public void GameModelTest()
        {
            GameModel model = new GameModel(10, 97);
            Assert.AreEqual<uint>(97, model.MineCount);
            Assert.AreEqual(10, model.MatrixSize);
            Assert.AreEqual(10, model.Cells.GetLength(0));
            Assert.AreEqual(0, model.Player.Position.Row);
            Assert.AreEqual(0, model.Player.Position.Col);

            int counter = 0;

            for (int i = 0; i < model.MatrixSize; i++)
            {
                for (int j = 0; j < model.MatrixSize; j++)
                {
                    if (model.Cells[i, j] == Cell.Mine)
                    {
                        counter++;
                    }
                }
            }

            Assert.AreEqual(97, counter);
            Assert.AreEqual(2, model.Enemies.Count);
            Assert.IsFalse(model.IsOver());

            model.SaveGame("./testFile.json");
        }

        [TestMethod()]
        public void GameModelTestLoaded()
        {
            GameModel model = Persistance.Persistance.LoadStateAsync("TestInputFiles/testFile1.json").Result;
            Assert.AreEqual<uint>(97, model.MineCount);
            Assert.AreEqual(10, model.MatrixSize);
            Assert.AreEqual(10, model.Cells.GetLength(0));
            Assert.AreEqual(0, model.Player.Position.Row);
            Assert.AreEqual(0, model.Player.Position.Col);

            int counter = 0;

            for (int i = 0; i < model.MatrixSize; i++)
            {
                for (int j = 0; j < model.MatrixSize; j++)
                {
                    if (model.Cells[i, j] == Cell.Mine)
                    {
                        counter++;
                    }
                }
            }

            Assert.AreEqual(97, counter);
            Assert.AreEqual(2, model.Enemies.Count);
            Assert.IsFalse(model.IsOver());
        }


        [TestMethod()]
        public void GetMinePositionsTest()
        {
            GameModel model1 = Persistance.Persistance.LoadStateAsync("TestInputFiles/testFile1.json").Result;
            GameModel model2 = Persistance.Persistance.LoadStateAsync("TestInputFiles/testFile2.json").Result;
            GameModel model3 = Persistance.Persistance.LoadStateAsync("TestInputFiles/testFile3.json").Result;
            GameModel model4 = new GameModel(10, 15);

            Assert.AreEqual(97, model1.GetMinePositions().Count);
            Assert.AreEqual(1, model2.GetMinePositions().Count);
            Assert.AreEqual(0, model3.GetMinePositions().Count);
            Assert.AreEqual(15, model4.GetMinePositions().Count);
        }

        [TestMethod()]
        public void TickTest()
        {
            GameModel model1 = Persistance.Persistance.LoadStateAsync("TestInputFiles/testFile1.json").Result;
            GameModel model2 = Persistance.Persistance.LoadStateAsync("TestInputFiles/testFile2.json").Result;

            model1.Tick(null, new EventArgs());
            Assert.IsTrue(model1.IsOver());
            Assert.IsFalse(model1.PlayerWon);

            model2.Tick(null, new EventArgs());
            Assert.AreEqual(0, model2.Player.Position.Row);
            Assert.AreEqual(1, model2.Player.Position.Col);
            Assert.AreEqual(0, model2.Player.PrevPosition.Row);
            Assert.AreEqual(0, model2.Player.PrevPosition.Col);
            Assert.AreEqual(8, model2.Enemies[0].Position.Row);
            Assert.AreEqual(0, model2.Enemies[0].Position.Col);


        }

        [TestMethod()]
        public void StartGameTest()
        {
            GameModel model1 = Persistance.Persistance.LoadStateAsync("TestInputFiles/testFile1.json").Result;
            GameModel model2 = Persistance.Persistance.LoadStateAsync("TestInputFiles/testFile2.json").Result;
            GameModel model3 = Persistance.Persistance.LoadStateAsync("TestInputFiles/testFile3.json").Result;

            model1.StartGame();
            while (!model1.IsOver())
            {
            }

            Assert.IsTrue(model1.IsOver());
            Assert.IsFalse(model1.PlayerWon);

            model2.StartGame(10);
            while (!model2.IsOver())
            {
            }
            Assert.IsTrue(model2.IsOver());
            Assert.IsFalse(model2.PlayerWon);


            model3.StartGame(10);
            while (!model3.IsOver())
            {

            }
            Assert.IsTrue(model3.IsOver());
            Assert.IsFalse(model3.PlayerWon);
        }

        [TestMethod()]
        public void IsOverTest()
        {
            GameModel model1 = Persistance.Persistance.LoadStateAsync("TestInputFiles/testFile1.json").Result;

            model1.Tick(null, new EventArgs());
            Assert.IsTrue(model1.IsOver());
            Assert.IsFalse(model1.PlayerWon);
        }

        [TestMethod()]
        public void ChangePlayerDirectionTest()
        {
            GameModel model1 = Persistance.Persistance.LoadStateAsync("TestInputFiles/testFile1.json").Result;
            model1.ChangePlayerDirection(Direction.Up);

            Assert.AreEqual(Direction.Up, model1.Player.LookingDirection);
            model1.Tick(null, new EventArgs());
            Assert.IsTrue(model1.IsOver());
            Assert.IsTrue(model1.PlayerWon);
        }

        [TestMethod()]
        public void SaveGameTest()
        {
            GameModel model = new GameModel(10, 10);
            model.SaveGame("SaveGameTestJson1234141.json");

            GameModel loadedModel = Persistance.Persistance.LoadStateAsync("SaveGameTestJson1234141.json").Result;

            File.Delete("SaveGameTestJson1234141.json");


            Assert.AreEqual(model.MatrixSize, loadedModel.MatrixSize);
            Assert.IsTrue(model.Player.Position.Equals(loadedModel.Player.Position));
            Assert.IsTrue(model.Enemies[0].Position.Equals(loadedModel.Enemies[0].Position));
            Assert.AreEqual(model.MineCount, loadedModel.MineCount);
        }

        [TestMethod()]
        public void PauseTest()
        {
            GameModel model = new GameModel(10, 10);

            model.Pause();

            Assert.IsFalse(model.Running);
        }

        [TestMethod()]
        public void ResumeTest()
        {
            GameModel model2 = Persistance.Persistance.LoadStateAsync("TestInputFiles/testFile2.json").Result;
            model2.StartGame(100);

            Assert.IsTrue(model2.Running);

            model2.Pause();

            Assert.IsFalse(model2.Running);

            model2.Resume();

            Assert.IsTrue(model2.Running);

        }
    }
}