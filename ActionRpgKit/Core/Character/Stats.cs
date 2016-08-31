using System;
using Character.Attribute;
using System.Collections.Generic;

namespace Character.Stats 
{
    public abstract class BaseStats : Dictionary<string, IAttribute>
    {
        public IAttribute Magic;
        public IAttribute Body;
        public IAttribute Mind;
        public IAttribute Soul;
        public IAttribute Experience;
        public IAttribute Level;
        public IAttribute MagicRegenerationRate;
        
        public BaseStats ()
        {
            System.Attribute[] attrs = System.Attribute.GetCustomAttributes(this); 
            Add("Body", Body);
            Add("Mind", Mind);
            Add("Soul", Soul);
            Add("Experience", Experience);
            Add("Level", Level);
            Add("MagicRegenerationRate", MagicRegenerationRate);
            Add("Magic", Magic);
        }
    }
    
    public class PlayerStats : BaseStats
    {
        public PlayerStats () : base()
        {
            // Primary Attributes
            Body = new PrimaryAttribute("Body", 0, 999, 0);
            Mind = new PrimaryAttribute("Mind", 0, 999, 0);
            Soul = new PrimaryAttribute("Soul", 0, 999, 0);
            Experience = new PrimaryAttribute("Experience");

            // Secondary Attributes
            Level = new SecondaryAttribute("Level",
                    x => (int)(Math.Sqrt(x[0].Value / 100)) * 1f, 
                    new IAttribute[] { Experience }, 0, 99);
            MagicRegenerationRate = new SecondaryAttribute(
                                    "MagicRegenerationRate",
                                    x => 1 + (x[0].Value / 1000),
                                    new IAttribute[] { Mind }, 0, 99);
  
            // Volume Attributes
            Magic = new VolumeAttribute("Magic", 
                    x => (int)(20 + 5 * x[0].Value + x[1].Value / 3) * 1f, 
                    new IAttribute[] { Level, Soul }, 0, 999);
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
            MagicRegenerationRate = new PrimaryAttribute("MagicRegenerationRate");
            Magic = new PrimaryAttribute("Magic", 0, 999, 0);
        }
    }
}
