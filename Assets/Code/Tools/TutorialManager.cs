using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialManager : MonoBehaviour {

    public GameObject frameStructure;
    public Button nextButton;
    private int currentFrame, nextFrame;
    private Image currentImage;
    public GameObject tutorialCore;

    public List<Sprite> tutorialFramesList;

	// Use this for initialization
	void Start () {
        currentImage = frameStructure.GetComponent<Image>();
        currentFrame = -1;
        nextFrame = 0;
    }
	
	// Update is called once per frame
	void Update () {

        if (nextFrame < tutorialFramesList.Count && DataManager.Instance.doingTutorial)
        {
            tutorialCore.SetActive(true);
            if (currentFrame < nextFrame)
            {
                currentImage.sprite = tutorialFramesList[nextFrame];
                currentFrame++;
            }

            nextButton.onClick.AddListener(IncreaseFrame);
        }
        else
        {
            DataManager.Instance.doingTutorial = false;
            if(nextFrame >= tutorialFramesList.Count && DataManager.Instance.playerData.gameComplete == 0)
            {
                DataManager.Instance.playerData.gameComplete = 1;
            }
            tutorialCore.SetActive(false);

        }
    }

    public void IncreaseFrame()
    {
        nextFrame++;
    }

    public void Restart()
    {
        currentImage = frameStructure.GetComponent<Image>();
        currentFrame = -1;
        nextFrame = 0;
    }
}
