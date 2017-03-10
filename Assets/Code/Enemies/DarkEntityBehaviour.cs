using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkEntityBehaviour : MonoBehaviour {

    private float intensity;
    public float lerpSmooth;
    public float speed;
    public float xMin, xMax;
	// Use this for initialization
	void Start () {
        speed = Mathf.Clamp(speed, 0, 1);
        intensity = 0;


	}
	
	// Update is called once per frame
	void Update () {
        intensity += Time.deltaTime * speed;

        intensity = Mathf.Clamp(intensity, 0, 1);

        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(AuxLib.Map(intensity, 0, 1, xMin, xMax), transform.localPosition.y, transform.localPosition.z), Time.deltaTime * lerpSmooth);
    }

    public void RecieveDamage(int percentDamage)
    {
        intensity -= AuxLib.Map(percentDamage, 0, 100, 0, 1);
    }

    public void Attack()
    {

    }
}
