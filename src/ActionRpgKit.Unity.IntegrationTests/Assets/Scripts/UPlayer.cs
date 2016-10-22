using UnityEngine;
using System.Collections;
using ActionRpgKit.Character;
using ActionRpgKit.Item;
using ActionRpgKit.Character.Skill;

public class UPlayer : MonoBehaviour
{

    public Player Character = new Player();

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Character.Position.Set(transform.position.x, transform.position.y);
        Character.Update();
    }
}
