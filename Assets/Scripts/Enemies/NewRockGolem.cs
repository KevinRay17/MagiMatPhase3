using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class NewRockGolem : MonoBehaviour
{
    public Transform TargetPos;

    public float MoveSpeed;

    private bool dir = false;
    private bool isWaiting = false;
    private bool isThrowing = false;
    private bool isCharging = false;
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
        //Check if Player is in Range and attack
        if (Physics2D.OverlapCircle(
            transform.position, 5, playerMask) && !isThrowing && !isCharging)
        {
            StartCoroutine(Charge());
        }
        else if (Physics2D.OverlapCircle(
            transform.position, 10, playerMask) && !isThrowing && !isCharging)
        {
            StartCoroutine(ThrowRock());
        }
        //Move unless attacking or at end of patrol
        if (!isWaiting)
        transform.position = Vector2.MoveTowards(transform.position, TargetPos.position, MoveSpeed * Time.deltaTime);
        
        if (Vector2.Distance(transform.position, TargetPos.position) < .2f && !isWaiting)
        {
            StartCoroutine(Wait());
        }
    }

    IEnumerator Charge()
    {
        isWaiting = true;
        isCharging = true;
        
        float t = 0f;
        while (t < 1f)
        {        
            transform.position = new Vector3(transform.position.x + Random.Range(-.05f,.05f), 
                transform.position.y, transform.position.z);
            t += Time.deltaTime;
            yield return 0;
        }
        
        Vector2 oldPlayerPos = PlayerManager.instance.gameObject.transform.position;
        float ChargePower = 800;
        gameObject.layer = 14;
        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.Normalize(new Vector2(oldPlayerPos.x - transform.position.x, 0)) * ChargePower);
        yield return new WaitForSeconds(.5f);
        isWaiting = false;
        isCharging = false;
        gameObject.layer = 13;
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
