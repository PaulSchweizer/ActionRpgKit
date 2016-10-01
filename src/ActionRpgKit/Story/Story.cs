using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using ActionRpgKit.Story.Quest;

namespace ActionRpgKit.Story
{
    /// <summary>
    /// The flow of the story.</summary>
    /// <remarks>
    /// The Storyline is divided into Chapters. Each Chapter contains a number 
    /// of Quests that need to be solved in order for the Chapter to be 
    /// completed.</remarks>
    public abstract class Storyline
    {
        private int _currentChapterIndex = -1;

        public override string ToString()
        {
            string repr = "";
            for (int i = 0; i < Chapters.Length; i++)
            {
                repr += string.Format("{0} {1}", i + 1, Chapters[i].ToString());
            }
            return repr;
        }

        /// <summary>
        /// Starting the Storyline and initialize it to the first Chapter.</summary>
        public void Start ()
        {
            NextChapter();
        }

        public void NextChapter()
        {
            if (_currentChapterIndex + 1 < Chapters.Length)
            {
                _currentChapterIndex += 1;
                CurrentChapter = Chapters[_currentChapterIndex];
            }
            else
            {
                End();
            }
        }

        public void End()
        {
            Console.WriteLine("STORYLINE Ended");
        }

        /// <summary>
        /// The Chapter of the Storyline, the list has to have at least one item.</summary>
        public Chapter[] Chapters { get; set; }

        /// <summary>
        /// The current Chapter in the Story.</summary>
        public Chapter CurrentChapter { get; set; }

        public void CheckProgress ()
        {
            CurrentChapter.CheckProgress();
            if (CurrentChapter.IsCompleted)
            {
                NextChapter();
            }
        }
    }

    /// <summary>
    /// A step in the progression of the story.</summary>
    public abstract class Chapter
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IQuest[] Quests { get; set; }

        public bool IsCompleted { get; set; }

        public void CheckProgress()
        {
            bool completed = true;
            for (int i = 0; i < Quests.Length; i++)
            {
                if (Quests[i].IsCompleted)
                {
                    continue;
                }
                Quests[i].CheckProgress();
                if (!Quests[i].IsCompleted)
                {
                    completed = false;
                }
            }
            if (completed)
            {
                OnCompletion();
            }
            IsCompleted = completed;
        }

        public void OnCompletion()
        {
            Console.WriteLine("\u2611 Chapter {0} COMPLETED!", Name);
        }

        public override string ToString()
        {
            string repr = "";
            if (IsCompleted)
            {
                repr += string.Format("\u2611 {0} [Chapter]\n  \n{1}\n\n", Name, Description);
            }
            else
            {
                repr += string.Format("\u2610 {0} [Chapter]\n  \"{1}\"\n", Name, Description);
            }

            for (int i = 0; i < Quests.Length; i++)
            {
                repr += string.Format("  {0} {1}\n", i+1, Quests[i].ToString());
            }
            return repr;
        }
    }
}
