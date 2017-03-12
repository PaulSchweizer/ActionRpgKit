using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class StoryViewer : MonoBehaviour
{

    public static StoryViewer Instance;
    public static StyleStruct Style = new StyleStruct(0, 1);

    public Text Headline;
    public Text Content;
    public Text Header;
    public Text XP;
    public Image Portrait;

    private bool _showNext;
    private string _savedHeadline;
    private string _savedContent;
    private int _savedStyle;
    private int _savedExperience;
    private bool _savedPause;

    private List<MessageStruct> _messages = new List<MessageStruct>();

    public void Awake()
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

    /// <summary>
    /// Push a new message on the stack
    /// </summary>
    public void PushMessage(MessageStruct message)
    {
        _messages.Add(message);
        if (!gameObject.active)
        {
            Show();
        }
    }

    public void Show()
    {
        var message = _messages[0];
        Headline.text = message.Headline;
        Content.text = message.Content;
        if (message.Experience == 0)
        {
            XP.enabled = false;
        }
        else
        {
            XP.enabled = true;
            XP.text = String.Format("+{0} XP", message.Experience);
        }

        Portrait.sprite = message.Portrait;

        if (message.Audio != null)
        {
            AudioControl.Instance.PlayText(message.Audio);
        }

        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Hide()
    {
        if (_messages.Count > 0)
        {
            if (_messages[0].Audio != null)
            {
                AudioControl.Instance.StopText();
            }

            _messages.Remove(_messages[0]);
            if (_messages.Count > 0) {
                Show();
                return;
            }
        }
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}


public struct StyleStruct
{
    public int Started;
    public int Completed;

    public StyleStruct(int started, int completed)
    {
        Started = started;
        Completed = completed;
    }
}


public struct MessageStruct
{
    public string Headline;
    public string Content;
    public int Experience;
    public int Style;
    public Sprite Portrait;
    public AudioClip Audio;

    public MessageStruct(string headline, string content, int style=0, int experience=0, Sprite portrait=null, AudioClip audio=null)
    {
        Headline = headline;
        Content = content;
        Style = style;
        Experience = experience;
        Portrait = portrait;
        Audio = audio;
    }
}