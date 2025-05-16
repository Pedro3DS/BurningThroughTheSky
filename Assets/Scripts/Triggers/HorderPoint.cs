using UnityEngine;

public class HordePoint : MonoBehaviour
{
    public int enemiesToSpawn = 3;
    public GameObject enemy;
    public float points;
    public float speed;
    private bool activated = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;
        if (!other.CompareTag("Player")) return;

        activated = true;
        HordersManager.Instance.StartHordeAtPoint(this);
    }
}
