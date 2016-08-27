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
        void UseSkill (ISkill skill);
    }

    public class Character : ICharacter
    {
        private string _name;
        private List<ISkill> _skills = new List<ISkill>();

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

        public void LearnSkill(ISkill skill)
        {
            _skills.Add(skill);
        }

        public void UseSkill(ISkill skill)
        {
            throw new NotImplementedException();
        }
    }

    public class Player : Character
    {
        public Player(string name) : base(name)
        {   
        }
    }

}
