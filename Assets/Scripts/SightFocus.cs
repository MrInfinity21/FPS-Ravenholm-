using UnityEngine;

public class SightFocus : MonoBehaviour
{
    [SerializeField] private Transform _targetOrigin;
    [SerializeField] private float _targetDistance = 1000f;
    [SerializeField] private LayerMask _layerMask;

    void Update()
    {
        Vector3 origin = _targetOrigin.position;
        
        Debug.DrawRay(origin, _targetOrigin.forward * _targetDistance, Color.green);

        if (Physics.Raycast(origin, _targetOrigin.forward, out RaycastHit hit, _targetDistance, _layerMask))
        {
            ColorSwap interactable = hit.collider.gameObject.GetComponent<ColorSwap>();
            if (interactable != null)
            {
                interactable.SwapColor();
            }
            //Debug.Log("Hit: " + hit.collider.name);
        }
    }
}
