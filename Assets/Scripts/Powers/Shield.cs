using UnityEngine;

public class Shield : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Asteroid"))
        {
            Destroy(other.gameObject);
            PointManager.Instance.SetPoints(200);
        }
        if (other.gameObject.CompareTag("EnemyShoot"))
        {
            Destroy(other.gameObject);
        }
    }
}
