using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forceMagnitude;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float rotationSpeed;
    
    private Rigidbody rb;
    private Camera mainCamera;

    private Vector3 movementDirection;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        KeepPlayerOnScreen();
        RotateToFaceVelocity();
    }

    void FixedUpdate() //is called every time the physics system updates
    {
        if(movementDirection == Vector3.zero) {return;}

        rb.AddForce(movementDirection * forceMagnitude, ForceMode.Force); //fixedupdate içinde  * Time.deltaTime gerekmiyor çünkü ztn öyle çalışıo

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity); //hız sınırlama

    }

    private void ProcessInput()
    {
        if(Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

            movementDirection = transform.position - worldPosition;
            movementDirection.z = 0f;
            movementDirection.Normalize();

        }
        else
        {
            movementDirection = Vector3.zero;
        }
    }

    private void KeepPlayerOnScreen()
    {
        Vector3 newPosition = transform.position;
        //Viewport posiiton cihaz farketmeksizin 0dan 1 e gider ondn onu kullanmamız lazım
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        if(viewportPosition.x > 1)
        {
            newPosition.x = -newPosition.x + 0.1f;
        }
        else if(viewportPosition.x < 0)
        {
            newPosition.x = -newPosition.x - 0.1f;
        }
        
        if(viewportPosition.y > 1)
        {
            newPosition.y = -newPosition.y + 0.1f;
        }
        else if(viewportPosition.y < 0)
        {
            newPosition.y = -newPosition.y - 0.1f;
        }


        transform.position = newPosition;
    }

    private void RotateToFaceVelocity()
    {
        if(rb.velocity == Vector3.zero) {return;} //başlangıçta 0 hız olunca saçma  bi yere rotate olmaması için
        
        Quaternion targetRotation = transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.back); //yukarı bakacak ve bize görünecek yani z back bize görünen kısım, hız ztn yukarı dogru
        transform.rotation = Quaternion.Lerp(transform.rotation , targetRotation , rotationSpeed * Time.deltaTime);
    }

}
