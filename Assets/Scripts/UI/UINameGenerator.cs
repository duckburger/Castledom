using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UINameGenerator : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInputField;

    [SerializeField] string[] prefixes;
    [SerializeField] string[] roots;
    [SerializeField] string[] endings;

    [Space]
    [SerializeField] SceneSwitcher sceneSwitcher;

    CanvasGroup cg;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        sceneSwitcher = FindObjectOfType<SceneSwitcher>();
    }


    #region AnimateIn / Out

    public void AnimateIn()
    {
        if (!cg)
            return;

        cg.interactable = true;
        cg.blocksRaycasts = true;

        LeanTween.alphaCanvas(cg, 1, 0.23f);
    }

    public void AnimateOut()
    {
        if (!cg)
            return;

        cg.interactable = false;
        cg.blocksRaycasts = false;

        LeanTween.alphaCanvas(cg, 0, 0.18f);
    }

    #endregion

    #region Generating Name

    public void GenerateName()
    {
        string prefix = prefixes[Random.Range(0, prefixes.Length)];
        string root = roots[Random.Range(0, prefixes.Length)];
        string ending = endings[Random.Range(0, endings.Length)];

        string name = $"{prefix}{root}{ending}";

        nameInputField.text = name;
    }

    #endregion

    #region Button Functions

    public void Continue()
    {
        if (ValidateString(nameInputField.text))
        {
            CurrentGameData.Instance?.AssignNewCharacterName(nameInputField.text);
            //sceneSwitcher?.SwitchToScene(1); // Assuming main scene is at 1
            sceneSwitcher?.SwitchToSceneAsync(1);
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



}
