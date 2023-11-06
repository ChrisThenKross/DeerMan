using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "New Spell", menuName = "Spells")]


public class SpellScriptableObject : ScriptableObject
{
    public float Damage = 10f;
    public float Lifetime = 2f;
    public float Speed = 15f;
    public float SpellRadius = 0.5f;
    public bool SplashDamage = false;
    public float SplashDamageRadius = 0f;
    public int enemiesCanPierce = 0;
    public GameObject onHitFX;
    public bool knockback = false;
    public float knockbackForce = 0f;
    public float knockbackRadius = 0f;

    //Status effects maybe
    //UI Thumbnail
    //Unique time between casts
    //Magic elements, etc...
}
