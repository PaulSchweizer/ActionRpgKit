using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using ActionRpgKit.Character;
using ActionRpgKit.Character.Attribute;
using SlotSystem;
using ActionRpgKit.Character.Skill;
using ActionRpgKit.Item;
using System.Collections;

public class GameMenu : MonoBehaviour, ISlotChanged
{
    /// <summary>
    /// Keep the game menu as Singleton.</summary>
    public static GameMenu Instance;

    /// <summary>
    /// Slider showing the Player's life.</summary>
    public Slider LifeSlider;

    /// <summary>
    /// Slider showing the Player's magic.</summary>
    public Slider MagicSlider;

    /// <summary>
    /// Text displaying the Currently active Quest.</summary>
    public Text ActiveQuestText;

    /// <summary>
    /// Text displaying the Currently active Quest.</summary>
    public Text InfoText;

    /// <summary>
    /// Slider showing the Player's targeted Enemy.</summary>
    public Slider EnemyLifeSlider;

    /// <summary>
    /// Text displaying the Enemy.</summary>
    public Text EnemyName;

    /// <summary>
    /// Text displaying the Enemy.</summary>
    public GameObject EnemyPanel;

    public ActionPanel ActionPanel;

    public SlotView InventoryView;

    public GameObject SaveButton;

    public GameObject HUDPanel;

    // Attributes
    public GameObject MainMenuPanel;
    public Text BodyValueText;
    public Text MindValueText;
    public Text SoulValueText;
    public Text AvailableAttributePointsText;
    public Text ExperienceValueText;
    public Text LevelValueText;
    public Text AttackRangeValueText;
    public Text AttacksPerSecondValueText;
    public Text DamageValueText;
    public Text MagicRegenerationRateValueText;
    public Text LifeRegenerationRateValueText;
    public Text LifeText;
    public Text MagicText;

    public GameObject SkillsPanel;
    public GameObject SkillPanelPrefab;

    // Quests
    public GameObject QuestPanel;
    public Text QuestText;
    public QuestPanel QuestPanelPrefab;
    public List<QuestPanel> QuestPanels;

    public GameObject StatsPanel;
    public GameObject QuestsPanel;

    // Internals
    private Player Player;

    /// <summary>
    /// Keep the GameMenu a Singleton.</summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        Player = (Player)GamePlayer.Instance.Character;

        // Connect the Signals from the Player
        Player.Stats.Life.OnValueChanged += new ValueChangedHandler(UpdateLifeSlider);
        Player.Stats.Magic.OnValueChanged += new ValueChangedHandler(UpdateMagicSlider);
        Player.OnCombatSkillUsed += new CombatSkillUsedHandler(UpdateEnemyLifeSlider);
        Player.OnStateChanged += new StateChangedHandler(ToggleEnemyPanel);
        Player.Stats.Body.OnValueChanged += new ValueChangedHandler(UpdateStats);
        Player.Stats.Mind.OnValueChanged += new ValueChangedHandler(UpdateStats);
        Player.Stats.Soul.OnValueChanged += new ValueChangedHandler(UpdateStats);
        Player.OnWeaponEquipped += new WeaponEquippedHandler(UpdateWeaponStats);
        Player.Stats.Level.OnValueChanged += new ValueChangedHandler(NextLevelReached);
        Player.OnMagicSkillLearned += new MagicSkillLearnedHandler(MagicSkillLearned);
        Player.Inventory.OnItemAdded += new ItemAddedHandler(FoundItem);

        // Set the initial values
        UpdateLifeSlider(Player.Stats.Life, Player.Stats.Life.Value);
        UpdateMagicSlider(Player.Stats.Magic, Player.Stats.Magic.Value);
        UpdateStats(Player.Stats.Body, Player.Stats.Body.Value);
        AvailableAttributePointsText.text = Player.AvailableAttributePoints.ToString();

        // The Inventory and the equipped Weapon
        InventoryView.Initialize();
        foreach (KeyValuePair<int, SlottableItem> entry in InventoryView._items)
        {
            if (entry.Value.Item is WeaponItemData)
            {
                var weaponItemData = entry.Value.Item as WeaponItemData;
                if (weaponItemData.Item.Id == Player.EquippedWeapon)
                {
                    ActionPanel.WeaponSlot.Swap(entry.Value);
                    break;
                }
            }
        }

