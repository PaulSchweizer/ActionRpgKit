namespace ActionRpgKit.Core
{
    public class Vector2D
    {
         /// <summary>
        /// The x and y components.</summary>
        private float[] _components = new float[2];
        
        /// <summary>
        /// Set the initial component values.</summary>
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
                return _components[0];
            }
            set
            {
                _components[0] = value;
            }
        }
        
        /// <summary>
        /// The y value.</summary>
        public float Y
        {
            get
            {
                return _components[1];
            }
            set
            {
                _components[1] = value;
            }
        }
       
        /// <summary>
        /// Set the Vector Components.</summary>
        /// <param name="x">The x value</param>
        /// <param name="y">The y value</param>
        public void Set (float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Get the Vector Components.</summary>
        /// <returns>The position</returns>
        public float[] Get ()
        {
            return _position;
        }
    }
}
