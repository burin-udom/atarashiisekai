using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraRaycastInput : MonoBehaviour
{
  //public UnityEngine.UI.Text objectNameDisplay;
  public UnityEvent<GameObject> onRayCastHitEvent;

  void Update()
  {
    if (Input.touchCount > 0)
    {
      Touch touch = Input.GetTouch(0);

      if (touch.phase == TouchPhase.Began)
      {
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
          if (hit.collider != null)
          {
            onRayCastHitEvent?.Invoke(hit.transform.gameObject);
            //objectNameDisplay.text = "Hit: " + hit.collider.gameObject.name;
          }
        }
      }
    }
    else if (Input.GetMouseButtonDown(0))
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      // Perform the raycast
      if (Physics.Raycast(ray, out hit))
      {
        if (hit.collider != null)
        {
          onRayCastHitEvent?.Invoke(hit.transform.gameObject);
          //objectNameDisplay.text = "Hit: " + hit.collider.gameObject.name;
        }
      }
    }
  }
}