using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    private static MenuManager _instance;

    public static MenuManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<MenuManager>();
            }

            return _instance;
        }
    }

    private Screens _currentScreen;

	private void Start ()
    {
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
    }

    private void SinglePlayer()
    {
        StartCoroutine(LoadAsyncSinglePlayer());
       
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
        SceneManager.LoadScene("Multiplayer");
    }
}

public enum Screens
{
    MAIN_MENU,
    SINGLEPLAYER,
    MULTIPLAYER_MENU,
    MULTIPLAYER
}
