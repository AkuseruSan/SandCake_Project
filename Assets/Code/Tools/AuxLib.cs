using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WorldModuleConnect { VOID = 0, TOP = 1, MIDDLE = 2, BOTTOM = 3 }

public static class AuxLib {

    public static float Map(float value, float minIn, float minOut, float maxIn, float maxOut)
    {
        return maxIn + (value - minIn) * (maxOut - maxIn) / (minOut - minIn);
    }

}

[System.Serializable]
public struct DualTexture
{
    public Texture day;
    public Texture night;
    public ParallaxController.ParallaxLayerOrder order;
}

[System.Serializable]
public struct WorldModuleData
{
    public WorldModuleConnect beginConnection;
    public WorldModuleConnect endConnection;
    public GameObject module;
}