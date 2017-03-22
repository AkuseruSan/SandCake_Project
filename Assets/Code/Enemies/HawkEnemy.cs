using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkEnemy : BaseEnemyBehaviour
{

    private Vector3 dir;

    void Start()
    {
        target = GameCore.Instance.player.transform;
        dir = target.position - transform.position;
        dir.Normalize();
    }
	// Update is called once per frame
	void Update () {
        transform.Translate(dir);
	}
}
