using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public GameObject[] particlePrefabs;
    public int particleCapacity = 3;
    public float lifeTime = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < particleCapacity; i++) {
            GameObject particlePrefab = Instantiate(particlePrefabs[Random.Range(0, particlePrefabs.Length)]);
            particlePrefab.transform.position = transform.position;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if( lifeTime <= 0 )
            Destroy(gameObject);
    }
    
    
}
