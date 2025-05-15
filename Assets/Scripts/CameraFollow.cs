using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 2f;
    public float maxCameraSpeed = 1f;
    public Vector3 offset;
    public float limitRight, limitLeft, limitUp, limitDown;
    public Transform starCloud;

    private float _timeSinceStart;
    public static CameraFollow Instance = null;
    public bool cameraPaused = false;

    void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);
    }

    public void LockCamera(bool isLocked)
    {
        cameraPaused = isLocked;
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.isGameStarted || cameraPaused) return;
        if (transform.position.y >= starCloud.position.y) return;

        _timeSinceStart += Time.deltaTime;

        smoothSpeed = Mathf.Min(7f + (_timeSinceStart * 1.2f), maxCameraSpeed);
        transform.position += Vector3.up * smoothSpeed * Time.deltaTime;
    }

    public void PauseCamera(float pauseDuration)
    {
        StartCoroutine(PauseRoutine(pauseDuration));
    }

    private System.Collections.IEnumerator PauseRoutine(float duration)
    {
        cameraPaused = true;
        yield return new WaitForSeconds(duration);
        cameraPaused = false;
    }

    public void IncreaseSpeed(float amount)
    {
        maxCameraSpeed += amount;
    }
}
