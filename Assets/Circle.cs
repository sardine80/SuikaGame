using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public bool isMerge = false;
    public int level;
    public bool isStart;

    private gamemanager gm;
    public void Init(gamemanager gm,int level,bool setGravity=false)
    {
        this.gm = gm;
        this.level = level;

        isMerge = false;
        isStart = false;
        if (setGravity)
            GetComponent<Rigidbody2D>().gravityScale = 1f;
    }

    private void Update()
    {
        if(transform.position.y>gm.maxHeight && isStart)
            gm.GameOver();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        isStart = true;
        
        if (isMerge)
            return;
        if (other.transform.CompareTag("circle"))
        {
            var circle = other.transform.GetComponent<Circle>();
            if (circle.isMerge)
                return;
            if (level != circle.level)
                return;

            isMerge = true;
            circle.isMerge = true;
            gm.Merge(this,circle);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (isMerge)
            return;
        if (other.transform.CompareTag("circle"))
        {
            var circle = other.transform.GetComponent<Circle>();
            if (circle.isMerge)
                return;
            if (level != circle.level)
                return;

            isMerge = true;
            circle.isMerge = true;
            gm.Merge(this,circle);
        }
    }
}
