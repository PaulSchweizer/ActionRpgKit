using System;
using NUnit.Framework;

namespace CharacterTests
{

    [TestFixture]
    [Category("Character.Stats")]
    public class CharacterStatsTests
    {

        [SetUp]
        public void SetUp ()                                                  
        {
            GameTime.time = 0f;
        }
    }
}
