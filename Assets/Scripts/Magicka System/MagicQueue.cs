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
    [SerializeField] private Spell med_fireball;
    [SerializeField] private Spell big_fireball;
    [SerializeField] private Spell spellToCast;

    private void Awake()
    {
        playerControls = new InputManager();

    }

    public void AddFireball(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Adding fireball");
            AddSpell("fireball");
        }
    }

    public void AddIce(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Adding ice");
            AddSpell("ice");
        }
    }

    public void AddHorn(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Adding horn");
            AddSpell("horn");
        }
    }

    public void AddSpell (string command)
    {
        spells.Add (command);
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
                spells.Clear();
                spellQueue.text = "Casted spell with " + spell;
                Debug.Log(spell);

                // instantiate whatever spell was just created

                // please replace after this works
                if (spell.Equals("fireball,fireball"))
                {
                    Instantiate(med_fireball, castPoint.position, castPoint.rotation);
                } else if (spell.Equals("fireball,fireball,fireball"))
                {
                    Instantiate(big_fireball, castPoint.position, castPoint.rotation);
                }
            }
            else
            {
                Debug.Log("There are no spells left!");
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
