
namespace ActionRpgKit.Core
{
    public interface IGameObject
    {
        Position Position { get; set; }
        void Update();
    }
}
