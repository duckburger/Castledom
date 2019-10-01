using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightInventory : MonoBehaviour
{
    [SerializeField] SpriteRenderer bodyRenderer;
    [Space]
    [SerializeField] ScriptableEvent onWeaponUpdated;
    [Space]
    [SerializeField] WeaponStatFile equippedWeapon;
    [SerializeField] WeaponStatFile defaultWeapon;
    [Space]
    [SerializeField] WeaponStatFile kickWeapon;
    [Space]
    [SerializeField] float weaponThrowStrength = 12f;

    KnightAttackController attackController;
    Collider2D nearbyPickuppable = null;
    Rigidbody2D myRB;
    Camera mainCam;
    bool nearPickuppableItem = false;


    public WeaponStatFile EquippedWeapon => equippedWeapon;
    public WeaponStatFile KickWeapon => kickWeapon;

    private void OnEnable()
    {
        attackController.SetupAttacksFromHandWeapon(equippedWeapon);
    }

    private void Start()
    {
        mainCam = Camera.main;
        myRB = GetComponent<Rigidbody2D>();
        attackController = GetComponent<KnightAttackController>();
        attackController.SetupAttacksFromHandWeapon(equippedWeapon); 
    }

    private void Update()
    {
        if (equippedWeapon != defaultWeapon && Input.GetKeyDown(KeyCode.G))
        {
            DropEquippedWeapon();
        }
    }

    private void DropEquippedWeapon()
    {
        Rigidbody2D rigidBody = Instantiate(equippedWeapon.prefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        Vector2 throwForce = myRB.velocity + (Vector2)(Input.mousePosition - mainCam.WorldToScreenPoint(transform.position)).normalized * weaponThrowStrength;
        rigidBody.AddRelativeForce(throwForce, ForceMode2D.Impulse);
        equippedWeapon = defaultWeapon;

        if (equippedWeapon.idleBodySprite && bodyRenderer)
            bodyRenderer.sprite = equippedWeapon.idleBodySprite;

        onWeaponUpdated?.RaiseWithData(equippedWeapon);
        attackController.SetupAttacksFromHandWeapon(equippedWeapon);
    }

    public void EquipWeapon(WeaponStatFile newWeapon, GameObject weaponInWorld)
    {
        if (equippedWeapon != defaultWeapon)
            DropEquippedWeapon();

        equippedWeapon = newWeapon;

        if (equippedWeapon.idleBodySprite && bodyRenderer)
            bodyRenderer.sprite = equippedWeapon.idleBodySprite;

        Destroy(weaponInWorld);
        onWeaponUpdated?.RaiseWithData(equippedWeapon);
        attackController.SetupAttacksFromHandWeapon(equippedWeapon);
    }


   
}
