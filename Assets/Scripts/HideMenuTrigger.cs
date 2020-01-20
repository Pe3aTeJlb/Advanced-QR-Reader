using UnityEngine;
using UnityEngine.EventSystems;

public class HideMenuTrigger : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler,IPointerUpHandler
{

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        MenuController.trig2 = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        MenuController.trig2 = false;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        MenuController.trig2 = true;
    }

}
