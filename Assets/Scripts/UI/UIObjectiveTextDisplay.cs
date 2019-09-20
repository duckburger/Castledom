using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObjectiveTextDisplay : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI objectiveText;


    public void UpdateObjectiveText(string newText)
    {
        if (!objectiveText)
            return;

        objectiveText.text = $"Objective: {newText}";
    }
}
