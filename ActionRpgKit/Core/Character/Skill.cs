using System;
using System.Collections;
using Character;
using Character.Attribute;

namespace Character.Skill
{
    /// <summary>
    /// Interface for Skills.
    /// </summary>
    public interface ISkill
    {
        int Id { get; }
        string Name { get; }
        string Description { get; }
        float Cost { get; }
        float PreUseTime { get; }
        float CooldownTime { get; }

        bool Match();
        void UseSkill();
    }

    /// <summary>
    /// A passive Skill acts on the User itself.
    /// </summary>
    public class PassiveSkill : ISkill
    {
        private int _id;
        private string _name;
        private string _description;
        private float _cost;
        private float _preUseTime;
        private float _cooldownTime;
        private float _endTime = -1;
        private float modifierValue;
        private string modifiedAttributeName;

        public PassiveSkill(string name)
        {
            _name = name;
        }

        public PassiveSkill(string name,
                             string description,
                             float cost,
                             float preUseTime,
                             float cooldownTime)
        {
            _name = name;
            _description = description;
            _cost = cost;
            _preUseTime = preUseTime;
            _cooldownTime = cooldownTime;
        }

        public PassiveSkill(int id,
                            string name,
                            string description,
                            float cost,
                            float preUseTime,
                            float cooldownTime)
        {
            _id = id;
            _name = name;
            _description = description;
            _cost = cost;
            _preUseTime = preUseTime;
            _cooldownTime = cooldownTime;
        }

        public int Id
        {
            get
            {
                return _id;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }

        public float Cost
        {
            get
            {
                return _cost;
            }
        }

        public float CooldownTime
        {
            get
            {
                return _cooldownTime;
            }
        }

        public float PreUseTime
        {
            get
            {
                return _preUseTime;
            }
        }

        public bool Match()
        {
            throw new NotImplementedException();
        }

        public void Use(ICharacter user)
        {
            Modifier modifier = new Modifier(Name, ); 
            throw new NotImplementedException();
        }
    }
}
