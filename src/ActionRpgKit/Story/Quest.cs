using System;
using System.Collections.Generic;

namespace ActionRpgKit.Story.Quest
{
    // -----------------------------------------------------------------------
    // Interfaces
    // -----------------------------------------------------------------------

    /// <summary>
    /// Interface for Quests.
    /// </summary>
    public interface IQuest
    {
        string Name { get; set; }
        string Description { get; set; }
        int Experience { get; set; }
        List<IObjective> Objectives { get; }
        bool IsCompleted { get; set; }
        void CheckProgress ();
        void OnObjectiveCompletion(IObjective objective);
        void OnCompletion ();
    }

    public interface IObjective
    {
        string Name { get; }
        string Description { get; }
        bool IsCompleted { get; }
        void CheckProgress();
        void OnCompletion();
    }

    // -----------------------------------------------------------------------
    // Abstracts
    // -----------------------------------------------------------------------

    public abstract class Objective : IObjective
    {

        public Objective () { }

        public Objective (string name, string description)
        {
            Name = name;
            Description = description;
        }

        public override string ToString()
        {
            if (IsCompleted)
            {
                return string.Format("\u2611 {0} [Objective]\n        \"{1}\"", Name, Description);
            }
            else
            {
                return string.Format("\u2610 {0} [Objective]\n        \"{1}\"", Name, Description);
            }
        }

        // -------------------------------------------------------------------
        // IObjective implementations
        // -------------------------------------------------------------------

        public string Name
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public bool IsCompleted
        {
            get; set;
        }

        public abstract void CheckProgress();

        public abstract void OnCompletion();
    }

    // -----------------------------------------------------------------------
    // Implementations
    // -----------------------------------------------------------------------

    public abstract class Quest : IQuest
    {
        private List<IObjective> _objectives = new List<IObjective>();

        public Quest () { }

        public Quest (string name,
                          string description,
                          int experience)
        {
            Name = name;
            Description = description;
            Experience = experience;
        }

        public override string ToString()
        {
            string repr = "";
            if (IsCompleted)
            {
                repr += string.Format("\u2611 {0} [Quest]\n      \"{1}\"\n", Name, Description);
            }
            else
            {
                repr += string.Format("\u2610 {0} [Quest]\n      \"{1}\"\n", Name, Description);
            }

            for (int i = 0; i < Objectives.Count; i++)
            {
                repr += string.Format("    {0} {1}\n", i+1, Objectives[i].ToString());
            }
            return repr;
        }

        // -------------------------------------------------------------------
        // IQuest implementations
        // -------------------------------------------------------------------

        public string Name
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public int Experience
        {
            get; set;
        }

        public List<IObjective> Objectives
        {
            get
            {
                return _objectives;
            }
        }

        public bool IsCompleted
        {
            get; set;
        }

        public void CheckProgress ()
        {
            bool completed = true;
            for (int i = 0; i < Objectives.Count; i++)
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

        public abstract void OnObjectiveCompletion(IObjective objective);

        public abstract void OnCompletion();
    }
}
