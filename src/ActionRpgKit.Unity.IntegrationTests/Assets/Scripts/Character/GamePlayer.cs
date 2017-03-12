using ActionRpgKit.Character;
using ActionRpgKit.Character.Attribute;
using System.Collections;
using UnityEngine;

public class GamePlayer : GameBaseCharacter
{
    // Unity Scripts related fields
    public static GamePlayer Instance;

    // ActionRpgKit related fields
    public PlayerCharacterData playerData;

    public GameObject[] weapons;
    public BaseEffect[] effects;
    public AudioClip[] sounds;

    public BaseEffect[] magicEffects;
    public AudioClip[] magicSounds;

    private int _level = 0;

    public Sprite LevelUpSprite;
    public Sprite NewSkillSprite;

    private bool LifeCrRunning;

    public override BaseCharacter Character
    {
        get
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                playerData = (PlayerCharacterData)CharacterData;
            }  
#endif
            return playerData.Character;
        }
    }

    #region Monobehaviour

    new void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
        playerData = (PlayerCharacterData)Instantiate(CharacterData);
        base.Awake();

        // Connect the signals from the ActionRpgKit Character
        Character.OnMagicSkillLearned += new MagicSkillLearnedHandler(MagicSkillLearned);
        Character.OnCombatSkillLearned += new CombatSkillLearnedHandler(CombatSkillLearned);
        Character.Stats.Life.OnMinReached += new MinReachedHandler(GameOver);
        Character.OnBeingAttacked += new BeingAttackedHandler(RegenerateLife);
        Character.Stats.Level.OnValueChanged += new ValueChangedHandler(LevelUp);
        Character.OnMagicSkillTriggered += new MagicSkillTriggeredHandler(MagicUsed);
        Character.OnMagicSkillTriggered += new MagicSkillTriggeredHandler(TriggerMagicEffect);
        Character.OnWeaponEquipped += new WeaponEquippedHandler(WeaponEquipped);
        Character.OnCombatSkillTriggered += new CombatSkillTriggeredHandler(TriggerWeaponEffect);
        Character.OnStateChanged += new StateChangedHandler(UpdateMusicOnStateChange);
    }
    
    public void Start()
    {
        // Start Regenerating Magic
        MagicUsed(Character, 0);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    #endregion

    #region Character Events

    public void LevelUp(BaseAttribute sender, float value)
    {
        if (Character.Stats.Level.Value > _level)
        {
            _level = (int)Character.Stats.Level.Value;
            var content = string.Format("You reached Level {0}.\nGo to the Menu and distribute your new attribute points.", (int)Character.Stats.Level.Value);
            StoryViewer.Instance.PushMessage(new MessageStruct("Level Up!", content, portrait: LevelUpSprite));

            if (_level == 2)
            {
                Character.LearnMagicSkill(1);
            }

            StartCoroutine(RegenerateLife(Character.Stats.Life.MaxValue));
        }
    }

    public void MagicSkillLearned(IMagicUser sender, int skillId)
    {
        var skill = ActionRpgKitController.Instance.SkillDatabase.GetMagicSkillById(skillId);
        var headline = string.Format("You learned a new Skill \"{0}\"", skill.Skill.Name);
        var content = string.Format("{0}\n\nTo use the Skill, assign the necessary Items to the Slots on the right and trigger them in the right order.\nGo to the Character Menu to see the Item combination.", skill.Skill.Description);
        StoryViewer.Instance.PushMessage(new MessageStruct(headline, content, portrait: NewSkillSprite));
    }

    public void CombatSkillLearned(IFighter sender, int skillId)
    {
    }

    public void UpdateMusicOnStateChange(ICharacter sender, IState previousState, IState newState)
    {
        if (newState is IdleState || newState is MoveState)
        {
            AudioControl.Instance.StartBackgroundMusic();
        }
        else
        {
            AudioControl.Instance.StartCombatMusic();
        }
    }

    /// <summary>
    /// Start the Magic Regeneration when the Player has used Magic.</summary>
    /// <param name="attribute"></param>
    /// <param name="value"></param>
    public void MagicUsed(IMagicUser sender, int skillId)
    {
        StopCoroutine("RegenerateMagic");
        StartCoroutine(RegenerateMagic(Character.Stats.Magic.MaxValue));
    }

    public void WeaponEquipped(int itemId)
    {
        for (int i=0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
            {
                continue;
            }

            if (i == itemId)
            {
                weapons[i].SetActive(true);
            }
            else
            {
                weapons[i].SetActive(false);
            }
        }
    }

    public void TriggerWeaponEffect(IFighter sender, int skillId)
    {
        if (Character.EquippedWeapon > -1)
        {
            if (effects[Character.EquippedWeapon] != null)
            {
                effects[Character.EquippedWeapon].Trigger(sender, skillId);
            }

            if (sounds[Character.EquippedWeapon] != null)
            {
                AudioControl.Instance.PlaySound(sounds[Character.EquippedWeapon]);
            }
        }
    }

    public void TriggerMagicEffect(IMagicUser sender, int skillId)
    {
        if (magicEffects[skillId] != null)
        {
            magicEffects[skillId].Trigger(sender, skillId);
        }

        if (magicSounds[skillId] != null)
        {
            AudioControl.Instance.PlaySound(magicSounds[skillId]);
        }
    }

    public void RegenerateLife(IFighter attacker)
    {
        if (!LifeCrRunning)
        {
            StartCoroutine(RegenerateLife(Character.Stats.Life.MaxValue));
        }
    }

    /// <summary>
    /// Regenerate the Magic Slider.</summary>
    public IEnumerator RegenerateMagic(float maxValue)
    {
        while (Character.Stats.Magic.Value < maxValue)
        {
            Character.Stats.Magic.Value += Character.Stats.MagicRegenerationRate.Value * Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Regenerate the Life Slider.</summary>
    public IEnumerator RegenerateLife(float maxValue)
    {
        LifeCrRunning = true;
        while (Character.Stats.Life.Value < Character.Stats.Life.MaxValue)
        {
            Character.Stats.Life.Value += Character.Stats.LifeRegenerationRate.Value * Time.deltaTime;
            yield return null;
        }
        LifeCrRunning = false;
    }

    public void GameOver(BaseAttribute attribute)
    {
        MainController.Instance.GameOver();
    }

    #endregion
}
