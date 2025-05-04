using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField]
    private float[] _intervalsList;
    [SerializeField]
    private GameObject[] _asteroids;
    public Transform target;

    private Camera _cam;
    private Vector3 _currentPos;
    void Awake()
    {
        _cam = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnAsteroid());
    }

    // Update is called once per frame
    void Update()
    {
        _currentPos = _cam.WorldToScreenPoint(Vector3.zero);
        // Debug.Log(GetRdnCameraPos());
    }
    IEnumerator SpawnAsteroid(){
        GameObject newAsteroid = Instantiate(GetRdnAsteroid(), GetRdnCameraPos(), Quaternion.identity);
        if(newAsteroid.GetComponent<AsteroidMovement>().followPlayer) newAsteroid.GetComponent<AsteroidMovement>().target = target;
        yield return new WaitForSeconds(GetRdnInterval());
        StartCoroutine(SpawnAsteroid());
    }
    float GetRdnInterval(){
        if(_intervalsList.Length <= 0) return 0;
        return _intervalsList[Random.Range(0,_intervalsList.Length-0)];
    }
    GameObject GetRdnAsteroid(){
        if(_asteroids.Length <= 0) return null;
        return _asteroids[Random.Range(0,_asteroids.Length-0)];
    }
    Vector3 GetRdnCameraPos(){
        return new Vector3(-_cam.WorldToScreenPoint(Vector3.zero).x, Random.Range(_cam.WorldToScreenPoint(Vector3.zero).y, -_cam.WorldToScreenPoint(Vector3.zero).y), 0f);
    }
}
