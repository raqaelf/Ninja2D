using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Kunai : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody2D myRigidbody;

    private Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        // direction = Vector2.right;
    }

    void FixedUpdate()
    {
        myRigidbody.velocity = direction*speed; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
