using System;

namespace ActionRpgKit.Core
{
    /// <summary>
    /// A 2D position in world space.</summary>
    [Serializable]
    public class Position
    {
        /// <summary>
        /// The position.</summary>
        private float[] _position = new float[2];

        /// <summary>
        /// Set the position with initial values.</summary>
        /// <param name="x">The x value</param>
        /// <param name="y">The y value</param>
        public Position (float x = 0, float y = 0)
        {
            Set(x, y);
        }

        /// <summary>
        /// The x value.</summary>
        public float X
        {
            get
            {
                return _position[0];
            }
            set
            {
                _position[0] = value;
            }
        }

        /// <summary>
        /// The y value.</summary>
        public float Y
        {
            get
            {
                return _position[1];
            }
            set
            {
                _position[1] = value;
            }
        }

        /// <summary>
        /// Set the Position.</summary>
        /// <param name="x">The x value</param>
        /// <param name="y">The y value</param>
        public void Set (float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Get the position.</summary>
        /// <returns>The position</returns>
        public float[] Get ()
        {
            return _position;
        }

        /// <summary>
        /// The squared distance to the given Position.
        /// The squared distance avoids the root calculation.</summary>
        /// <param name="other">The other position.</param>
        /// <returns>The distance</returns>
        public float SquaredDistanceTo(Position other)
        {
            return (other.X - this.X) * (other.X - this.X) + 
                   (other.Y - this.Y) * (other.Y - this.Y);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", X, Y);
        }
    }
}
