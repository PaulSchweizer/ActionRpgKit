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
        public int Size { get; set; }

        /// <summary>
        /// The internal List of Enemies.</summary>
        private List<Enemy> Enemies { get; set; }

        /// <summary>
        /// Initialize the EnemyPool to the given size.</summary>
        /// <param name="size">The size of the pool.</param>
        public EnemyPool (int size=10)
        {
            Enemies = new List<Enemy>();
            Size = size;
            for(int i=0; i < Size; i++)
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
        public void Release(Enemy enemy)
        {
            Enemies.Add(enemy);
        }
    }
}
