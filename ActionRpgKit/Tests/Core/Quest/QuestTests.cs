using System;
using NUnit.Framework;
using ActionRpgKit.Core.Quest;

namespace ActionRpgKit.Tests.Core.Quest
{
    [TestFixture]
    [Category("Quest")]
    public class QuestTests
    {
        IQuest simpleQuest;

        [SetUp]
        public void SetUp ()
        {
            simpleQuest = new CleanseTheCellars();
        }

        [Test]
        public void SimpleQuestTest ()
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
            Assert.IsTrue(simpleQuest.IsCompleted);
        }
    }

    class CleanseTheCellars : BaseQuest
    {
        public CleanseTheCellars ()
        {
            Name = "Cleanse the cellars";
            Description = "Get rid of the rats";
            Experience = 100;

            IObjective objectiveA = new GetRidOfRatsObjective();
            Objectives.Add(objectiveA);
        }

        public override void CheckProgress()
        {
            bool completed = true;
            for (int i=0; i < Objectives.Count; i++)
            {
                if (Objectives[i].IsCompleted)
                {
                    continue;
                }
                Objectives[i].CheckProgress();
                if (!Objectives[i].IsCompleted)
                {
                    completed = false;
                }
                else
                {
                    OnObjectiveCompletion(Objectives[i]);
                }
            }
            if (completed)
            {
                OnCompletion();
            }
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

        public GetRidOfRatsObjective ()
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
}
