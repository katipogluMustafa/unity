using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public GameObject bulletPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        Shoot();
    }
    
    private void Shoot() {
        GameObject bulletObject = null;
        if (Input.GetMouseButtonDown(0))
            bulletObject = Instantiate(bulletPrefab);

        if (bulletObject == null)
            return;

        Bullet bullet = bulletObject.GetComponent<Bullet>();
        Vector3 randomDirection = new Vector3(Random.Range(-0.25f,0.25f), Random.Range(-0.15f,0.15f),1);
        bullet.shootingDirection = randomDirection.normalized;            // since we don't need a point but a direction, we get its normalized form
    }

  
}
