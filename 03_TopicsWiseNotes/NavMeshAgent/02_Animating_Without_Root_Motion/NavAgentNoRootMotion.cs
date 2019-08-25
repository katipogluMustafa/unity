using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentNoRootMotion : MonoBehaviour {
    private NavMeshAgent _navMeshAgent = null;
    private Animator _animator = null;										// Animator component
    private float _originalMaxSpeed = 0;									
    
    public WaypointNetwork waypointNetwork = null;
    public int targetWaypointIndex = 0;
    
    void Start() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        if( _navMeshAgent )
            _originalMaxSpeed = _navMeshAgent.speed;						// Cache original speed

        if (waypointNetwork == null)
            return;
        if (waypointNetwork.waypoints[targetWaypointIndex] == null) {
            Debug.Log("Null Target Waypoint Reference");
            return;
        }
        
        SetNextDestionation(false);
    }

    void Update() {
        int turnOnSpot;
        
        // Take Cross product of the two unit vector, forward vector and desiredVelocity
        // remember |a x b| = |a| * |b| * sin(Teta)
        Vector3 crossProduct = Vector3.Cross(transform.forward, _navMeshAgent.desiredVelocity.normalized);		
        // Both vectors are in x-z axis so cross product in y axis
        // If the cross product vector's y axis negative, then we need to turn left, otherwise right
        float horizontal = ((crossProduct.y < 0) ? -1 : 1) * crossProduct.magnitude;
        // Since sin function will return result between -1 and 1, we convert the range to the range that we need from our blend tree
        horizontal = Mathf.Clamp( horizontal * 2.32f, -2.32f, 2.32f);
        // Set the Animator parameters so that it will chose appropriate animations from blend tree
        // Btw to not to change speed for example from 0 to directly 5 use lineer interpolation
        // the code below gives adds 10 percent of the new horizontal value to existing value per frame 
        _animator.SetFloat("Horizontal", horizontal, 0.1f, Time.deltaTime);
        _animator.SetFloat("Vertical", _navMeshAgent.desiredVelocity.magnitude, 0.1f, Time.deltaTime);
        
        // In the steep turnings, our animations become incomplete so we'll try no to show this
        // We'll stop the agent as soon as it starts to slow down and give animation by the state machines with the help of turnOnSpot parameter
        // Agen shall return to its original speed when the angle between agent and velocity vector passes the threshold value
        if (_navMeshAgent.desiredVelocity.magnitude < 1f && Vector3.Angle( transform.forward, _navMeshAgent.desiredVelocity) > 10f) {
            _navMeshAgent.speed = 0.1f;
            turnOnSpot = Math.Sign(horizontal);
        }
        else {
            _navMeshAgent.speed = _originalMaxSpeed;
            turnOnSpot = 0;
        }    
        _animator.SetInteger("TurnOnSpot", turnOnSpot);

        if( (!_navMeshAgent.hasPath && !_navMeshAgent.pathPending) 
               || _navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
            SetNextDestionation(true);
        if( _navMeshAgent.isPathStale )
            SetNextDestionation(false); 
          
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
        _navMeshAgent.destination = targetWaypoint.transform.position;
    }
    
    
}
