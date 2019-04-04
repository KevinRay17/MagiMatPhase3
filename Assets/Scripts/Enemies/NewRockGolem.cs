using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NewRockGolem : MonoBehaviour
{
    public Transform TargetPos;

    public float MoveSpeed;

    private bool dir = false;
    private bool isWaiting = false;
    public float patrolDistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, TargetPos.position, MoveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, TargetPos.position) < .2f && !isWaiting)
        {
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        isWaiting = true;
        if (!dir)
        {
            TargetPos.position += new Vector3(patrolDistance, 0, 0);
            dir = true;
        }
        else if (dir)
        {
            TargetPos.position -= new Vector3(patrolDistance, 0, 0);
            dir = false;
        }
        yield return new WaitForSeconds(3);
        

        isWaiting = false;
    }
}
