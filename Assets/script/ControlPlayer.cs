using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    public float speed = 5f; // Kecepatan pergerakan karakter
    public float rotationSpeed = 500f; // Kecepatan rotasi karakter
    public float jumpHeight = 2f; // Ketinggian lompatan
    public float gravity = -9.81f; // Gaya gravitasi
    public Animator animator; // Referensi ke komponen Animator

    private CharacterController characterController;
    private Vector3 velocity; // Kecepatan vertikal

    private void Start()
    {
        // Mendapatkan referensi ke CharacterController
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Mengambil input dari pengguna
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Membuat vektor gerakan berdasarkan input
        Vector3 movement = new Vector3(horizontal, 0f, vertical);

        // Mengubah arah gerakan relatif terhadap orientasi karakter
        movement = transform.TransformDirection(movement);

        // Menggerakkan karakter
        characterController.Move(movement * speed * Time.deltaTime);

        // Mengatur rotasi karakter jika bergerak maju atau mundur
        if (movement.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Mengatur animasi berjalan
        bool isWalking = movement.magnitude > 0;
        animator.SetBool("IsWalking", isWalking);

        // Logika lompat
        if (characterController.isGrounded)
        {
            // Reset kecepatan vertikal saat berada di tanah
            velocity.y = 0f;

            if (Input.GetButtonDown("Jump"))
            {
                // Memicu lompatan
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                animator.SetBool("IsJumping", true); // Mengatur parameter lompat di Animator
            }
            else
            {
                animator.SetBool("IsJumping", false); // Mengatur parameter lompat di Animator
            }
        }

        // Menerapkan gravitasi ke kecepatan vertikal
        velocity.y += gravity * Time.deltaTime;

        // Menggerakkan karakter berdasarkan kecepatan vertikal
        characterController.Move(velocity * Time.deltaTime);
    }
}
