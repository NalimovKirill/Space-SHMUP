using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Предотвращает выход игрового объекта за границы экрана.
/// Важно: работает ТОЛЬКО с ортографической камерой Main Camera в [0,0,0]
/// </summary>
public class BoundsCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 1f;
    public bool keepOnScreen = true;

    [Header("Set dinamically")]
    public bool IsOnScreen = true;
    public float camWidth;
    public float camHeight;

    [HideInInspector]
    public bool offRight, offLeft, offUp, offDown;

    private void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        IsOnScreen = true;
        offRight = offLeft = offUp = offDown = false;

        if (pos.x > camWidth - radius)
        {
            pos.x = camWidth - radius;
            IsOnScreen = false;
            offRight = true;
        }
        if (pos.x < - camWidth + radius)
        {
            pos.x = - camWidth + radius;
            IsOnScreen = false;
            offLeft = true;
        }
        if (pos.y > camHeight - radius)
        {
            pos.y = camHeight - radius;
            IsOnScreen = false;
            offUp = true;
        }
        if (pos.y < - camHeight + radius)
        {
            pos.y = - camHeight + radius;
            IsOnScreen = false;
            offDown = true;
        }

        IsOnScreen = !(offRight || offLeft || offUp || offDown);
        if (keepOnScreen && !IsOnScreen)
        {
            transform.position = pos;
            IsOnScreen = true;
            offRight = offLeft = offUp = offDown = true;
        }
        
    }

    // Рисует границы в панели Scene с помощью OnDrawGizmos()
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
