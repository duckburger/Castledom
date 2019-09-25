using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IPickuppable
{
    [SerializeField] WeaponStatFile stats;
    SpriteRenderer spriteRenderer;
    int tweenID = 0;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool AutoPickupped()
    {
        return false;
    }

    public AudioClip PickupSound()
    {
        return null;
    }

    public object GetPickuppableObject()
    {
        return stats;
    }

    public GameObject GetWorldObject()
    {
        return this.gameObject;
    }

    private void OnDisable()
    {
        LeanTween.cancel(tweenID);
    }

    private void OnDestroy()
    {
        LeanTween.cancel(tweenID);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 10)
            TurnOnAttractor();
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 10)
            TurnOffAttractor();
    }

    public void TurnOnAttractor()
    {
        if (!spriteRenderer)
        {
            Debug.LogError($"No sprite renderer found on {gameObject.name}");
            return;
        }

        if (tweenID > 0)
            LeanTween.cancel(tweenID);

        Color outlineColour = spriteRenderer.material.GetColor("_OutlineColour");
        tweenID = LeanTween.value(1, 0, 0.23f).setOnUpdate((float val) =>
        {
            spriteRenderer.material.SetColor("_OutlineColour", new Color(outlineColour.r, outlineColour.g, outlineColour.b, val));
        }).setLoopPingPong().id;
    }

    public void TurnOffAttractor()
    {
        LeanTween.cancel(tweenID);
        tweenID = 0;
        Color outlineColour = spriteRenderer.material.GetColor("_OutlineColour");
        spriteRenderer.material.SetColor("_OutlineColour", new Color(outlineColour.r, outlineColour.g, outlineColour.b, 1));
    }
}
