using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private GUIStyle myStyle;
    private GUIStyle myStyleButton;
    
    public bool reset;
    public bool death;
    [SerializeField]private float resetTime;
    private float tempResetTime;

    private PlayerController playerController;
    private void Start()
    {
        tempResetTime = resetTime;
        reset = true;

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        myStyle = new GUIStyle();
        myStyle.fontSize = 50;
        myStyle.alignment = TextAnchor.MiddleCenter;
        

        myStyleButton = new GUIStyle();
        myStyleButton.fontSize = 50;
        myStyleButton.alignment = TextAnchor.MiddleCenter;
        
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(850, 10, 200, 100));
        GUILayout.Label(tempResetTime.ToString(), myStyle);
        GUI.skin.box = myStyleButton;
        if (GUILayout.Button("RESET", myStyleButton))
        {
            reset = true;
        }
        GUILayout.EndArea();

    }


    void Update()
    {
        if (reset)
        {
            tempResetTime = tempResetTime - Time.deltaTime;

            if (tempResetTime <= 0)
            {
                reset = false;
                tempResetTime = resetTime;
            }

        }
    }

    public void ResetGame()
    {
        death = false;

        playerController.UpdatePlayerXPosition(Side.Middle);
        playerController.MovePlayer();
        playerController.SetPlayerAnimator(playerController.IdRun, false);
        playerController.myTransform.position = new Vector3(0, 0, 0);
    }

    public void PlayerDeath()
    {
        playerController.swipeLeft = false;
        playerController.swipeRight = false;
        playerController.swipeUp = false;
        playerController.swipeDown = false;

        playerController.yPosition = 0;
    }


}
