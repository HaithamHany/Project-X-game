using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class should be Abstract class only
public class Character : MonoBehaviour {
    public enum Oriantation
    {
        left, right
    }

    [SerializeField]
    private float _speed;
    [SerializeField]private float _jumpPower;
    private float _health;
    private Animator _anim;
    private Rigidbody _characterRigBody;
    private GameObject _opponent;

    //for jumping
    private bool _Grounded=false;
    [SerializeField] private LayerMask _WhatIsGround;
    const float _GroundedRadius = .2f;

    [SerializeField][Range(0.01f,0.15f)]protected float _rotationSpeed= 0.08f;
    protected Oriantation _oriantation;

    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }
    public float JumpPower
    {
        get { return _jumpPower; }
        set { _jumpPower = value; }
    }
    public float Health
    {
        get { return _health; }
        set { _health = value; }
    }


    protected virtual void Awake () {

        _oriantation = Oriantation.left;
        _anim = GetComponent<Animator>();
        _characterRigBody = GetComponent<Rigidbody>();
        _opponent = GameObject.FindGameObjectWithTag("Opponent");
	}

    public void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal, _characterRigBody.velocity.y, _characterRigBody.velocity.z);
        _characterRigBody.velocity = (movement*Speed)/4;
        if (_oriantation == Oriantation.left)
            _anim.SetFloat("Velocity", _characterRigBody.velocity.x);
        else
            _anim.SetFloat("Velocity", -1*(_characterRigBody.velocity.x));

        LookAtOpponent(_rotationSpeed);

    }
    public void Jump()
    {
        
    }
    private void LookAtOpponent(float rotSpeed)
    {
        //getting the direction that should be looking at which is in this case the opponent
        Vector3 direction = _opponent.transform.position - transform.position;
        //Then, get the dot product of the direction with the direction the opponent is looking 
        float dot = Vector3.Dot(direction, _opponent.transform.forward);
        //The result will be -1 if the target is directly behind, +1 if directly ahead and zero if it is side-on to the NPC in any direction (including above, below, etc). 
        // the dot product is being used here to see the direction 1 if they are same direction, -1 if the exact opposite and zero if they are
        //perpendicular
        if (dot <= 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 90, 0),  rotSpeed);
            _oriantation = Oriantation.right;
            
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -90, 0),  rotSpeed);
            _oriantation = Oriantation.left;
        }

    }
 
    private void FixedUpdate()
    {
       
        Move();
        Jump();
    }
}
