using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  // Pastikan menggunakan Input System yang benar

public class ControlPlayer2 : MonoBehaviour
{
    public float speed;
    private Vector2 move, mouselook, joystickLook; 
    private Vector3 rotationTarget;
    public bool isPc;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>(); // Perbaikan: RedValue diubah menjadi ReadValue
    }
    
    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouselook = context.ReadValue<Vector2>(); // Perbaikan: RedValue diubah menjadi ReadValue
    }

    public void OnJoystickLook(InputAction.CallbackContext context)
    {
        joystickLook = context.ReadValue<Vector2>(); // Perbaikan: RedValue diubah menjadi ReadValue
    }

    // Start is called before the first frame update
    void Start()
    {
        // Kode inisialisasi bisa ditambahkan di sini
    }

    // Update is called once per frame
    void Update()
    {
        if (isPc)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mouselook);

            if (Physics.Raycast(ray, out hit)) // Perbaikan: 'physics' diubah menjadi 'Physics'
            {
                rotationTarget = hit.point;
            }

            movePlayerWithAim();
        }
        else
        {
            if (joystickLook.x != 0 || joystickLook.y != 0)
            {
                movePlayerWithAim();
            }
            else
            {
                movePlayer(); // Pastikan pemain tetap bisa bergerak saat tidak mengarahkan
            }
        }
    }

    public void movePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }
    }

    public void movePlayerWithAim()
    {
        if (isPc)
        {
            var lookPos = rotationTarget - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);

            Vector3 aimDirection = new Vector3(rotationTarget.x, 0f, rotationTarget.z);

            if (aimDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f); // Perbaikan: '0,15f' diubah menjadi '0.15f'
            }
        }
        else
        {
            Vector3 aimDirection = new Vector3(joystickLook.x, 0f, joystickLook.y);

            if (aimDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(aimDirection), 0.15f); // Perbaikan: '0,15f' diubah menjadi '0.15f'
            }
        }
    
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}