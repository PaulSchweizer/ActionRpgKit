using System;
using ActionRpgKit.Core;

namespace ActionRpgKit.Character
{

    /// <summary>
    /// A projectile moving at certain speed in a certain direction.
    /// On hitting, the projectile is being destroyed and damage is applied to the target.</summary>
    public class Projectile : IGameObject
    {
        public float Speed;
        public Position Destination;
        public float Damage;
        public Position Position { get; set; }
        
        public void OnDestinationReached ()
        {
        
        }
        
        public void OnCharacterHit (BaseCharacter character)
        {
        
        }
    }
}
