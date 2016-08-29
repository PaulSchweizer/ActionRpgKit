using System;
using Character.Attribute;

namespace Character.Stats 
{
    public interface IStats
    {
        IAttribute Magic { get; set; }
    }
    
    public class PlayerStats : IStats
    {
        public IAttribute Soul;
        public IAttribute Experience;
        public IAttribute Level;
        private IAttribute _magic;
       
        public PlayerStats ()
        {
            // Primary Attributes
            Soul = new PrimaryAttribute("Soul", 0, 999, 0);
            Experience = new PrimaryAttribute("Experience");

            // Secondary Attributes
            Level = new SecondaryAttribute("Level",
                x => (int)(Math.Sqrt(x[0].Value / 100)) * 1f, 
                new IAttribute[] { Experience }, 0, 99);
            Magic = new VolumeAttribute("Magic", 
                x => (int)(20 + 5 * x[0].Value + x[1].Value / 3) * 1f, 
                new IAttribute[] { Level, Soul }, 0, 999);
        }
        
        public IAttribute Magic 
        {
            get 
            {
                return _magic;
            }
            set
            {
                _magic = value;
            }
        }
    }
    
    public class EnemyStats : IStats
    {
        private IAttribute _magic;
        
        public EnemyStats ()
        {
            Magic = new BaseAttribute("Magic", 0, 999, 0);
        }
        
        public IAttribute Magic 
        {
            get 
            {
                return _magic;
            }
            set
            {
                _magic = value;
            }
        }
    }
}
