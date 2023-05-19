using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerController : MonoBehaviour
{
    public bool isWalking;
    public bool isJumping;
    public bool shootingLaser;
    public bool canShoot = true;

    public float moveSpeed;
    public float jumpForce;
    private float horizontalInput;
    public float coolDown;

    private Animator anim;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    public GameObject Laser;

    public Transform rightSpawnPoint;
    public Transform leftSpawnPoint;
    public Transform currentSpawnPoint;
    public Transform respawnAnchor;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);
        if (horizontalInput != 0)
        {
            isWalking = true;
            anim.SetBool("isWalking", isWalking);
        }
        else
        {
            isWalking = false;
            anim.SetBool("isWalking", isWalking);
        }

        if (horizontalInput > 0)
        {
            sr.flipX = false;
            currentSpawnPoint = rightSpawnPoint;
        }else if (horizontalInput < 0)
        {
            sr.flipX = true;
            currentSpawnPoint = leftSpawnPoint;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            anim.SetBool("isJumping", isJumping);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.E) && !shootingLaser && canShoot)
        {
            Instantiate(Laser, currentSpawnPoint.position, currentSpawnPoint.rotation);
            shootingLaser = true;
            canShoot = false;
            anim.SetBool("shootingLaser", shootingLaser);
            Invoke(nameof(resetShooting), coolDown);
            Invoke(nameof(resetAnim), 0.5f);
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            anim.SetBool("isJumping", isJumping);
        }
        if (collision.gameObject.CompareTag("Death"))
        {
            transform.position = respawnAnchor.position;
        }
    }

    void resetAnim()
    {
        shootingLaser = false;
        anim.SetBool("shootingLaser", shootingLaser);
    }
    void resetShooting()
    {
        canShoot = true;
    }
}
