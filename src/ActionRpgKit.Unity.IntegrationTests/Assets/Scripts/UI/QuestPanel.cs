using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using ActionRpgKit.Character;
using ActionRpgKit.Character.Attribute;
using SlotSystem;
using ActionRpgKit.Character.Skill;

public class QuestPanel : MonoBehaviour
{

    public UQuest Quest;
    public Text Headline;
    public Text XP;

    public Color Regular;
    public Color Highlighted;

    public void Initialize(UQuest quest)
    {
        Quest = quest;
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (Quest.IsCompleted)
        {
            gameObject.SetActive(true);
            Headline.text = String.Format("☑ {0}", Quest.Name);
            XP.enabled = true;
            XP.text = String.Format("+{0} XP", Quest.Experience);

        }
        else if (Quest.IsActive)
        {
            gameObject.SetActive(true);
            Headline.text = String.Format("☐ {0}", Quest.Name);
            XP.enabled = false;
        }
        else
        {
            gameObject.SetActive(false);
            //Headline.text = String.Format("☐ {0}", Quest.Name);
            //XP.enabled = false;
        }
    }

    public void ShowText()
    {
        if (Quest.IsCompleted)
        {
            string text = String.Format("{0}\n\n{1}", Quest.StartText, Quest.CompleteText);
            GameMenu.Instance.QuestText.text = text;
        }
        else
        {
            GameMenu.Instance.QuestText.text = Quest.StartText;
        }
        
        foreach(QuestPanel panel in GameMenu.Instance.QuestPanels)
        {
            panel.RegularColor();
        }

        Highlight();
    }

    public void Highlight()
    {
        Headline.color = Highlighted;
    }

    public void RegularColor()
    {
        Headline.color = Regular;
    }
}
