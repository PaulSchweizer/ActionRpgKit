using System.Collections.Generic;
using System.Linq;
using ActionRpgKit.Character.Stats;
using ActionRpgKit.Item;

namespace ActionRpgKit.Character
{
    /// <summary>
    /// Holds all available enemy types.
    /// Allows to retrieve an enemy.</summary>
    public class EnemyFactory
    {
        /// <summary>
        /// The internal List of Enemy types.</summary>
        private Enemy[] Enemies { get; set; }
        
        /// <summary>
        /// The Stats ordered by enemy type</summary>
        private Dictionary<string, EnemyStats> EnemyStats = new Dictionary<string, EnemyStats>();
        
        /// <summary>
        /// The Items based on the enemy type</summary>
        private Dictionary<string, IItem[]> Items = new Dictionary<string, IItem[]>();

        /// <summary>
        /// The Stats ordered by enemy type</summary>
        public Enemy GetEnemyByType (string enemyType)
        {
            var enemy = EnemyPool.Acquire();
            enemy.Stats = EnemyStats[enemyType];
            return enemy;
        }
    }

    /// <summary>
    /// Holds available Enemies.</summary>
    public static class EnemyPool
    {
        /// <summary>
        /// The size of the EnemyPool.</summary>
        public static int Size { get; set; }

        /// <summary>
        /// The internal List of Enemies.</summary>
        private static List<Enemy> Enemies { get; set; }

        /// <summary>
        /// Initialize the EnemyPool to the given size.</summary>
        /// <param name="size">The size of the pool.</param>
        public static void Initialize(int size = 10)
        {
            Enemies = new List<Enemy>();
            Size = size;
            for (int i = 0; i < Size; i++)
            {
                Enemies.Add(new Enemy());
            }
        }

        /// <summary>
        /// Retrieve an Enemy from the EnemyPool. 
        /// Create a new Enemy and increase the size, if no Enemy is available.</summary>
        /// <returns>An Enemy.</returns>
        public static Enemy Acquire()
        {
            if (Enemies.Count() > 0)
            {
                var enemy = Enemies[0];
                Enemies.RemoveAt(0);
                return enemy;
            }
            else
            {
                Size += 1;
                return new Enemy();
            }
        }

        /// <summary>
        /// Add the Enemy back to the EnemyPool.</summary>
        /// <param name="enemy">The Enemy</param>
        public static void Release(Enemy enemy)
        {
            Enemies.Add(enemy);
        }
    }
}
