using System;
using NUnit.Framework;
using ActionRpgKit.Core;

namespace ActionRpgKit.Tests.Core
{
    [TestFixture]
    [Category("Core")]
    public class GameObjectTests
    {
        GameObject gameObject;
        
        [SetUp]
        public void SetUp()
        {
            gameObject = new GameObject();
        }
    }
 }
