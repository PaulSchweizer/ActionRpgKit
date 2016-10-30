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

                    CharacterInAlertnessRange(Player, Enemies[i], distance);
                    CharacterInAlertnessRange(Enemies[i], Player, distance);

                    Enemies[i].Update();
                }
                Player.Update();
            }
        }

        private static void CharacterInAlertnessRange(BaseCharacter origin, 
                                                      BaseCharacter other, 
                                                      float distance)
        {
            if (distance <= origin.Stats.AlertnessRange.Value)
            {
                origin.AddEnemy(other);
                CharacterInAttackRange(origin, other, distance);
            }
            else
            {
                origin.RemoveEnemy(other);
                CharacterInAttackRange(origin, other, distance);
                //if (origin.EnemiesInAttackRange.Contains(other))
                //{
                //    origin.EnemiesInAttackRange.Remove(other);
                //}
            }
        }

        private static void CharacterInAttackRange(BaseCharacter origin,
                                                   BaseCharacter other,
                                                   float distance)
        {
            float range = origin.Stats.AttackRange.Value;
            if (origin.EquippedWeapon > -1)
                range += ItemDatabase.GetWeaponItemById(origin.EquippedWeapon).Range;
            {
            }
            if(distance <= range)
            {
                if (!origin.EnemiesInAttackRange.Contains(other))
                {
                    origin.EnemiesInAttackRange.Add(other);
                }
            }
            else
            {
                if (origin.EnemiesInAttackRange.Contains(other))
                {
                    origin.EnemiesInAttackRange.Remove(other);
                }
            }
        }
    }
}





//public bool EnemyInAttackRange(IFighter enemy)
//{
//    float range = Stats.AttackRange.Value;
//    if (EquippedWeapon > -1)
//    {
//        range += ItemDatabase.GetWeaponItemById(EquippedWeapon).Range;
//    }
//    return Position.SquaredDistanceTo(enemy.Position) <= range;
//}

//public IFighter[] EnemiesInAttackRange
//{
//    get
//    {
//        var enemiesInAttackRange = new List<IFighter>();
//        for (int i = 0; i < Enemies.Count; i++)
//        {
//            if (EnemyInAttackRange(Enemies[i]))
//            {
//                enemiesInAttackRange.Add(Enemies[i]);
//            }
//        }
//        return enemiesInAttackRange.ToArray();
//    }
//}