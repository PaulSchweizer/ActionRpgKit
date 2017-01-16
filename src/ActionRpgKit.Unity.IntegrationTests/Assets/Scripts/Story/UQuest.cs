using ActionRpgKit.Story.Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class UQuest : ScriptableObject
{
    public string Name;

    public string Description;

    public string StartText;

    public string CompleteText;

    public int Experience;

    public bool IsActive;

    public bool IsCompleted;

    public UQuest NextQuest;

    public float viewerTime = 10;

    public void Start()
    {
        StoryViewer.Instance.Show(Name, StartText, viewerTime, StoryViewer.Style.Started);
        IsActive = true;
    }

    public void Complete()
    {
        StoryViewer.Instance.Show(Name, CompleteText, viewerTime, StoryViewer.Style.Completed, Experience);
        IsActive = false;
        IsCompleted = true;
        if (NextQuest != null)
        {
            UStoryline.Instance.StartCoroutine(StartNextQuest());
        }
    }

    public void Completed()
    {
        GamePlayer.Instance.Character.Stats.Experience.Value += Experience;
        OnCompletion();
        Complete();
    }

    public IEnumerator StartNextQuest()
    {
        float endTime = Time.time + viewerTime;
        Debug.Log(viewerTime);
        while (Time.time < endTime)
        {
            yield return null;
        }
        NextQuest.Start();
    }

    public virtual bool CheckProgress()
    {
        return false;
    }

    public virtual void OnCompletion() { }
}
