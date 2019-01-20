using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public LayerMask blockingLayer;
    
    [HideInInspector]
    public Rigidbody2D rb2D;

    [HideInInspector]
    public Vector3 wrld;
    [HideInInspector]
    public float half_y;
    [HideInInspector]
    public float half_x;

    protected virtual void Start() {
        wrld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        Vector3 size = GetComponent<Renderer>().bounds.size;
        half_y = size.y / 2;
        half_x = size.x / 2;

        rb2D = GetComponent<Rigidbody2D>();
    }

    protected void MoveTo (Vector2 end) {

        if (end.y > (wrld.y - half_y)) {
            end = new Vector2(end.x, wrld.y - half_y);
        } else if (end.y < (half_y - wrld.y)) {
            end = new Vector2(end.x, half_y - wrld.y);
        }

        // Do I need this?
        //if (end.x > (wrld.x - half_x)) {
        //    end = new vector2(wrld.x - half_x, end.y);
        //} else if (end.x < (half_x - wrld.x)) {
        //    end = new vector2(half_x - wrld.x, end.y);
        //}

        rb2D.MovePosition(end);
    }
}
