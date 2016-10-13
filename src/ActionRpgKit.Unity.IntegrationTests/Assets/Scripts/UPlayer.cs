using UnityEngine;
using System.Collections;
using ActionRpgKit.Character;
using ActionRpgKit.Item;
using ActionRpgKit.Character.Skill;

public class UPlayer : MonoBehaviour
{

    public Player Character = new Player();

    // Use this for initialization
    void Start()
    {
        var meleeSkill = new MeleeCombatSkill(id: 1,
                                        name: "SwordFighting",
                                        description: "Wield a sword effectively.",
                                        preUseTime: 1,
                                        cooldownTime: 1,
                                        damage: 1,
                                        maximumTargets: 1,
                                        range: 1,
                                        itemSequence: new IItem[] { });
        Character.LearnCombatSkill(meleeSkill);
        Character.CurrentAttackSkill = meleeSkill;
    }

    // Update is called once per frame
    void Update()
    {
        Character.Position.Set(transform.position.x, transform.position.y);
        Character.Update();
    }
}
