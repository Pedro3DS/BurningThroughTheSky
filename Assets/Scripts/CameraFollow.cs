using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Referência para o transform do jogador
    public float smoothSpeed = 0.125f; // Velocidade de suavização do movimento da câmera
    public Vector3 offset; // Distância entre a câmera e o jogador
    public float limitRight, limitLeft, limitUp, limitDown;

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            desiredPosition.z = transform.position.z; // Manter a posição Z da câmera fixa
            Vector3 smoothedPosition = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, transform.position.z), desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
            transform.position = new Vector3(
                Mathf.Clamp(smoothedPosition.x, limitLeft, limitRight),
                Mathf.Clamp(smoothedPosition.y, limitDown, limitUp),
            smoothedPosition.z);
        }
    }
}
