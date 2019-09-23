using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] ScriptableEvent onSwitchedScenes;
    [SerializeField] GameObject loadingBarPrefab;

    UILoadingScreen currentLoadingScreen;

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
        loadingOperation.allowSceneActivation = false;
                

        while (!loadingOperation.isDone)
        {
            currentLoadingScreen?.UpdateLoadingValue(loadingOperation.progress);
            if (loadingOperation.progress >= 0.9f)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    loadingOperation.allowSceneActivation = true;
                    Destroy(currentLoadingScreen.gameObject);
                    currentLoadingScreen = null;
                    onSwitchedScenes?.Raise();
                }
            }           
            yield return null;
        }
    }
}
