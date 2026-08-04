using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.ShaderGraph.Internal;

public class Shop_CustomerMove : MonoBehaviour
{
    [Header("Wandering")]
    public List<Vector2> availablePoints;
    public Vector3 target;
    private Vector3 direction;
    private int currentPos;
    private int movePos;

    public float pauseDuration;
    public float speed = .2f;

    private Rigidbody rb;
    private Animator anim;
    public bool isPaused;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        this.gameObject.transform.position = new Vector3(availablePoints[0].x, 0.33f, availablePoints[0].y);
    }

    private void OnEnable()
    {
        target = GetRandomTarget();
    }

    private void Update()
    {
        if (isPaused)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        if (Vector3.Distance(transform.position, target) < .1f)
        {
            StartCoroutine(PauseGetDirection());
        }

        Vector3 direction = ((Vector3)target - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        if (!isPaused)
        {
            if (Mathf.Abs(rb.linearVelocity.x) > Mathf.Abs(rb.linearVelocity.z))
            {
                if (rb.linearVelocity.x < 0)
                { anim.SetBool("WalkDown", true); }
                else { anim.SetBool("WalkUp", true); }
            }
            else
            {
                if (rb.linearVelocity.z < 0)
                { anim.SetBool("WalkRight", true); }
                else { anim.SetBool("WalkLeft", true); }
            }
        }
    }

    IEnumerator PauseGetDirection()
    {
        isPaused = true;
        anim.SetBool("WalkDown", false);
        anim.SetBool("WalkUp", false);
        anim.SetBool("WalkRight", false);
        anim.SetBool("WalkLeft", false);
        pauseDuration = Random.Range(1, 6);
        yield return new WaitForSeconds(pauseDuration);

        target = GetRandomTarget();
        isPaused = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
        StartCoroutine(PauseGetDirection());
    }

    private Vector3 GetRandomTarget()
    {
        movePos = Random.Range(0, availablePoints.Count);
        if(movePos != currentPos)
        {
            currentPos = movePos;

            Vector3 newPos = new Vector3(Random.Range(-.85f, .25f), .33f, Random.Range(-.82f, .6f));
            return newPos;
        }
        else
        {
            return GetRandomTarget();
        }
    }
}
