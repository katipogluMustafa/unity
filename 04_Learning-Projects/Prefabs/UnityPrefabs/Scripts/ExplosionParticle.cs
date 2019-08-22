using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticle : MonoBehaviour {
    public float explosionForce = 1500f;
    public float lifeTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f) );

        float randomForce = Random.Range(0, explosionForce);
        
        GetComponent<Rigidbody>().AddForce(randomDirection.normalized * randomForce );
    }

    // Update is called once per frame
    void Update() {
        lifeTime -= Time.deltaTime;
        if( lifeTime <= 0)
            Destroy(gameObject);
    }

}
