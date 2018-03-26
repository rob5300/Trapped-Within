using UnityEngine;

public class LevelStartUI : MonoBehaviour
{
    public PlayerMovement movement;
    public float FadeRate = 1;
    public CanvasGroup FadeGroup;

    private bool doFade = false;
    private float lerpVal = 0;

	public void Start()
	{
        //Disable player movement.
        movement.DoMovement = false;
        movement.DoRotation = false; 
	}

    public void Update()
    {
        if (Input.GetMouseButtonUp(0) && !doFade)
        {
            doFade = true;
        }

        if (doFade)
        {
            lerpVal += FadeRate * Time.deltaTime;
            FadeGroup.alpha = Mathf.Lerp(1, 0, lerpVal);
            if (lerpVal > 0.9)
            {
                movement.DoMovement = true;
                movement.DoRotation = true;
            }
            if (lerpVal >= 1)
            {
                Ui.LevelStartUiInvoke();
                Destroy(gameObject);
            }
        }
    }
}
