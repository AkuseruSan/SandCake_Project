using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenEnemy : BaseEnemyBehaviour
{

    float bulletSpeed;
    Vector2 bulletDir;
    float ctr;

    public enum States { INIT, IDDLE, ATTACK };
    public States state = States.INIT;

    // Update is called once per frame
    void Update () {
        switch (state)
        {
            case States.INIT:
                {
                    spawnHeight = 5;
                    bulletDir = new Vector2(-0.8f, 0);
                    ctr = Random.Range(1, 5);
                    state = States.IDDLE;
                }
                break;
            case States.IDDLE:
                {
                    if (ctr <= 0) state = States.ATTACK;
                }
                break;
            case States.ATTACK:
                {
                    ctr = Random.Range(1, 5);

                    //LOGIC
                    SpawnBullets(new Vector2(0,0));

                    state = States.IDDLE;
                }
                break;
            default:
                break;
        }

        ctr -= Time.deltaTime;
    }

    void SpawnBullets(Vector2 size)
    {
        //for(int i = 0; i < size.x; i++)
        //{

        //}

        GameObject go = Instantiate(Resources.Load("Prefabs/Enemies/Bullet"), transform.position, Quaternion.identity) as GameObject;
        go.GetComponent<RavenBulletBehaviour>().SetSpeed(1);
    }
}
