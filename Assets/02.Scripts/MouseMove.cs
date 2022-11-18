using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseMove : MonoBehaviour
{
    Ray ray; 
    RaycastHit hit; // ���� �浹 �ڷ���
    Vector3 target = Vector3.zero;
    NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ������ ī�޶󿡼� ���콺 ������ �������� ���ض�
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green); // ����� Ȯ�ζ�
        if (Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                target = hit.point;
                agent.destination = target;
                // agent.SetDestination(target);
            }
        }
        UpdateAnimator();
    }
    void UpdateAnimator()
    {
        Vector3 velocity = agent.velocity;
        Vector3 localvelocity = transform.InverseTransformDirection(velocity); // ������ǥ(������ǥ)�� ������ǥ�� ��ȯ��
        float speed = localvelocity.z;
        GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
    }
}
