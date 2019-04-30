using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // State
    bool jumping, doubleJumped, grounded, rising, gliding;

    // Player Specs
    float jumpBuffer, jumpTime;
    int numJumps;
    [SerializeField]
    float jumpPower, maxJumpTime, jumpScale, respawnDelay;

    // Components
    Rigidbody2D rb;
    Animator anim;
    
    void Start() {
        grounded = false;
        jumping = false;
        doubleJumped = false;
        rising = false;
        gliding = false;
        jumpBuffer = 0;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update() {
        // animation
        anim.SetFloat("YVelocity", rb.velocity.y);

        // if inputs are allowed
        if (GameManager.instance.allowInputs) {
            if (GameManager.instance.hasGlide && rb.velocity.y < 0 && Input.GetKey(KeyCode.Space)) {
                gliding = true;
                anim.SetBool("Gliding", true);

            } else {
                gliding = false;
                anim.SetBool("Gliding", false);
            }

            // restart
            if (Input.GetKeyDown(KeyCode.R))
                GameManager.instance.Restart();

            // pause
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
                GameManager.instance.Pause();

            // slo-mo
            if (GameManager.instance.hasSloMo && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
                GameManager.instance.SloMo();            

            // jump
            if (Input.GetKeyDown(KeyCode.Space) && (grounded || jumpBuffer < 0.1f)) {
                AudioManager.instance.Play("Jump");
                jumping = true;
                jumpTime = maxJumpTime;
            }

            // double jump
            if (GameManager.instance.hasDoubleJump && Input.GetKeyDown(KeyCode.Space) && grounded)
                doubleJumped = true;

            // continue rising
            if (Input.GetKey(KeyCode.Space))
                rising = true;

            // stop jumping
            if (Input.GetKeyUp(KeyCode.Space)) {
                jumping = false;
                rising = false;
            }
        }        

        if (!grounded)
            jumpBuffer += Time.deltaTime;
    }

    void FixedUpdate() {

        // jump
        if (jumping || doubleJumped) {
            if (doubleJumped) {
                doubleJumped = false;
                grounded = false;
            }

            rb.velocity = Vector2.up * jumpPower;

            // if still holding space keep moving up
            if (rising && jumpTime > 0) {
                rb.velocity = Vector2.up * jumpPower * jumpScale;
                jumpTime -= Time.deltaTime;

            } else {
                jumping = false;
                rising = false;
            }            
        }

        if (gliding)
            rb.gravityScale = 0.5f;
        else
            rb.gravityScale = 3;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        GameObject obj = collision.gameObject;
        // grounded update
        if (obj.CompareTag("Platform") || obj.CompareTag("Sm Platform") || obj.CompareTag("Med Platform") || obj.CompareTag("Lg Platform")) {
            grounded = true;
        }

        if (collision.gameObject.CompareTag("Killzone")) {
            GameManager.instance.lives--;
            GameManager.instance.LoseHeart();

            if (GameManager.instance.lives == 0)
                GameManager.instance.GameOver();
            else
                Respawn();
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        GameObject obj = collision.gameObject;

        // grounded update
        if (obj.CompareTag("Platform") || obj.CompareTag("Sm Platform") || obj.CompareTag("Med Platform") || obj.CompareTag("Lg Platform")) {
            if (!GameManager.instance.hasDoubleJump)
                grounded = false;
            else
                grounded = true;

            jumpBuffer = 0;
        }
            
    }

    void Respawn() {
        GameManager.instance.SoftReset();
        
        // countdown?
    }
}
