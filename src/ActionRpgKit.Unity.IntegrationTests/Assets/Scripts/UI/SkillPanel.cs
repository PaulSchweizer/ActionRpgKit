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

public class SkillPanel : MonoBehaviour
{

    public Text Name;
    public Image[] TriggerItems;
    public Text Description;

    public void Initialize(BaseSkill skill)
    {
        transform.localScale = new Vector3(1, 1, 1);
        Name.text = skill.Name;
        Description.text = skill.Description;
        
        for(int i = 0; i < skill.ItemSequence.Length; i++)
        {
            var itemId = skill.ItemSequence[i];
            var item = ActionRpgKitController.Instance.ItemDatabase.Items[itemId];
            TriggerItems[i].sprite = item.Sprite;
            TriggerItems[i].enabled = true;
        }
    }
}
