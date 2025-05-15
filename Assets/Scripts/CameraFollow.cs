using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float limitRight, limitLeft, limitUp, limitDown;
    public Transform starCloud;

    private bool isIntro = true;

    public static CameraFollow Instance = null;
    [SerializeField] private float speedIncreaseRate = 0.05f;
    [SerializeField] private float maxCameraSpeed = 1f;
    private float _timeSinceStart;
    public bool cameraPaused = false;

    void Awake()
    {
        if(!Instance) Instance = this;
        else Destroy(Instance);
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

        // Aumenta gradualmente até o limite
        // smoothSpeed = Mathf.Min(smoothSpeed + speedIncreaseRate * Time.deltaTime, maxCameraSpeed);

        transform.position += transform.up * smoothSpeed * Time.deltaTime;
    }

    public void PauseCamera(float pauseDuration)
    {
        StartCoroutine(PauseRoutine(pauseDuration));
    }

    private IEnumerator PauseRoutine(float duration)
    {
        cameraPaused = true;
        yield return new WaitForSeconds(duration);
        
        cameraPaused = false;

        // Dá um boost temporário na velocidade
        smoothSpeed = maxCameraSpeed * 2f;

        yield return new WaitForSeconds(1f); // tempo com boost
        smoothSpeed = Mathf.Min(smoothSpeed, maxCameraSpeed); // volta ao normal
    }
}

