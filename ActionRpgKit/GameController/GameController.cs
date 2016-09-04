using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionRpgKit.Character;

namespace ActionRpgKit
{
    public class GameController
    {
        Player player;

        public void CreatePlayerController()
        {
            player = new Player();
        }
    }
}
