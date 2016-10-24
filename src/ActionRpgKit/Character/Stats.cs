using System;
using System.Collections.Generic;
using ActionRpgKit.Character.Attribute;
using System.Runtime.Serialization;

namespace ActionRpgKit.Character.Stats 
{
    #region Abstracts

    /// <summary>
    /// Basic stats each Character has to have.</summary>
    [Serializable]
    public abstract class BaseStats
    {
        public BaseAttribute Body;
        public BaseAttribute Mind;
        public BaseAttribute Soul;
        public BaseAttribute Experience;
        public BaseAttribute Level;
        public BaseAttribute Life;
        public BaseAttribute Magic;
        public BaseAttribute MagicRegenerationRate;
        public BaseAttribute AlertnessRange;
        public BaseAttribute AttackRange;

        public Dictionary<string, BaseAttribute> Dict = new Dictionary<string, BaseAttribute>();

        protected void AssignAttributesToDict()
        {
            Dict.Add("Body", Body);
            Dict.Add("Mind", Mind);
            Dict.Add("Soul", Soul);
            Dict.Add("Experience", Experience);
            Dict.Add("Level", Level);
            Dict.Add("Life", Life);
            Dict.Add("MagicRegenerationRate", MagicRegenerationRate);
            Dict.Add("Magic", Magic);
            Dict.Add("AlertnessRange", AlertnessRange);
            Dict.Add("AttackRange", AttackRange);
        }

        public abstract void Set(BaseStats stats);

        public override string ToString()
        {
            string repr = string.Format("--- Primary Attributes ------------\n" +
                                         "{0}\n{1}\n{2}\n{3}\n" +
                                         "--- Secondary Attributes ------------\n" +
                                         "{4}\n{5}\n{6}\n{7}\n{8}\n{9}\n",
                                         Body.ToString(),
                                         Mind.ToString(),
                                         Soul.ToString(),
                                         Experience.ToString(),
                                         Level.ToString(),
                                         Life.ToString(),
                                         Magic.ToString(),
                                         MagicRegenerationRate.ToString(),
                                         AlertnessRange.ToString(),
                                         AttackRange.ToString());
            return repr;
        }
    }

    #endregion

    #region Implementations

    [Serializable]
    public class PlayerStats : BaseStats
    {

        public PlayerStats()
        {
            // Primary Attributes
            Body = new PrimaryAttribute("Body", 0, 999, 0);
            Mind = new PrimaryAttribute("Mind", 0, 999, 0);
            Soul = new PrimaryAttribute("Soul", 0, 999, 0);
            Experience = new PrimaryAttribute("Experience", 0, 980100, 0);
            AlertnessRange = new PrimaryAttribute("AlertnessRange", 0, 999, 1);
            AttackRange = new PrimaryAttribute("AttackRange", 0, 999, 1);

            // Secondary Attributes
            Level = new SecondaryAttribute("Level",
                    x => (int)(Math.Sqrt(x[0].Value / 100)) * 1f, 
                    new BaseAttribute[] { Experience }, 0, 99);
            MagicRegenerationRate = new SecondaryAttribute(
                                    "MagicRegenerationRate",
                                    x => 1 + (x[0].Value / 1000),
                                    new BaseAttribute[] { Mind }, 0, 99);

            // Volume Attributes
            Life = new VolumeAttribute("Life", 
                   x => (int)(20 + 5 * x[0].Value + x[1].Value / 3) * 1f, 
                   new BaseAttribute[] { Level, Body }, 0, 999);
            Magic =new VolumeAttribute("Magic", 
                    x => (int)(20 + 5 * x[0].Value + x[1].Value / 3) * 1f, 
                    new BaseAttribute[] { Level, Soul }, 0, 999);
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

    [Serializable]
    public class EnemyStats : BaseStats
    {
        public EnemyStats()
        {
            Body = new PrimaryAttribute("Body", 0, 999, 0);
            Mind = new PrimaryAttribute("Mind", 0, 999, 0);
            Soul = new PrimaryAttribute("Soul", 0, 999, 0);
            Experience = new PrimaryAttribute("Experience", 0, 980100, 0);
            Level = new PrimaryAttribute("Level", 0, 99, 0);
            Life = new PrimaryAttribute("Life", 0, 999, 0);
            MagicRegenerationRate = new PrimaryAttribute("MagicRegenerationRate", 0, 2, 0);
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
