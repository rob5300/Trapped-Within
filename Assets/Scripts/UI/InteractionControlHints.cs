using UnityEngine;
using UnityEngine.UI;

public class InteractionControlHints : MonoBehaviour
{
    public PlayerMovement movement;
    public CanvasGroup FadeGroup;
    public Text ContinueText;
    public float BeginDelay = 5f;

    public float FadeRate = 1;
    public float ClickDelay = 0.5f;

    private bool doFadeIn = false;
    private bool doFadeOut = false;
    private bool checkContinue = false;
    private float lerpVal = 0;
    private float beginTime;
    private bool doUpdate = false;

    public void Awake()
    {
        Ui.LevelStartUIDismissed += () => {
            doUpdate = true;
            beginTime = Time.unscaledTime; 
        };
    }

    public void Update ()
    {
        if (!doUpdate) return;

        if (Time.unscaledTime > beginTime + BeginDelay)
        {
            doFadeIn = true;
            beginTime = Time.unscaledTime;
            checkContinue = true;
            Game.Pause();
        }

        if (checkContinue)
        {
            if (ContinueText && (beginTime + FadeRate + ClickDelay) < Time.unscaledTime)
            {
                Color c = ContinueText.color;
                c.a = 1;
                ContinueText.color = c;
                ContinueText = null;
            }
            if (Input.GetMouseButtonUp(0) && (beginTime + FadeRate + ClickDelay) < Time.unscaledTime)
            {
                lerpVal = 0;
                doFadeOut = true;
            } 
        }

        if (doFadeIn)
        {
            lerpVal += FadeRate * Time.unscaledDeltaTime;
            FadeGroup.alpha = Mathf.Lerp(0, 1, lerpVal);
            if (lerpVal > 0.35)
            {
                movement.DoMovement = false;
                movement.DoRotation = false;
            }
            if (lerpVal >= 1)
            {
                doFadeIn = false;
            }
        }
        else if (doFadeOut)
        {
            lerpVal += FadeRate * Time.unscaledDeltaTime;
            FadeGroup.alpha = Mathf.Lerp(1, 0, lerpVal);
            if (lerpVal > 0.9)
            {
                movement.DoMovement = true;
                movement.DoRotation = true;
                Game.Unpause();
            }
            if (lerpVal >= 1)
            {
                gameObject.SetActive(false);
                Destroy(this);
            }
        }
    }
}
