using System;
using ActionRpgKit.Core;

namespace ActionRpgKit.Character
{

    /// <summary>
    /// A projectile moving at certain speed in a certain direction.
    /// On hitting, the projectile is being destroyed and damage is applied to the target.</summary>
    public class Projectile : IGameObject
    {
        /// <summary>
        /// The current Position of the Projectile.<summary>
        public Position Position { get; set; }

        /// <summary>
        /// The Direction of the Projectile.<summary>
        private float Direction
        
        /// <summary>
        /// The speed in meter/seconds.<summary>
        public float Speed;
        
        /// <summary>
        /// The lifetime of the Projectile in seconds.<summary>
        public float LifeTime;
        
        /// <summary>
        /// The damage carried by the Projectile.<summary>
        public float Damage;
        
        public Projectile(Position origin, Vector2D direction)
        {
            Position = origin;
            Direction = direction
        }
        
        public void Update()
        {
            if (Position.SquaredDistanceTo(Destination) <= threshold)
            {
                OnDestinationReached();
            }
            Position = Position + Direction * GameTime.deltaTime * Speed;
        }
        
        public void OnDestinationReached ()
        {
            // Emit a signal that the destination has been reached
            // Signal will remove the object and 
        }
        
        public void OnCharacterHit (BaseCharacter character)
        {
        
        }
    }
}
