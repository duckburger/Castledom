using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEscapeMenu : MonoBehaviour
{
    [SerializeField] UISaveLoadScreen saveLoadMenu;
    [Space]
    [SerializeField] ScriptableEvent onGamePaused;

    CanvasGroup cg;
    CurrentGameData currentGameData;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();

        currentGameData = FindObjectOfType<CurrentGameData>();

        if (currentGameData == null)
        {
            // Generate a game data
            currentGameData = new GameObject("CurrentGameData", typeof(CurrentGameData)).GetComponent<CurrentGameData>();
            AnimateIn();
            saveLoadMenu.AnimateIn();
            saveLoadMenu.DisableBackButton();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (cg.alpha <= 0)
                AnimateIn();
            else
                AnimateOut();
        }

    }

    #region Animate In / Out

    public void AnimateIn()
    {
        if (!cg)
            return;

        cg.blocksRaycasts = true;
        cg.interactable = true;

        LeanTween.alphaCanvas(cg, 1, 0.23f);
        onGamePaused?.Activate();
    }

    public void AnimateOut()
    {
        if (!cg)
            return;

        cg.blocksRaycasts = false;
        cg.interactable = false;

        LeanTween.alphaCanvas(cg, 0, 0.18f);
        onGamePaused?.Deactivate();
    }

    #endregion


    #region Menu Functions

    public void QuitToMenu()
    {
        SceneSwitcher switcher = FindObjectOfType<SceneSwitcher>();
        if (switcher)
        {
            //switcher.SwitchToScene(0);
            switcher.SwitchToSceneAsync(0);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    #endregion
}
