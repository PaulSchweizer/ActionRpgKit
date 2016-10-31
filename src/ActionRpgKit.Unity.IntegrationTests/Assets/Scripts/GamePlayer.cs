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
#if UNITY_EDITOR
            playerData = (PlayerCharacterData)CharacterData;
#endif
            return playerData.Character;
        }
    }

    #region Monobehaviour

    new void Awake()
    {
        playerData = (PlayerCharacterData)CharacterData;

        base.Awake();

        // Connect the signals from the ActionRpgKit Character
        Character.OnMagicSkillLearned += new MagicSkillLearnedHandler(MagicSkillLearned);
        Character.OnCombatSkillLearned += new CombatSkillLearnedHandler(CombatSkillLearned);
    }

    public void Start()
    {

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

    public override void StateChanged(ICharacter sender, IState previousState, IState newState)
    {
    }

    public override void MagicSkillLearned(IMagicUser sender, int skillId)
    {
    }

    public override void MagicSkillTriggered(IMagicUser sender, int skillId)
    {
    }

    public override void CombatSkillLearned(IFighter sender, int skillId)
    {
    }

    #endregion
}
