using UnityEngine;

public class Shield : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid") || other.CompareTag("Dust") || other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            PointManager.Instance.SetPoints(200);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid") || collision.gameObject.CompareTag("Dust") || collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            PointManager.Instance.SetPoints(200);
        }
        
    }
}
