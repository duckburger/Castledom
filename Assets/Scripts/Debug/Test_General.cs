using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_General : MonoBehaviour
{
    [SerializeField] GlobalObjectiveController objectiveController;
    [Space]
    [SerializeField] Transform testObjectiveLocation;
    [TextArea(3, 10)]
    [SerializeField] string textObjectiveText;
    public void TestObjective()
    {
        objectiveController?.UpdateObjective(testObjectiveLocation, textObjectiveText);
    }
}
