using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Side { Left = -2, Middle = 0, Right = 2 };

public class PlayerController : MonoBehaviour
{
    public Transform myTransform;
    private Animator animator;
    private CharacterController _mycharacterController;
    public CharacterController MycharacterController { get => _mycharacterController; set => _mycharacterController = value; }
    private PlayerCollision playerCollision;

    
    private GameManager gameManager;
    private SceneController sceneController;

    public bool sideCollision = false;

    private Side position;
    private Side posIni;
    private Vector3 motionVector;
    [Header("Player Controller")]
    [SerializeField] public float forwardSpeed;
    [SerializeField] public float jumpPower;
    [SerializeField] public float dodgeSpeed;
    private float rollTimer;
    private float newXPosition;
    public float yPosition;
    private float xPosition;
    private int IdDodgeLeft = Animator.StringToHash("DodgeLeft");
    private int IdDodgeRight = Animator.StringToHash("DodgeRight");
    private int IdJump = Animator.StringToHash("Jump");
    private int IdFall = Animator.StringToHash("Fall");
    private int IdLanding = Animator.StringToHash("Landing");
    private int IdRoll = Animator.StringToHash("Roll");
    
    public int IdRun = Animator.StringToHash("Run");

    private int _IdStumbleLow = Animator.StringToHash("StumbleLow");
    public int IdStumbleLow { get => _IdStumbleLow; set => _IdStumbleLow = value; }
    private int _IdStumbleCornerLeft = Animator.StringToHash("StumbleCornerLeft");
    public int IdStumbleCornerLeft { get => _IdStumbleCornerLeft; set => _IdStumbleCornerLeft = value; }
    private int _IdStumbleCornerRight = Animator.StringToHash("StumbleCornerRight");
    public int IdStumbleCornerRight { get => _IdStumbleCornerRight; set => _IdStumbleCornerRight = value; }
    private int _IdStumbleFall = Animator.StringToHash("StumbleFall");
    public int IdStumbleFall { get => _IdStumbleFall; set => _IdStumbleFall = value; }
    private int _IdStumbleOffLeft = Animator.StringToHash("StumbleOffLeft");
    public int IdStumbleOffLeft { get => _IdStumbleOffLeft; set => _IdStumbleOffLeft = value; }
    private int _IdStumbleOffRight = Animator.StringToHash("StumbleOffRight");
    public int IdStumbleOffRight { get => _IdStumbleOffRight; set => _IdStumbleOffRight = value; }
    private int _IdStumbleSideLeft = Animator.StringToHash("StumbleSideLeft");
    public int IdStumbleSideLeft { get => _IdStumbleOffRight; set => _IdStumbleOffRight = value; }
    private int _IdStumbleSideRight = Animator.StringToHash("StumbleSideRight");
    public int IdStumbleSideRight { get => _IdStumbleSideRight; set => _IdStumbleSideRight = value; }
    private int _IdDeathBounce = Animator.StringToHash("DeathBounce");
    public int IdDeathBounce { get => _IdDeathBounce; set => _IdDeathBounce = value; }
    private int _IdDeathLower = Animator.StringToHash("DeathLower");
    public int IdDeathLower { get => _IdDeathLower; set => _IdDeathLower = value; }
    private int _IdDeathMovingTrain = Animator.StringToHash("DeathMovingTrain");
    public int IdDeathMovingTrain { get => _IdDeathMovingTrain; set => _IdDeathMovingTrain = value; }
    private int _IdDeathUpper = Animator.StringToHash("DeathUpper");
    public int IdDeathUpper { get => _IdDeathUpper; set => _IdDeathUpper = value; }

    public bool swipeLeft, swipeRight, swipeUp, swipeDown;

    [Header("Player States")]
    [SerializeField] private bool isJumping;
    [SerializeField] private bool _isRolling;
    public bool IsRolling { get => _isRolling; set => _isRolling = value; }

    [SerializeField] private bool isGrounded;

    void Start()
    {
        position = Side.Middle;
        myTransform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        _mycharacterController = GetComponent<CharacterController>();
        playerCollision = GetComponent<PlayerCollision>();
        yPosition = -7;
        

        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        sceneController = GameObject.FindGameObjectWithTag("Player").GetComponent<SceneController>();

    }
    
