using UnityEngine;
using System.Collections;



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
