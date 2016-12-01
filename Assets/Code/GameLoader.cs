using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour {

    private enum LogoState {IN, STAY, OUT }
    private LogoState logoState;

    private int iterator;

    private float counter, maxTimeTransition;

    public Transform[] logos;
	// Use this for initialization
	void Start ()
    {
        logoState = LogoState.IN;
        maxTimeTransition = 0.5f;
        counter = maxTimeTransition;
        iterator = 0;
    }
	
	// Update is called once per frame
	void Update () {

        
        Debug.Log(logoState);
        Debug.Log(iterator);

        switch (logoState)
        {
            case LogoState.IN:
                {
                    logos[iterator].GetComponent<Image>().color = new Color(logos[iterator].GetComponent<Image>().color.r, logos[iterator].GetComponent<Image>().color.g, logos[iterator].GetComponent<Image>().color.b, logos[iterator].GetComponent<Image>().color.a + Time.deltaTime* maxTimeTransition);
                    if (logos[iterator].GetComponent<Image>().color.a >= 1) logoState = LogoState.STAY;
                }
                break;
            case LogoState.STAY:
                {
                    counter -= Time.deltaTime;
                    if (counter <= 0)
                    {
                        logoState = LogoState.OUT;
                        counter = maxTimeTransition;
                    }
                    
                }
                break;
            case LogoState.OUT:
                {
                    logos[iterator].GetComponent<Image>().color = new Color(logos[iterator].GetComponent<Image>().color.r, logos[iterator].GetComponent<Image>().color.g, logos[iterator].GetComponent<Image>().color.b, logos[iterator].GetComponent<Image>().color.a - Time.deltaTime* maxTimeTransition);
                    if (logos[iterator].GetComponent<Image>().color.a <= 0)
                    {
                        iterator++;
                        logoState = LogoState.IN;
                    }
                }
                break;
            default:
                break;
        }

        if (iterator == logos.Length) SceneManager.LoadScene(1);
    }
}
