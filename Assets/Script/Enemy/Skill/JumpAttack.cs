using System.Collections;
using UnityEngine;

public class JumpAttack : MonoBehaviour
{
    public Transform player; // Vị trí của người chơi
    public float jumpHeight = 5f; // Chiều cao nhảy
    public float jumpDuration = 1f; // Thời gian nhảy lên
    public float attackDuration = 0.5f; // Thời gian tấn công khi tiếp đất
    public float damageAmount = 10f; // Số lượng sát thương
    public float jumpOffset = 1f; // Khoảng cách offset khi nhảy tới người chơi

    private bool isJumping = false;
    private bool isFalling = false;
    private Vector3 targetPosition; // Vị trí mục tiêu khi nhảy đến
    private Vector3 startPosition; // Vị trí bắt đầu nhảy
    private CharacterController controller;
    private float jumpStartTime;
    private float verticalVelocity = 0f; // Dùng để áp dụng trọng lực
    private float gravity = -9.8f; // Trọng lực

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (isJumping)
        {
            Jump(); // Gọi phương thức nhảy
        }

        if (isFalling)
        {
            Fall(); // Gọi phương thức lao xuống
        }
    }

    // Phương thức gọi để bắt đầu nhảy tấn công
    public void JumpToAttack()
    {
        if (isJumping || isFalling) return; // Nếu đang nhảy hoặc đang rơi thì không gọi lại

        isJumping = true;
        startPosition = transform.position;

        // Tạo vị trí mục tiêu với một khoảng cách nhỏ so với người chơi
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        targetPosition = player.position - directionToPlayer * jumpOffset; // Cách người chơi một khoảng nhỏ

        jumpStartTime = Time.time; // Lưu lại thời gian bắt đầu nhảy
    }

    // Phương thức nhảy lên
    public void Jump()
    {
        float t = (Time.time - jumpStartTime) / jumpDuration;

        if (t < 1f)
        {
            // Tính toán quỹ đạo nhảy (Đi lên)
            float height = Mathf.Sin(t * Mathf.PI) * jumpHeight; // Tạo độ cao nhảy
            Vector3 horizontalMovement = Vector3.Lerp(startPosition, targetPosition, t); // Di chuyển về vị trí người chơi

            verticalVelocity += gravity * Time.deltaTime; // Áp dụng trọng lực

            // Tính toán vị trí mới của nhân vật
            Vector3 newPosition = new Vector3(horizontalMovement.x, startPosition.y + height + verticalVelocity, horizontalMovement.z);
            
            // Sử dụng Move thay vì thay đổi trực tiếp transform.position
            controller.Move(newPosition - transform.position);
        }
        else
        {
            // Khi đạt đến độ cao tối đa, chuyển sang rơi xuống
            isJumping = false;
            isFalling = true; // Bắt đầu phần rơi xuống
        }
    }

    // Phương thức rơi xuống và lao tới vị trí người chơi
    public void Fall()
    {
        // Áp dụng trọng lực để rơi xuống
        verticalVelocity += gravity * Time.deltaTime;

        // Di chuyển thẳng đến vị trí của người chơi
        Vector3 fallPosition = new Vector3(targetPosition.x, transform.position.y + verticalVelocity, targetPosition.z);
        
        // Di chuyển nhân vật đến vị trí người chơi (Lao tới)
        transform.position = Vector3.MoveTowards(transform.position, fallPosition, Time.deltaTime * 10f);

        if (transform.position == fallPosition)
        {
            isFalling = false; // Kết thúc quá trình rơi xuống
        }
    }

    // Kết thúc tấn công
    // Kết thúc tấn công và reset lại để có thể thực hiện lần tiếp theo
    public void FinishingAttack()
    {
        // Reset các trạng thái nhảy và rơi
        isJumping = false;
        isFalling = false;
        verticalVelocity = 0f; // Reset trọng lực

        // Đặt lại vị trí của nhân vật (nếu cần thiết)
        transform.position = startPosition; // Đảm bảo nhân vật quay về vị trí ban đầu (nếu cần)

        // Bật lại Root Motion sau khi kết thúc tấn công
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.applyRootMotion = true;
        }
    }
}
