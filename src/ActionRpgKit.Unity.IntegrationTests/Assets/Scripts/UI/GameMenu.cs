using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using ActionRpgKit.Character;
using ActionRpgKit.Character.Attribute;

public class GameMenu : MonoBehaviour
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
    /// Slider showing the Player's targeted Enemy.</summary>
    public Slider EnemyLifeSlider;

    /// <summary>
    /// Text displaying the Enemy.</summary>
    public Text EnemyName;

    /// <summary>
    /// Text displaying the Enemy.</summary>
    public GameObject EnemyPanel;

    public GameObject HUDPanel;
    public GameObject MainMenuPanel;
    public Text BodyValueText;
    public Text MindValueText;
    public Text SoulValueText;

    public Text ExperienceValueText;
    public Text LevelValueText;
    public Text AttackRangeValueText;
    public Text AttacksPerSecondValueText;
    public Text DamageValueText;
    public Text MagicRegenerationRateValueText;

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
        // Connect the Signals from the Player
        GamePlayer.Instance.Character.Stats.Life.OnValueChanged += new ValueChangedHandler(UpdateLifeSlider);
        GamePlayer.Instance.Character.Stats.Magic.OnValueChanged += new ValueChangedHandler(UpdateMagicSlider);
        GamePlayer.Instance.Character.OnCombatSkillUsed += new CombatSkillUsedHandler(UpdateEnemyLifeSlider);
        GamePlayer.Instance.Character.OnStateChanged += new StateChangedHandler(ToggleEnemyPanel);
        GamePlayer.Instance.Character.Stats.Body.OnValueChanged += new ValueChangedHandler(UpdateStats);
        GamePlayer.Instance.Character.Stats.Mind.OnValueChanged += new ValueChangedHandler(UpdateStats);
        GamePlayer.Instance.Character.Stats.Soul.OnValueChanged += new ValueChangedHandler(UpdateStats);
        GamePlayer.Instance.Character.OnWeaponEquipped += new WeaponEquippedHandler(UpdateWeaponStats);

        // Set the initial values
        UpdateLifeSlider(GamePlayer.Instance.Character.Stats.Life, GamePlayer.Instance.Character.Stats.Life.Value);
        UpdateMagicSlider(GamePlayer.Instance.Character.Stats.Magic, GamePlayer.Instance.Character.Stats.Magic.Value);
        UpdateStats(GamePlayer.Instance.Character.Stats.Body, GamePlayer.Instance.Character.Stats.Body.Value);
    }

    public void SwitchToGame()
    {
        HUDPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void SwitchToMainMenu()
    {
        HUDPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
        Time.timeScale = 0;
    }

    void UpdateLifeSlider(BaseAttribute sender, float value)
    {
        LifeSlider.minValue = sender.MinValue;
        LifeSlider.maxValue = sender.MaxValue;
        LifeSlider.value = value;
    }

    void UpdateMagicSlider(BaseAttribute sender, float value)
    {
        MagicSlider.minValue = sender.MinValue;
        MagicSlider.maxValue = sender.MaxValue;
        MagicSlider.value = value;
    }

    void UpdateEnemyLifeSlider(IFighter sender, int skillId)
    {
        EnemyName.text = sender.TargetedEnemy.Name;
        EnemyLifeSlider.minValue = sender.TargetedEnemy.Life.MinValue;
        EnemyLifeSlider.maxValue = sender.TargetedEnemy.Life.MaxValue;
        EnemyLifeSlider.value = sender.TargetedEnemy.Life.Value;
    }

    void ToggleEnemyPanel(ICharacter sender, IState previousState, IState newState)
    {
        if (newState is AttackState)
        {
            UpdateEnemyLifeSlider(GamePlayer.Instance.Character, 0);
            EnemyPanel.SetActive(true);
        }
        else
        {
            EnemyPanel.SetActive(false);
        }
    }

    void UpdateStats(BaseAttribute sender, float value)
    {
        BodyValueText.text = GamePlayer.Instance.Character.Stats.Body.Value.ToString();
        MindValueText.text = GamePlayer.Instance.Character.Stats.Mind.Value.ToString();
        SoulValueText.text = GamePlayer.Instance.Character.Stats.Soul.Value.ToString();

        ExperienceValueText.text = GamePlayer.Instance.Character.Stats.Experience.Value.ToString();
        LevelValueText.text = GamePlayer.Instance.Character.Stats.Level.Value.ToString();
        AttackRangeValueText.text = GamePlayer.Instance.Character.Stats.AttackRange.Value.ToString();
        AttacksPerSecondValueText.text = GamePlayer.Instance.Character.AttacksPerSecond.ToString();
        DamageValueText.text = GamePlayer.Instance.Character.Damage.ToString();
        MagicRegenerationRateValueText.text = GamePlayer.Instance.Character.Stats.MagicRegenerationRate.Value.ToString();
    }

    void UpdateWeaponStats(int weaponId)
    {
        Debug.Log(weaponId);
        AttackRangeValueText.text = GamePlayer.Instance.Character.Stats.AttackRange.Value.ToString();
        AttacksPerSecondValueText.text = GamePlayer.Instance.Character.AttacksPerSecond.ToString();
        DamageValueText.text = GamePlayer.Instance.Character.Damage.ToString();
    }
}

