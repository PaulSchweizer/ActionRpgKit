using System.Collections.Generic;
using System.Linq;
using ActionRpgKit.Character.Stats;
using ActionRpgKit.Item;

namespace ActionRpgKit.Character
{
    /// <summary>
    /// Holds all available enemy types.
    /// Allows to retrieve an enemy.</summary>
    public static class EnemyFactory
    {
        
        /// <summary>
        /// The Stats ordered by enemy type</summary>
        private static Dictionary<string, EnemyStats> Stats = new Dictionary<string, EnemyStats>();
        
        /// <summary>
        /// The Items based on the enemy type</summary>
        private static Dictionary<string, BaseInventory> Inventories = new Dictionary<string, BaseInventory>();

        /// <summary>
        /// Add a new enemy type to the factory.</summary>
        /// <param name="type">The Enemy type.</param>
        /// <param name="stats">The Enemy Stats.</param>
        /// <param name="items">The Items in the Enemies Inventory.</param>
        public static void AddEnemyType(string type, 
                                        EnemyStats stats, 
                                        BaseInventory inventory)
        {
            Stats[type] = stats;
            Inventories[type] = inventory;
        }

        /// <summary>
        /// Get an enemy of a certain type</summary>
        public static Enemy GetEnemyByType (string type)
        {
            var enemy = EnemyPool.Acquire();
            enemy.Stats.Set(Stats[type]);
            enemy.Inventory.Items = Inventories[type].Items;
            enemy.Inventory.Quantities = Inventories[type].Quantities;
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
