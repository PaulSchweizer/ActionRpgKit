using System;
using System.Collections.Generic;
using ActionRpgKit.Character.Attribute;

namespace ActionRpgKit.Character.Stats 
{
    #region Abstracts

    /// <summary>
    /// Basic stats each Character has to have.</summary>
    public abstract class BaseStats : Dictionary<string, IAttribute>
    {
        public IAttribute Body;
        public IAttribute Mind;
        public IAttribute Soul;
        public IAttribute Experience;
        public IAttribute Level;
        public IAttribute Life;
        public IAttribute Magic;
        public IAttribute MagicRegenerationRate;
        public IAttribute AlertnessRange;
        public IAttribute AttackRange;

        protected void AssignAttributesToDict ()
        {
            Add("Body", Body);
            Add("Mind", Mind);
            Add("Soul", Soul);
            Add("Experience", Experience);
            Add("Level", Level);
            Add("Life", Life);
            Add("MagicRegenerationRate", MagicRegenerationRate);
            Add("Magic", Magic);
            Add("AlertnessRange", AlertnessRange);
            Add("AttackRange", AttackRange);
        }

        public abstract void Set(BaseStats stats);
    }

    #endregion

    #region Implementations

    public class PlayerStats : BaseStats
    {
        public PlayerStats () : base()
        {
            // Primary Attributes
            Body = new PrimaryAttribute("Body", 0, 999, 0);
            Mind = new PrimaryAttribute("Mind", 0, 999, 0);
            Soul = new PrimaryAttribute("Soul", 0, 999, 0);
            Experience = new PrimaryAttribute("Experience");
            AlertnessRange = new PrimaryAttribute("AlertnessRange", 0, 999, 1);
            AttackRange = new PrimaryAttribute("AttackRange", 0, 999, 1);

            // Secondary Attributes
            Level = new SecondaryAttribute("Level",
                    x => (int)(Math.Sqrt(x[0].Value / 100)) * 1f, 
                    new IAttribute[] { Experience }, 0, 99);
            MagicRegenerationRate = new SecondaryAttribute(
                                    "MagicRegenerationRate",
                                    x => 1 + (x[0].Value / 1000),
                                    new IAttribute[] { Mind }, 0, 99);

            // Volume Attributes
            Life = new VolumeAttribute("Life", 
                   x => (int)(20 + 5 * x[0].Value + x[1].Value / 3) * 1f, 
                   new IAttribute[] { Level, Body }, 0, 999);
            Magic =new VolumeAttribute("Magic", 
                    x => (int)(20 + 5 * x[0].Value + x[1].Value / 3) * 1f, 
                    new IAttribute[] { Level, Soul }, 0, 999);
            AssignAttributesToDict();
        }

        public override void Set(BaseStats stats)
        {
            Body.Value = stats.Body.Value;
            Mind.Value = stats.Mind.Value;
            Soul.Value = stats.Soul.Value;
            Experience.Value = stats.Experience.Value;
            Life.Value = stats.Life.Value;
            Magic.Value = stats.Magic.Value;
            AlertnessRange.Value = stats.AlertnessRange.Value;
            AttackRange.Value = stats.AttackRange.Value;
        }
    }
    
    public class EnemyStats : BaseStats
    {
        public EnemyStats () : base()
        {
            Body = new PrimaryAttribute("Body", 0, 999, 0);
            Mind = new PrimaryAttribute("Mind", 0, 999, 0);
            Soul = new PrimaryAttribute("Soul", 0, 999, 0);
            Experience = new PrimaryAttribute("Experience");
            Level = new PrimaryAttribute("Level");
            Life = new PrimaryAttribute("Life", 0, 999, 0);
            MagicRegenerationRate = new PrimaryAttribute("MagicRegenerationRate");
            Magic = new PrimaryAttribute("Magic", 0, 999, 0);
            AlertnessRange = new PrimaryAttribute("AlertnessRange", 0, 999, 1);
            AttackRange = new PrimaryAttribute("AttackRange", 0, 999, 1);
            AssignAttributesToDict();
        }

        public override void Set(BaseStats stats)
        {
            Body.Value = stats.Body.Value; 
            Mind.Value = stats.Mind.Value; 
            Soul.Value = stats.Soul.Value; 
            Experience.Value = stats.Experience.Value; 
            Level.Value = stats.Level.Value; 
            Life.Value = stats.Life.Value; 
            MagicRegenerationRate.Value = stats.MagicRegenerationRate.Value; 
            Magic.Value = stats.Magic.Value; 
            AlertnessRange.Value = stats.AlertnessRange.Value; 
            AttackRange.Value = stats.AttackRange.Value; 
        }
    }

    #endregion
}
