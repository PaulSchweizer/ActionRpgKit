using ActionRpgKit.Character;
using ActionRpgKit.Character.Attribute;
using System.Collections;
using UnityEngine;

public class GamePlayer : GameBaseCharacter
{
    // Unity Scripts related fields
    public static GamePlayer Instance;

    // ActionRpgKit related fields
    private PlayerCharacterData playerData;

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
        Character.OnMagicSkillTriggered += new MagicSkillTriggeredHandler(MagicUsed);
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

    public void MagicSkillLearned(IMagicUser sender, int skillId)
    {
    }

    public void CombatSkillLearned(IFighter sender, int skillId)
    {
    }

    /// <summary>
    /// Start the Magic Regeneration when the Player has used Magic.</summary>
    /// <param name="attribute"></param>
    /// <param name="value"></param>
    public void MagicUsed(IMagicUser sender, int skillId)
    {
        StopCoroutine(RegenerateMagic(Character.Stats.Magic.MaxValue));
        StartCoroutine(RegenerateMagic(Character.Stats.Magic.MaxValue));
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

    public void GameOver(BaseAttribute attribute)
    {
        MainController.Instance.GameOver();
    }

    #endregion
}
