using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMenu : MonoBehaviour {

    public Screens DestinyScreen;


    public void SwitchScreen()
    {
        MenuManager.Instance.SwitchScreen(DestinyScreen);
    }
}
