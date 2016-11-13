using UnityEngine;

public class ActionRpgKitController : MonoBehaviour
{

    public static ActionRpgKitController Instance;

    public GameItemDatabase ItemDatabase;
    public GameSkillDatabase SkillDatabase;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
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
