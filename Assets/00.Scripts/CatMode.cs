using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Animator))]
public class CatMode : MonoBehaviour
{
    void Awake()
    {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player.isWalking || player.isRunning)
            animator.SetTrigger(walkId);
        else
            animator.SetTrigger(IdleId);
    }

    Player player;
    Animator animator;

    readonly int IdleId= Animator.StringToHash("Idle");
    readonly int walkId = Animator.StringToHash("Walk");
}
