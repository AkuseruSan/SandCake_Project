using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkEnemy : BaseEnemyBehaviour
{

    public enum States { INIT, IDDLE, ATTACK };
    public States state = States.INIT;
    private Vector3 dir;
    

	// Update is called once per frame
	void Update () {

        switch (state)
        {
            case States.INIT:
                {

                    spawnHeight = 10;
                    target = GameCore.Instance.player.transform;
                    dir = target.position - transform.position;
                    dir.Normalize();

                    dmg = 5;

                    state = States.IDDLE;
                }
                break;
            case States.IDDLE:
                {
                    state = States.ATTACK;
                }
                break;
            case States.ATTACK:
                {
                    Debug.Log("Enemy Position: " + transform.position);
                    //transform.Translate(dir);
                }
                break;
            default:
                break;
        }
        
	}
}
