using System;
using System.Collections.Generic;
using Character.Skill;

namespace Character
{
    public interface ICharacter
    {
        string Name { get; }
        List<ISkill> Skills { get; }
        void LearnSkill(ISkill skill);
        bool TriggerSkill(ISkill skill);
    }

    public class Character : ICharacter
    {
        private string _name;
        private List<ISkill> _skills = new List<ISkill>();
        private List<float> _skillEndTimes = new List<float>();

        public Character(string name)
        {
            _name = name;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public List<ISkill> Skills
        {
            get
            {
                return _skills;
            }
        }

        public void LearnSkill (ISkill skill)
        {
            _skills.Add(skill);
            _skillEndTimes.Add(-1);
        }

        public bool TriggerSkill (ISkill skill)
        {
            if (!SkillCanBeUsed(skill))
            {
                return false;
            }
            _skillEndTimes[Skills.IndexOf(skill)] = GameTime.time + skill.CooldownTime;
            return true;
        }

        private bool SkillCanBeUsed (ISkill skill)
        {
            return GameTime.time > _skillEndTimes[Skills.IndexOf(skill)];
        }
    }

    public class Player : Character
    {
        public Player(string name) : base(name)
        {   
        }
    }

}
