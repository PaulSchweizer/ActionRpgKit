using UnityEngine;

public class ActionRpgKitController : MonoBehaviour
{

    public GameItemDatabase ItemDatabase;
    public GameSkillDatabase SkillDatabase;

    void Awake()
    {
        ItemDatabase.InitDatabase();
        SkillDatabase.InitDatabase();
    }

    // Update the ActionRpgKit GameTime
    void Update() {
        ActionRpgKit.Core.GameTime.time = Time.time;
        ActionRpgKit.Core.GameTime.deltaTime = Time.deltaTime;
        ActionRpgKit.Character.Controller.Update();
    }
}
