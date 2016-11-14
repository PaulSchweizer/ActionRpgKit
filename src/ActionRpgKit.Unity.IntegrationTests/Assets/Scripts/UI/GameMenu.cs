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

        // Set the initial values
        UpdateLifeSlider(GamePlayer.Instance.Character.Stats.Life, GamePlayer.Instance.Character.Stats.Life.Value);
        UpdateMagicSlider(GamePlayer.Instance.Character.Stats.Magic, GamePlayer.Instance.Character.Stats.Magic.Value);
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

}

