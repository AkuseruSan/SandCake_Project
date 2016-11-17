using UnityEngine;
using System.Collections;


public class CS_RenderBlend : MonoBehaviour
{
    private Camera sourceCam;
    private Texture2D sourceRender, destinationRender;

    public Camera depthCam, destCam;
    public RenderTexture renderTexture;

    void Start()
    {
        depthCam.depthTextureMode = DepthTextureMode.Depth;
    }

    void OnPreRender()
    {
        destCam.SetTargetBuffers(renderTexture.colorBuffer, renderTexture.depthBuffer);
        //Resources.Load("RenderTex");
    }
    void Update()
    {

    }
}