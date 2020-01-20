using UnityEngine;
using UnityEngine.EventSystems;

public class ShowMenuTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public void OnPointerDown(PointerEventData pointerEventData)
    {
       MenuController.trig = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        MenuController.trig = false;
    }

}
