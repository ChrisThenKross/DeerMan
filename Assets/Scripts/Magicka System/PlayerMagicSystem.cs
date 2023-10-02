using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicSystem : MonoBehaviour
{
    [SerializeField] private Spell spellToCast;

    //following tutorial, our system doesn't use mana but rather the "queuing" spell magicka system
    [SerializeField] private float maxMana = 100f;
    [SerializeField] private float rechargeRate = 2f;
    [SerializeField] private float timeBetweenCast = 0.25f;
    [SerializeField] private float currentMana;
    [SerializeField] private Transform castPoint;

    private float currentCastTimer;
    private bool castingMagic = false;

    private InputManager playerControls;

    private void Awake()
    {
        playerControls = new InputManager();

    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void onDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        bool isSpellCastHeldDown = playerControls.Player.CastSpell.ReadValue<float>() > 0.1;

        if (!castingMagic && isSpellCastHeldDown)
        {
            castingMagic = true;
            currentCastTimer = 0;
            //print("Hello");
            castSpell();
        }

        if (castingMagic)
        {
            currentCastTimer += Time.deltaTime;

            if (currentCastTimer > timeBetweenCast) castingMagic = false;
        }
    }

    void castSpell()
    {
        print("Casting spell");
        Instantiate(spellToCast, castPoint.position, castPoint.rotation);
    }

}
