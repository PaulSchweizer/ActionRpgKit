using UnityEngine;

public class GameTime : MonoBehaviour
{
    // Update the ActionRpgKit GameTime
    void Update () {
        ActionRpgKit.Core.GameTime.time = Time.time;
        ActionRpgKit.Core.GameTime.deltaTime = Time.deltaTime;
    }
}
