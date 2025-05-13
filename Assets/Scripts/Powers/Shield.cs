using UnityEngine;

public class Shield : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        if (other.gameObject.CompareTag("Asteroid") || other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("ghjkl");
            Destroy(other.gameObject);
            PointManager.Instance.SetPoints(200);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.CompareTag("Asteroid") || collision.gameObject.CompareTag("Dust") || collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("ghjkl");
            Destroy(collision.gameObject);
            PointManager.Instance.SetPoints(200);
        }
        
    }
}
