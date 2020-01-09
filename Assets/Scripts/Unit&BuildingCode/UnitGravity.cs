using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class UnitGravity : MonoBehaviour {


	public float gravity = -5.8f;
	public float terminalVelocity = -1.5f;
    public float minFall = -0.1f;
    private float _vertSpeed;


    private CharacterController charCon;

    // Use this for initialization
    void Start () {
        charCon = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 movement = Vector3.zero;
        movement = new Vector3(0, 0, 0);
        if (charCon.isGrounded)
        {
            if (Input.GetKey("space") && 1<0)
            {
                _vertSpeed = _vertSpeed * 0.1f;
            }
            else
            {
                _vertSpeed = minFall * 0.1f;
            }
        }
        else
        {
            _vertSpeed += gravity * 1 * Time.deltaTime;
            if (_vertSpeed < terminalVelocity)
            {
                _vertSpeed = terminalVelocity;
            }
            movement.y = _vertSpeed;

            charCon.Move(movement);
        }   


    }
}
