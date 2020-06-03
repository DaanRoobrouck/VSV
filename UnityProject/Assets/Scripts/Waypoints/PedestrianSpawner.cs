using System.Collections;
using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private int _pedestriansToSpawn;

    private Animator _anim;

    void Awake()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        int count = 0;
        int randomNumber;
        while (count < _pedestriansToSpawn)
        {
            randomNumber = UnityEngine.Random.Range(0, _prefabs.Length);
            GameObject go = Instantiate(_prefabs[randomNumber]);
            Transform child = transform.GetChild(UnityEngine.Random.Range(0, transform.childCount - 1));
            go.GetComponent<WaypointNavigator>().currentWaypoint = child.GetComponent<Waypoint>();
            go.transform.position = child.position;

            _anim = go.GetComponent<Animator>();
            _anim.SetBool("isWalking", true);

            yield return new WaitForEndOfFrame();

            count++;
        }
    }
}
