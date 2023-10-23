using UnityEngine;

public class ArmaRayos : MonoBehaviour
{
    public float grabDistance = 2f;
    public float sensitivity = 3f;  // Factor de sensibilidad para el movimiento
    private Transform grabbedObject;
    public bool isGrabbing = false;
    private LineRenderer lineRenderer;
    private Camera mainCamera;
    private GameObject intermediateParent;
    private Vector3 originalGlobalScale;
    private float minGrabDistance = 0.5f;
    private float maxGrabDistance = 5f;
    private float scrollSpeed = 1f;
    private Rigidbody grabbedObjectRb;
    private Vector3 lastMousePosition;

    void Start()
    {
        mainCamera = Camera.main;

        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Standard"));
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
        }
        lineRenderer.enabled = false;

        intermediateParent = new GameObject("IntermediateParent");
        intermediateParent.transform.SetParent(transform);
        intermediateParent.transform.localPosition = Vector3.zero;
        intermediateParent.transform.localRotation = Quaternion.identity;
        intermediateParent.transform.localScale = Vector3.one;
    }

    void Update()
    {
        if (mainCamera)
        {
            Plane plane = new Plane(Vector3.up, transform.position);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            float enter = 0.0f;
            if (plane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Vector3 direction = hitPoint - transform.position;
                transform.forward = direction;
            }
        }

        if (Input.GetMouseButton(0))
        {
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.blue;

            if (!isGrabbing)
            {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("agarrable"))
                {
                    grabbedObject = hit.transform;
                    isGrabbing = true;

                    originalGlobalScale = grabbedObject.lossyScale;
                    grabbedObject.SetParent(intermediateParent.transform);
                    grabbedObject.localRotation = Quaternion.Euler(0, 0, 0);
                    grabbedObject.localScale = new Vector3(originalGlobalScale.x / intermediateParent.transform.lossyScale.x,
                                                           originalGlobalScale.y / intermediateParent.transform.lossyScale.y,
                                                           originalGlobalScale.z / intermediateParent.transform.lossyScale.z);

                    grabbedObjectRb = grabbedObject.GetComponent<Rigidbody>();
                    if (grabbedObjectRb)
                    {
                        grabbedObjectRb.useGravity = false;
                        grabbedObjectRb.freezeRotation = true;  // Congelar rotación
                    }

                    Collider objCollider = grabbedObject.GetComponent<Collider>();
                    if (objCollider)
                    {
                        objCollider.isTrigger = true;
                    }

                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, hit.point);
                    lineRenderer.enabled = true;

                    lastMousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, grabDistance));
                }
            }
            else
            {
                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, grabDistance));
                Vector3 deltaMousePosition = (mousePosition - lastMousePosition) * sensitivity;  // Aplicar factor de sensibilidad

                grabbedObject.position += deltaMousePosition;

                lastMousePosition = mousePosition;
                

                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, grabbedObject.position);
            }
        }
        else if (Input.GetMouseButtonUp(0) && isGrabbing)
        {
            if (grabbedObjectRb)
            {
                grabbedObjectRb.useGravity = true;
                grabbedObjectRb.freezeRotation = false;  // Descongelar rotación
            }

            Collider objCollider = grabbedObject.GetComponent<Collider>();
            if (objCollider)
            {
                objCollider.isTrigger = false;
            }

            grabbedObject.SetParent(null);
            grabbedObject = null;
            isGrabbing = false;

            lineRenderer.enabled = false;
        }
    }
}
