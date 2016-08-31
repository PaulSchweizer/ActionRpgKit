using System;
using System.Collections.Generic;
using Character.Skill;
using Character.Stats;

namespace Character
{
    public interface ICharacter
    {
        string Name { get; }
        BaseStats Stats { get; set;}
        List<ISkill> Skills { get; }
        void LearnSkill (ISkill skill);
        bool TriggerSkill (ISkill skill);
    }

    public class BaseCharacter : ICharacter
    {
        private string _name;
        private BaseStats _stats;
        private List<ISkill> _skills = new List<ISkill>();
        private List<float> _skillEndTimes = new List<float>();

        public BaseCharacter (string name)
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

        public BaseStats Stats
        {
            get
            {
                return _stats;
            }
            set
            {
                _stats = value;
            }
        }

        public List<ISkill> Skills
        {
            get
            {
                return _skills;
            }
        }

        private bool SkillCanBeUsed(ISkill skill)
        {
            if (!Skills.Contains(skill))
            {
                return false;
            }
            if (Stats.Magic.Value < skill.Cost)
            {
                return false;
            }
            return GameTime.time >= _skillEndTimes[Skills.IndexOf(skill)];
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
            Stats.Magic.Value -= skill.Cost;
            _skillEndTimes[Skills.IndexOf(skill)] = GameTime.time + skill.CooldownTime;
            PreUseCountdown(skill);
            return true;
        }

        public virtual void PreUseCountdown(ISkill skill)
        {
            //
            // Implement a Coroutine in Monobehaviour
            //
            UseSkill(skill);
        }

        private void UseSkill(ISkill skill)
        {
            skill.Use(this);
        }
    }

    public class Player : BaseCharacter
    {
        public Player(string name) : base(name)
        {   
            Stats = new PlayerStats();
        }
    }

    public class Enemy : BaseCharacter
    {
        public Enemy(string name) : base(name)
        {   
            Stats = new EnemyStats();
        }
    }

}
