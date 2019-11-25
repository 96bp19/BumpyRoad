using UnityEngine;

public class GroundHugging : MonoBehaviour
{
    public GameObject carModel;
    public Transform raycastPoint;
    [SerializeField]
    private float hoverHeight = 5.0f;
    [SerializeField]
    private float speed = 20.0f;
    private float terrainHeight;
    private float rotationAmount;
    private RaycastHit hit;
    private Vector3 pos;
    private Vector3 forwardDirection;
    void Update()
    {
        Physics.Raycast(raycastPoint.position, Vector3.down, out hit, 10);
        Debug.DrawRay(raycastPoint.position, Vector3.down, Color.blue);
        Debug.DrawRay(hit.point, Vector3.up * 5, Color.red);
        // Keep at specific height above terrain
        pos = transform.position;
        //float terrainHeight = Terrain.activeTerrain.SampleHeight(pos);
        transform.position = new Vector3(pos.x,
                                         hit.point.y + hoverHeight,
                                         pos.z);

        // Rotate to align with terrain
        transform.up -= (transform.up - hit.normal) * 0.2f;

        // Rotate with input
        rotationAmount = Input.GetAxis("Vertical") * 120.0f;
        rotationAmount *= Time.deltaTime;
        carModel.transform.Rotate(0.0f, rotationAmount, 0.0f);    // Move forward
        forwardDirection = carModel.transform.forward;
          float InputX = Input.GetAxis("Horizontal");
       carModel.GetComponent<Rigidbody>().AddForceAtPosition( forwardDirection * Time.deltaTime * speed * InputX,carModel.transform.position);
           

    }
}