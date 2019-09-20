using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GlobalObjectiveController : MonoBehaviour
{
    [Serializable]
    public class KnightObjective
    {
        public Transform objectiveLocation;
        public string objectiveText;

        public KnightObjective(Transform location, string text)
        {
            this.objectiveLocation = location;
            this.objectiveText = text;
        }
    }

    [SerializeField] ScriptableEvent onObjectiveChanged;

    KnightObjective currentObjective;


    public void UpdateObjective(Transform objectiveLocation, string objectiveText)
    {
        KnightObjective newObjective = new KnightObjective(objectiveLocation, objectiveText);
        currentObjective = newObjective;
        onObjectiveChanged?.RaiseWithData(currentObjective);
    }
}
