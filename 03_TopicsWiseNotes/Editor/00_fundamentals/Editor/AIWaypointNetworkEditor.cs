using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

// we say to unity that this is the editor of WaypointNetwork component type
[CustomEditor(typeof(WaypointNetwork))]
public class AIWaypointNetworkEditor : Editor {

	// Draw somethings on Scene
	// Just like transform component does draws the movement lines
    void OnSceneGUI() {
        WaypointNetwork waypointNetwork = (WaypointNetwork) target;
        List<Waypoint> waypoints = waypointNetwork.waypoints;
        LabelPoints(waypoints);
        
        if( waypointNetwork.mode == PathMode.Connecions)
            DrawConnections(waypoints);
        else if (waypointNetwork.mode == PathMode.Paths)
            DrawPaths(waypointNetwork);
    }

    // We override the default behaviour , Custom Inspector
    public override void OnInspectorGUI() {
        WaypointNetwork waypointNetwork = (WaypointNetwork) target;
        waypointNetwork.mode = (PathMode) EditorGUILayout.EnumPopup("Display Mode: ",waypointNetwork.mode);
        
        if (waypointNetwork.mode == PathMode.Paths) {
            waypointNetwork.startWaypointIndex = EditorGUILayout.IntSlider("Start Point",waypointNetwork.startWaypointIndex, 0,waypointNetwork.waypoints.Count - 1);
            waypointNetwork.endWaypointIndex = EditorGUILayout.IntSlider("Start Point",waypointNetwork.endWaypointIndex, 0,waypointNetwork.waypoints.Count - 1);            
        }

        // Last but not least, we just want to draw the default inspector
        // why?
        // Because we didn't want to customize everything but the things we want
        DrawDefaultInspector();
    }


    // Draw the actual path that will be taken by an agent if we were to go from start to end
    private void DrawPaths(WaypointNetwork waypointNetwork) {
        NavMeshPath navMeshPath = new NavMeshPath();
        if (waypointNetwork == null || waypointNetwork.waypoints == null )
            return;

        Waypoint start = waypointNetwork.waypoints[waypointNetwork.startWaypointIndex];
        Waypoint end = waypointNetwork.waypoints[waypointNetwork.endWaypointIndex];

        if (start == null || end == null)
            return;
        
        NavMesh.CalculatePath(start.transform.position, end.transform.position, NavMesh.AllAreas, navMeshPath); 
        Handles.color = Color.yellow;
        Handles.DrawPolyLine(navMeshPath.corners);
    }

    // Label each point
    private void LabelPoints(List<Waypoint> waypoints) {
        int i;
        Handles.color = Color.white;
        for (i = 0; i < waypoints.Count; i++)
            if (waypoints[i] != null)
                Handles.Label(waypoints[i].transform.position, "Waypoint " + i);        
    }

    // Draw lines between points
    private void DrawConnections( List<Waypoint> waypoints) {
        int i;
        Vector3[] waypointVectors = new Vector3[ waypoints.Count + 1];
        for (i = 0; i < waypoints.Count; i++)
            if( waypoints[i] != null )
                waypointVectors[i] = waypoints[i].transform.position;
            else 
                waypointVectors[i] = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);        // Show the user something is wrong, one of the points is null
        waypointVectors[i] = waypoints[0].transform.position;
        
        Handles.color = Color.cyan;
        Handles.DrawPolyLine(waypointVectors);
    }
}
