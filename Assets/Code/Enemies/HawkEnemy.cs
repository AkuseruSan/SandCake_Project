using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkEnemy : BaseEnemyBehaviour
{

    public enum States { };
    public States state;
    private Vector3 dir;
    
    void Start()
    {
        target = GameCore.Instance.player.transform;
        dir = target.position - transform.position;
        dir.Normalize();

        dmg = 5;
    }
	// Update is called once per frame
	void Update () {
        transform.Translate(dir);
        
	}
}
