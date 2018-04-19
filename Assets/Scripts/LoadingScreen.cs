using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

    public Text LoadingText;
    public Slider LoadingProgress;
    public CanvasGroup ParentGroup;
    public Animator animator;

    public event Action LevelLoadDone;

    public void Awake()
    {
        DontDestroyOnLoad(this);
        if (!animator) animator = GetComponent<Animator>();
    }

    public void LoadScene(string sceneName)
    {
        animator.SetTrigger("FadeIn");
        LoadingProgress.value = 0;
        StartCoroutine(LoadAsync(sceneName));
    }

    IEnumerator LoadAsync(string sceneName)
    {
        yield return new WaitForFixedUpdate();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            LoadingProgress.value = asyncLoad.progress;
            yield return null;
        }
        if (LevelLoadDone != null) LevelLoadDone();
        animator.SetTrigger("FadeOut");
    }

    public void FadeEnd()
    {
        gameObject.SetActive(false);
    }
}
