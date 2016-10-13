using UnityEngine;
using System.Collections;
using ActionRpgKit.Character;

public class UEnemy : MonoBehaviour
{

    public Enemy Character = new Enemy();

    // Use this for initialization
    void Start()
    {
        Character.Life = 10;
    }

    // Update is called once per frame
    void Update()
    {
        Character.Position.Set(transform.position.x, transform.position.y);
        Character.Update();
    }
}
