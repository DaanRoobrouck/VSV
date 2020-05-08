using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint PreviousWaypoint;
    public Waypoint NextWaypoint;

    [Range(0, 5)]
    public float Width = 1f;

    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * Width / 2f;
        Vector3 maxBound = transform.position - transform.right * Width / 2f;

        float _random = Random.Range(0, 2);
        int _randomInt = Mathf.RoundToInt(_random);

        if (_randomInt == 0)
        {
            return Vector3.Lerp(maxBound, minBound, Random.Range(0f, 1f));
        }
        else
        {
            return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
        }

        
    }
}
