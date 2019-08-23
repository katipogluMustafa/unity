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
    
    public AnimationCurve jumpCurve = new AnimationCurve();		// Take an Animation curve from user
    
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

        if (_navMeshAgent.isOnOffMeshLink) {
            StartCoroutine(Jump(2));
            return;        // for this frame, we don't need to further check agent for its path state so we just return
        }

        if( (!_navMeshAgent.hasPath && !_navMeshAgent.pathPending) 
               || _navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
            SetNextDestionation(true);
        
        // If the current path becomes invalid for some reason
        // or any changes to the costs of the paths make the path Stale
        if( _navMeshAgent.isPathStale )
            SetNextDestionation(false);
    }

    IEnumerator Jump(float animationDuration) {
        OffMeshLinkData data = _navMeshAgent.currentOffMeshLinkData;			
        Vector3 currentAgentPosition = transform.position;					
        Vector3 jumpEndPosition = data.endPos + (_navMeshAgent.baseOffset * Vector3.up);    // We add the height vector of the agent so that we don't get stuck inside of some object or float around
        float timePassed = 0f;

        while (timePassed <= animationDuration) {
            float percantageCompleted = timePassed / animationDuration;        
            _navMeshAgent.transform.position = Vector3.Lerp( currentAgentPosition, jumpEndPosition, percantageCompleted) + // Move along with x 
                                               ( jumpCurve.Evaluate(percantageCompleted) * Vector3.up );                         // Add the curve's height position, so that we move along both axis 
            timePassed += Time.deltaTime;
            yield return null;				// Wait for the next frame
        }
        _navMeshAgent.CompleteOffMeshLink();		// We're done, give control back
    }
}
