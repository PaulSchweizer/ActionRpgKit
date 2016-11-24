using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Control the switching between scenes and the loading and saving of game states.</summary>
public class MainController : MonoBehaviour
{
    /// <summary>
    /// To keep the Controller a Singleton</summary>
    public static MainController Instance;

    /// <summary>
    /// The name of the starting scene for the Game.</summary>
    public string StartScene;
    public GameObject PlayerPrefab;
    public GameObject GameMenuPrefab;
    public GameObject CameraRigPrefab;

    /// <summary>
    /// The list of actual Game Scenes as opposed to other scenes that do not contain GamePlay.</summary>
    public string[] GameScenes;

    // Fade out
    public Canvas FadingCanvas;
    public Image FadingScreen;
    public float FadingSpeed = 3;
    public Text LoadingText;

    // From the Tutorial
    public string CurrentSceneName;
    public string NextSceneName;
    private AsyncOperation resourceUnloadTask;
    private AsyncOperation sceneLoadTask;
    private enum SceneState { FadeOut, Reset, Preload, Load, Unload, Postload, Ready, Run, FadeIn, Count };
    private SceneState sceneState;
    private delegate void UpdateDelegate();
    private UpdateDelegate[] updateDelegates;

    /// <summary>
    /// Switch the scene to the given scene name.</summary>
    /// <param name="nextSceneName"></param>
    public static void SwitchScene(string nextSceneName)
    {
        if (Instance != null)
        {
            if (Instance.CurrentSceneName != nextSceneName)
            {
                Instance.NextSceneName = nextSceneName;
            }
        }
    }

    /// <summary>
    /// Keep the Controller a Singleton.</summary>
    protected void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }

        updateDelegates = new UpdateDelegate[(int)SceneState.Count];
        updateDelegates[(int)SceneState.FadeOut] = UpdateSceneFadeOut;
        updateDelegates[(int)SceneState.Reset] = UpdateSceneReset;
        updateDelegates[(int)SceneState.Preload] = UpdateScenePreload;
        updateDelegates[(int)SceneState.Load] = UpdateSceneLoad;
        updateDelegates[(int)SceneState.Unload] = UpdateSceneUnload;
        updateDelegates[(int)SceneState.Postload] = UpdateScenePostload;
        updateDelegates[(int)SceneState.Ready] = UpdateSceneReady;
        updateDelegates[(int)SceneState.FadeIn] = UpdateSceneFadeIn;
        updateDelegates[(int)SceneState.Run] = UpdateSceneRun;

        sceneState = SceneState.Ready;
    }

    /// <summary>
    /// Delete the delegates and the mainController statics.</summary>
    protected void OnDestroy()
    {
        if (updateDelegates != null)
        {
            for (int i = 0; i < (int)SceneState.Count; i++)
            {
                updateDelegates[i] = null;
            }
            updateDelegates = null;
        }
    }

    #region SceneManagement

    /// <summary>
    /// Update the current Delegate.</summary>
    protected void Update()
    {
        if (updateDelegates[(int)sceneState] != null)
        {
            updateDelegates[(int)sceneState]();
        }
    }

    /// <summary>
    /// Fade out the level.</summary>
    private void UpdateSceneFadeOut()
    {
        if (FadingScreen.color.a < 1)
        {
            FadingCanvas.sortingOrder = 999;
            FadingCanvas.enabled = true;
            FadingScreen.color = new Color(0, 0, 0, FadingScreen.color.a + Time.deltaTime * FadingSpeed);
        }
        else
        {
            LoadingText.enabled = true;
            sceneState = SceneState.Reset;
        }
    }

    /// <summary>
    /// Attach the new scene controller to start cascade of loading.</summary>
    private void UpdateSceneReset()
    {
        GC.Collect();
        sceneState = SceneState.Preload;
    }

    /// <summary>
    /// Handle anything that needs to happen before loading.</summary>
    private void UpdateScenePreload()
    {
        sceneLoadTask = SceneManager.LoadSceneAsync(NextSceneName);
        sceneState = SceneState.Load;
    }

    /// <summary>
    /// Show the loading screen until it's loaded.</summary>
    private void UpdateSceneLoad()
    {
        if (sceneLoadTask.isDone)
        {
            sceneState = SceneState.Unload;
        }
        else
        {
            // Update some scene loading progress bar
        }
    }

    /// <summary>
    /// Clean up unused resources by unloading them.</summary>
    private void UpdateSceneUnload()
    {
        if (resourceUnloadTask == null)
        {
            resourceUnloadTask = Resources.UnloadUnusedAssets();
        }
        else
        {
            if (resourceUnloadTask.isDone)
            {
                resourceUnloadTask = null;
                sceneState = SceneState.Postload;
            }
        }
    }

    /// <summary>
    /// Handle anything that needs to happen immediately after loading.</summary>
    private void UpdateScenePostload()
    {
        CurrentSceneName = NextSceneName;
        sceneState = SceneState.Ready;
    }

    /// <summary>
    /// Handle anything that needs to happen immediately before running.
    /// </summary>
    private void UpdateSceneReady()
    {
        // Reset the ActionRpgKit Controller
        ActionRpgKitController.Instance.Reset();

        // TODO
        // TODO Instantiate an Event System??? 
        // TODO Or attach it to the maincontroller itself???
        // TODO

        // Init the Player and other game objects
        if (Array.Exists(GameScenes, element => element == NextSceneName))
        {
            // Instantiate the Player
            if (GamePlayer.Instance == null)
            {
                Instantiate(PlayerPrefab);
            }

            // Instantiate the PlayerMenu 
            if (GameMenu.Instance == null)
            {
                Instantiate(GameMenuPrefab);
                GameMenu.Instance.SwitchToGame();
            }

            // Instantiate the CameraRig
            if (CameraRig.Instance == null)
            {
                // Get all cameras in the scene and disable them
                foreach(Camera camera in FindObjectsOfType<Camera>())
                {
                    camera.gameObject.SetActive(false);
                }

                // Get all audio listeners in the scene and disable them
                foreach (AudioListener audio in FindObjectsOfType<AudioListener>())
                {
                    audio.gameObject.SetActive(false);
                }

                Instantiate(CameraRigPrefab);
                CameraRig.Instance.Target = GamePlayer.Instance.transform;
                CameraRig.Instance.Update();
            }

            // Initialize the ActionRpgKit Controller
            ActionRpgKitController.Instance.Initialize();
        }


        // The Game is now ready to run
        sceneState = SceneState.FadeIn;
    }

    /// <summary>
    /// Fade out the level.</summary>
    private void UpdateSceneFadeIn()
    {
        LoadingText.enabled = false;
        if (FadingScreen.color.a > 0)
        {
            FadingScreen.color = new Color(0, 0, 0, FadingScreen.color.a - Time.deltaTime * FadingSpeed);
        }
        else
        {
            FadingCanvas.sortingOrder = 0;
            FadingCanvas.enabled = false;
            sceneState = SceneState.Run;
        }
    }

    /// <summary>
    /// Wait for scene change.</summary>
    private void UpdateSceneRun()
    {
        if (CurrentSceneName != NextSceneName)
        {
            sceneState = SceneState.FadeOut;
        }
    }

    #endregion

    #region Actions

    /// <summary>
    /// Start a new game by switching to the starting scene.</summary>
    public void StartNewGame()
    {
        SwitchScene(StartScene);
    }

    #endregion

}
