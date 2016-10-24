using UnityEngine;
using ActionRpgKit.Character;
using System;
using System.Collections.Generic;
using ActionRpgKit.Character.Attribute;

public class GamePlayer : GameBaseCharacter
{

    private PlayerCharacterData playerData;

    public override BaseCharacter Character
    {
        get
        {
            return playerData.Character;
        }
    }

    #region Monobehaviour

    new void Awake()
    {
        playerData = (PlayerCharacterData)CharacterData;

        base.Awake();
        // Connect the signals fromt the ActionRpgKit Character
        Character.OnMagicSkillLearned += new MagicSkillLearnedHandler(MagicSkillLearnedTest);
        Character.OnCombatSkillLearned += new CombatSkillLearnedHandler(CombatSkillLearnedTest);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    #endregion

    #region Character Events

    public override void StatsChanged(BaseAttribute sender, float value)
    {
    }

    public override void StateChangedTest(ICharacter sender, IState previousState, IState newState)
    {
        throw new NotImplementedException();
    }

    public override void MagicSkillLearnedTest(IMagicUser sender, int skillId)
    {
        throw new NotImplementedException();
    }

    public override void MagicSkillTriggeredTest(IMagicUser sender, int skillId)
    {
        throw new NotImplementedException();
    }

    public override void CombatSkillLearnedTest(IFighter sender, int skillId)
    {
        throw new NotImplementedException();
    }

    public override void CombatSkillTriggeredTest(IFighter sender, int skillId)
    {
        throw new NotImplementedException();
    }

    #endregion
}
