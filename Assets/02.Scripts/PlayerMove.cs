using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rbody;
    [SerializeField]
    private CapsuleCollider capCol;
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private AudioClip swordClip;
    [SerializeField]
    private AudioClip[] footSteps;
    [SerializeField]
    private Transform tr;
    [SerializeField]
    private Animator anim;
    private float h = 0f, v = 0f;

    private float lastAttackTime = 0f;
    private float lastSkillTime = 0f;
    private float lastDashTime = 0f;

    private bool isAttack = false;
    private bool isDash = false;

    private readonly int hashCombo = Animator.StringToHash("isCombo");
    private readonly int hashSkill = Animator.StringToHash("Skill");
    private readonly int hashDash = Animator.StringToHash("DashStart");

    void Awake()
    {
        rbody = GetComponent<Rigidbody>();
        capCol = GetComponent<CapsuleCollider>();
        source = GetComponent<AudioSource>();
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        footSteps = Resources.LoadAll<AudioClip>("FootStep");
        swordClip = Resources.Load("coconut_throw") as AudioClip;
    }

    public void OnStickChange(Vector2 stick)
    {
        h = stick.x;
        v = stick.y;
    }

    public void OnAttackDown()
    {
        isAttack = true;
        anim.SetBool(hashCombo, true);
        StartCoroutine(StartAttack());
    }
    IEnumerator StartAttack()
    {
        if (Time.time - lastAttackTime > 1.0f)
        {
            lastAttackTime = Time.time;
            while(isAttack)
            {
                anim.SetBool(hashCombo, true);
                yield return new WaitForSeconds(1f);
            }
        }
    }
    public void OnAttackUp()
    {
        isAttack = false;
        anim.SetBool(hashCombo, false);
    }

    public void OnSkillDown()
    {
        if (Time.time - lastSkillTime > 1f)
        {
            anim.SetTrigger(hashSkill);
            lastSkillTime = Time.time;
            source.clip = swordClip;
            source.PlayDelayed(0.7f);
        }
    }
    public void OnSkillUp()
    {

    }
    public void OnDashDown()
    {
        if(Time.time - lastDashTime > 1f)
        {
            lastDashTime = Time.time;
            isDash = true;
            anim.SetTrigger(hashDash);
        }
    }   
    public void OnDashUp()
    {

    }
    void FixedUpdate()
    {
        if(anim)
        {
            anim.SetFloat("Speed", h * h + v * v);
            if(rbody)
            {
                Vector3 Speed = rbody.velocity;
                Speed.x = 4 * h;
                Speed.z = 4 * v;
                rbody.velocity = Speed;
                if(Speed != Vector3.zero)
                {
                    tr.rotation = Quaternion.LookRotation(new Vector3(h, 0f, v)); // 바라보는 방향으로 턴을 한다
                    if (!source.isPlaying) // 오디오가 플레이 중이 아니라면
                        source.PlayOneShot(footSteps[Random.Range(0, 1)], 1f);
                }
            }
        }
    }
    
}
