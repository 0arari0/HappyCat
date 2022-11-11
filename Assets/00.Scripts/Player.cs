using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        curHp = maxHp;
        isMoving = false;
        isRunning = false;
        isJumping = false;
        isExhausted = false;
        prevTime = 0f;
        speedMultiplier = 1f;
        speedDivider = 1f;
        
    }

    void Update()
    {
        Move();
        Rotate();
        Jump();
        AutoHeal();
    }

    void OnCollisionEnter(Collision collision)
    {
        var gameObject = collision.gameObject;
        if (gameObject.CompareTag("Ground") || gameObject.CompareTag("Rock"))
            isJumping = false;
    }

    public void LoseHp(float loseAmount)
    {
        loseAmount = Mathf.Abs(loseAmount);
        curHp -= loseAmount;
        if (curHp < 0f)
            curHp = 0f;
        isExhausted = curHp == 0f ? true : false;
    }

    public void HealHp(float healAmount)
    {
        healAmount = Mathf.Abs(healAmount);
        curHp += healAmount;
        if (curHp > maxHp)
            curHp = maxHp;
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
            isRunning = true;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            isRunning = false;

        #region 움직일 때마다 체력 서서히 감소
        if (!(h == 0 && v == 0)) // 움직일 때
        {
            isMoving = true;
            speedDivider = isExhausted ? 0.5f : 1f;
            if (isRunning && !isExhausted) // 뛸 때 & 지치지 않았을 때
            {
                LoseHp(0.02f);
                speedMultiplier = 2f;
            }
            else // 걸을 때
            {
                LoseHp(0.01f);
                speedMultiplier = 1f;
            }
            Canvas.instance.UpdateTiredBar(curHp, maxHp);
        }
        else // 멈춰있을 때
            isMoving = false;
        #endregion

        transform.Translate((Vector3.forward * v + Vector3.right * h).normalized * moveSpeed * speedMultiplier * speedDivider * Time.deltaTime);
    }

    void Rotate()
    {
        float vr = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up * vr * rotateSpeed * Time.deltaTime);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    void AutoHeal()
    {
        if (!isMoving && !isJumping) // 가만히 있을 때
        {
            if (Time.time - prevTime > 3f) // 3초가 지났다면
            {
                HealHp(autoHealDelta);
                Canvas.instance.UpdateTiredBar(curHp, maxHp);
            }
        }
        else // 움직이게 되면
            prevTime = Time.time;
    }

    [Header("Player Info")]
    public float maxHp;
    public float curHp;
    public float moveSpeed;
    public float rotateSpeed;
    public float jumpPower;
    public float autoHealDelta;

    [Header("State Variables")]
    public bool isMoving;
    public bool isRunning;
    public bool isJumping;
    public bool isExhausted;
    
    Rigidbody rigidbody;
    float prevTime;
    float speedMultiplier = 1f; // 뛰거나 걸을 경우 곱해질 인자(이동 속도 상승)
    float speedDivider = 1f; // 체력이 특정 퍼센트 미만일 경우 곱해질 인자(이동 속도 감소)
}
