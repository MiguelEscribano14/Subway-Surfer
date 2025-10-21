using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public Transform myTransform;


    private void Start()
    {
        myTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void ResetLevel()
    {
        myTransform.position = new Vector3(0, 0, 0);
    }

}
