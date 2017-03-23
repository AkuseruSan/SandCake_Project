using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenEnemy : BaseEnemyBehaviour
{

    float bulletSpeed;
    float separation;
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
                    separation = 10; 
                    dmg = 5;
                    bulletDir = new Vector2(-0.8f, 0);
                    ctr = Random.Range(1, 3);
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
                    ctr = Random.Range(1, 3);

                    //LOGIC
                    SpawnBullets(10);

                    state = States.IDDLE;
                }
                break;
            default:
                break;
        }

        ctr -= Time.deltaTime;
    }

    void SpawnBullets(int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject go = Instantiate(Resources.Load("Prefabs/Enemies/Bullet"), transform.position, Quaternion.identity) as GameObject;
            go.transform.eulerAngles = new Vector3(0,0,(size * separation)/2 - i * separation);
            go.GetComponent<RavenBulletBehaviour>().SetSpeed(5.0f);

            Destroy(go, Time.deltaTime * 240);
        }
    }
}
