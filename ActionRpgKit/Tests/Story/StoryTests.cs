using System;
using NUnit.Framework;
using ActionRpgKit.Story;
using ActionRpgKit.Story.Quest;

namespace ActionRpgKit.Tests.Story
{

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
            simpleQuest = new CleanseTheCellars();

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

            // Solve the second quest, this will move to the next Chapter
            GetRidOfRatsObjective.rats = 0;
            storyline.CheckProgress();
            Assert.AreEqual("Epilog", storyline.CurrentChapter.Name);

            // The Storyline has been completed now
            storyline.CheckProgress();
        }
    }

    // -----------------------------------------------------------------------
    // Test Classes
    // -----------------------------------------------------------------------

    class CleanseTheCellars : BaseQuest
    {
        public CleanseTheCellars()
        {
            Name = "Cleanse the cellars";
            Description = "Get rid of the rats";
            Experience = 100;

            IObjective objectiveA = new GetRidOfRatsObjective();
            Objectives.Add(objectiveA);
            IObjective objectiveB = new Find10HerbsObjective();
            Objectives.Add(objectiveB);
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

    class GetRidOfRatsObjective : BaseObjective
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

    class Find10HerbsObjective : BaseObjective
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

    public class GameStoryline : Storyline
    {
        public GameStoryline()
        {
            Chapters = new Chapter[] { new PrologChapter(), new EpilogChapter() };
        }
    }

    class PrologChapter : Chapter
    {
        public PrologChapter()
        {
            Name = "Prolog";
            Description = "The beginnings ...";
            Quests = new IQuest[] { new CleanseTheCellars() };
        }
    }

    class EpilogChapter : Chapter
    {
        public EpilogChapter()
        {
            Name = "Epilog";
            Description = "The end";
            Quests = new IQuest[] {};
        }
    }
}