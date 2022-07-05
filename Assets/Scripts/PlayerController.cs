#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{

    public float moveSpeed;
    public float maxSpeed;
    public float acceleration;
    public float deceleration;

    public float jumpForce;
    public CharacterController controller;
    public float gravityScale;

    public Animator animator;

    public float animationSpeed;
    public float maxAnimationSpeed;

    public Transform pivot;
    public float rotateSpeed;

    public GameObject sonic;

    private Vector3 moveDirection;

    

    // Start is called before the first frame update
    void Start(){
       controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update(){

        float yStore = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) +
            (transform.right * Input.GetAxis("Horizontal"));

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) 
            && moveSpeed < maxSpeed)
        {
            moveSpeed += acceleration;

            if(animationSpeed < maxAnimationSpeed)
            {
                animationSpeed += 0.01f;
            }
            
        }

        if ((!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            && moveSpeed > 1)
        {
            moveSpeed -= deceleration;
            if(animationSpeed > 1)
            {
                animationSpeed -= 0.1f;
            }

            if(moveSpeed <= 1)
            {
                animationSpeed = 1;
            }
            
        }

        moveDirection = moveDirection.normalized * moveSpeed;
        moveDirection.y = yStore;

        moveDirection.y += (Physics.gravity.y * gravityScale * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);

        if (controller.isGrounded){
            moveDirection.y = -1f;
            if (Input.GetButtonDown("Jump")){
            moveDirection.y = jumpForce;
            }
        }

        //Move the player in different directions based on camera look direction
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            sonic.transform.rotation = Quaternion.Slerp(sonic.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }
        
        animator.SetFloat("Speed", moveSpeed);
        animator.SetBool("isGrounded", controller.isGrounded);
        animator.SetFloat("animationSpeed", animationSpeed);

    }
}
#endif