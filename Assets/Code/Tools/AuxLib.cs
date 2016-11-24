using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WorldModuleConnect { TOP = 0, MIDDLE = 1, BOTTOM = 2 }
public enum WorldModuleType { VOID = 0, SIMPLE_JUMP = 1, SIMPLE_PAINT = 2, COMPLEX_PAINT = 3, INDIRECT_PAINT = 4 }

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
public class WorldModuleData
{
    public WorldModuleType type;
    public WorldModuleConnect beginConnection;
    public WorldModuleConnect endConnection;
    public GameObject module;

    public WorldModuleData(WorldModuleType t, WorldModuleConnect b, WorldModuleConnect e, GameObject m)
    {
        type = t;
        beginConnection = b;
        endConnection = e;
        module = m;
    }
}