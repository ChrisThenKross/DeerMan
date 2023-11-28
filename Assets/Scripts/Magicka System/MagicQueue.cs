using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MagicQueue : MonoBehaviour
{

    public AudioSource fireAudio;
    public AudioSource iceAudio;
    public AudioSource hornAudio;
    public AudioSource comboAudio;
    public AudioSource smallSpellAudio;

    //private Queue<string> spells = new Queue<string> ();
    private List<string> spells = new List<string>();
    private InputManager playerControls;
    [SerializeField] private TMP_Text spellQueue;
    [SerializeField] private Transform castPoint;
    private int activeBluds = 1;
    // LIST OF ALL OUR POSSIBLE SPELLS
    [SerializeField] private Spell small_fireball;
    [SerializeField] private Spell med_fireball;
    [SerializeField] private Spell big_fireball;

    [SerializeField] private GameObject blud;


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



    public void PlayFire(){
        fireAudio.PlayOneShot(fireAudio.clip);
        Debug.Log("ran fire");
    }
    public void PlayIce()
    {
        iceAudio.PlayOneShot(iceAudio.clip);
        Debug.Log("ran ice");
    }
    public void PlayHorn()
    {
        hornAudio.PlayOneShot(hornAudio.clip);
        Debug.Log("ran horn");
    }

    public void PlayCombo()
    {
        comboAudio.PlayOneShot(comboAudio.clip);
        Debug.Log("ran combo");
    }
    public void PlaySmallSpell()
    {
        smallSpellAudio.PlayOneShot(smallSpellAudio.clip);
        Debug.Log("ran small spell");
    }


    private void Update()
    {
        GameObject[] bluds = GameObject.FindGameObjectsWithTag("Blud");
        activeBluds = bluds.Length;
    }


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
            AddSpell("blud");
        }
    }

    public void AddHorn(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AddSpell("horn");
        }
    }

    public void AddSpell(string command)
    {
        if (spells.Count == 3)
        {
            Debug.Log("You hit the max of 3 spells! Replacing the first spell");
            spells.RemoveAt(0);
            spells.Add(command);

        }
        else
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
                    PlaySmallSpell();
                }
                else if (spell.Equals("fireball,fireball"))
                {
                    spellQueue.text = "Casted spell: medium fireball";
                    Instantiate(med_fireball, castPoint.position, castPoint.rotation);
                    PlaySmallSpell();
                }
                else if (spell.Equals("fireball,fireball,fireball"))
                {
                    
                    spellQueue.text = "Casted spell: large fireball";
                    Instantiate(big_fireball, castPoint.position, castPoint.rotation);
                    PlayFire();
                }
                else if (spell.Equals("blud"))
                {
                    spellQueue.text = "Casted spell: summon blud";
                    SummonBlud(1);
                    PlaySmallSpell();
                }
                else if (spell.Equals("blud,blud"))
                {
                    spellQueue.text = "Casted spell: summon bluds";
                    SummonBlud(2);
                    PlaySmallSpell();
                }
                else if (spell.Equals("blud,blud,blud"))
                {
                    spellQueue.text = "Casted spell: summon 3 bluds";
                    SummonBlud(3);
                    PlayIce();
                }
                else if (spell.Equals("horn"))
                {

                    spellQueue.text = "Casted spell: small horn";
                    Instantiate(small_horn, castPoint.position, castPoint.rotation);
                    PlaySmallSpell();
                }
                else if (spell.Equals("horn,horn"))
                {
                    spellQueue.text = "Casted spell: medium horn";
                    Instantiate(med_horn, castPoint.position, castPoint.rotation);
                    PlaySmallSpell();
                }
                else if (spell.Equals("horn,horn,horn"))
                {
                    spellQueue.text = "Casted spell: large horn";
                    Instantiate(big_horn, castPoint.position, castPoint.rotation);
                    PlayHorn();
                }
                else if (spell.Equals("fireball,fireball,horn"))
                {
                    spellQueue.text = "Casted spell: BIG OP MEGA HORN!";
                    Instantiate(firefirehorn, castPoint.position, castPoint.rotation);
                }
                else
                {
                    spellQueue.text = "The spell does nothing.";
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

    private void SummonBlud(int amount)
    {
        if (activeBluds > 0)
        {
            spellQueue.text = "Your companions are still alive!";
            return;
        }
        for (int x = 0; x < amount; x++)
            Instantiate(blud, transform.position + (transform.forward * 2), transform.rotation);
    }
}
