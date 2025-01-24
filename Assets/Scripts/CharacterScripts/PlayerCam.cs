using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    float xRotation = 0.0f;
    public float sensitivity = 100.0f;
    public Transform playerBody;

    void Start(){ Cursor.lockState = CursorLockMode.Locked; }

    void Update(){ MouseControls(); }

    void MouseControls(){
        float rotX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float rotY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= rotY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
        playerBody.Rotate(Vector3.up * rotX);
    }
}
