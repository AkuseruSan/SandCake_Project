using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public Vector2 randomSpawnTimeRange;
    private float counter;
    public List<string> enemyPrefabPaths;
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
                    if (counter <= 0)
                    {
                        counter = Random.Range(randomSpawnTimeRange.x, randomSpawnTimeRange.y);

                        Debug.Log(GameCore.Instance.player.transform.position);
                        Spawn("Prefabs/Enemies/Hawk", GameCore.Instance.player.transform.position + new Vector3(40, 10, 0));
                        Spawn("Prefabs/Enemies/Raven", GameCore.Instance.player.transform.position + new Vector3(40, 10, 0));
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
        go = Instantiate(Resources.Load(path) as GameObject);
        go.transform.position = startPos;
    }
}
