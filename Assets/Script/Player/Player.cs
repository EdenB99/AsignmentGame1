using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public const float CanReInputTime = 1f;
    public float JumpPower = 1.0f; // 변경 가능한 점프력의 값
    public float moveSpeed = 1.0f; // 유니티에서 변경 가능한 이동속도값
    public float AttackSpeed = 1.0f; //공격이 가능할때까지의 시간
    public int AttackCombo = 0; //현재 공격 단계
    public int hp = 3;
    private int Hp
    {
        get => hp;
        set
        {
            hp = value;
            if (hp <= 0)
            {
                hp = 0;
                OnDie();
            }
        }
    }

    int inputX = Animator.StringToHash("InputX"); // inputX에 입력받는 x값 삽입
    
    PlayerInputAction inputAction; //inputAction 삽입 
    Animator playerani; // 오브젝트 내 에니메이터
    Rigidbody2D rigid2d; // 리지드 바디 선언
    SpriteRenderer spriteRenderer;
    Coroutine CheckTimeCor;
    EnemyBase enemy;
    private bool isHit = false;
    private bool isDead = false;
    private bool isAttack = false;



    Vector3 inputDir = Vector3.zero; //input값을 저장할 벡터 inputDir

    private void OnMove(InputAction.CallbackContext context)
    {
        isAttack = false;
        playerani.SetBool("IsAttack", isAttack);
        inputDir.x = context.ReadValue<Vector2>().x;
        //입력 받은 x좌표를 inputDir에 저장
        playerani.SetFloat(inputX, inputDir.x);
        switch (inputDir.x)
        {
            case 1:
                transform.eulerAngles = new Vector3(0, 0, 0);
                playerani.SetBool("IsMove", true);
                break;

            case -1:
                transform.eulerAngles = new Vector3(0, 180, 0);
                inputDir.x = -inputDir.x;
                playerani.SetBool("IsMove", true);
                break;

            case 0:
                playerani.SetBool("IsMove", false);
                break;
            default:
                Debug.Log("input 값 오류");
                break;
        }
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        isAttack = false;
        playerani.SetBool("IsAttack", isAttack);
        if (playerani.GetBool("OnAir") == false)
        {
            playerani.SetBool("OnAir", true);
            inputDir.y += JumpPower;
            transform.Translate(Time.deltaTime * moveSpeed * inputDir);
        }
        
    }
    public void IsHit(int damge) 
    {
        if (!isAttack || !isDead) {
            Hp -= damge;
            if (hp > 0)
            {
                isHit = true;
                playerani.SetBool("IsHit", isHit);
            }
        }
    }

    public void OnDie()
    {
        isDead = true;
        playerani.SetTrigger("OnDie");
    }


    //IEnumerator CheckAttackCoroutine() {}

    private void IsAttack(InputAction.CallbackContext context)
    {
        if (context.performed == true)
        {
            isAttack = true;
            playerani.SetBool("IsAttack", isAttack);
            playerani.SetFloat("AttackSpeed", AttackSpeed);
            AttackComboCheck();

            AttackCombo++;
            if (AttackCombo >= 4)
            {
                AttackCombo = 1;
            }
            playerani.SetInteger("AttackCombo", AttackCombo);

            Debug.Log(AttackCombo);
        }

        //https://angliss.cc/animator-animatorstateinfo/

    }
    private void AttackComboCheck()
    {
       
    }
    private void EndAttackAni()
    {
        AttackCombo = 0;
        playerani.SetInteger("AttackCombo", AttackCombo);
        isAttack = false;
        playerani.SetBool("IsAttack", isAttack);
    }
    
   
    private void EndHitAni()
    {
        isHit = false;
        playerani.SetBool("IsHit", isHit);
    }
    private void damgeToEnemy()
    {
        enemy.OnHit();

    }

    private void Awake()
    {
        inputAction = new PlayerInputAction(); //inputAction 생성
        playerani = GetComponent<Animator>();
        //오브젝트가 가진 애니메이터 컴포넌트를 가져옴
        enemy = FindAnyObjectByType<EnemyBase>();
    }
    private void OnEnable()
    {
        inputAction.Player.Enable();
        inputAction.Player.Move.performed += OnMove;
        inputAction.Player.Move.canceled += OnMove;
        inputAction.Player.Jump.performed += OnJump;
        inputAction.Player.Jump.canceled += OnJump;
        inputAction.Player.Attack.performed += IsAttack;
        inputAction.Player.Attack.canceled += IsAttack;
    }

    

    private void OnDisable()
    {
        inputAction.Player.Attack.canceled -= IsAttack;
        inputAction.Player.Attack.performed -= IsAttack;
        inputAction.Player.Jump.canceled -= OnJump;
        inputAction.Player.Jump.performed -= OnJump;
        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Disable();
    }
    void Start()
    {
        playerani.SetFloat("AttackSpeed", AttackSpeed);
    }

    void Update()
    {
        if (isHit || isDead)
        {

        }  else
        {
            transform.Translate(Time.deltaTime * moveSpeed * inputDir);
        }
        
        // 1초당 moveSpeed만큼의 속도로, inputDir 방향으로 움직여라
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tile")) //collision의 게임 오브젝트가 enemy라는 태그를 가지는지 확인
        {
            playerani.SetBool("OnAir", false);
            inputDir.y = 0;
            Debug.Log("OnLand");
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isAttack)
            {
                damgeToEnemy();
            }
            
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }


}
