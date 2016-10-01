using System;

namespace ActionRpgKit.Core
{
    /// <summary>
    /// A position in world space.</summary>
    public class Position
    {
        /// <summary>
        /// The position.</summary>
        private float[] _position = new float[3];

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
        /// The z value.</summary>
        public float Z
        {
            get
            {
                return _position[2];
            }
            set
            {
                _position[2] = value;
            }
        }

        public Position (float x = 0, float y = 0, float z = 0)
        {
            Set(x, y, z);
        }

        /// <summary>
        /// Set the Position.</summary>
        /// <param name="x">The x value</param>
        /// <param name="y">The y value</param>
        /// <param name="z">The z value</param>
        public void Set (float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Get the position.</summary>
        /// <returns>The position</returns>
        public float[] Get ()
        {
            return _position;
        }

        /// <summary>
        /// The distance to the given Position.</summary>
        /// <param name="position">The other position.</param>
        /// <returns>The distance</returns>
        public float DistanceTo(Position position)
        {
            return (float)Math.Sqrt((position.X - this.X) * (position.X - this.X) + 
                                    (position.Y - this.Y) * (position.Y - this.Y) +
                                    (position.Z - this.Z) * (position.Z - this.Z));
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", X, Y, Z);
        }
    }
}
