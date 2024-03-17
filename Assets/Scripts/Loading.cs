using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Loading : MonoBehaviour
{
    public Slider loadingBar;
    public TMP_Text loadingText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        float progress = 0;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        asyncLoad.allowSceneActivation = true;
        while (progress < 1)
        {
            progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingBar.SetValueWithoutNotify(progress);
            loadingText.SetText("Loading... {0}%", progress * 100);
            yield return null;
        }
    }
}
