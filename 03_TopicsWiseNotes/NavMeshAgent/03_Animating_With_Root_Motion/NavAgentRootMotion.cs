using System;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentRootMotion : MonoBehaviour {
    private NavMeshAgent _navMeshAgent = null;
    private Animator _animator = null;
    private float _smoothAngle = 0f;
    
    public WaypointNetwork waypointNetwork = null;
    public int targetWaypointIndex = 0;
    
    void Start() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        
        _navMeshAgent.updateRotation = false;                            // NavMesh will no longer update the rotation of the game object
        // _navMeshAgent.updateRotation is true, just rotation false
        
        if (waypointNetwork == null)
            return;
        if (waypointNetwork.waypoints[targetWaypointIndex] == null) {
            Debug.Log("Null Target Waypoint Reference");
            return;
        }
        
        SetNextDestionation(false);
    }
    
    
    void Update() {
        if( (!_navMeshAgent.hasPath && !_navMeshAgent.pathPending) 
            || _navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
            SetNextDestionation(true);
        if( _navMeshAgent.isPathStale )
            SetNextDestionation(false);
          
        // Send our animator the speed we'd like to go based on what navAgent tells us
        
        // _navMeshAgent.desiredVelocity contains the velocity relative to world
        // localDesiredVelocity contains the direction we wish to move specified relative to our local forward vector(Vector3.forward)
        Vector3 localDesiredVelocity = transform.InverseTransformVector( _navMeshAgent.desiredVelocity  );

        // get the angle between x and z component using arctan
        float desiredAngleToMoveInRadians = Mathf.Atan2(localDesiredVelocity.x, localDesiredVelocity.z);   
        // angle is the angle the agent wish to rotate     
        float angle = desiredAngleToMoveInRadians * Mathf.Rad2Deg;        

        // Interpolate the angle smoothly
        _smoothAngle = Mathf.MoveTowardsAngle(_smoothAngle, angle, 80.0f * Time.deltaTime);    // Last param says we never gonna be able to move more than 80 degrees in a frame

        // Send animator the desired speed of the agent to move
        _animator.SetFloat("Angle", _smoothAngle);        // we didn't interpolate the angle while sending but we did before sending in this call
 
        //float speed = localDesiredVelocity.magnitude;        // we could've use _navMesgAgent.desiredVelocity since both have same magnitude but from different reference

        // If we set the speed as magnitude it never gets to 0 since as soon as the reach the point the velocity vector will point towards new target with max magnitude
        // so we use z axis of localDesiredVelocity vector so we'll see the negative values when it changes direction
        float speed = localDesiredVelocity.z;
        _animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);    // Interpolate the speed while sending

    }
    
    // Called just prior to the lateUpdate() pass, used to manually handle the root motion
    // Root motion and root rotation calculated but the game object has not been updated yet
    private void OnAnimatorMove() {
        // In update functiton we send our animator the speed we'd like to go based on what navAgent tells us according to its navigation mesh calculations
        // Now after the animator calculated root motion this function has been called 
        // we say to the agent what will be the the rotation and velocity(direction and speed) it will use when moving instead of the one it likes to use
        transform.rotation = _animator.rootRotation;        // Just like it automatically does, update the rotation of the agent by the calculated rotation
        _navMeshAgent.velocity = _animator.deltaPosition / Time.deltaTime;         // velocity = DeltaX/DeltaT 

       // Since  we may not have tons of animations, sometimes we may never reach to exact target
        // think about turning 75 degree but havin only left turn of 10 fold degree animations like 10,20,..50,70,80
        // we'll never reach to 75 degree and this kinda of thing may happen to the speed as well as rotation
        // So always be careful and have a stopping distance and rotation degree threshold so that don't find yourself in a infinite loop 
        
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
