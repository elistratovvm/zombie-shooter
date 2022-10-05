using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    Player player;
    NavMeshAgent navMeshAgent;
    CapsuleCollider capsuleCollider;
    Animator animator;
    MovementAnimator movementAnimator;
    bool dead;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        player = FindObjectOfType<Player>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        movementAnimator = GetComponent<MovementAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
            return;

        navMeshAgent.SetDestination(player.transform.position);
        transform.rotation = Quaternion.LookRotation(navMeshAgent.velocity.normalized);
    }

    public void Kill()
    {
        if (!dead)
        {
            dead = true;
            animator.SetTrigger("death");
            GetComponentInChildren<ParticleSystem>().Play();
            Destroy(capsuleCollider);
            Destroy(movementAnimator);
            Destroy(navMeshAgent);
            Destroy(gameObject, 5);
            
        }
    }
}
