using ActionRpgKit.Story.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UQuest : ScriptableObject
{
    public string Name;

    public string Description;

    public int Experience;

    public bool IsActive;

    public bool IsCompleted;

    public UQuest NextQuest;

    public void Start()
    {
        Debug.Log(String.Format("Start Quest {0}", Name));
        IsActive = true;
    }

    public void Finish()
    {
        Debug.Log(String.Format("End Quest {0}", Name));
        IsActive = false;
    }

    public virtual bool CheckProgress()
    {
        return false;
    }

    public void Completed()
    {
        GamePlayer.Instance.Character.Stats.Experience.Value += Experience;
        OnCompletion();
        Finish();
        if (NextQuest != null)
        {
            NextQuest.Start();
        }
    }

    public virtual void OnCompletion() { }
}
