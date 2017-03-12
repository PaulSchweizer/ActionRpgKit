using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LevelExit: MonoBehaviour
{

    public string TargetScene;

    public UQuest[] NecessaryQuests;

    /// <summary>
    /// Collect the Items on collision.
    /// Destroy the Loot Object.</summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            foreach(UQuest quest in NecessaryQuests)
            {
                if (!quest.IsCompleted)
                {
                    return;
                }
            }
            DontDestroyOnLoad(GamePlayer.Instance.gameObject);
            DontDestroyOnLoad(GameMenu.Instance.gameObject);
            DontDestroyOnLoad(AudioControl.Instance.gameObject);
            DontDestroyOnLoad(StoryViewer.Instance.gameObject);
            DontDestroyOnLoad(CameraRig.Instance.gameObject);
            MainController.SwitchScene(TargetScene);
        }
    }
}
