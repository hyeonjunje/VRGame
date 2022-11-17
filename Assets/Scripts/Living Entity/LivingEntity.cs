using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingEntity : MonoBehaviour
{
    [SerializeField] protected int hp;
    private int _currentHp;
    protected int currentHp
    {
        get { return _currentHp; }
        set
        {
            _currentHp = value;


            if (_currentHp <= 0)
                Dead();
        }
    }
    protected bool isDead => currentHp <= 0;


    public abstract void Hit();


    public abstract void Dead();


    public void init()
    {
        currentHp = hp;
    }
}
