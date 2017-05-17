using UnityEngine;
using System.Collections;

public class ParallaxLayerBehaviour : MonoBehaviour {

    public float movementSpeed;
    private float movementSpeedAux;

    private Transform player;

    private Vector2 animatedOffset;


    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {
        GetComponent<MeshRenderer>().sortingOrder = -100;
        animatedOffset = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameCore.Instance.gameState == GameState.PLAY)
        {
            movementSpeedAux = movementSpeed * AuxLib.Map(GameCore.Instance.playerController.rBody.velocity.x, GameCore.Instance.playerController.minSpeed, GameCore.Instance.playerController.maxSpeed, 0, 1);
            animatedOffset += new Vector2(movementSpeedAux, 0) * Time.deltaTime;


            this.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", animatedOffset);
        }
    }
}
