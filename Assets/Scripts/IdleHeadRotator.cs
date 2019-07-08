using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleHeadRotator : MonoBehaviour
{    
    [SerializeField] float minRotation = -20f;
    [SerializeField] float maxRotation = 20f;
    [SerializeField] float rotationFrequency = 2.5f;
    Vector3 initialRotation;

    float startZRotation;
    float endZRotation;

    private void Start()
    {
        initialRotation = transform.eulerAngles;

        startZRotation = Random.Range(minRotation, minRotation / 2);
        endZRotation = Random.Range(maxRotation / 2, maxRotation);

        LeanTween.rotate(gameObject, initialRotation + new Vector3(0, 0, startZRotation), 2.2f)
            .setRotateLocal().setDelay(3.5f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() => 
            {
                LeanTween.rotate(gameObject, initialRotation + new Vector3(0, 0, endZRotation), 2.2f)
                    .setEase(LeanTweenType.easeInOutSine).setRotateLocal().setLoopPingPong(-1).setDelay(3.5f).setOnCompleteOnRepeat(true);

            }).setOnCompleteOnRepeat(true);       
    }

    

}
