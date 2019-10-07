using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEscapeMenu : MonoBehaviour
{
    [SerializeField] UISaveLoadScreen saveLoadMenu;
    [SerializeField] UINameGenerator nameGeneratorMenu;
    [Space]
    [SerializeField] ScriptableEvent onGamePaused;

    Canvas canvas;
    CurrentGameData currentGameData;

    bool lockedOnScreen = false;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        currentGameData = FindObjectOfType<CurrentGameData>();

        if (currentGameData == null)
        {
            // Generate a game data
            currentGameData = new GameObject("CurrentGameData", typeof(CurrentGameData)).GetComponent<CurrentGameData>();
            AnimateIn(true);
            saveLoadMenu.AnimateIn();
            saveLoadMenu.DisableBackButton();
        }
    }

    private void Update()
    {
        if (!lockedOnScreen && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!canvas.enabled)
                AnimateIn();
            else
                AnimateOut();
        }

    }

    #region Animate In / Out

    public void AnimateIn(bool locked = false)
    {
        if (!canvas)
            return;

        canvas.enabled = true;

        onGamePaused?.Activate();

        if (locked)
            lockedOnScreen = true;
    }

    public void AnimateOut()
    {
        if (!canvas)
            return;

        canvas.enabled = false;

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

    public void OpenOffspringGenerator()
    {
        if (!nameGeneratorMenu)
        {
            Debug.LogError("No name generator connected to the Esc menu");
            return;
        }
        nameGeneratorMenu?.AnimateIn(null, true);
        AnimateIn(true); // Turning on this canvas because the name generator is inside
    }

    #endregion
}
