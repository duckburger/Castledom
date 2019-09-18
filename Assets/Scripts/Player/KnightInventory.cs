using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightInventory : MonoBehaviour
{ 
    [SerializeField] LayerMask pickuppableLayerMask;
    [SerializeField] ScriptableEvent onWeaponUpdated;
    [SerializeField] ScriptableEvent onNearPickuppable;
    [SerializeField] ScriptableEvent onMoveAwayFromPickuppable;
    [Space]
    [SerializeField] WeaponStatFile equippedWeapon;
    [SerializeField] WeaponStatFile defaultWeapon;
    [Space]
    [SerializeField] float weaponThrowStrength = 12f;

    Rigidbody2D myRB;
    Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        myRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (equippedWeapon != defaultWeapon)
            {
                Rigidbody2D rigidBody = Instantiate(equippedWeapon.prefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
                Vector2 throwForce = myRB.velocity + (Vector2)(Input.mousePosition - mainCam.WorldToScreenPoint(transform.position)).normalized * weaponThrowStrength;
                rigidBody.AddRelativeForce(throwForce, ForceMode2D.Impulse);
                //LeanTween.value(0, 1, 0.23f).setOnUpdate((float val) =>
                //{
                //    rigidBody.transform.rotation = Quaternion.Slerp(rigidBody.transform.rotation, Quaternion.Euler(throwForce), Time.deltaTime * 12f);
                //});
                equippedWeapon = defaultWeapon;
                onWeaponUpdated?.RaiseWithData(equippedWeapon);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pickuppableLayerMask == (1 << collision.gameObject.layer | pickuppableLayerMask))
            onNearPickuppable?.RaiseWithData(collision.transform);

        if (collision.GetComponent<Weapon>() != null)
        {
            collision.GetComponent<Weapon>().AnimateOutline();
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (pickuppableLayerMask == (1 << collision.gameObject.layer | pickuppableLayerMask))
        {
            onNearPickuppable?.RaiseWithData(collision.transform);
           
            if (collision.GetComponent<IPickuppable>() != null && Input.GetKeyDown(KeyCode.F))
            {
                object pickedUpObject = collision.GetComponent<IPickuppable>().GetPickuppableObject();
                switch (pickedUpObject)
                {
                    case WeaponStatFile w:
                        equippedWeapon = w;
                        Destroy(collision.gameObject);
                        onWeaponUpdated?.RaiseWithData(equippedWeapon);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (pickuppableLayerMask == (1 << collision.gameObject.layer | pickuppableLayerMask))
            onMoveAwayFromPickuppable?.Raise();

        if (collision.GetComponent<Weapon>() != null)
        {
            collision.GetComponent<Weapon>().StopAnimatingOutline();
        }
    }
}
