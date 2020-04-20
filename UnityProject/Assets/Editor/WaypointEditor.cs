using UnityEngine;
using UnityEditor;


[InitializeOnLoad()]
public class WaypointEditor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmo(Waypoint waypoint, UnityEditor.GizmoType gizmoType)
    {
        if ((gizmoType & GizmoType.Selected) != 0)
        {
            UnityEngine.Gizmos.color = Color.yellow;
        }
        else
        {
            UnityEngine.Gizmos.color = Color.yellow * 0.5f;
        }

        UnityEngine.Gizmos.DrawSphere(waypoint.transform.position, 0.2f);

       UnityEngine.Gizmos.color = Color.white;
       UnityEngine.Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.Width / 2f), 
            waypoint.transform.position - ((waypoint.transform.right * waypoint.Width / 2f)));

        if (waypoint.PreviousWaypoint != null)
        {
            UnityEngine.Gizmos.color = Color.red;
            Vector3 offset = waypoint.transform.right * waypoint.Width / 2f;
            Vector3 offsetTo = waypoint.PreviousWaypoint.transform.right * waypoint.PreviousWaypoint.Width / 2f;

            UnityEngine.Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.PreviousWaypoint.transform.position + offsetTo);

            UnityEngine.Gizmos.color = Color.green;
            UnityEngine.Gizmos.DrawLine(waypoint.transform.position - offset, waypoint.PreviousWaypoint.transform.position - offsetTo);
        }
    }
}
