using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public enum WorldModuleConnect { TOP = 0, MIDDLE = 1, BOTTOM = 2 }

[System.Serializable]
public enum WorldModuleType { VOID = 0, SIMPLE_JUMP = 1, SIMPLE_PAINT = 2, COMPLEX_PAINT = 3, INDIRECT_PAINT = 4 }

public static class AuxLib {



    public static float Map(float value, float minIn, float maxIn, float minOut, float maxOut)
    {
        return minOut + (value - minIn) * (maxOut - minOut) / (maxIn - minIn);
    }

    public static Vector3 SetPositionOnRaycastHit2D(Vector3 pos, string tg, Vector2 dir, float height)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, dir, 100);
        if (hit.collider != null)
        {

            if (hit.collider.tag == tg)
            {
                //Debug.Log("Found Terrain: " + hit.transform.name);
                //Debug.Log("Raycast hitpoint y:" + hit.point.y);

                pos = new Vector3(pos.x, hit.point.y + height, pos.z);
            }
        }

        return pos;
    }
}

[System.Serializable]
public struct DualTexture
{
    public float speed;
    public Texture day;
    public Texture night;
    public ParallaxController.ParallaxLayerOrder order;
}

[System.Serializable]
public class WorldModuleData
{
    public WorldModuleConnect beginConnection;
    public WorldModuleConnect endConnection;
    public GameObject module;

    public WorldModuleData(WorldModuleConnect b, WorldModuleConnect e, GameObject m)
    {
        beginConnection = b;
        endConnection = e;
        module = m;
    }
}

[System.Serializable]
public struct WorldDictionaryList
{
    public WorldModuleType type;
    public List<WorldModuleData> worldModules;
}