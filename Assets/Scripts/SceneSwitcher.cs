using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher Instance;

    [SerializeField] ScriptableEvent onSwitchedScenes;
    [SerializeField] GameObject loadingBarPrefab;

    UILoadingScreen currentLoadingScreen;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SwitchToScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void SwitchToSceneAsync(int index)
    {
        StartCoroutine(LoadAsyncScene(index));
    }

    IEnumerator LoadAsyncScene(int index)
    {
        currentLoadingScreen = Instantiate(loadingBarPrefab).GetComponent<UILoadingScreen>();
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
                
        while (!loadingOperation.isDone)
        {
            currentLoadingScreen?.UpdateLoadingValue(loadingOperation.progress);
            if (loadingOperation.progress >= 0.9f)
            {
                currentLoadingScreen?.UpdateLoadingValue(1f);
                loadingOperation.allowSceneActivation = true;
                if (currentLoadingScreen)
                    Destroy(currentLoadingScreen.gameObject);
                currentLoadingScreen = null;
                LeanTween.delayedCall(0.1f, () => onSwitchedScenes?.Raise());               
            }           
            yield return null;
        }
    }
}
