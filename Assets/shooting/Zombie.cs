using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myZombie
{
    public class Zombie : MonoBehaviour
    {
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void Hit()
        {
            animator.SetTrigger("Hit");
            Debug.Log("¾Æ¾ß");
        }
    }
}

