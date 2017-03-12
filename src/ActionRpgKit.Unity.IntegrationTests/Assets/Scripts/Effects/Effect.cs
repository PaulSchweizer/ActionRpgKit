using ActionRpgKit.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseEffect : MonoBehaviour
{
    public virtual void Trigger(IFighter sender, int skillId) {}

    public virtual void Trigger(IMagicUser sender, int skillId) { }
}
