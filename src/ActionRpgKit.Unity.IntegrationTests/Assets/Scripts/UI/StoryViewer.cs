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

    public void Show(string headline, string content, float time, int style, int experience = 0)
    {
        if (style == Style.Started)
        {
            Header.text = "New Quest";
            XP.enabled = false;
        }
        else if (style == Style.Completed)
        {
            Header.text = "Quest Completed!";
            XP.text = String.Format("+ {0} XP", experience);
            XP.enabled = true;
        }
        Headline.text = headline;
        Content.text = content;
        gameObject.SetActive(true);
        StopCoroutine("Display");
        StartCoroutine(Display(time));
    }

    public void Hide()
    {
        StopCoroutine("Display");
        gameObject.SetActive(false);
    }

    public IEnumerator Display(float time)
    {
        float endTime = Time.time + time;
        while (Time.time < endTime)
        {
            yield return null;
        }
        Hide();
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