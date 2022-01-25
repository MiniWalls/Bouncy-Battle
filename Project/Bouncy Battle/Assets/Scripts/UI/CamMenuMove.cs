using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMenuMove : MonoBehaviour
{
    [SerializeField]
    private Vector2 min;
    [SerializeField]
    private Vector2 max;
    [SerializeField]
    private Vector2 yRotationRange;
    [SerializeField]
    [Range(0.01f,0.1f)]
    private float lerpSpeed = 0.05f;

    Vector3 newPosition;
    Quaternion newRotation;

    void Awake()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * lerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * lerpSpeed);        
        if(Vector3.Distance(transform.position, newPosition) < 1f)
        {
            GetNewPos();
        }
        
    }

    void GetNewPos()
    {
        var xPos = Random.Range(min.x, max.x);
        var yPos = Random.Range(min.y, max.y);
        newRotation = Quaternion.Euler(30, Random.Range(yRotationRange.x, yRotationRange.y), 0);
        newPosition = new Vector3(xPos, 0, yPos);
    }
}
