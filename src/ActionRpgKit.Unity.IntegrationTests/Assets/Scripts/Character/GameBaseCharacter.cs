using UnityEngine;
using ActionRpgKit.Character;
using System;
using System.Collections.Generic;
using ActionRpgKit.Character.Attribute;
using System.Collections;
using ActionRpgKit.Character.Skill;
using ActionRpgKit.Core;

public class GameBaseCharacter : MonoBehaviour
{
    // Unity related fields
    public NavMeshAgent NavMeshAgent;
    public Animator Animator;
    public Collider Collider;
    public Loot LootPrefab;
    protected GameCharacterState CurrentState;
    protected GameCharacterState IdleState = GameIdleState.Instance;
    protected GameCharacterState MoveState = GameMoveState.Instance;
    protected GameCharacterState AlertState = GameAlertState.Instance;
    protected GameCharacterState ChaseState = GameChaseState.Instance;
    protected GameCharacterState AttackState = GameAttackState.Instance;
    protected GameCharacterState DefeatedState = GameDefeatedState.Instance;

    // ActionRpgKit related fields
    public BaseCharacterData CharacterData;
    private BaseCharacterData _characterData;

    public virtual BaseCharacter Character
    {
        get
        {
            return CharacterData.Character;
        }
    }

    #region Monobehaviour

    public virtual void Awake()
    {
        // Connect the signals fromt the ActionRpgKit Character
        Character.OnStateChanged += new StateChangedHandler(StateChanged);
        Character.OnMagicSkillTriggered += new MagicSkillTriggeredHandler(MagicSkillTriggered);
        Character.OnCombatSkillTriggered += new CombatSkillTriggeredHandler(CombatSkillTriggered);
        foreach (KeyValuePair<string, BaseAttribute> attr in Character.Stats.Dict)
        {
            attr.Value.OnModifierAdded += new ModifierAddedHandler(ModifierAdded);
        }

        // NavMeshAgent
        Character.Stats.MovementSpeed.OnValueChanged += new ValueChangedHandler(SpeedChanged);
        SpeedChanged(Character.Stats.MovementSpeed, Character.Stats.MovementSpeed.Value);

        // State
        CurrentState = IdleState;
    }

    /// <summary>
    /// Set the position of the ARPG Character to the position of the transform.</summary>
    public virtual void Update()
    {
        // Set the position of the Arpg Character
        Character.Position.Set(transform.position.x, transform.position.z);

        // Update the current state
        CurrentState.Update(this);
    }

    #endregion

    #region Character Events

    /// <summary>
    /// Update the Speed and dependent values on the navMeshAgent.</summary>
    /// <param name="sender">The MovementSpeed Attribute</param>
    /// <param name="value">The value</param>
    public void SpeedChanged(BaseAttribute sender, float value)
    {
        NavMeshAgent.speed = value;
        NavMeshAgent.angularSpeed = value * 100;
    }

    /// <summary>
    /// A Modifier has been added.</summary>
    /// <param name="sender">The Attribute</param>
    /// <param name="modifier">The Modifier</param>
    public virtual void ModifierAdded(BaseAttribute sender, AttributeModifier modifier)
    {
        if (modifier is TimeBasedModifier)
        {
            StartCoroutine(TimeBasedModifierCountdown(sender, (TimeBasedModifier)modifier));
        }
    }

    private IEnumerator TimeBasedModifierCountdown(BaseAttribute sender, TimeBasedModifier modifier)
    {
        while (modifier.RemainingTime > 0)
        {
            yield return null;
        }
        sender.RemoveModifier(modifier);
    }

    public virtual void StateChanged(ICharacter sender, IState previousState, IState newState)
    {
        CurrentState.Exit(this);
        if (newState is IdleState)
        {
            CurrentState = IdleState;
        }
        else if (newState is MoveState)
        {
            CurrentState = MoveState;
        }
        else if (newState is AlertState)
        {
            CurrentState = AlertState;
        }
        else if (newState is ChaseState)
        {
            CurrentState = ChaseState;
        }
        else if (newState is AttackState || newState is PlayerAttackState)
        {
            CurrentState = AttackState;
        }
        else if (newState is DefeatedState)
        {
            CurrentState = DefeatedState;
        }
        else
        {
            CurrentState = IdleState;
        }
        CurrentState.Enter(this);
    }

    public virtual void MagicSkillTriggered(IMagicUser sender, int skillId)
    {
        StartCoroutine(MagicSkillCountdown(sender, skillId));
    }

    public virtual void CombatSkillTriggered(IFighter sender, int skillId)
    {
        StartCoroutine(CombatSkillCountdown(sender, skillId));
    }

    /// <summary>
    /// Run a countdown after a skill has been used.</summary>
    private IEnumerator MagicSkillCountdown(IMagicUser sender, int skillId)
    {
        float endTime = Character.MagicSkillEndTimes[Character.MagicSkills.IndexOf(skillId)];

        var skillData = ActionRpgKitController.Instance.SkillDatabase.GetMagicSkillById(skillId);

        Animator.SetFloat("MagicAnimation", skillData.AnimationIndex);
        Animator.SetTrigger("Magic");

        endTime = Time.time + skillData.CastingTime;

        while (Time.time < endTime)
        {
            yield return null;
        }
        Character.UseMagicSkill(skillId);
    }

