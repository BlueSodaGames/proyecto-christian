using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private SpriteRenderer rend;
    public Sprite clickCursor;
    public Sprite normalCursor;
    public Sprite interactuableCursor;

    void Start()
    {
        Cursor.visible = false;
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;

        //if (Input.GetButtonDown("Fire1"))
        //{
        //    rend.sprite = clickCursor;
        //}
        //else if (Input.GetButtonUp("Fire1"))
        //{
        //    rend.sprite = normalCursor;
        //}
    }

    public void setInteractuableCursor()
    {
        rend.sprite = interactuableCursor;
    }

    public void setNormalCursor()
    {
        rend.sprite = normalCursor;
    }
}
