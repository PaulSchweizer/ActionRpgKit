using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActionRpgKit.Item;

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
        /// Calculate the distance between the Enemies and the Player.
        /// Compare the altertness ranges and attack ranges.
        /// Update all the Characters</summary>
        public static void Update()
        {
            if (Player != null)
            {
                for(int i=0; i < Enemies.Count; i++)
                {
                    if (Enemies[i].IsDefeated)
                    {
                        Player.RemoveEnemy(Enemies[i]);
                        continue;
                    }
                    var distance = Player.Position.SquaredDistanceTo(Enemies[i].Position);

                    CharacterInAlertnessRange(Player, Enemies[i], distance);
                    CharacterInAlertnessRange(Enemies[i], Player, distance);

                    CharacterInAttackRange(Player, Enemies[i], distance);
                    CharacterInAttackRange(Enemies[i], Player, distance);

                    Enemies[i].Update();
                }
                Player.Update();
            }
        }

        /// <summary>
        /// Compare the distance of the two Characters to the alterness range. </summary>
        /// <param name="source">The source Character for the testing.</param>
        /// <param name="target">The targeted Character.</param>
        /// <param name="distance">The squared distance between them.</param>
        private static void CharacterInAlertnessRange(BaseCharacter source, 
                                                      BaseCharacter target, 
                                                      float distance)
        {
            if (distance <= source.Stats.AlertnessRange.Value)
            {
                source.AddEnemy(target);
            }
            else
            {
                source.RemoveEnemy(target);
            }
        }

        /// <summary>
        /// Compare the distance of the two Characters to the attack range. </summary>
        /// <param name="source">The source Character for the testing.</param>
        /// <param name="target">The targeted Character.</param>
        /// <param name="distance">The squared distance between them.</param>
        private static void CharacterInAttackRange(BaseCharacter source,
                                                   BaseCharacter target,
                                                   float distance)
        {
            if(distance <= source.AttackRange)
            {
                if (!source.EnemiesInAttackRange.Contains(target))
                {
                    source.EnemiesInAttackRange.Add(target);
                }
            }
            else
            {
                if (source.EnemiesInAttackRange.Contains(target))
                {
                    source.EnemiesInAttackRange.Remove(target);
                }
            }
        }
    }
}