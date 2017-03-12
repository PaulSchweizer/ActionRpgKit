using UnityEngine;
using ActionRpgKit.Character;
using System;
using System.Collections.Generic;
using ActionRpgKit.Character.Attribute;

public class GameEnemy : GameBaseCharacter
{

    private EnemyCharacterData enemyData;

    public ParticleSystem Blood;

    public AudioClip DamagedSound;
    public AudioClip[] AttackSounds;
    public AudioClip DefeatedSound;

    public override BaseCharacter Character
    {
        get
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                enemyData = (EnemyCharacterData)CharacterData;
            }
#endif
            return enemyData.Character;
        }
    }

    #region Monobehaviour

    new void Awake()
    {
        enemyData = (EnemyCharacterData)Instantiate(CharacterData);
        base.Awake();
        Blood.Stop();
        enemyData.Character.Stats.Life.OnValueChanged += new ValueChangedHandler(BloodSplatter);
        enemyData.Character.Stats.Life.OnValueChanged += new ValueChangedHandler(PlayDamagedSound);
        enemyData.Character.OnCombatSkillUsed += new CombatSkillUsedHandler(PlayAttackSound);
        enemyData.Character.OnStateChanged += new StateChangedHandler(PlayDefeatedSound);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    #endregion

    #region Character Events

    public void BloodSplatter(BaseAttribute sender, float value)
    {
        if (Blood != null)
        {
            Blood.Stop();
            Blood.Play();
        }
    }

    public void PlayDamagedSound(BaseAttribute sender, float value)
    {
        AudioControl.Instance.PlaySound(DamagedSound);
    }

    public void PlayAttackSound(IFighter sender, int skillId)
    {
        AudioControl.Instance.PlaySound(AttackSounds[UnityEngine.Random.Range(0, AttackSounds.Length)]);
    }

    public void PlayDefeatedSound(ICharacter sender, IState previousState, IState newState)
    {
        if (newState is DefeatedState)
        {
            AudioControl.Instance.PlaySound(DefeatedSound);
        }
    }
    #endregion
}
