using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{

    public string SceneName;

    public void SwitchToScene()
    {
        MainController.SwitchScene(SceneName);
    }
}
