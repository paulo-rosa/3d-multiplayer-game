using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour {

    public LayerMask Layer;

    private GameManager _gameManager;

	private void Start ()
    {
        _gameManager = GameManager.Instance;
	}

    private void OnTriggerEnter(Collider other)
    {
        if(_gameManager.GetState() == GameState.GAME)
        if(Utils.CompareLayer(Layer, other.gameObject.layer))
        {
            _gameManager.EndLevel();
        }
    }
}
