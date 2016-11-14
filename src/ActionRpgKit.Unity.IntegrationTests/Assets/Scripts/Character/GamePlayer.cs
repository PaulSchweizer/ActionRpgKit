using ActionRpgKit.Character;
using ActionRpgKit.Character.Attribute;

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

    public void MagicSkillLearned(IMagicUser sender, int skillId)
    {
    }

    public override void MagicSkillTriggered(IMagicUser sender, int skillId)
    {
    }

    public void CombatSkillLearned(IFighter sender, int skillId)
    {
    }

    #endregion
}
