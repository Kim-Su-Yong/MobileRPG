using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseMove : MonoBehaviour
{
    Ray ray; 
    RaycastHit hit; // 광선 충돌 자료형
    Vector3 target = Vector3.zero;
    NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 광선을 카메라에서 마우스 포지션 방향으로 향해라
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green); // 디버그 확인띠
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
        Vector3 localvelocity = transform.InverseTransformDirection(velocity); // 절대좌표(월드좌표)를 로컬좌표로 변환함
        float speed = localvelocity.z;
        GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
    }
}
