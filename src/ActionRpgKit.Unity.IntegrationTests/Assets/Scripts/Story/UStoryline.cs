using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UStoryline : MonoBehaviour
{
    public static UStoryline Instance;

    public List<UQuest> Quests;

    public bool Paused;

    public AudioClip QuestStartedSound;
    public AudioClip QuestCompletedSound;

    private UQuest _nextQuest;

    public void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

    public void StartStory ()
    {
        foreach(UQuest quest in Quests)
        {
            quest.IsActive = false;
            quest.IsCompleted = false;
        }

        Quests[0].Start();
    }

    public void PauseStory ()
    {
        Paused = true;
    }

    public void ResumeStory()
    {
        Paused = false;
    }

    public void Update ()
    {
        if (Paused)
        {
            return;
        }

        for (int i = 0; i < Quests.Count; i++)
        {
            if (Quests[i].IsActive)
            {
                var completed = Quests[i].CheckProgress();
                if (completed)
                {
                    Quests[i].Completed();
                    AudioControl.Instance.PlaySound(QuestCompletedSound);
                }
            }
        }
    }
}
