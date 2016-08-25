using System;
using System.Collections.Generic;

namespace Character
{
    /// <summary>
    /// The Stats describe a Character.
    /// </summary>
    public interface IStats
    {
        // Primary Attributes
        IAttribute Body {get; set; }
        IAttribute Mind {get; set; }
        IAttribute Soul {get; set; }
        IAttribute Experience {get; set; }

        // Secondary Attributes
        IAttribute Level {get; set; }
        IAttribute Life {get; set; }
        IAttribute Magic {get; set; }
    }

    /// <summary>
    /// These Stats describe an Enemy Character.
    /// </summary>
    public class EnemyStats : IStats
    {
        // Primary Attributes
        public Attribute Body;
        public Attribute Mind;
        public Attribute Soul;
        public Attribute Experience;

        // Secondary Attributes
        public IAttribute Level;
        public IAttribute Life;
        public IAttribute Magic;

        /// <summary>
        /// Initialize the Stats with default primary attribute values.
        /// </summary>
        public EnemyStats (float body = 0, float mind = 0, float soul = 0)
        {
            // Primary Attributes
            Body = new Attribute("Body", 0, 999, body);
            Mind = new Attribute("Mind", 0, 999, mind);
            Soul = new Attribute("Soul", 0, 999, soul);
            Experience = new Attribute("Experience");

            // Secondary Attributes
        }
    }
}
