using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "FindItemsQuest", menuName = "ActionRpgKit/Quest/FindItems")]
public class FindItemsQuest : UQuest
{
    public int[] items;

    public int[] quantities;

    public override bool CheckProgress()
    {
        var inventory = GamePlayer.Instance.Character.Inventory;
        for(int i=0; i < items.Length; i++)
        {
            if (!inventory.Items.Contains(items[i]))
            {
                return false;
            }
            else
            {
                if (inventory.GetQuantity(items[i]) < quantities[i])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public override void OnCompletion()
    {

    }
}