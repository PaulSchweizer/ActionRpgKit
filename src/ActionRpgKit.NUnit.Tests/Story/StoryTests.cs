using System;
using System.IO;
using System.Diagnostics;
using NUnit.Framework;
using ActionRpgKit.Story;
using ActionRpgKit.Story.Quest;

namespace ActionRpgKit.Tests.Story
{

    #region Tests

    [TestFixture]
    [Category("Story")]
    public class QuestTests
    {
        Storyline storyline;
        IQuest simpleQuest;

        [SetUp]
        public void SetUp()
        {
            storyline = new GameStoryline();
            simpleQuest = new CleanseTheCellarsQuest();

            // Simulate the existence of 10 rats
            GetRidOfRatsObjective.rats = 10;

            // Simulate the possession of 0 herbs
            Find10HerbsObjective.herbs = 0;
        }

        [Test]
        public void QuestTest()
        {
            // The quest starts
            Assert.IsFalse(simpleQuest.IsCompleted);
            simpleQuest.CheckProgress();
            Assert.IsFalse(simpleQuest.IsCompleted);

            // The rats have been diminished
            GetRidOfRatsObjective.rats = 4;
            simpleQuest.CheckProgress();
            Assert.IsFalse(simpleQuest.IsCompleted);

            // All the rats have been eradicated
            GetRidOfRatsObjective.rats = 0;
            simpleQuest.CheckProgress();
            Assert.IsFalse(simpleQuest.IsCompleted);

            // Gather all the herbs and complete the entire quest
            Find10HerbsObjective.herbs = 10;
            simpleQuest.CheckProgress();
            Assert.IsTrue(simpleQuest.IsCompleted);
        }

        [Test]
        public void StoryControllerTest()
        {
            storyline.Start();
            Assert.AreEqual("Prolog", storyline.CurrentChapter.Name);

            // No Progress
            storyline.CheckProgress();
            Assert.IsFalse(storyline.CurrentChapter.Quests[0].IsCompleted);

            // Solve a quest
            Find10HerbsObjective.herbs = 10;
            storyline.CheckProgress();
            Assert.IsFalse(storyline.CurrentChapter.Quests[0].IsCompleted);
            Assert.AreEqual("Prolog", storyline.CurrentChapter.Name);

            // Call ToString to test it
            storyline.ToString();

            // Solve the second quest, this will move to the next Chapter
            GetRidOfRatsObjective.rats = 0;
            FindTreasureChestObjective.ChestFound = true;
            storyline.CheckProgress();
            Assert.AreEqual("Epilog", storyline.CurrentChapter.Name);

            // The Storyline has been completed now
            storyline.CheckProgress();

            storyline.CheckProgress();

            // Trying the pretty representation just to make sure
            storyline.ToString();
        }
    }

    #endregion

    #region Test Classes

    public class GameStoryline : Storyline
    {
        public GameStoryline()
        {
            Chapters = new Chapter[] { new PrologChapter(),
                                       new EpilogChapter() };
        }
    }

    class PrologChapter : Chapter
    {
        public PrologChapter()
        {
            Name = "Prolog";
            Description = "The beginnings ...";
            Quests = new IQuest[] { new CleanseTheCellarsQuest(), new FindTreasureQuest() };
        }
    }

    class EpilogChapter : Chapter
    {
        public EpilogChapter()
        {
            Name = "Epilog";
            Description = "The end";
            Quests = new IQuest[] { };
        }
    }

    class CleanseTheCellarsQuest : Quest
    {
        public CleanseTheCellarsQuest()
        {
            Name = "Cleanse the cellars";
            Description = "Get rid of the rats to reach the magic herbs.";
            Experience = 100;
            Objectives = new IObjective[] {new GetRidOfRatsObjective(), 
                                           new Find10HerbsObjective()};
        }

        public override void OnObjectiveCompletion(IObjective objective)
        {
            objective.OnCompletion();
        }

        public override void OnCompletion()
        {
            IsCompleted = true;
            Console.WriteLine("Quest \"{0}\" completed.", Name);
        }
    }

    class FindTreasureQuest : Quest
    {
        public FindTreasureQuest()
        {
            Name = "Find a treasure";
            Description = "Find the treasure by the big willow tree.";
            Experience = 200;
            Objectives = new IObjective[] {new FindTreasureChestObjective()};
        }

        public override void OnObjectiveCompletion(IObjective objective)
        {
            objective.OnCompletion();
        }

        public override void OnCompletion()
        {
            IsCompleted = true;
            Console.WriteLine("Quest \"{0}\" completed.", Name);
        }
    }

    class GetRidOfRatsObjective : Objective
    {
        public static float rats = 10;

        public GetRidOfRatsObjective()
        {
            Name = "Get rid of giant rats.";
            Description = "Get rid of the 10 rats.";
        }

        public override void CheckProgress()
        {
            if (rats == 0)
            {
                IsCompleted = true;
            }
        }

        public override void OnCompletion()
        {
            Console.WriteLine("Objective \"{0}\" completed.", Name);
        }
    }

    class Find10HerbsObjective : Objective
    {
        public static float herbs = 0;

        public Find10HerbsObjective()
        {
            Name = "Find 10 Herbs";
            Description = "Gather 10 magic herbs for the wizard.";
        }

        public override void CheckProgress()
        {
            if (herbs == 10)
            {
                IsCompleted = true;
            }
        }

        public override void OnCompletion()
        {
            Console.WriteLine("Objective \"{0}\" completed.", Name);
        }
    }

    class FindTreasureChestObjective : Objective
    {
        public static bool ChestFound = false;

        public FindTreasureChestObjective()
        {
            Name = "Find the treasure chest";
            Description = "The chest is burried at the old willow tree.";
        }

        public override void CheckProgress()
        {
            if (ChestFound)
            {
                IsCompleted = true;
            }
        }

        public override void OnCompletion()
        {
            Console.WriteLine("Objective \"{0}\" completed.", Name);
        }
    }
    #endregion
}
