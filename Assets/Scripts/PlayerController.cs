using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Side { Left = -2, Middle = 0, Right = 2};


public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    public Animator Animator { get => _animator; set => _animator = value; }
    private Side _position;
    public Side position { get => _position; set => _position = value; }
    private Transform _myTransform;
    public Transform myTransform { get => _myTransform; set => _myTransform = value; }
    private CharacterController _myCharacterController;
    public CharacterController myCharacterController { get => _myCharacterController; set => _myCharacterController = value; }
    private PlayerCollision _playerCollision; //Referencia por asociacion al PlayerCollision
    public PlayerCollision playerCollision { get => _playerCollision; set => _playerCollision = value; }
    
    private Vector3 motionVector;
    [Header("Player Controller")]

    [SerializeField] private float _forwardSpeed;
    public float forwardSpeed { get => _forwardSpeed; set => _forwardSpeed = value; }

    [SerializeField] private float _jumpPower;
    public float jumpPower { get => _jumpPower; set => _jumpPower = value; }

    [SerializeField] private float _dodgeSpeed;
    public float dodgeSpeed { get => _dodgeSpeed; set => _dodgeSpeed = value; }
    private float _newXPosition;
    public float newXPosition { get => _newXPosition; set => _newXPosition = value; }
    private float _xPosition;
    public float xPosition { get => _xPosition; set => _xPosition = value; }
    public float yPosition { get => _yPosition; set => _yPosition = value; }
    private float _yPosition;
    private int IdDodgeLeft = Animator.StringToHash("DodgeLeft");
    private int IdDodgeRight = Animator.StringToHash("DodgeRight");
    private int IdJump = Animator.StringToHash("Jump");
    private int IdFall = Animator.StringToHash("Fall");
    private int IdLanding = Animator.StringToHash("Landing");
    private int IdRoll = Animator.StringToHash("Roll");
    private int _IdStumbleLow = Animator.StringToHash("StumbleLow");
    private int _IdRunDelay = Animator.StringToHash("RunDelay");
    public int IdRunDelay { get => _IdRunDelay; set => _IdRunDelay = value; }
    public int IdStumbleLow{ get => _IdStumbleLow; set => _IdStumbleLow = value; }
    private int _IdStumbleCornerRight = Animator.StringToHash("StumbleCornerRight");
    public int IdStumbleCornerRight { get => _IdStumbleCornerRight; set => _IdStumbleCornerRight = value; }
    private int _IdStumbleCornerLeft = Animator.StringToHash("StumbleCornerLeft");
    public int IdStumbleCornerLeft { get => _IdStumbleCornerLeft; set => _IdStumbleCornerLeft = value; }
    private int _IdStumbleFall = Animator.StringToHash("StumbleFall");
    public int IdStumbleFall { get => _IdStumbleFall; set => _IdStumbleFall = value; }
    private int _IdStumbleOffLeft = Animator.StringToHash("StumbleOffLeft");
    public int IdStumbleOffLeft { get => _IdStumbleOffLeft; set => _IdStumbleOffLeft = value; }
    private int _IdStumbleOffRight = Animator.StringToHash("StumbleOffRight");
    public int IdStumbleOffRight { get => _IdStumbleOffRight; set => _IdStumbleOffRight = value; }
    private int _IdStumbleSideLeft = Animator.StringToHash("StumbleSideLeft");
    public int IdStumbleSideLeft { get => _IdStumbleSideLeft; set => _IdStumbleSideLeft = value; }
    private int _IdStumbleSideRight = Animator.StringToHash("StumbleSideRight");
    public int IdStumbleSideRight { get => _IdStumbleSideRight; set => _IdStumbleSideRight = value; }
    private int _IdDeathBounce = Animator.StringToHash("DeathBounce");
    public int IdDeathBounce { get => _IdDeathBounce; set => _IdDeathBounce = value; }
    private int _IdDeathLower = Animator.StringToHash("DeathLower");
    public int IdDeathLower { get => _IdDeathLower; set => _IdDeathLower = value; }
    private int _IdDeathMovingTrain = Animator.StringToHash("DeathMovingTrain");
    public int IdDeathMovingTrain  { get => _IdDeathMovingTrain; set => _IdDeathMovingTrain = value; }
    private int _IdDeathUpper = Animator.StringToHash("DeathUpper");
    public int IdDeathUpper { get => _IdDeathUpper; set => _IdDeathUpper = value; }
    private bool swipeLeft, swipeRight, swipeUp, swipeDown;
    [Header("Player States")]
    private float rollTimer;
    [SerializeField] private bool _isJumping;
    [SerializeField] private bool _isRolling;
    public bool IsRolling { get => _isRolling; set => _isRolling = value; }
    public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }
    [SerializeField] private bool _isGrounded;
    public static PlayerController instance;
   
    void Start()
    {
        instance = this;
        myTransform = GetComponent<Transform>();
        Animator = GetComponent<Animator>();
        myCharacterController = GetComponent<CharacterController>(); //Buscamos referencia de CharacterController
        playerCollision = GetComponent<PlayerCollision>();
        position = Side.Middle;
        yPosition = -7; //Esta es la posicion del Player en Y en el primer frame
    }

    void Update()
    {
        _isGrounded = _myCharacterController.isGrounded;
        print(position);
        print("Pos del GO en X: " + gameObject.transform.position);
    }

    public void ResetPlayerPosition()
    {
        newXPosition = (int)position;
        ResetCollision();
    }

    public void GetSwipe()
    {
        swipeLeft = Input.GetKeyDown(KeyCode.LeftArrow);
        swipeRight = Input.GetKeyDown(KeyCode.RightArrow);
        swipeUp = Input.GetKeyDown(KeyCode.UpArrow);
        swipeDown = Input.GetKeyDown(KeyCode.DownArrow);
    }
    public void SetPlayerPosition()
    {
        if (swipeLeft && !IsRolling)
        {
            if (position == Side.Middle) 
            {
                UpdatePlayerXPosition(Side.Left);
                SetPlayerAnimator(IdDodgeLeft, false);
            }
            else if (position == Side.Right)
            {
                UpdatePlayerXPosition(Side.Middle);
                SetPlayerAnimator(IdDodgeLeft, false);
            }
        }
        else if(swipeRight && !IsRolling)
        {
            if (position == Side.Middle) 
            {
                UpdatePlayerXPosition(Side.Right);
                SetPlayerAnimator(IdDodgeRight, false);
            }
            else if (position == Side.Left)
            {
                UpdatePlayerXPosition(Side.Middle);
                SetPlayerAnimator(IdDodgeRight, false);
            }
        }
    }

    public void UpdatePlayerXPosition(Side plPosition) //Mandamos de Parm Side
    {
        newXPosition = (int) plPosition;
        position = plPosition;
    }

    public void SetPlayerAnimator(int id, bool isCrossFade, float fadeTime = 0.1f)
    {
        _animator.SetLayerWeight(0, 1);

        if (isCrossFade)
        {
            _animator.CrossFadeInFixedTime(id, fadeTime);
        }
        else
        {
            _animator.Play(id);
        }
    }
    public void SetPlayerAnimatorWithLayer(int id)
    {
        _animator.SetLayerWeight(1,1); //1 significa el numero del Layer, y el segundo 1 significa que aplique el 100% del peso (enmascarará lo seleccionado y se combinarán ambas animaciones)
        _animator.Play(id);
        ResetCollision();
    }
    private void ResetCollision()
    {
        print(_playerCollision.CollisionX.ToString() + " " + _playerCollision.CollisionY.ToString() + " " + _playerCollision.CollisionZ.ToString());
        _playerCollision.CollisionX = CollisionX.None;
        _playerCollision.CollisionY = CollisionY.None;
        _playerCollision.CollisionZ = CollisionZ.None;
    }

    public void MovePlayer()
    {
        motionVector = new Vector3(xPosition - myTransform.position.x, _yPosition * Time.deltaTime, forwardSpeed * Time.deltaTime);
         xPosition = Mathf.Lerp(xPosition, newXPosition, Time.deltaTime * dodgeSpeed); //Se moverá el Player de xPosition a newPosition
                                        // -2 - 0 = -2 >> -2 - -2 = 0
                                        // 0 - -2 = 2 >> 0 - 0 = 0
                                        // 2 - 0 = 2 >> 2 - 2 = 0
                                    //-Con esto se forza a que se mueva una sola vez y no haya incrementos
        _myCharacterController.Move(motionVector);
    }

    public void Jump()
    {
        if (_myCharacterController.isGrounded)
        {
            _isJumping = false;
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
                SetPlayerAnimator(IdLanding, false);
            if (swipeUp && !_isRolling)
            {
                _isJumping = true;
                _yPosition = jumpPower;
                SetPlayerAnimator(IdJump, true); //CrossFadeInFixedTime mezcla la animación Actual con la Animación                                               deseada (IdJump), y el tiempo de transición (0.1f)
            }
        }
        else
        {
            _yPosition -= jumpPower * 2 * Time.deltaTime;
            if(_myCharacterController.velocity.y <= 0)
            SetPlayerAnimator(IdFall, false); 
        }
    }

    public void Roll()
    {
        rollTimer -= Time.deltaTime;
        if (rollTimer <= 0)
        {
            IsRolling = false;
            rollTimer = 0;
            //Poner CH tamaño normal
            _myCharacterController.center = new Vector3(0, .45f, 0);
            _myCharacterController.height = .9f;
        }
        if (swipeDown && !_isJumping) 
        {
            IsRolling = true;
            rollTimer = .5f;
            SetPlayerAnimator(IdRoll, true);
            _myCharacterController.center = new Vector3(0, .2f, 0);
            _myCharacterController.height = .4f; 
        }
    }

}
