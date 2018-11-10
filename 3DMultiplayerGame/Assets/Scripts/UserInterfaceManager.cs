using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour
{

    private static UserInterfaceManager _instance;

    public static UserInterfaceManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<UserInterfaceManager>();
            }
            return _instance;
        }
    }
    public Text _scoreTxt;
    public GameObject _lifesHolder;
    public Sprite _lifeHeart;
    public Sprite _noLifeHeart;
    public GameObject GamePanel;
    public GameObject MenuPanel;
    public GameObject GameOverPanel;

    private GameObject _currentPanel;
    private GameManager _gameManager;
    private Image[] _lifes = new Image[3];

	private void Awake ()
    {
        GetImages();
        _gameManager = GameManager.Instance;
        AssignToEvents();
    }

    private void Start()
    {

    }

    private void AssignToEvents()
    {
        _gameManager.onStateChange += StateChanged;
    }

    private void StateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.MENU:
                ChangePanel(MenuPanel);
                break;
            case GameState.GAME:
                ChangePanel(GamePanel);
                break;
            case GameState.GAME_OVER:
                ChangePanel(GameOverPanel);
                break;
        }
    }

    //Troca o painel ativo na tela
    private void ChangePanel(GameObject panel)
    {
        if(_currentPanel != null)
        {
            _currentPanel.SetActive(false);
        }
        _currentPanel = panel;
        _currentPanel.SetActive(true);
    }

    #region Game State
    //Atualiza a pontuação do player
    public void UpdateScore(int score)
    {
        _scoreTxt.text = score.ToString();
    }

    //Atualiza a quantidade de vidas do player
    public void UpdateLifes(int lifes)
    {
        for(int i = 0; i < _lifes.Length; i++)
        {
            if(i < lifes)
            {
                _lifes[i].GetComponent<Image>().sprite = _lifeHeart;
            }
            else
            {
                _lifes[i].GetComponent<Image>().sprite = _noLifeHeart;
            }
        }
    }
    #endregion

    #region GameOver State
    
    #endregion

    //Inicia o vetor de imagens da vida
    private void GetImages()
    {
        for(int i = 0; i < _lifes.Length; i++)
        {
            _lifes[i] = _lifesHolder.transform.GetChild(i).GetComponent<Image>();
        }
    }
}