        // The equipped items 
        for (int i = 0; i < Player.EquippedItems.Length; i++) { 
            foreach (KeyValuePair<int, SlottableItem> entry in InventoryView._items)
            {
                if (entry.Value.Item is UsableItemData)
                {
                    var usableItemData = entry.Value.Item as UsableItemData;
                    if (usableItemData.Item.Id == Player.EquippedItems[i])
                    {
                        ActionPanel.Slots[i].Swap(entry.Value);
                        break;
                    }
                }
            }
        }

        // The Skill Panels
        foreach (int skillId in Player.MagicSkills)
        {
            if (skillId > -1)
            {
                BaseSkill skill = SkillDatabase.GetMagicSkillById(skillId);
                AddSkillPanel(skill);
            }
        }

        // The Quest Panel
        foreach (UQuest quest in UStoryline.Instance.Quests)
        {
            var panel = Instantiate<QuestPanel>(QuestPanelPrefab);
            panel.Initialize(quest);
            panel.transform.SetParent(QuestPanel.transform);
            panel.transform.localScale = new Vector3(1, 1, 1);
            panel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            QuestPanels.Add(panel);
        }

        // Reset the UI
        EnemyPanel.SetActive(false);
        InfoText.enabled = false;
        DisableSaving();
    }

    public void SwitchToGame()
    {
        HUDPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
        ActionPanel.SlotA.AllowsDrag = false;
        ActionPanel.SlotB.AllowsDrag = false;
        ActionPanel.SlotC.AllowsDrag = false;
        ActionPanel.SlotD.AllowsDrag = false;
        ActionPanel.WeaponSlot.AllowsDrag = false;
        ActionPanel.Enabled = true;
        Time.timeScale = 1;
    }

    public void SwitchToMainMenu()
    {
        HUDPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
        ActionPanel.SlotA.AllowsDrag = true;
        ActionPanel.SlotB.AllowsDrag = true;
        ActionPanel.SlotC.AllowsDrag = true;
        ActionPanel.SlotD.AllowsDrag = true;
        ActionPanel.WeaponSlot.AllowsDrag = true;
        ActionPanel.Enabled = false;
        Time.timeScale = 0;
    }

    public void SwitchToStatsMenu()
    {
        StatsPanel.SetActive(true);
        QuestsPanel.SetActive(false);
    }

    public void SwitchToQuestsMenu()
    {
        QuestsPanel.SetActive(true);
        StatsPanel.SetActive(false);
    }

    public void UseAttributePoint(string attribute)
    {
        if (Player.AvailableAttributePoints > 0 && 
            Player.Stats.Dict[attribute].BaseValue != Player.Stats.Dict[attribute].MaxValue)
        {
            Player.Stats.Dict[attribute].BaseValue += 1;
            Player.AvailableAttributePoints -= 1;
            AvailableAttributePointsText.text = Player.AvailableAttributePoints.ToString();
        }
    }

    public void SaveGameState()
    {
        MainController.Instance.SaveGameState();
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1;
        MainController.Instance.LoadingText.text = "Loading Main Menu";
        MainController.SwitchScene(MainController.Instance.MainMenuScene);
    }

    public void EnableSaving()
    {
        SaveButton.SetActive(true);
    }

    public void DisableSaving()
    {
        SaveButton.SetActive(false);
    }

    private void UpdateLifeSlider(BaseAttribute sender, float value)
    {
        LifeSlider.minValue = sender.MinValue;
        LifeSlider.maxValue = sender.MaxValue;
        LifeSlider.value = value;
    }

    private void UpdateMagicSlider(BaseAttribute sender, float value)
    {
        MagicSlider.minValue = sender.MinValue;
        MagicSlider.maxValue = sender.MaxValue;
        MagicSlider.value = value;
    }

    private void UpdateEnemyLifeSlider(IFighter sender, int skillId)
    {
        if (sender.TargetedEnemy == null)
        {
            EnemyPanel.SetActive(false);
            return;
        }
        var character = (BaseCharacter)sender.TargetedEnemy;
        EnemyName.text = character.Name;
        EnemyLifeSlider.minValue = sender.TargetedEnemy.Life.MinValue;
        EnemyLifeSlider.maxValue = sender.TargetedEnemy.Life.MaxValue;
        EnemyLifeSlider.value = sender.TargetedEnemy.Life.Value;
    }

    private void ToggleEnemyPanel(ICharacter sender, IState previousState, IState newState)
    {
        if (newState is PlayerAttackState)
        {
            UpdateEnemyLifeSlider(Player, 0);
            EnemyPanel.SetActive(true);
        }
        else
        {
            EnemyPanel.SetActive(false);
        }
    }

    private void UpdateStats(BaseAttribute attribute, float value)
    {
        BodyValueText.text = Player.Stats.Body.Value.ToString();
        MindValueText.text = Player.Stats.Mind.Value.ToString();
        SoulValueText.text = Player.Stats.Soul.Value.ToString();

        ExperienceValueText.text = Player.Stats.Experience.Value.ToString();
        LevelValueText.text = Player.Stats.Level.Value.ToString();
        AttackRangeValueText.text = Player.Stats.AttackRange.Value.ToString();
        AttacksPerSecondValueText.text = (1f / Player.AttacksPerSecond).ToString(); 
        DamageValueText.text = Player.Damage.ToString();
        LifeRegenerationRateValueText.text = Player.Stats.LifeRegenerationRate.Value.ToString();
        MagicRegenerationRateValueText.text = Player.Stats.MagicRegenerationRate.Value.ToString();
        LifeText.text =Player.Stats.Life.MaxValue.ToString();
        MagicText.text = Player.Stats.Magic.MaxValue.ToString();
    }

    private void UpdateWeaponStats(int weaponId)
    {
        AttackRangeValueText.text = Player.Stats.AttackRange.Value.ToString();
        AttacksPerSecondValueText.text = (1f / Player.AttacksPerSecond).ToString();
        DamageValueText.text = Player.Damage.ToString();
    }

    private void NextLevelReached(BaseAttribute attribute, float value)
    {
        AvailableAttributePointsText.text = Player.AvailableAttributePoints.ToString();
        UpdateStats(attribute, value);
    }

    private void MagicSkillLearned(IMagicUser sender, int skillId)
    {
        if (skillId > -1)
        {
            BaseSkill skill = SkillDatabase.GetMagicSkillById(skillId);
            AddSkillPanel(skill);
        }
    }

    public void SlotChanged(Slot slot, SlottableItem newItem, SlottableItem oldItem)
    {
        if (!Array.Exists(ActionPanel.Slots, element => element == slot))
        {
            return;
        }
        int id = -1;
        if (newItem != null)
        {
            UsableItemData item = newItem.Item as UsableItemData;
            if (item != null)
            {
                id = item.Id;
            }
        }
        if (slot == ActionPanel.SlotA)
        {
            Player.EquippedItems[0] = id;
        }
        else if (slot == ActionPanel.SlotB)
        {
            Player.EquippedItems[1] = id;
        }
        else if (slot == ActionPanel.SlotC)
        {
            Player.EquippedItems[2] = id;
        }
        else if (slot == ActionPanel.SlotD)
        {
            Player.EquippedItems[3] = id;
        }
    }

    public void FoundItem(int itemId, int quantity)
    {
        if (InfoText.enabled)
        {
            var item = ItemDatabase.GetItemById(itemId).Name;
            if (!InfoText.text.Contains(item))
            {
                InfoText.text = String.Format("{0}, {1}", InfoText.text, item);
            }
        }
        else
        {
            InfoText.text = String.Format("Found: {0}", 
                                          ItemDatabase.GetItemById(itemId).Name);
            StartCoroutine("ShowInfoText");
        }
    }

    public void AddSkillPanel(BaseSkill skill)
    {
        var skillsPanel = (GameObject)Instantiate(SkillPanelPrefab, SkillsPanel.transform);
        skillsPanel.GetComponent<SkillPanel>().Initialize(skill);
    }

    public void UpdateQuestPanels()
    {
        foreach(QuestPanel panel in QuestPanels)
        {
            panel.UpdateDisplay();
        }
    }

    public IEnumerator ShowInfoText()
    {
        InfoText.enabled = true;
        float endTime = Time.time + 5;
        while (Time.time < endTime)
        {
            yield return null;
        }
        InfoText.enabled = false;
    }
}

