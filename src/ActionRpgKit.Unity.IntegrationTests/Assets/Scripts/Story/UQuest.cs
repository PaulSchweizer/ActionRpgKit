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

    [TextArea(3, 10)]
    public string Description;

    [TextArea(3, 10)]
    public string StartText;

    [TextArea(3, 10)]
    public string CompleteText;

    public int Experience;

    public bool IsActive;

    public bool IsCompleted;

    public UQuest NextQuest;

    public Sprite StartPortrait;
    public Sprite CompletePortrait;

    public AudioClip StartAudio;
    public AudioClip CompleteAudio;

    public void Start()
    {
        AudioControl.Instance.PlaySound(UStoryline.Instance.QuestStartedSound);
        StoryViewer.Instance.PushMessage(new MessageStruct(Name, StartText, portrait: StartPortrait, audio: StartAudio));

        IsActive = true;
        GameMenu.Instance.ActiveQuestText.text = Name;
        OnStarted();
        GameMenu.Instance.UpdateQuestPanels();
    }

    public void Complete()
    {
        StoryViewer.Instance.PushMessage(new MessageStruct(Name, CompleteText, 1, Experience, CompletePortrait, CompleteAudio));

        IsActive = false;
        IsCompleted = true;
        if (NextQuest != null)
        {
            NextQuest.Start();
        }
        else
        {
            GameMenu.Instance.ActiveQuestText.text = "";
        }
        GameMenu.Instance.UpdateQuestPanels();
    }

    public void Completed()
    {
        GamePlayer.Instance.Character.Stats.Experience.Value += Experience;
        OnCompletion();
        Complete();
    }

    public virtual bool CheckProgress()
    {
        return false;
    }

    public virtual void OnStarted() { }

    public virtual void OnCompletion() { }
}
