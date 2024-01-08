using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform player;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x + 9.5f, transform.position.y, transform.position.z);
    }
}
