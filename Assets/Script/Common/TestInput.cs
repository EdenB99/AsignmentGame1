using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInput : MonoBehaviour
{
    Player Player;
    TestInputAction inputAction;
    EnemyBase enemy;
    Transform spawnPoint;
    
    public float interval = 5f;


    private void Awake()
    {
        inputAction = new TestInputAction();   
    }
    private void Start()
    {
        Player = FindAnyObjectByType<Player>();  
        enemy = FindAnyObjectByType<EnemyBase>();
        spawnPoint = transform.transform.GetChild(0);
        StartCoroutine(SpawnCoroutine());
    }

    private void OnEnable()
    {
        inputAction.Test.Enable();
        inputAction.Test.Test1.performed += OnTest1;
        inputAction.Test.Test2.performed += OnTest2;
    }
    private void OnDisable()
    {
        inputAction.Test.Test2.performed -= OnTest2;
        inputAction.Test.Test1.performed -= OnTest1;
        inputAction.Test.Disable();
    }
    private void OnTest1(InputAction.CallbackContext context)
    {
        Player.IsHit(1);
        Debug.Log("hit");
        enemy.OnHit();
    }

    private void OnTest2(InputAction.CallbackContext context)
    {
        Factory.Instance.GetFlyingEye(spawnPoint.position);
    }

    void Update()
    {
        spawnPoint.position = Player.transform.position + new Vector3(11.5f, 7.5f, 0);
    }
    IEnumerator SpawnCoroutine() {
    
        while(true)
        {
            yield return new WaitForSeconds(interval);
            Factory.Instance.GetFlyingEye(spawnPoint.position);
        }

    }
}
