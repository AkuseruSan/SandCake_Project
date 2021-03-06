﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public Vector2 randomSpawnTimeRange;
    private float counter;
    public List<string> enemyPrefabPaths;
    protected float spawnHeight = 5;
    // Use this for initialization
    void Start () {
        counter = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {

        switch (GameCore.Instance.gameState)
        {
            case GameState.AWAKE:
                break;
            case GameState.PAUSE:
                break;
            case GameState.PLAY:
                {
                    if (counter <= 0 && GameCore.Instance.currentStage != WorldConstructor.Stage.Z_BOSS)
                    {
                        counter = Random.Range(randomSpawnTimeRange.x, randomSpawnTimeRange.y);
                        spawnHeight = Random.Range(3.0f, 7.0f);

                        //Debug.Log(GameCore.Instance.player.transform.position);
                        Vector3 playerDistance = new Vector3(GameCore.Instance.player.transform.position.x + 30, 10, 0);
                        Vector3 spawnPosition = AuxLib.SetPositionOnRaycastHit2D(playerDistance, "Terrain", Vector2.down, spawnHeight);

                        if(spawnPosition.y >= 7)
                        {
                            counter = 0;
                        }
                        else
                        {
                            GameObject go = Instantiate(Resources.Load("Prefabs/Enemies/Raven"), spawnPosition, Quaternion.identity) as GameObject;

                        }

                    }

                    counter -= Time.deltaTime;
                }
                break;
            case GameState.GAMEOVER:
                break;
            default:
                break;
        }
	}

    public void Spawn(string path, Vector3 startPos)
    {
        GameObject go;

        go = Instantiate(Resources.Load(path), AuxLib.SetPositionOnRaycastHit2D(startPos, "Terrain", Vector2.down, spawnHeight), Quaternion.identity) as GameObject;
        //Debug.Log("Initial Position Assigned: "+startPos);
    }
}
