using System.Collections.Generic;
using System.Linq;
using ActionRpgKit.Character.Stats;
using ActionRpgKit.Item;

namespace ActionRpgKit.Character
{
    /// <summary>
    /// Holds all available enemy types.
    //// Allows to retrieve an enemy.</summary>
    public class EnemyDatabase
    {
        /// <summary>
        /// The internal List of Enemy types.</summary>
        private Enemy[] Enemies { get; set; }
        
        /// <summary>
        /// The Stats ordered by enemy type</summary>
        private Dictionary<string, EnemyStats> EnemyStats = new Dictionary<string, EnemyStats>();
        
        /// <summary>
        /// The Items based on the enemy type</summary>
        private Dictionary<string, Item[]> Items = new Dictionary<string, Item[]>();

        /// <summary>
        /// The Stats ordered by enemy type</summary>
        public Enemy GetEnemyByType (string enemyType)
        {
            var enemy = EnemyPool.Acquire();
            enemy.Stats = EnemyStats[string];
        }
    }
}
