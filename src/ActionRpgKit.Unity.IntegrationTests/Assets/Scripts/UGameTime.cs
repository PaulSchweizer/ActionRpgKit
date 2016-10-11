using UnityEngine;
using ActionRpgKit.Core;

public class UGameTime : MonoBehaviour
{
    
    // Update the ActionRpgKit GameTime
    void Update () {
        GameTime.time = Time.time;
        GameTime.deltaTime = Time.deltaTime;
    }
}
