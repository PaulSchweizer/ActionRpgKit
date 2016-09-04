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

    public abstract class BaseObjective : IObjective
    {
        private string _name;
        private string _description;

        public BaseObjective () { }

        public BaseObjective (string name, string description)
        {
            _name = name;
            _description = description;
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
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

    public abstract class BaseQuest : IQuest
    {
        private string _name;
        private string _description;
        private int _experience;
        private List<IObjective> _objectives = new List<IObjective>();

        public BaseQuest () { }

        public BaseQuest (string name,
                          string description,
                          int experience)
        {
            _name = name;
            _description = description;
            _experience = experience;
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public int Experience
        {
            get
            {
                return _experience;
            }
            set
            {
                _experience = value;
            }
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
