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
}
