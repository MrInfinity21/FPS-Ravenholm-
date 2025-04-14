using UnityEngine;

public class ColorSwap : MonoBehaviour
{
[SerializeField] Material _swapMaterial;

public void SwapColor()
{
    GetComponent<Renderer>().material = _swapMaterial;
}

}
