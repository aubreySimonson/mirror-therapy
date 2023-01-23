using UnityEngine;

// Transform.rotation example.

// Rotate a GameObject using a Quaternion.
// Tilt the cube using the arrow keys. When the arrow keys are released
// the cube will be rotated back to the center using Slerp.

public class ExampleScript : MonoBehaviour
{
    float smooth = 5.0f;
    float tiltAngle = 60.0f;
    public GameObject worldOrigin;


    void Update()
    {
      transform.RotateAround(worldOrigin.transform.position, Vector3.up, 20 * Time.deltaTime);
    }
}
