using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.UI;
using System;

public class UINameGenerator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI windowTitle;
    [SerializeField] TMP_InputField nameInputField;

    [SerializeField] string[] prefixes;
    [SerializeField] string[] roots;
    [SerializeField] string[] endings;

    [Space]
    [SerializeField] SceneSwitcher sceneSwitcher;
    [Space]
    [SerializeField] Button[] allButtons;
    [Space]
    [SerializeField] string defaultTitle;
    [SerializeField] string offspringTitle;

    bool offspringMode = false;
    CanvasGroup cg;
    Action continueCallback;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        sceneSwitcher = FindObjectOfType<SceneSwitcher>();
        allButtons = GetComponentsInChildren<Button>();
    }

    #region AnimateIn / Out


    public void AnimateIn(Action charCreatedCallback = null, bool createOffspring = false)
    {
        if (!cg)
            return;

        if (createOffspring)
        {
            ChangeWindowTitle(defaultTitle);
            offspringMode = true;
            allButtons[allButtons.Length - 1].gameObject.SetActive(false); // Turning the back button off
        }
        else
        {
            ChangeWindowTitle(offspringTitle);
            offspringMode = false;
            allButtons[allButtons.Length - 1].gameObject.SetActive(true);
        }

        cg.interactable = true;
        cg.blocksRaycasts = true;
        LeanTween.alphaCanvas(cg, 1, 0.23f);
        continueCallback = charCreatedCallback;
    }

    public void AnimateOut()
    {
        if (!cg)
            return;

        cg.interactable = false;
        cg.blocksRaycasts = false;

        LeanTween.alphaCanvas(cg, 0, 0.18f);
        continueCallback = null;
    }

    #endregion

    #region Generating Name

    public void GenerateName()
    {
        string prefix = prefixes[UnityEngine.Random.Range(0, prefixes.Length)];
        string root = roots[UnityEngine.Random.Range(0, prefixes.Length)];
        string ending = endings[UnityEngine.Random.Range(0, endings.Length)];

        string name = $"{prefix}{root}{ending}";

        nameInputField.text = name;
    }

    #endregion

    #region Button Functions

    public void Continue()
    {
        if (ValidateString(nameInputField.text))
        {
            if (!offspringMode)
            {
                if (continueCallback != null)
                    continueCallback?.Invoke();
                CurrentGameData.Instance?.AssignNewCharacterName(nameInputField.text);
                sceneSwitcher?.SwitchToSceneAsync(1);
            }
            else
            {
                if (continueCallback != null)
                    continueCallback?.Invoke();
                CurrentGameData.Instance?.ReplaceCharWithOffspring(nameInputField.text);
                AnimateOut();
                sceneSwitcher?.SwitchToSceneAsync(1);
            }
            
        }
        else
        {
            LeanTween.moveLocalX(nameInputField.gameObject, nameInputField.transform.localPosition.x + 3f, 0.12f).setEase(LeanTweenType.easeShake).setLoopPingPong(1);
        }
    }

    #endregion

    #region Validating String

    bool ValidateString(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return false;
        }
        if (text.Contains(" "))
        {
            return false;
        }
        text = char.ToUpper(text[0]) + text.Substring(1);
        return true;
    }

    #endregion


    public void ChangeWindowTitle(string title)
    {
        if (windowTitle)
            windowTitle.text = title;
    }


}
