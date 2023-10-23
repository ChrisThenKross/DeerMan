using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MagicQueue : MonoBehaviour
{
    //private Queue<string> spells = new Queue<string> ();
    private List<string> spells = new List<string> ();
    private InputManager playerControls;
    [SerializeField] private TMP_Text spellQueue;
    [SerializeField] private Transform castPoint;
    // LIST OF ALL OUR POSSIBLE SPELLS
    [SerializeField] private Spell small_fireball;
    [SerializeField] private Spell med_fireball;
    [SerializeField] private Spell big_fireball;
    [SerializeField] private Spell small_ice;
    [SerializeField] private Spell med_ice;
    [SerializeField] private Spell big_ice;
    [SerializeField] private Spell small_horn;
    [SerializeField] private Spell med_horn;
    [SerializeField] private Spell big_horn;
    [SerializeField] private Spell firefirehorn;
    [SerializeField] private Spell spellToCast;

    /*TODO:
     * FIRE,FIRE,ICE
    FIRE,FIRE,HORN done for demo
    FIRE,ICE,ICE
    FIRE,HORN,ICE
    FIRE,HORN,HORN
    HORN,ICE,ICE
    HORN,HORN,ICE
    FIRE,ICE
    FIRE,HORN
    HORN,ICE
    */

    private void Awake()
    {
        playerControls = new InputManager();

    }

    public void AddFireball(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AddSpell("fireball");
        }
    }

    public void AddIce(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AddSpell("ice");
        }
    }

    public void AddHorn(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AddSpell("horn");
        }
    }

    public void AddSpell (string command)
    {
        if (spells.Count == 3)
        {
            Debug.Log("You hit the max of 3 spells! Replacing the first spell");
            spells.RemoveAt(0);
            spells.Add(command);

        } else
        {
            spells.Add(command);
        }
        UpdateQueuedSpells();
    }

    public void castSpell(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (spells.Count > 0)
            {
                spells.Sort();
                string spell = string.Join(",", spells);
                Debug.Log(spell);
                spells.Clear();

                // instantiate whatever spell was just created

                // please replace after this works
                if (spell.Equals("fireball"))
                {
                    spellQueue.text = "Casted spell: small fireball";
                    Instantiate(small_fireball, castPoint.position, castPoint.rotation);
                }
                else if (spell.Equals("fireball,fireball"))
                {
                    spellQueue.text = "Casted spell: medium fireball";
                    Instantiate(med_fireball, castPoint.position, castPoint.rotation);
                } 
                else if (spell.Equals("fireball,fireball,fireball"))
                {
                    spellQueue.text = "Casted spell: large fireball";
                    Instantiate(big_fireball, castPoint.position, castPoint.rotation);
                }
                else if (spell.Equals("ice"))
                {
                    spellQueue.text = "Casted spell: small iceball";
                    Instantiate(small_ice, castPoint.position, castPoint.rotation);
                }
                else if (spell.Equals("ice,ice"))
                {
                    spellQueue.text = "Casted spell: medium iceball";
                    Instantiate(med_ice, castPoint.position, castPoint.rotation);
                }
                else if (spell.Equals("ice,ice,ice"))
                {
                    spellQueue.text = "Casted spell: large iceball";
                    Instantiate(big_ice, castPoint.position, castPoint.rotation);
                }
                else if (spell.Equals("horn"))
                {
                    spellQueue.text = "Casted spell: small horn";
                    Instantiate(small_horn, castPoint.position, castPoint.rotation);
                }
                else if (spell.Equals("horn,horn"))
                {
                    spellQueue.text = "Casted spell: medium horn";
                    Instantiate(med_horn, castPoint.position, castPoint.rotation);
                }
                else if (spell.Equals("horn,horn,horn"))
                {
                    spellQueue.text = "Casted spell: large horn";
                    Instantiate(big_horn, castPoint.position, castPoint.rotation);
                }
                else if (spell.Equals("fireball,fireball,horn"))
                {
                    spellQueue.text = "Casted spell: BIG OP MEGA HORN!";
                    Instantiate(firefirehorn, castPoint.position, castPoint.rotation);
                }
            }
            else
            {
                spellQueue.text = "No spell casted!";
            }
        }
    }

    void UpdateQueuedSpells()
    {
        spellQueue.text = string.Empty;

        foreach (string spell in spells)
        {
            spellQueue.text += spell + ", ";
        }
    }
}
