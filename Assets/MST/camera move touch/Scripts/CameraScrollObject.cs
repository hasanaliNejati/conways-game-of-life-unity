using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrollObject : MonoBehaviour
{
    [SerializeField] Camera cam;
    [Header("property")]
    [SerializeField] float scrollSpeed = 10;
    [SerializeField] float zomeSpeed = 10;
    [Header("Constraint")]
    [SerializeField] MinMax minMaxOrthographicSize;
    public Vector2 areaSize;
    //LOGIC
    Vector3 startPos;


    float _orthographicSize;
    public float orthographicSize
    {
        get { return _orthographicSize; }
        set
        {
            _orthographicSize = Mathf.Clamp(value, minMaxOrthographicSize.Min, minMaxOrthographicSize.Max);
            //setConstraint
            position = position;
        }
    }
    Vector2 _position;
    public Vector2 position
    {
        get
        {
            return _position;
        }
        set
        {
            float xOrthographicSize = cam.orthographicSize * cam.aspect;
            float x = 0;
            if (xOrthographicSize < areaSize.x)
                x = Mathf.Clamp(value.x, startPos.x - areaSize.x + xOrthographicSize, startPos.x + areaSize.x - xOrthographicSize);
            else
                x = startPos.x;
            float y = 0;
            if (cam.orthographicSize < areaSize.y)
                y = Mathf.Clamp(value.y, startPos.y - areaSize.y + orthographicSize, startPos.y + areaSize.y - orthographicSize);
            else
                y = startPos.y;

            float z = transform.position.z;
            _position = new Vector3(x, y, z);
        }
    }


    private void Start()
    {
        startPos = transform.position;
        position = startPos;
        orthographicSize = cam.orthographicSize;
    }
    private void Update()
    {
        Vector3 pos = new Vector3(position.x, position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, pos, scrollSpeed * Time.deltaTime);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, orthographicSize, zomeSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
            Gizmos.DrawWireCube(startPos, areaSize * 2);
        else
            Gizmos.DrawWireCube(transform.position, areaSize * 2);


    }
}
