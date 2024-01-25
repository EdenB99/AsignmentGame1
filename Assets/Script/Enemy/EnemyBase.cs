using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyBase : RecycleObject
{
    public float movespeed = 1f;        //이동속도
    public int hp = 2;                  //체력
    public bool IsAttack = false;       //공격확인
    public bool onHit = false;          //피해확인
    public bool onDeath = false;        //사망확인
    public int damage = 1;              //입히는 피해
    private int HP                      //체력 변화에 따른 계산
    {
        get => hp;
        set
        {
            hp = value;
            if (hp <= 0)
            {
                hp = 0;
                Ondie();
            }  
        }
    }
    Animator Enemyani;                  //개체의 animator
    public Transform playerpos = null;  //플레이어의 위치정보
    SpriteRenderer spriteRenderer;      //개체의 스프라이트 랜더러
    Player player;                      // 플레이어 데이터

    private void ChasePlayer()          //플레이어 추적
    {
        if (transform.position.x < playerpos.position.x)    //현 위치가 플레이어보다 왼쪽이면 
        {
            transform.eulerAngles = new Vector3(0, 180, 0);

        }
        else        //현 위치가 플레이어보다 오른족이면
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (!IsAttack && !onHit && !onDeath) //공격,피격,사망하지 않았다면
        {
            transform.position = Vector3.MoveTowards(transform.position, playerpos.position, movespeed * Time.deltaTime); //플레이어의 위치로 이동속도만큼 이동
        }
        
    }
    public void OnHit()
    {
        HP--; // 체력 감소 후
        Debug.Log($"hit : {hp}"); //현재 체력을 출력
        if (hp > 0 ) //체력이 남아있다면
        {
            onHit = true;
            Enemyani.SetBool("OnHit", onHit); //온히트로 변경
            
        }
    }
    private void EndHitAni() //피격 애니메이션 종료시 변경
    {
        onHit = false;
        Enemyani.SetBool("OnHit", onHit);
    }
    private void Ondie() //사망 애니메이션 재생
    {
        onDeath = true;
        Enemyani.SetTrigger("OnDeath");
    }
    
    private void EndDeadAni() // 사망 애니메이션 종료 후
    {
        Debug.Log("사망");
        gameObject.SetActive(false); //비활성화
        transform.position = new Vector3(10, 6, 0); //화면 밖으로 전환
    }
    private void damgeToPlayer()
    {
        player.IsHit(damage);
    }

    private void OnAttack()
    {
        IsAttack = true;
        Enemyani.SetBool("IsAttack", IsAttack);
    }

    private void EndAttackani()
    {
        IsAttack = false;
        Enemyani.SetBool("IsAttack", false);
    }

    private void Awake()
    {
        Enemyani = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        player = FindAnyObjectByType<Player>();
    }
    void Start()
    {
        // 씬에서 Player 오브젝트를 찾아서 playerpos에 할당
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerpos = playerObject.transform;
        }
    }
    void Update()
    {
        ChasePlayer();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            if (!onHit)
            {
                if (IsAttack)
                {
                    damgeToPlayer();
                }
                else
                {
                    OnAttack();
                }
            }
            
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {

        }

    }


}
