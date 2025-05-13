using UnityEngine;

public class Shield : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Asteroid") || other.gameObject.CompareTag("Enemy"))
        {
            // Debug.Log("ghjkl");
            Destroy(other.gameObject);
            PointManager.Instance.SetPoints(200);
        }
    }
}
