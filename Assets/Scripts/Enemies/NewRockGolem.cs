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
    private bool isThrowing = false;
    public float patrolDistance;
    private LayerMask playerMask = 1 << 11;

    public GameObject boulder;

    public float throwPower;

    public int health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapCircle(
            transform.position, 10, playerMask) && !isThrowing)
        {
            StartCoroutine(ThrowRock());
        }

        if (!isWaiting)
        transform.position = Vector2.MoveTowards(transform.position, TargetPos.position, MoveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, TargetPos.position) < .2f && !isWaiting)
        {
            StartCoroutine(Wait());
        }
    }

    IEnumerator ThrowRock()
    {
        isWaiting = true;
        isThrowing = true;
        yield return new WaitForSeconds(3);
        GameObject BoulderClone = Instantiate(boulder, transform.position, Quaternion.identity);
        BoulderClone.GetComponent<Rigidbody2D>().AddForce(Vector3.Normalize(new Vector3(
                                                            BoulderClone.gameObject.transform.position.x -
                                                            PlayerManager.instance.gameObject.transform.position.x,
                                                            BoulderClone.gameObject.transform.position.y -
                                                            PlayerManager.instance.gameObject.transform.position.y,
                                                            BoulderClone.gameObject.transform.position.z -
                                                            PlayerManager.instance.gameObject.transform.position.z)) * throwPower);
        yield return 0;
        isThrowing = false;
        isWaiting = false;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("HurtBox"))
        {
            health -= 1;
            if (health <= 0)
                Destroy(gameObject);
        }
    }
}
