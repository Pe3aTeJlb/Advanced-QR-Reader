using UnityEngine;
using UnityEngine.EventSystems;

public class FocusTrigger : MonoBehaviour, IPointerUpHandler {

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        VuforiaScanner.Focus();
    }

}