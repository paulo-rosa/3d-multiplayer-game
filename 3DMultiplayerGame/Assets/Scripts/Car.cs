using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Car : MonoBehaviour
{
    public static Car instance;
    public new Transform transform;

    void Awake()
    {
        instance = this;
    }
}
