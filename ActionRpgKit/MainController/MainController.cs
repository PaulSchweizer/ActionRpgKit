using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionRpgKit.Character;

namespace ActionRpgKit
{
    public sealed class MainController
    {
        /// <summary>
        /// Singleton instance.</summary>
        public static readonly MainController Instance = new MainController();

        public static Player player;

        static MainController()
        {
        }

        private MainController()
        {
        }

        public static void CreatePlayer()
        {
            player = new Player();
        }
    }
}
