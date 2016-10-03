using System.Collections.Generic;
using System.Linq;

namespace ActionRpgKit.Character
{
    /// <summary>
    /// Holds available Enemies.</summary>
    public class EnemyPool
    {
        /// <summary>
        /// The size of the EnemyPool.</summary>
        public int size;

        /// <summary>
        /// The internal List of Enemies.</summary>
        List<Enemy> Enemies { get; }

        /// <summary>
        /// Initialize the EnemyPool to the given size.</summary>
        /// <param name="size">The size of the pool.</param>
        public EnemyPool (int size=10)
        {
            size = 10;
            for(int i=0; i < size; i++)
            {
                Enemies.Add(new Enemy());
            }
        }

        /// <summary>
        /// Retrieve an Enemy from the EnemyPool. 
        /// Create a new Enemy and increase the size, if no Enemy is available.</summary>
        /// <returns>An Enemy.</returns>
        public Enemy Acquire()
        {
            if (Enemies.Count() > 0)
            {
                var enemy = Enemies[0];
                Enemies.Remove(enemy);
                return enemy;
            }
            else
            {
                size += 1;
                return new Enemy();
            }
        }

        /// <summary>
        /// Add the Enemy back to the EnemyPool.</summary>
        /// <param name="enemy">The Enemy</param>
        public void Release(Enemy enemy)
        {
            Enemies.Add(enemy);
        }
    }
}
