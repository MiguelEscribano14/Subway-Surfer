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
    float y;

    //nuevo
    private GameManager gameManager;


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        myTransform = GetComponent<Transform>();
        cameraOffset = myTransform.position;

        //nuevo
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if ( gameManager.reset == false)
        {
            followPosition = target.position + cameraOffset;
            myTransform.position = followPosition;
            UpdateCameraOffset();
        }
        else if ( gameManager.reset == true )
        {
            myTransform.position = new Vector3(0, 3.11f, -6.99f);
        }

        

    }

    private void UpdateCameraOffset()
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(target.position, Vector3.down, out hitInfo, rayDistance))
        {
            y = Mathf.Lerp(y, hitInfo.point.y, Time.deltaTime * speedOffset);
        }
        //else y = Mathf.Lerp(y, target.position.y, Time.deltaTime * speedOffset);
        

        followPosition.y = cameraOffset.y + y;
        myTransform.position = followPosition;

    }
}
