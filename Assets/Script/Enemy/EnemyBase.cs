using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyBase : RecycleObject
{
    public float movespeed = 1f;        //�̵��ӵ�
    public int hp = 2;                  //ü��
    public bool IsAttack = false;       //����Ȯ��
    public bool onHit = false;          //����Ȯ��
    public bool onDeath = false;        //���Ȯ��
    public int damage = 1;              //������ ����
    private int HP                      //ü�� ��ȭ�� ���� ���
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
    Animator Enemyani;                  //��ü�� animator
    public Transform playerpos = null;  //�÷��̾��� ��ġ����
    SpriteRenderer spriteRenderer;      //��ü�� ��������Ʈ ������
    Player player;                      // �÷��̾� ������

    private void ChasePlayer()          //�÷��̾� ����
    {
        if (transform.position.x < playerpos.position.x)    //�� ��ġ�� �÷��̾�� �����̸� 
        {
            transform.eulerAngles = new Vector3(0, 180, 0);

        }
        else        //�� ��ġ�� �÷��̾�� �������̸�
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (!IsAttack && !onHit && !onDeath) //����,�ǰ�,������� �ʾҴٸ�
        {
            transform.position = Vector3.MoveTowards(transform.position, playerpos.position, movespeed * Time.deltaTime); //�÷��̾��� ��ġ�� �̵��ӵ���ŭ �̵�
        }
        
    }
    public void OnHit()
    {
        HP--; // ü�� ���� ��
        Debug.Log($"hit : {hp}"); //���� ü���� ���
        if (hp > 0 ) //ü���� �����ִٸ�
        {
            onHit = true;
            Enemyani.SetBool("OnHit", onHit); //����Ʈ�� ����
            
        }
    }
    private void EndHitAni() //�ǰ� �ִϸ��̼� ����� ����
    {
        onHit = false;
        Enemyani.SetBool("OnHit", onHit);
    }
    private void Ondie() //��� �ִϸ��̼� ���
    {
        onDeath = true;
        Enemyani.SetTrigger("OnDeath");
    }
    
    private void EndDeadAni() // ��� �ִϸ��̼� ���� ��
    {
        Debug.Log("���");
        gameObject.SetActive(false); //��Ȱ��ȭ
        transform.position = new Vector3(10, 6, 0); //ȭ�� ������ ��ȯ
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
        // ������ Player ������Ʈ�� ã�Ƽ� playerpos�� �Ҵ�
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
