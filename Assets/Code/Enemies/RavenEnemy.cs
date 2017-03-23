using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenEnemy : BaseEnemyBehaviour
{

    public enum States { INIT, IDDLE, ATTACK };
    public States state = States.INIT;

    // Update is called once per frame
    void Update () {
        switch (state)
        {
            case States.INIT:
                {

                }
                break;
            case States.IDDLE:
                {

                }
                break;
            case States.ATTACK:
                {

                }
                break;
            default:
                break;
        }
    }
}
