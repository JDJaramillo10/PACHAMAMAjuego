using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Seguir al jugador
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        }

    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }


}
