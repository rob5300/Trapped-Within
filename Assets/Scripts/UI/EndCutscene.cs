using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class EndCutscene : MonoBehaviour {

    public Text OutputText;
    public List<string> Dialogue;

    public float NextTextDelay = 0.5f;
    public bool CanFade = false;

    private Animator animator;
    private float intime;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StartSequence()
    {
        Game.Unpause();
        animator.SetTrigger("FadeBackground");
    }

    public void Update()
    {
        //On left mouse
        if (CanFade && Time.time > intime + NextTextDelay && Input.GetMouseButtonUp(0))
        {
            CanFade = false;
            BeginFadeOut();
        }
    }

    public void BeginFadeIn()
    {
        animator.SetTrigger("FadeIn");
        OutputText.text = Dialogue[0];
        Dialogue.RemoveAt(0);
        intime = Time.time;
        CanFade = true;
    }

    public void BeginFadeOut()
    {
        animator.SetTrigger("FadeOut");
    }

    public void FadeOutEnd()
    {
        if (Dialogue.Count < 1)
        {
            Game.LoadLevel("MainMenu", "Main Menu");
        }
        else BeginFadeIn();
    }
}