    /// <summary>
    /// Run a countdown after a skill has been used.</summary>
    private IEnumerator CombatSkillCountdown(IFighter sender, int skillId)
    {
        float endTime = Character.CombatSkillEndTimes[Character.CombatSkills.IndexOf(skillId)];

        // Change the Animator clip values
        var skillData = ActionRpgKitController.Instance.SkillDatabase.GetCombatSkillById(skillId);
        AnimationClip clip = skillData.AnimationClip;

        float duration = endTime - GameTime.time;
        if (duration > clip.length)
        {
            Animator.SetFloat("AttackSpeed", clip.length / duration);
        }
        else
        {
            Animator.SetFloat("AttackSpeed", duration / clip.length);
        }
        Animator.SetFloat("AttackAnimation", skillData.AnimationIndex);
        Animator.SetTrigger("Attack");

        endTime = Time.time + skillData.HitTime;
        while (Time.time < endTime)
        {
            yield return null;
        }
        if (Character.TargetedEnemy != null)
        {
            Character.UseCombatSkill(skillId);
        }
    }

    #endregion


#if UNITY_EDITOR
    /// <summary>
    /// Draw some Debug helper shapes.</summary>
    void OnDrawGizmos()
    {
        Color color;
        if (Character.CurrentState is IdleState)
        {
            color = Color.green;
        }
        else if (Character.CurrentState is MoveState)
        {
            color = Color.blue;
        }
        else if (Character.CurrentState is AlertState)
        {
            color = Color.yellow;
        }
        else if (Character.CurrentState is ChaseState)
        {
            color = Color.magenta;
        }
        else if (Character.CurrentState is AttackState || Character.CurrentState is PlayerAttackState)
        {
            color = Color.red;
        }
        else if (Character.CurrentState is DefeatedState)
        {
            color = Color.black;
        }
        else
        {
            color = Color.grey;
        }
        // Draw a circle for sight range
        DebugExtension.DebugCircle(transform.position, color, 
                (float)Math.Sqrt(Character.Stats.AlertnessRange.Value), 0);
        if (UnityEditor.EditorApplication.isPlaying)
        {
            DebugExtension.DebugCircle(transform.position, color, 
                    (float)Math.Sqrt(Character.AttackRange), 0);
        }
    }
#endif

}


public abstract class GameCharacterState
{
    /// <summary>
    /// Called when entering the State.</summary>
    public virtual void Enter(GameBaseCharacter character) { }

    /// <summary>
    /// Called every frame when the State is active.</summary>
    public abstract void Update(GameBaseCharacter character);

    /// <summary>
    /// Called before changing to the next State.</summary>
    public virtual void Exit(GameBaseCharacter character) { }
}


public class GameIdleState : GameCharacterState
{
    public static GameIdleState Instance = new GameIdleState();

    /// <summary>
    /// Stop the NavMeshAgent.</summary>
    public override void Enter(GameBaseCharacter character)
    {
        character.NavMeshAgent.Stop();
        character.Animator.SetBool("IdleState", true);
        character.Animator.SetFloat("MovementSpeed", 0);
    }

    /// <summary>
    /// Bring the movement speed down to a still stand.</summary>
    public override void Update(GameBaseCharacter character)
    {
        if (character.Character.Enemies.Count > 0)
        {
            character.NavMeshAgent.SetDestination(new Vector3(character.Character.Enemies[0].Position.X, 0,
                                                              character.Character.Enemies[0].Position.Y));
        }
    }

    /// <summary>
    /// Stop the NavMeshAgent.</summary>
    public override void Exit(GameBaseCharacter character)
    {
        character.NavMeshAgent.Resume();
        character.Animator.SetBool("IdleState", false);
    }
}


public class GameMoveState : GameCharacterState
{
    public static GameMoveState Instance = new GameMoveState();

    /// <summary>
    /// Stop the NavMeshAgent.</summary>
    public override void Enter(GameBaseCharacter character)
    {
        character.NavMeshAgent.Resume();
        character.Animator.SetBool("MoveState", true);
        character.Character.IsMoving = true;
    }

    /// <summary>
    /// Bring the movement speed down to a still stand.</summary>
    public override void Update(GameBaseCharacter character)
    {
        if (character.NavMeshAgent.pathPending)
        {
            return;
        }
        if (character.Animator.GetFloat("MovementSpeed") < character.Character.Stats.MovementSpeed.Value)
        {
            character.Animator.SetFloat("MovementSpeed", 
                Math.Min(character.Character.Stats.MovementSpeed.Value, 
                         character.Animator.GetFloat("MovementSpeed") + Time.deltaTime));
        }

        if (character.NavMeshAgent.remainingDistance <= character.NavMeshAgent.stoppingDistance)
        {
            if (!character.NavMeshAgent.hasPath || Mathf.Abs(character.NavMeshAgent.velocity.sqrMagnitude) < float.Epsilon)
            {
                character.Character.IsMoving = false;
            }
        }
    }

