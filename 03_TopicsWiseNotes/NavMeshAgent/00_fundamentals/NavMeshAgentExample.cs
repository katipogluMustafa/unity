using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentExample : MonoBehaviour {
    private NavMeshAgent _navMeshAgent = null;    
    
    // WaypointNetwork contains list of Waypoints
    // Waypoint object does not have any property but its transform
    public WaypointNetwork waypointNetwork = null;
    public int targetWaypointIndex = 0;
    
    // These fields just for debugging purposes
    public NavMeshPathStatus status = NavMeshPathStatus.PathInvalid;
    public bool pathPending = false;
    public bool hasPath = false;
    public bool pathStale = false;
    
    // Start is called before the first frame update
    void Start() {
        // Get the NavMeshAgent component of the current object
        _navMeshAgent = GetComponent<NavMeshAgent>();
        if (waypointNetwork == null)
            return;

        if (waypointNetwork.waypoints[targetWaypointIndex] == null) {
            Debug.Log("Null Target Waypoint Reference");
            return;
        }

        /* See the ghost is moving :P
        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = false;
        */
    }

    void SetNextDestionation(bool inc) {
        if (waypointNetwork == null || waypointNetwork.waypoints == null)
            return;
        
        int step = inc ? 1 : 0;
        targetWaypointIndex = (targetWaypointIndex + step) % waypointNetwork.waypoints.Count;
        Waypoint targetWaypoint = waypointNetwork.waypoints[targetWaypointIndex];

        if (targetWaypoint == null) {
            targetWaypointIndex++;
            return;
        }
        // The moment the destionation is set, the agent finds path and keep moving
        _navMeshAgent.destination = targetWaypoint.transform.position;
    }

    // Update is called once per frame
    void Update() {
        pathPending = _navMeshAgent.pathPending;
        hasPath = _navMeshAgent.hasPath;
        pathStale = _navMeshAgent.isPathStale;
        status = _navMeshAgent.pathStatus;    
        
        if( (!_navMeshAgent.hasPath && !_navMeshAgent.pathPending) 
               || _navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
            SetNextDestionation(true);
        
        // If the current path becomes invalid for some reason
        // or any changes to the costs of the paths make the path Stale
        if( _navMeshAgent.isPathStale )
            SetNextDestionation(false);

    }
}
