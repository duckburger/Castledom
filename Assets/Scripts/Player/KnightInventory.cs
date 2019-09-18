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

    Collider2D nearbyPickuppable = null;
    Rigidbody2D myRB;
    Camera mainCam;
    bool nearPickuppableItem = false;

    private void Start()
    {
        mainCam = Camera.main;
        myRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (equippedWeapon != defaultWeapon && Input.GetKeyDown(KeyCode.G))
        {
            Rigidbody2D rigidBody = Instantiate(equippedWeapon.prefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
            Vector2 throwForce = myRB.velocity + (Vector2)(Input.mousePosition - mainCam.WorldToScreenPoint(transform.position)).normalized * weaponThrowStrength;
            rigidBody.AddRelativeForce(throwForce, ForceMode2D.Impulse);
            equippedWeapon = defaultWeapon;
            onWeaponUpdated?.RaiseWithData(equippedWeapon);               
        }

        if (nearPickuppableItem && nearbyPickuppable)
        {
            if (nearbyPickuppable.GetComponent<IPickuppable>() != null && Input.GetKeyDown(KeyCode.F))
            {
                object pickedUpObject = nearbyPickuppable.GetComponent<IPickuppable>().GetPickuppableObject();
                switch (pickedUpObject)
                {
                    case WeaponStatFile w:
                        equippedWeapon = w;
                        Destroy(nearbyPickuppable.gameObject);
                        onWeaponUpdated?.RaiseWithData(equippedWeapon);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pickuppableLayerMask == (1 << collision.gameObject.layer | pickuppableLayerMask))
        {
            nearbyPickuppable = collision;
            onNearPickuppable?.RaiseWithData(nearbyPickuppable.transform);
            nearPickuppableItem = true;            
        }

        if (nearbyPickuppable.GetComponent<Weapon>() != null)
        {
            nearbyPickuppable.GetComponent<Weapon>().AnimateOutline();
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (pickuppableLayerMask == (1 << collision.gameObject.layer | pickuppableLayerMask))
        {
            nearbyPickuppable = collision;
            nearPickuppableItem = true;
            onNearPickuppable?.RaiseWithData(nearbyPickuppable.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (pickuppableLayerMask == (1 << collision.gameObject.layer | pickuppableLayerMask))
        {
            onMoveAwayFromPickuppable?.Raise();
            nearPickuppableItem = false;
            nearbyPickuppable = null;
        }

        if (collision.GetComponent<Weapon>() != null)
        {
            collision.GetComponent<Weapon>().StopAnimatingOutline();
        }
    }


   
}
