using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAbsorberParticle : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    
    public float moveSpeed;
    public float explosionSpeed;

    private float _timer;
    public float timeTilReturn; //time in seconds until particle flies to player
    
    public float explosionRadius; //radius of circle that particle could initially fly within
    private Vector2 _explosionPoint;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Vector2 pos = transform.position;
        _explosionPoint = pos + (Random.insideUnitCircle * explosionRadius);
    }

    void Update()
    {
        if (_timer < timeTilReturn)
        {
            transform.position = Vector2.Lerp(transform.position, _explosionPoint, explosionSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, PlayerManager.instance.player.transform.position, moveSpeed * (_timer - timeTilReturn) * Time.deltaTime);
        }

        if (Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position) < 0.1f)
        {
            Destroy(this.gameObject);
        }
 
        _timer += Time.deltaTime;
    }
}
