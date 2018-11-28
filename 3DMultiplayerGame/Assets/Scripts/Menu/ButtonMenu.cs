using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMenu : MonoBehaviour {

    public SCREENS DestinyScreen;


    public void SwitchScreen()
    {
        MenuManager.Instance.SwitchScreen(DestinyScreen);
    }
}
