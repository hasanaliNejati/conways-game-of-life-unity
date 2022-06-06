using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class FingerControllInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] CameraScrollObject scrollObject;
    [SerializeField] float scrollSensitivity = 1;
    [SerializeField] float zomeSensitivity = 1.5f;

    //LOGIC
    List<PointerEventData> pointers = new List<PointerEventData>();
    float beforDobleTouchDistance;
    Vector2 beforTouchCenter;


    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointers.Add(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointers.Remove(eventData);
    }


    void Update()
    {
#if UNITY_STANDALONE
        OneFingerScroll();
#else
        TwoFingerScroll();
#endif

        CameraZome();

        //if (pointers.Count >= 1)
        //{
        //    if(pointers.Count == 1)
        //    {
        //        //SCROLL BY 1 FINGER
        //    }
        //    else
        //    {
        //        //scroll by 2 finger

        //        //resize
        //    }

        //    Vector2 pos0 = Camera.main.ScreenToViewportPoint(pointers[0].position);
        //    //Vector2 pos0 = Camera.main.transform.position - Camera.main.ScreenToWorldPoint(pointers[0].position);
        //    Vector2 pos1 = Camera.main.ScreenToViewportPoint(pointers[1].position);
        //    //Vector2 pos1 = Camera.main.transform.position - Camera.main.ScreenToWorldPoint(pointers[1].position);
        //    Vector2 direction = pos1 - pos0;
        //    float distance = direction.magnitude;
        //    Vector2 touchCenter = pos0 + (direction / 2);
        //    if (beforDobleTouchDistance != 0 && beforTouchCenter != Vector2.zero)
        //    {
        //        scrollObject.position += (beforTouchCenter - touchCenter) * scrollSensitivity;
        //        scrollObject.orthographicSize += (beforDobleTouchDistance - distance) * zomeSensitivity;
        //    }
        //    beforTouchCenter = touchCenter;
        //    beforDobleTouchDistance = distance;
        //}
        //else
        //{
        //    beforTouchCenter = Vector2.zero;
        //    beforDobleTouchDistance = 0;
        //}
    }

    void OneFingerScroll()
    {
        if (pointers.Count == 1)
        {
            Vector2 pos0 = Camera.main.transform.position - Camera.main.ScreenToWorldPoint(pointers[0].position);
            Vector2 touchCenter = pos0;
            if (beforTouchCenter != Vector2.zero)
            {

                scrollObject.position += (touchCenter - beforTouchCenter ) * scrollSensitivity;
            }
            beforTouchCenter = touchCenter;
        }
        else
        {
            beforTouchCenter = Vector2.zero;
        }
    }
    void TwoFingerScroll()
    {
        if (pointers.Count >= 2)
        {
            Vector2 pos0 = Camera.main.transform.position - Camera.main.ScreenToWorldPoint(pointers[0].position);
            Vector2 pos1 = Camera.main.transform.position - Camera.main.ScreenToWorldPoint(pointers[1].position);
            Vector2 direction = pos1 - pos0;
            Vector2 touchCenter = pos0 + (direction / 2);
            if (beforTouchCenter != Vector2.zero)
            {
                scrollObject.position += (touchCenter - beforTouchCenter) * scrollSensitivity;
            }
            beforTouchCenter = touchCenter;
        }
        else
        {
            beforTouchCenter = Vector2.zero;
        }
    }
    void CameraZome()
    {
        if (pointers.Count >= 2)
        {
            Vector2 pos0 = Camera.main.ScreenToViewportPoint(pointers[0].position);
            Vector2 pos1 = Camera.main.ScreenToViewportPoint(pointers[1].position);
            Vector2 direction = pos1 - pos0;
            float distance = direction.magnitude;
            if (beforDobleTouchDistance != 0)
            {
                scrollObject.orthographicSize += (beforDobleTouchDistance - distance) * zomeSensitivity * scrollObject.orthographicSize;
            }
            beforDobleTouchDistance = distance;
        }
        else
        {
            beforDobleTouchDistance = 0;
        }

        scrollObject.orthographicSize -= Input.mouseScrollDelta.y;
    }
}
