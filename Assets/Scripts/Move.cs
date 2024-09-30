using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float speed = 15.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.0f;
    public LayerMask groundLayer; // Lớp mặt đất để raycast

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        PlaceOnGround();
    }

    void Update()
    {
        // Kiểm tra xem nhân vật có đang trên mặt đất không
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Đặt lại vận tốc khi nhân vật trên mặt đất
        }

        // Lấy input từ bàn phím
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Tạo vector di chuyển trên mặt phẳng xz
        Vector3 move = new Vector3(moveX, 0, moveZ);

        // Chuyển đổi vector di chuyển từ local space sang world space
        move = transform.TransformDirection(move);

        // Di chuyển nhân vật
        controller.Move(move * speed * Time.deltaTime);

        // Nhảy
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Áp dụng trọng lực
        velocity.y += gravity * Time.deltaTime;

        // Di chuyển nhân vật theo trục y
        controller.Move(velocity * Time.deltaTime);
    }

    void PlaceOnGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 groundPosition = hit.point;
            transform.position = new Vector3(transform.position.x, groundPosition.y + controller.height / 2, transform.position.z);
        }
    }
}