using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Rigidbody _rigidbody;

    public float shootingForce = 1500f;
    public Vector3 shootingDirection;
    public float lifeTime = 3f;
    public GameObject explosionPrefab;
    
    // Start is called before the first frame update
    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(shootingDirection * shootingForce);
    }

    // Update is called once per frame
    void Update() {
        lifeTime -= Time.deltaTime;
        if( lifeTime <= 0 )
            Destroy(gameObject);
    }
    
    void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("ExplosionTrigger")) {
            GameObject explosionObject = Instantiate(explosionPrefab);
            explosionObject.transform.position = transform.position;
        }

    }
    
}
