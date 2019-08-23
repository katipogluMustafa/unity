using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cylinder : MonoBehaviour {
    public WaypointNetwork waypointNetwork;
    
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(FollowPoints());
    }

    IEnumerator FollowPoints() {
        foreach (var waypoint in waypointNetwork.waypoints)
            yield return Move(waypoint.transform.position,1);        // Pause until function returns
    }
    
    
    IEnumerator Move(Vector3 destionation, float speed) {
        while ( transform.position != destionation ) {
            transform.position = Vector3.MoveTowards(transform.position, destionation, speed * Time.deltaTime);
            yield return null;            // Pause until next framme
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
