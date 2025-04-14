using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    [SerializeField] private DoorController doorController;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            doorController.OpenDoor();
        }   
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            doorController.CloseDoor();
        }
    }
}
