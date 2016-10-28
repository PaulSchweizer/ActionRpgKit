using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActionRpgKit.Character
{
    /// <summary>
    /// Controller for the interaction between the Player and the Enemies.</summary>
    public static class Controller
    {
        /// <summary>
        /// Reference to the Player.</summary>
        public static Player Player;

        /// <summary>
        /// Reference to the Enemies.</summary>
        public static List<Enemy> Enemies = new List<Enemy>();

        /// <summary>
        /// Register the Player.</summary>
        public static void Register(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// Register a new enemy.</summary>
        public static void Register(Enemy enemy)
        {
            if (!Enemies.Contains(enemy))
            {
                Enemies.Add(enemy);
            }
        }

        /// <summary>
        /// Calculate the distance between the Enemies and the Player.</summary>
        public static void Update()
        {
            if (Player != null)
            {
                for(int i=0; i < Enemies.Count; i++)
                {
                    if (Enemies[i].IsDead)
                    {
                        Player.RemoveEnemy(Enemies[i]);
                        continue;
                    }
                    var distance = Player.Position.SquaredDistanceTo(Enemies[i].Position);
                    if (distance <= Player.Stats.AlertnessRange.Value)
                    {
                        Player.AddEnemy(Enemies[i]);
                    }
                    else
                    {
                        Player.RemoveEnemy(Enemies[i]);
                    }
                    if (distance <= Enemies[i].Stats.AlertnessRange.Value)
                    {
                        Enemies[i].AddEnemy(Player);
                    }
                    else
                    {
                        Enemies[i].RemoveEnemy(Player);
                    }
                    Enemies[i].Update();
                }
                Player.Update();
            }
        }
    }
}
