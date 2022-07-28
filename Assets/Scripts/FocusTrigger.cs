using UnityEngine;
using UnityEngine.EventSystems;

public class FocusTrigger : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    public void OnPointerUp(PointerEventData pointerEventData)
    {
		VuforiaScanner.Focus();
    }

	public void OnPointerDown(PointerEventData pointerEventData)
	{
		
	}

}