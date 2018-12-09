using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MenuManager : MonoBehaviour
{

    private static MenuManager _instance;

    public event Action onReadyChange;

    public static MenuManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MenuManager>();
            }

            return _instance;
        }
    }

    private Screens _currentScreen;

    private void Start()
    {
        DontDestroyOnLoad(this);
        _currentScreen = Screens.MAIN_MENU;
    }

    public void SwitchScreen(Screens screen)
    {
        ChangeScreen(screen);
        Debug.Log(screen);
    }

    private void ChangeScreen(Screens screen)
    {
        _currentScreen = screen;

        switch (_currentScreen)
        {
            case Screens.MAIN_MENU:
                MainMenu();
                break;
            case Screens.SINGLEPLAYER:
                SinglePlayer();
                break;
            case Screens.MULTIPLAYER_MENU:
                MultiPlayerMenu();
                break;
            case Screens.MULTIPLAYER:
                Multiplayer();
                break;
            default:
                MainMenu();
                break;
        }
    }

    private void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        if (onReadyChange != null)
        {
            onReadyChange();
        }
    }

    private void SinglePlayer()
    {
        StartCoroutine(LoadAsyncSinglePlayer());

    }

    public void ExitToMenu()
    {
        SwitchScreen(Screens.MAIN_MENU);

    }

    private IEnumerator LoadAsyncSinglePlayer()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("spLevelOne");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private void MultiPlayerMenu()
    {
        SceneManager.LoadScene("MultiplayerMenu");
    }

    private void Multiplayer()
    {
        
    }

    public Screens GetGameState()
    {
        return _currentScreen;
    }
}

public enum Screens
{
    MAIN_MENU,
    SINGLEPLAYER,
    MULTIPLAYER_MENU,
    MULTIPLAYER
}
