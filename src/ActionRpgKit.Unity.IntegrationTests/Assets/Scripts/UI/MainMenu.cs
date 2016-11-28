using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public void StartNewGame()
    {
        MainController.Instance.StartNewGame();
    }


    public void LoadGameState()
    {
        MainController.Instance.LoadGameState();
    }
}
