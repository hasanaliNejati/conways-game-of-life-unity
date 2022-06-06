using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDrow : MonoBehaviour
{
    public float size;
    public ConwayOfLife life;
    public bool penActive;
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (penActive && Input.GetMouseButton(0) && Physics.Raycast(ray, out hit, 100))
        {
            Vector2 point = hit.transform.position - hit.point;
            point /= size;
            point += Vector2.one / 2;
            point *= life.size;
            Vector2Int pointInt = new Vector2Int((int)point.x, (int)point.y);
            life.SetCell(true, pointInt.x, pointInt.y);
        }
    }

    public void SetPenActive(bool active)
    {
        penActive = active;
    }
}
