using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "Chapter", menuName = "ActionRpgKit/Chapter")]
public class UChapter : ScriptableObject
{
    public string Name;

    public string Description;

    public List<UQuest> Quests;

    public bool IsCompleted;

    public void CheckProgress()
    {
        for (int i = 0; i < Quests.Count; i++)
        {
            if (Quests[i].IsActive)
            {
                var completed = Quests[i].CheckProgress();
                if (completed)
                {
                    Quests[i].Completed();
                }
            }
        }
    }
}
