using UnityEngine;

public class playercontroller : MonoBehaviour
{
    private Rigidbody rb;
    public float gravityMultiplier = 1.0f;
    public float jumpForce = 5.0f;
    private Vector3 gravityDirection = Vector3.down;

    public float distanceToGround = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetGravity(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetGravity(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetGravity(Vector3.forward);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetGravity(Vector3.back);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        VisualizeGroundCheck();
    }

    void FixedUpdate()
    {
        Vector3 gravityForce = gravityDirection * gravityMultiplier;
        rb.AddForce(gravityForce, ForceMode.Acceleration);
    }

    void SetGravity(Vector3 direction)
    {
        gravityDirection = direction;

        Quaternion targetRotation = Quaternion.LookRotation(-gravityDirection, Vector3.up);
        rb.rotation = targetRotation;
    }

    void Jump()
    {
        Debug.Log("Jump called");

        if (IsGrounded())
        {
            Vector3 jumpForceDirection = -gravityDirection;
            rb.AddForce(jumpForceDirection * jumpForce, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        bool grounded = Physics.Raycast(transform.position, gravityDirection, distanceToGround);

        Debug.Log("Grounded: " + grounded);
        return grounded;
    }

    void VisualizeGroundCheck()
    {
        Debug.DrawRay(transform.position, gravityDirection * distanceToGround, Color.yellow);
    }
}
