using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    private Transform myTransform;
    private Vector3 cameraOffset;
    private Vector3 followPosition;
    [SerializeField] private float rayDistance;
    [SerializeField] private float speedOffset;
    private float y;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        myTransform = GetComponent<Transform>();
        cameraOffset = myTransform.position;
    }

    void LateUpdate() //Lo recomendado por Unity en el LateUpdate insertar los seguimientos de Camaras
    {
        followPosition = target.position + cameraOffset;
        myTransform.position = followPosition;
        UpdateCameraOffset();
    }

    private void UpdateCameraOffset()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(target.position, Vector3.down, out hitInfo, rayDistance))
        {
            y = Mathf.Lerp(y, hitInfo.point.y, Time.deltaTime * speedOffset); //Con Lerp nos aseguramos que exista una Transici�n de C�mara para que el movimiento sea mas suave y no de jal�n por ej sin el Lerp
        }
        //else y = Mathf.Lerp(y, target.position.y, Time.deltaTime * speedOffset);
            
        followPosition.y = cameraOffset.y + y;
        myTransform.position = followPosition;
    }
}
