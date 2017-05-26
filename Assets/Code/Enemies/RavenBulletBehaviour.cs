using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RavenBulletBehaviour : MonoBehaviour {

    float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.down * speed * Time.deltaTime, Space.Self);
	}

    public void SetSpeed(float s)
    {
        speed = s;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = Instantiate(GameCore.Instance.bulletExplosion, collision.gameObject.transform.position, Quaternion.identity);
        Destroy (gameObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "KillEnemy")
        {
            GameObject go = Instantiate(GameCore.Instance.bulletExplosion, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
    }
}
