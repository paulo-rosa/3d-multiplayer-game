using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorPool : MonoBehaviour {

    public List<GameObject> _meteorList = new List<GameObject>();
    public GameObject MeteorObj;

    private int _currentIndex;
    private int _poolSize = 20;

    void Start()
    {
        StartPool();
    }

    private void StartPool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            var go = Instantiate(MeteorObj);
            go.transform.parent = this.transform;
            go.SetActive(false);
            _meteorList.Add(go);
        }
    }

    public GameObject GetObject()
    {
        var obj = _meteorList[_currentIndex];
        IncrementIndex();
        //é copia ou o objeto em si?
        obj.SetActive(true);
        return obj;
    }

    private void IncrementIndex()
    {
        _currentIndex++;
        _currentIndex = _currentIndex % _poolSize;
    }

    private void ResetObject()
    {

    }
}
