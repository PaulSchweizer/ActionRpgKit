using ActionRpgKit.Character;
using System.Collections.Generic;
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

    public void Reset()
    {
        ActionRpgKit.Character.Controller.Enemies = new List<Enemy>();
    }

    public void Initialize()
    {
        ActionRpgKit.Character.Controller.Player = (Player)GamePlayer.Instance.Character;
        foreach (GameEnemy enemy in FindObjectsOfType<GameEnemy>())
        {
            ActionRpgKit.Character.Controller.Enemies.Add((Enemy)enemy.Character);
        }
    }

    // Update the ActionRpgKit GameTime
    void Update() {
        ActionRpgKit.Core.GameTime.time = Time.time;
        ActionRpgKit.Core.GameTime.deltaTime = Time.deltaTime;
        ActionRpgKit.Character.Controller.Update();
    }
}
