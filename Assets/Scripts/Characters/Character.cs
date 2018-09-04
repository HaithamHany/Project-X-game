using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Oriantation
{
left,right
}
enum PlayerType
{
    Player1,Player2,NPC
}

public class Character : MonoBehaviour {

    [SerializeField]
    private float _speed;
    private float _power;
    private float _health;
    private Animator _anim;
    private Rigidbody _characterRigBody;
    private GameObject _opponent;
    private PlayerType _player_type;
    private Oriantation _oriantation;
    [SerializeField][Range(0.01f,0.09f)]
    private float _rotationSpeed= 0.01f;

    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }
    public float Power
    {
        get { return _power; }
        set { _power = value; }
    }
    public float Health
    {
        get { return _health; }
        set { _health = value; }
    }


    protected virtual void Start () {

        _player_type = PlayerType.Player1;
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
    private void LookAtOpponent(float rotSpeed)
    {
        //getting the direction that should be looking at which is in this case the opponent
        Vector3 direction = _opponent.transform.position - transform.position;
        //Then, get the dot product of the durection with the direction the opponent is looking 
        float dot = Vector3.Dot(direction, _opponent.transform.forward);
        //The result will be -1 if the target is directly behind, +1 if directly ahead and zero if it is side-on to the NPC in any direction (including above, below, etc). 

        if (dot <= 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 90, 0), Time.time * rotSpeed);
            _oriantation = Oriantation.right;
            
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -90, 0), Time.time * rotSpeed);
            _oriantation = Oriantation.left;
        }

    }

    private void Update()
    {
        Move();
        
    }
}
