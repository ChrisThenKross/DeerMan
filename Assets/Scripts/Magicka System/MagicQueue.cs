using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MagicQueue : MonoBehaviour
{
    private Queue<string> spells = new Queue<string> ();
    private InputManager playerControls;
    [SerializeField] private TMP_Text spellQueue;

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
        spells.Enqueue (command);
        UpdateQueuedSpells();
    }

    public void RunNextSpell(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (spells.Count > 0)
            {
                string spell = string.Join(",", spells);
                spells.Clear ();
                spellQueue.text = "Casted spell with " + spell;
                Debug.Log(spell);
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
