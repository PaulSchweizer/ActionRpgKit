using System;
using ActionRpgKit.Story;
using ActionRpgKit.Story.Quest;
using ActionRpgKit.Item;

namespace ActionRpgKit.Tests.IntegrationTests
{

    class CleanseTheCellars : Quest
    {
        public CleanseTheCellars()
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
            int collectedHerbs = MainController.Player.Inventory.GetQuantity(ItemDatabase.GetItemByName("Herb").Id);
            if (collectedHerbs == 10)
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
