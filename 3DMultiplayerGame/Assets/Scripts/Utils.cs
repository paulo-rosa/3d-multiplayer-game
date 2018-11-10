using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils  {

    public static bool CompareLayer(LayerMask layer, int objLayer)
    {
        if ((layer | (1 << objLayer)) == layer)
        {
            return true;
        }
        return false;
    }
}
