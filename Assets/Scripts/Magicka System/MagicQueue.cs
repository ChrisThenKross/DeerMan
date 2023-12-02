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
    public AudioSource wrongSpell;
    public AudioSource summonBlud;

    private Animator anim;

    //private Queue<string> spells = new Queue<string> ();
    private List<string> spells = new List<string>();
    private InputManager playerControls;
    [SerializeField] private TMP_Text spellQueue;
    [SerializeField] private Transform castPoint;
    private int activeBluds = 1;
    // LIST OF ALL OUR POSSIBLE SPELLS
    [SerializeField] private GameObject blud;

    [SerializeField] private Spell small_fireball;
    [SerializeField] private Spell med_fireball;
    [SerializeField] private Spell big_fireball;

    [SerializeField] private Spell small_horn;
    [SerializeField] private Spell med_horn;
    [SerializeField] private Spell big_horn;

    [SerializeField] private Spell firehorn;
    [SerializeField] private Spell firefirehorn;
    [SerializeField] private Spell firehornhorn;

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
    public void PlayWrongSpell()
    {
        wrongSpell.PlayOneShot(wrongSpell.clip);
        Debug.Log("ran wrong spell");
    }
    public void PlaySummonBlud()
    {
        summonBlud.PlayOneShot(summonBlud.clip);
        Debug.Log("ran blud spell");
    }


    private void Update()
    {
        GameObject[] bluds = GameObject.FindGameObjectsWithTag("Blud");
        activeBluds = bluds.Length;
    }


    private void Awake()
    {
        playerControls = new InputManager();
        anim = gameObject.GetComponent<Animator>();
    }

    public void AddFireball(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AddSpell("Fireball");
        }
    }

    public void AddBlud(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AddSpell("Blud");
        }
    }

    public void AddHorn(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AddSpell("Horn");
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
        anim.SetTrigger("Shoot");
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
                if (spell.Equals("Fireball"))
                {
                    spellQueue.text = "Cast: Small Fireball";
                    Instantiate(small_fireball, castPoint.position, castPoint.rotation);
                    PlaySmallSpell();
                }
                else if (spell.Equals("Fireball,Fireball"))
                {
                    spellQueue.text = "Cast: Medium Fireball";
                    Instantiate(med_fireball, castPoint.position, castPoint.rotation);
                    PlayFire();
                }
                else if (spell.Equals("Fireball,Fireball,Fireball"))
                {
                    
                    spellQueue.text = "Cast: Large Fireball";
                    Instantiate(big_fireball, castPoint.position, castPoint.rotation);
                    PlayFire();
                }
                else if (spell.Equals("Blud"))
                {
                    spellQueue.text = "Cast: Summon Blud";
                    SummonBlud(1);
                    //PlaySmallSpell();
                }
                else if (spell.Equals("Blud,Blud"))
                {
                    spellQueue.text = "Cast: Summon Bluds";
                    SummonBlud(2);
                    //PlaySmallSpell();
                }
                else if (spell.Equals("Blud,Blud,Blud"))
                {
                    spellQueue.text = "Cast: Summon 3 Bluds";
                    SummonBlud(3);
                    //PlayIce();
                }
                else if (spell.Equals("Horn"))
                {

                    spellQueue.text = "Cast: Small Horn";
                    Instantiate(small_horn, castPoint.position, castPoint.rotation);
                    PlaySmallSpell();
                }
                else if (spell.Equals("Horn,Horn"))
                {
                    spellQueue.text = "Cast: Medium Horn";
                    Instantiate(med_horn, castPoint.position, castPoint.rotation);
                    PlaySmallSpell();
                }
                else if (spell.Equals("Horn,Horn,Horn"))
                {
                    spellQueue.text = "Cast: Large Horn";
                    Instantiate(big_horn, castPoint.position, castPoint.rotation);
                    PlayIce();
                }
                else if (spell.Equals("Fireball,Fireball,Horn"))
                {
                    spellQueue.text = "Cast: BIG MEGA HORN!";
                    Instantiate(firefirehorn, castPoint.position, castPoint.rotation);
                    PlayCombo();
                }
                else if (spell.Equals("Fireball,Horn,Horn"))
                {
                    spellQueue.text = "Cast: FIRE HORN!";
                    Instantiate(firehornhorn, castPoint.position, castPoint.rotation);
                    PlayCombo();
                }
                else
                {
                    spellQueue.text = "The spell does nothing.";
                    PlayWrongSpell();
                }
            }
            else
            {
                spellQueue.text = "No spell casted!";
                //PlayWrongSpell();
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
            PlayWrongSpell();
            return;
        }
        for (int x = 0; x < amount; x++)
            Instantiate(blud, transform.position + (transform.forward * 2), transform.rotation);
        PlaySummonBlud();
    }
}
