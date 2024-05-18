using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{

    [Header("Base")]
    public Enemy profile;

    [Header("Health")]
    public short health;
    public short maxHealth;

    public void Init(Vector2 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        health = maxHealth;

        OnInit();
    }

    public abstract void OnInit();

    public abstract void Damage(short damage, Vector2 position);

    public void Final()
    {
        gameObject.SetActive(false);

        OnFinal();
    }

    public abstract void OnFinal();

    public void Kill()
    {
        EnemyManager.instance.Kill(this);
    }

}
