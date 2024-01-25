using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    TestInputAction action;
    Player player;
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }

    public void Start()
    {
        animator = GetComponent<Animator>();

    }
    public void OnAnimationEnd()
    {
        animator.SetBool("IsAttack", false);
    }

}
