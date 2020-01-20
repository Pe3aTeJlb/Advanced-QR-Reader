using UnityEngine;

public class MenuController : MonoBehaviour
{
    public float moveSpeed = 600;

    private Vector2 startPos;
    private Vector2 target;
    public static bool trig,trig2,trig3;
    public int width;
    public GameObject obj;

    void Start()
    {
        var tr = transform as RectTransform;
        target = tr.anchoredPosition;
    }

    void Update()
    {
       
        var tr = transform as RectTransform;
        tr.anchoredPosition = Vector2.MoveTowards(tr.anchoredPosition, target, moveSpeed * Time.deltaTime);

        if (trig3 == true)
        {
            target = new Vector2(width, 0);//hide menu
            VuforiaScanner.MenuOpened = false;
            trig3 = false;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            switch (touch.phase)
            {
                case TouchPhase.Began: startPos = touch.position; break;

                case TouchPhase.Moved:
                    if (trig == true)
                    {
                        if (touch.position.x - startPos.x > 20)
                        {
                            VuforiaScanner.MenuOpened = true;
                            target = new Vector2(0, 0);//show menu
                     
                        }
                    }
                    if (trig2 == true)
                    {


                        if (touch.position.x - startPos.x < -20)
                        {
                            target = new Vector2(width, 0);//hide menu
                            VuforiaScanner.MenuOpened = false;
                           
                        }
                    }

                    break;
            }
        }
    }

}
