using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFollowWorld : MonoBehaviour
{
    [Header("Tweaks")]
    public Transform lookAt;
    public Vector3 offset;
    
    [Header("Logic")]
    private Camera cam;
    
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        transform.position = cam.WorldToScreenPoint(lookAt.position + offset);
    }
}