    /// <summary>
    /// Stop the NavMeshAgent.</summary>
    public override void Exit(GameBaseCharacter character)
    {
        character.NavMeshAgent.Resume();
        character.Animator.SetBool("MoveState", false);
        character.Character.IsMoving = false;
    }
}


public class GameAlertState : GameCharacterState
{
    public static GameAlertState Instance = new GameAlertState();

    /// <summary>
    /// Stop the NavMeshAgent.</summary>
    public override void Enter(GameBaseCharacter character)
    {
        character.NavMeshAgent.Stop();
        character.Animator.SetBool("AlertState", true);
    }

    /// <summary>
    /// Bring the movement speed down to a still stand.</summary>
    public override void Update(GameBaseCharacter character)
    {
        if (character.Animator.GetFloat("MovementSpeed") > 0)
        {
            character.Animator.SetFloat("MovementSpeed", 
                Math.Max(0, character.Animator.GetFloat("MovementSpeed") - Time.deltaTime * 6));
        }
    }

    /// <summary>
    /// Stop the NavMeshAgent.</summary>
    public override void Exit(GameBaseCharacter character)
    {
        character.NavMeshAgent.Resume();
        character.Animator.SetBool("AlertState", false);
    }
}


public class GameChaseState : GameCharacterState
{
    public static GameChaseState Instance = new GameChaseState();

    /// <summary>
    /// Stop the NavMeshAgent.</summary>
    public override void Enter(GameBaseCharacter character)
    {
        character.NavMeshAgent.Resume();
        character.Animator.SetBool("ChaseState", true);
        character.Character.IsMoving = true;
    }

    /// <summary>
    /// Bring the movement speed down to a still stand.</summary>
    public override void Update(GameBaseCharacter character)
    {
        if (character.NavMeshAgent.pathPending)
        {
            return;
        }
        if (character.Animator.GetFloat("MovementSpeed") < 1)
        {
            character.Animator.SetFloat("MovementSpeed",
                Math.Min(1, character.Animator.GetFloat("MovementSpeed") + Time.deltaTime * 6));
        }
        character.NavMeshAgent.destination = new Vector3(character.Character.TargetedEnemy.Position.X, 0,
                                                         character.Character.TargetedEnemy.Position.Y);
    }

    /// <summary>
    /// Stop the NavMeshAgent.</summary>
    public override void Exit(GameBaseCharacter character)
    {
        character.Animator.SetBool("ChaseState", false);
        character.Character.IsMoving = false;
    }
}


public class GameAttackState : GameCharacterState
{
    public static GameAttackState Instance = new GameAttackState();

    /// <summary>
    /// Stop the NavMeshAgent.</summary>
    public override void Enter(GameBaseCharacter character)
    {
        character.NavMeshAgent.Stop();
        character.Animator.SetBool("AttackState", true);
    }

    /// <summary>
    /// Look at the TargetedEnemy.</summary>
    public override void Update(GameBaseCharacter character)
    {
        var target = new Vector3(character.Character.TargetedEnemy.Position.X, 0,
                                 character.Character.TargetedEnemy.Position.Y);
        character.transform.LookAt(target);
    }

    /// <summary>
    /// Stop the NavMeshAgent.</summary>
    public override void Exit(GameBaseCharacter character)
    {
        character.StopCoroutine("CombatSkillCountdown");
        character.Animator.SetBool("AttackState", false);
    }
}


public class GameDefeatedState : GameCharacterState
{
    public static GameDefeatedState Instance = new GameDefeatedState();

    /// <summary>
    /// Stop the NavMeshAgent.</summary>
    public override void Enter(GameBaseCharacter character)
    {
        character.Animator.SetBool("DefeatedState", true);
        character.NavMeshAgent.enabled = false;
        character.gameObject.tag = "Untagged";
        character.Collider.enabled = false;
        character.StartCoroutine(DefeatAnimation(character));
    }

    /// <summary>
    /// Nothing to do here.</summary>
    public override void Update(GameBaseCharacter character) { }

    /// <summary>
    /// Reset the dying state.</summary>
    public override void Exit(GameBaseCharacter character)
    {
        character.enabled = true;
        character.Animator.enabled = true;
        character.Animator.SetBool("DefeatedState", false);
    }

    /// <summary>
    /// Play the dying animation and remove the Character after a time.</summary>
    public IEnumerator DefeatAnimation(GameBaseCharacter character)
    {
        float endTime = Time.time + 3;
        character.Animator.SetFloat("DefeatAnimation", UnityEngine.Random.Range(0, 2));
        while (Time.time < endTime)
        {
            yield return null;
        }
        DropLoot(character);
        character.Animator.enabled = false;
        character.enabled = false;
    }

    /// <summary>
    /// Instantiate loot.</summary>
    public void DropLoot(GameBaseCharacter character)
    {
        var loot = (Loot)ScriptableObject.Instantiate(character.LootPrefab,
                                                      new Vector3(character.Character.Position.X, 0,
                                                                  character.Character.Position.Y),
                                                      character.transform.rotation);
        loot.Inventory = character.Character.Inventory;
        character.transform.SetParent(loot.transform);
    }
}