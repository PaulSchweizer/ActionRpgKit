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
        string EnergyAttributeName { get; }
        IModifier Modifier { get; }
        float PreUseTime { get; }
        float CooldownTime { get; }

        bool Match();
        void Use(ICharacter user);
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
        private float _duration;
        private float _preUseTime;
        private float _cooldownTime;
        private float _endTime = -1;
        private float _modifierValue;
        private string _modifiedAttributeName;
        private string _energyAttributeName;

        public PassiveSkill(string name)
        {
            _name = name;
        }

        public PassiveSkill(int id,
                            string name,
                            string description,
                            float cost,
                            float duration,
                            float preUseTime,
                            float cooldownTime,
                            float modifierValue,
                            string modifiedAttributeName,
                            string energyAttributeName)
        {
            _id = id;
            _name = name;
            _description = description;
            _cost = cost;
            _duration = duration;
            _preUseTime = preUseTime;
            _cooldownTime = cooldownTime;
            _modifierValue = modifierValue;
            _modifiedAttributeName = modifiedAttributeName;
            _energyAttributeName = energyAttributeName;
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

        public float EnergyAttributeName
        {
            get
            {
                retuen _energyAttributeName;
            }
        }

        public IModifier Modifier 
        { 
            get 
            {
                return new TimeBasedModifier(Name, _modifierValue, _duration);
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
             user.Stats[_modifiedAttributeName].AddModifier(Modifier);
        }
    }
}
