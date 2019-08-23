using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaypointNetwork : MonoBehaviour {

	// We use [HideInInspector] to hide the field in default inspector so that we can implement ourselves
    [HideInInspector]
    public PathMode mode = PathMode.None;
    [HideInInspector]
    public int startWaypointIndex;
    [HideInInspector]
    public int endWaypointIndex;
    
    // This one will be painted as default
    public List<Waypoint> waypoints = new List<Waypoint>();


    // Start is called before the first frame update
    void Start() {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
    
}


public enum PathMode {None, Connecions, Paths }