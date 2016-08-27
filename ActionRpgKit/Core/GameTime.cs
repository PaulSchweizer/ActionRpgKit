public static class GameTime
{
    private static float _fixedTime;

    public static float time
    {
        get
        {
            return _fixedTime;
        }
        set
        {
            _fixedTime = value;
        }
    }
}
