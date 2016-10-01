using System;
using System.Collections.Generic;

namespace ActionRpgKit.Story.Quest
{

    #region Interfaces

    /// <summary>
    /// Interface for Quests.</summary>
    public interface IQuest
    {
        string Name { get; set; }
        string Description { get; set; }
        int Experience { get; set; }
        IObjective[] Objectives { get; set; }
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
    
    #endregion
    
    #region Abstracts

    public abstract class Quest : IQuest
    {

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

            for (int i = 0; i < Objectives.Length; i++)
            {
                repr += string.Format("    {0} {1}\n", i+1, Objectives[i].ToString());
            }
            return repr;
        }

        // -------------------------------------------------------------------
        // IQuest implementations
        // -------------------------------------------------------------------

        public string Name { get; set; }

        public string Description { get; set; }

        public int Experience { get; set; }

        public IObjective[] Objectives { get; set; }

        public bool IsCompleted { get; set; }

        public void CheckProgress ()
        {
            bool completed = true;
            for (int i = 0; i < Objectives.Length; i++)
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
    
    public abstract class Objective : IObjective
    {

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

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public abstract void CheckProgress();

        public abstract void OnCompletion();
    }
    
    #endregion
}
