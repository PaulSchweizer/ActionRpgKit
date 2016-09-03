namespace ActionRpgKit.Core
{
    public static class GameTime
    {
        private static float _fixedTime;
        private static float _fixedDeltaTime;

        public static void Reset()
        {
            _fixedTime = 0;
            _fixedDeltaTime = 0;
        }

        public static float time
        {
            get
            {
                return _fixedTime;
            }
            set
            {
                GameTime.deltaTime = value - _fixedTime;
                _fixedTime = value;
            }
        }

        public static float deltaTime
        {
            get
            {
                return _fixedDeltaTime;
            }
            set
            {
                _fixedDeltaTime = value;
            }
        }
    }
}