    void Update()
    {
        
        if (gameManager.reset == false && gameManager.death == false)
        {
            
            GetSwipe();
            SetPlayerPosition();
            MovePlayer();
            Jump();
            Roll();
            isGrounded = _mycharacterController.isGrounded;
            
        }
        if (gameManager.reset == true)
        { 
        gameManager.ResetGame();
        }

        if (gameManager.death == true)
        {
        gameManager.PlayerDeath();
        }

        if (myTransform.position.z >= 2000)
        {
            sceneController.ResetLevel();
        }

        if (sideCollision == true)
        {
            SideCollision();
        }

    }


    private void GetSwipe()
    {
        swipeLeft = Input.GetKeyDown(KeyCode.LeftArrow);
        swipeRight = Input.GetKeyDown(KeyCode.RightArrow);
        swipeUp = Input.GetKeyDown(KeyCode.UpArrow);
        swipeDown = Input.GetKeyDown(KeyCode.DownArrow);
    }

    public void SetPlayerPosition()
    {
            if (swipeLeft && !_isRolling)
            {
                if (position == Side.Middle)
                {
                    UpdatePlayerXPosition(Side.Left);
                    SetPlayerAnimator(IdDodgeLeft, false);
                    posIni = Side.Middle;
                }
                else if (position == Side.Right)
                {
                    UpdatePlayerXPosition(Side.Middle);
                    SetPlayerAnimator(IdDodgeLeft, false);
                    posIni = Side.Right;
                }
            }
            else if (swipeRight && !_isRolling)
            {
                if (position == Side.Middle)
                {
                    UpdatePlayerXPosition(Side.Right);
                    SetPlayerAnimator(IdDodgeRight, false);
                    posIni = Side.Middle;
                }
                else if (position == Side.Left)
                {
                    UpdatePlayerXPosition(Side.Middle);
                    SetPlayerAnimator(IdDodgeRight, false);
                    posIni = Side.Left;
            }
            }
    }

    public void UpdatePlayerXPosition(Side plPosition)
    {
        newXPosition = (int)plPosition;
        position = plPosition;
    }

    public void SetPlayerAnimator(int id, bool isCrossFAde, float fadeTime = 0.1f)
    {
        animator.SetLayerWeight(0, 1);
        if (isCrossFAde)
        {
            animator.CrossFadeInFixedTime(id, fadeTime);
        }
        else
        {
            animator.Play(id);
        }
        ResetCollision();
        
    }

    public void SetPlayerAnimatorWithLayer(int id)
    {
        animator.SetLayerWeight(1, 1);
        animator.Play(id);
        ResetCollision();

    }

    private void ResetCollision()
    {
        playerCollision.CollisionX = CollisionX.None;
        playerCollision.CollisionY = CollisionY.None;
        playerCollision.CollisionZ = CollisionZ.None;
    }

    public void MovePlayer()
    {
        motionVector = new Vector3(xPosition - myTransform.position.x, yPosition * Time.deltaTime, forwardSpeed*Time.deltaTime);
        xPosition = Mathf.Lerp(xPosition, newXPosition, Time.deltaTime * dodgeSpeed);
        _mycharacterController.Move(motionVector);
    }

    private void Jump()
    {
        if (_mycharacterController.isGrounded) 
        {
            isJumping = false;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
                SetPlayerAnimator(IdLanding, false);
            if (swipeUp && !_isRolling)
            {
                isJumping = true;
                yPosition = jumpPower;
                SetPlayerAnimator(IdJump, true);
            }
        
        }
        else
        {
            yPosition -= jumpPower * 2 * Time.deltaTime;
            if (_mycharacterController.velocity.y <= 0)
            SetPlayerAnimator(IdFall, false);
        }
    }

    private void Roll()
    {
        rollTimer -= Time.deltaTime;
        if (rollTimer <= 0)
        {
            _isRolling = false;
            rollTimer = 0;
            _mycharacterController.center = new Vector3(0, .45f, 0);
            _mycharacterController.height = .9f;

        }
        if (swipeDown && !isJumping) 
        {
            _isRolling = true;
            rollTimer = .5f;
            SetPlayerAnimator(IdRoll, true);
            _mycharacterController.center = new Vector3(0, .2f, 0);
            _mycharacterController.height = .4f;

        }
    }

    private void SideCollision()
    {
        UpdatePlayerXPosition(posIni);
        MovePlayer();
        sideCollision = false;
    }

    
    

}
