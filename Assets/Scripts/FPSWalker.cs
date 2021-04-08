using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class FPSWalker : MonoBehaviour
{
    public float speed = 8.0f;
    public float gravity = 10.0f;
    public float maxVelocityChange = 10.0f;
    public bool canJump = true;
    public float jumpHeight = 2.0f;
    private bool grounded = false;

    public GameObject fpsCam;

    public int health = 100;
    public GameObject myself;
    public GameObject graphics;

    public PhotonView name;

    private Rigidbody rigidbody;

    public Animator animator;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
        rigidbody.useGravity = false;
        name.RPC("updateName", PhotonTargets.AllBuffered, PhotonNetwork.playerName);
    }

    void FixedUpdate()
    {
        if (grounded)
        {
            // Calculate how fast we should be moving
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= speed;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = rigidbody.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

            #region animator 
            //// Animations
            //animator.SetFloat("moveX", Input.GetAxis("Horizontal"));
            //animator.SetFloat("moveZ", Input.GetAxis("Vertical"));

            //if (Input.GetKey(KeyCode.LeftShift))
            //{
            //    animator.SetBool("IsRunning", true);
            //    speed = 14.0f;
            //}
            //else
            //{
            //    animator.SetBool("IsRunning", false);
            //    speed = 8.0f;
            //}

            //if (Input.GetMouseButton(0))
            //{
            //    animator.SetBool("IsFiring", true);
            //}
            //else
            //{
            //    animator.SetBool("IsFiring", false);
            //}
            #endregion

            // Jump
            if (canJump && Input.GetButton("Jump"))
            {
                rigidbody.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
            }
        }

        // We apply gravity manually for more tuning control
        rigidbody.AddForce(new Vector3(0, -gravity * rigidbody.mass, 0));

        grounded = false;

        if (health <= 0)
        {
            GetComponent<PhotonView>().RPC("KillPlayer", PhotonTargets.AllBuffered, null);
        }
    }

    void OnCollisionStay()
    {
        grounded = true;
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    void OnGUI()
    {
        GUI.Box(new Rect(20, 20, 120, 40), "HP: " + health);
    }

    [PunRPC]
    public void applyDamage(int damage)
    {
        health = health - damage;
        if (health < 0)
        {
            health = 0;
        }
        Debug.Log("hit! " + health);
    }

    [PunRPC]
    public void KillPlayer()
    {
        PhotonNetwork.Destroy(myself);
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
}
