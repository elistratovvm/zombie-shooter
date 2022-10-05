using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Animator animator;
    Cursor cursor;
    Shot shot;
    public float moveSpeed;
    public Transform gunBarrel;
    bool shoot;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        cursor = FindObjectOfType<Cursor>();
        shot = FindObjectOfType<Shot>();

        navMeshAgent.updateRotation = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Vector3.zero;
        Vector3 forward = cursor.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(new Vector3(forward.x, 0, forward.z));

        if (shoot)
        {
            shoot = false;
            animator.SetBool("shoot", false);
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir.z = -1.0f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            dir.z = 1.0f;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            dir.x = -1.0f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            dir.x = 1.0f;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("m_pistol_shoot"))
        {
            navMeshAgent.velocity = Vector3.zero;
        }
        else
        {
            navMeshAgent.velocity = dir.normalized * moveSpeed;
        }

        if (Input.GetMouseButtonDown(0))
        {
            shoot = true;
            animator.SetBool("shoot", true);
            var from = gunBarrel.position;
            var target = cursor.transform.position;
            var to = new Vector3(target.x, from.y, target.z);
            var direction = (to - from).normalized;

            RaycastHit hit;
            if (Physics.Raycast(from, to - from, out hit, 100))
                to = new Vector3(hit.point.x, from.y, hit.point.z);
            else
                to = from + direction * 100;

            shot.Show(from, to);

            if (hit.transform != null)
            {
                var zombie = hit.transform.GetComponent<Zombie>();
                if (zombie != null)
                    zombie.Kill();
            }
        }
    }
}
