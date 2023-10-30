using UnityEngine;

public class ObjectViewer : MonoBehaviour {
    private bool isRotating = false;
    private Vector3 lastMousePosition;

    public float zoomSpeed = 5.0f; // Velocidad de zoom ajustable

    void Update()
    {
        // Comienza a rotar cuando se hace clic
        if (Input.GetMouseButtonDown(0))
        {
            isRotating = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isRotating = false;
        }

        // Rotar solo si se mantiene presionado el botón izquierdo
        if (isRotating)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Invertir el sentido del movimiento sobre el eje Y
            mouseX *= -1.0f;

            // Ajustar la velocidad de rotación según tus necesidades
            float rotationSpeed = 5.0f;

            // Rotar alrededor del eje Y (vertical)
            transform.Rotate(Vector3.up, mouseX * rotationSpeed, Space.World);

            // Rotar también alrededor del eje X (horizontal)
            transform.Rotate(Vector3.right, mouseY * rotationSpeed, Space.World);
        }

        // Zoom con la rueda del ratón
        float zoomDelta = Input.mouseScrollDelta.y;
        float zoomAmount = zoomDelta * zoomSpeed * Time.deltaTime;
        transform.Translate(Vector3.forward * zoomAmount, Space.Self);
    }
}
