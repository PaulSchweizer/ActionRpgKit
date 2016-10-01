using ActionRpgKit.Character;

namespace ActionRpgKit
{ 
    /// <summary>
    /// Main Controller overseeing the Game.</summary>{
    public sealed class MainController
    {
        /// <summary>
        /// Singleton instance.</summary>
        public static readonly MainController Instance = new MainController();

        /// <summary>
        /// Reference to the Player.</summary>
        public static Player Player;

        static MainController() { }

        private MainController() { }

        /// <summary>
        /// Create the Player.</summary>
        public static void CreatePlayer()
        {
            Player = new Player();
        }
    }
}
