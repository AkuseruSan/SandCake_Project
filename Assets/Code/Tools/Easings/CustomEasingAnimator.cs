using UnityEngine;
using System.Collections;

public class CustomEasingAnimator : MonoBehaviour
{

    #region Variables

    public Easing.EasingTargetRelativeSpace easingTargetRelativeSpace;
    public Easing.EasingTarget easingTarget;
    public Easing.EasingFunctionType easingFunctionType;

    public Transform targetTransform;

    public float initialValue;
    public float finalValue;

    private float currentValue;
    public float timeDuration;
    private float timeCounter;

    private bool startEasing;

    //public Transform targetValue;

    public float timeDelay;
    private float timeDelayAux;

    public bool firstPlayNoDelay;
    public bool playOnAwake;
    public bool loopOnAwake;
    #endregion

    // Use this for initialization
    void Start()
    {

        timeDelayAux = timeDelay;
        if (firstPlayNoDelay) timeDelay = 0;

        if (targetTransform == null) targetTransform = this.transform;

        timeCounter = 0.0f;
        currentValue = initialValue;

        if (playOnAwake || loopOnAwake) startEasing = true;
        else startEasing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startEasing == true)
            EasingsUpdate();
        else { }
    }

    //Call this function on OnClick() Event in the UI Button.
    //This is the only function you need to start an easing, it will restart automatically when it ends.

    private void FixedUpdate()
    {

    }

    public void StartEasing()
    {
        Restart();
        startEasing = true;
    }

    void Restart()
    {
        if (!loopOnAwake)
            startEasing = false;
        else
            startEasing = true;

        timeDelay = timeDelayAux;

        timeCounter = 0;
    }

    void EasingsUpdate()
    {
        if (GameCore.Instance.playerController.spawnGiantSun && (timeDelay > 0) && !GameCore.Instance.reviveFirstFrame)
        {
            timeDelay -= Time.deltaTime * 1000f;
        }
        else if (timeDelay > 0) timeDelay -= Time.deltaTime;
        else if (timeCounter < timeDuration)
        {
            if (GameCore.Instance.playerController.spawnGiantSun && !GameCore.Instance.reviveFirstFrame)
            {
                timeCounter += Time.deltaTime * 1000f;
            }
            else timeCounter += Time.deltaTime;

            #region Easings List
            switch (easingFunctionType)
            {
                case Easing.EasingFunctionType.BackEaseIn:
                    {
                        currentValue = (float)Easing.BackEaseIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.BackEaseInOut:
                    {
                        currentValue = (float)Easing.BackEaseInOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.BackEaseOut:
                    {
                        currentValue = (float)Easing.BackEaseOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.BackEaseOutIn:
                    {
                        currentValue = (float)Easing.BackEaseOutIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.BounceEaseIn:
                    {
                        currentValue = (float)Easing.BounceEaseIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.BounceEaseInOut:
                    {
                        currentValue = (float)Easing.BounceEaseInOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.BounceEaseOut:
                    {
                        currentValue = (float)Easing.BounceEaseOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.BounceEaseOutIn:
                    {
                        currentValue = (float)Easing.BounceEaseOutIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.CircEaseIn:
                    {
                        currentValue = (float)Easing.CircEaseIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.CircEaseInOut:
                    {
                        currentValue = (float)Easing.CircEaseInOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.CircEaseOut:
                    {
                        currentValue = (float)Easing.CircEaseOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.CircEaseOutIn:
                    {
                        currentValue = (float)Easing.CircEaseOutIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.CubicEaseIn:
                    {
                        currentValue = (float)Easing.CubicEaseIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.CubicEaseInOut:
                    {
                        currentValue = (float)Easing.CubicEaseInOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.CubicEaseOut:
                    {
                        currentValue = (float)Easing.CubicEaseOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.CubicEaseOutIn:
                    {
                        currentValue = (float)Easing.CubicEaseOutIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.ElasticEaseIn:
                    {
                        currentValue = (float)Easing.ElasticEaseIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.ElasticEaseInOut:
                    {
                        currentValue = (float)Easing.ElasticEaseInOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.ElasticEaseOut:
                    {
                        currentValue = (float)Easing.ElasticEaseOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.ElasticEaseOutIn:
                    {
                        currentValue = (float)Easing.ElasticEaseOutIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.ExpoEaseIn:
                    {
                        currentValue = (float)Easing.ExpoEaseIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.ExpoEaseInOut:
                    {
                        currentValue = (float)Easing.ExpoEaseInOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.ExpoEaseOut:
                    {
                        currentValue = (float)Easing.ExpoEaseOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.ExpoEaseOutIn:
                    {
                        currentValue = (float)Easing.ExpoEaseOutIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.Linear:
                    {
                        currentValue = (float)Easing.Linear(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.QuadEaseIn:
                    {
                        currentValue = (float)Easing.QuadEaseIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.QuadEaseInOut:
                    {
                        currentValue = (float)Easing.QuadEaseInOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.QuadEaseOut:
                    {
                        currentValue = (float)Easing.QuadEaseOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.QuadEaseOutIn:
                    {
                        currentValue = (float)Easing.QuadEaseOutIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.QuartEaseIn:
                    {
                        currentValue = (float)Easing.QuartEaseIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.QuartEaseInOut:
                    {
                        currentValue = (float)Easing.QuartEaseInOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.QuartEaseOut:
                    {
                        currentValue = (float)Easing.QuartEaseOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.QuartEaseOutIn:
                    {
                        currentValue = (float)Easing.QuartEaseOutIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.QuintEaseIn:
                    {
                        currentValue = (float)Easing.QuintEaseIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.QuintEaseInOut:
                    {
                        currentValue = (float)Easing.QuintEaseInOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.QuintEaseOut:
                    {
                        currentValue = (float)Easing.QuintEaseOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.QuintEaseOutIn:
                    {
                        currentValue = (float)Easing.QuintEaseOutIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.SineEaseIn:
                    {
                        currentValue = (float)Easing.SineEaseIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.SineEaseInOut:
                    {
                        currentValue = (float)Easing.SineEaseInOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.SineEaseOut:
                    {
                        currentValue = (float)Easing.SineEaseOut(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;

                case Easing.EasingFunctionType.SineEaseOutIn:
                    {
                        currentValue = (float)Easing.SineEaseOutIn(timeCounter, initialValue, (finalValue - initialValue), timeDuration);
                    }
                    break;
            }
            //End Switch Here
            #endregion

            #region Easing Target Colors

            switch (easingTarget)
            {
                case Easing.EasingTarget.ColorR:
                    {
                        targetTransform.GetComponent<Renderer>().material.color = new Color(currentValue,
                                                                                    targetTransform.GetComponent<Renderer>().material.color.g,
                                                                                    targetTransform.GetComponent<Renderer>().material.color.b,
                                                                                    targetTransform.GetComponent<Renderer>().material.color.a);
                    }
                    break;

                case Easing.EasingTarget.ColorG:
                    {
                        targetTransform.GetComponent<Renderer>().material.color = new Color(targetTransform.GetComponent<Renderer>().material.color.r,
                                                                                    currentValue,
                                                                                    targetTransform.GetComponent<Renderer>().material.color.b,
                                                                                    targetTransform.GetComponent<Renderer>().material.color.a);
                    }
                    break;

                case Easing.EasingTarget.ColorB:
                    {
                        targetTransform.GetComponent<Renderer>().material.color = new Color(targetTransform.GetComponent<Renderer>().material.color.r,
                                                                                    targetTransform.GetComponent<Renderer>().material.color.g,
                                                                                    currentValue,
                                                                                    targetTransform.GetComponent<Renderer>().material.color.a);
                    }
                    break;

                case Easing.EasingTarget.ColorA:
                    {
                        targetTransform.GetComponent<Renderer>().material.color = new Color(targetTransform.GetComponent<Renderer>().material.color.r,
                                                                                    targetTransform.GetComponent<Renderer>().material.color.g,
                                                                                    targetTransform.GetComponent<Renderer>().material.color.b,
                                                                                    currentValue);
                    }
                    break;
            }
            #endregion

            #region Easing Transform Targets

            if (easingTargetRelativeSpace == Easing.EasingTargetRelativeSpace.WORLD)
                switch (easingTarget)
                {
                    case Easing.EasingTarget.PositionX:
                        {
                            targetTransform.position = new Vector3(currentValue, targetTransform.position.y, targetTransform.position.z);
                        }
                        break;

                    case Easing.EasingTarget.PositionY:
                        {
                            targetTransform.position = new Vector3(targetTransform.position.x, currentValue, targetTransform.position.z);
                        }
                        break;

                    case Easing.EasingTarget.PositionZ:
                        {
                            targetTransform.position = new Vector3(targetTransform.position.x, targetTransform.position.y, currentValue);
                        }
                        break;

                    case Easing.EasingTarget.RotationX:
                        {
                            targetTransform.eulerAngles = new Vector3(currentValue, targetTransform.eulerAngles.y, targetTransform.eulerAngles.z);
                        }
                        break;

                    case Easing.EasingTarget.RotationY:
                        {
                            targetTransform.eulerAngles = new Vector3(targetTransform.eulerAngles.x, currentValue, targetTransform.eulerAngles.z);
                        }
                        break;

                    case Easing.EasingTarget.RotationZ:
                        {
                            targetTransform.eulerAngles = new Vector3(targetTransform.eulerAngles.x, targetTransform.eulerAngles.y, currentValue);
                        }
                        break;

                    case Easing.EasingTarget.ScaleX:
                        {
                            Debug.Log("Unity Engine doesn't have 'WORLD Scale'");
                        }
                        break;

                    case Easing.EasingTarget.ScaleY:
                        {
                            Debug.Log("Unity Engine doesn't have 'WORLD Scale'");
                        }
                        break;

                    case Easing.EasingTarget.ScaleZ:
                        {
                            Debug.Log("Unity Engine doesn't have 'WORLD Scale'");
                        }
                        break;

                    case Easing.EasingTarget.ScaleALL:
                        {
                            Debug.Log("Unity Engine doesn't have 'WORLD Scale'");
                        }
                        break;
                }

            else if (easingTargetRelativeSpace == Easing.EasingTargetRelativeSpace.LOCAL)
                switch (easingTarget)
                {
                    case Easing.EasingTarget.PositionX:
                        {
                            targetTransform.localPosition = new Vector3(currentValue, targetTransform.localPosition.y, targetTransform.localPosition.z);
                        }
                        break;

                    case Easing.EasingTarget.PositionY:
                        {
                            targetTransform.localPosition = new Vector3(targetTransform.position.x, currentValue, targetTransform.localPosition.z);
                        }
                        break;

                    case Easing.EasingTarget.PositionZ:
                        {
                            targetTransform.localPosition = new Vector3(targetTransform.localPosition.x, targetTransform.localPosition.y, currentValue);
                        }
                        break;

                    case Easing.EasingTarget.RotationX:
                        {
                            targetTransform.localEulerAngles = new Vector3(currentValue, targetTransform.localEulerAngles.y, targetTransform.localEulerAngles.z);
                        }
                        break;

                    case Easing.EasingTarget.RotationY:
                        {
                            targetTransform.localEulerAngles = new Vector3(targetTransform.localEulerAngles.x, currentValue, targetTransform.localEulerAngles.z);
                        }
                        break;

                    case Easing.EasingTarget.RotationZ:
                        {
                            targetTransform.localEulerAngles = new Vector3(targetTransform.localEulerAngles.x, targetTransform.localEulerAngles.y, currentValue);
                        }
                        break;

                    case Easing.EasingTarget.ScaleX:
                        {
                            targetTransform.localScale = new Vector3(currentValue, targetTransform.localScale.y, targetTransform.localScale.z);
                        }
                        break;

                    case Easing.EasingTarget.ScaleY:
                        {
                            targetTransform.localScale = new Vector3(targetTransform.localScale.x, currentValue, targetTransform.localScale.z);
                        }
                        break;

                    case Easing.EasingTarget.ScaleZ:
                        {
                            targetTransform.localScale = new Vector3(targetTransform.localScale.x, targetTransform.localScale.y, currentValue);
                        }
                        break;
                    case Easing.EasingTarget.ScaleALL:
                        {
                            targetTransform.localScale = new Vector3(currentValue, currentValue, currentValue);
                        }
                        break;
                }
            #endregion
        }
        else
            Restart();
    }
}
