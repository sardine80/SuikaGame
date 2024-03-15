using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class gamemanager : MonoBehaviour
{
    public GameObject[] circles;
    public float boundXMin =  -4;
    public float boundXMax = 4;
    public float makeTime = 2f;
    public int maxIndex = 2;
    public float mergeTime = 0.5f;
    public int maxHeight=1;
    public float makeHeight = 18;

    public GameObject particle;
    private List<GameObject> circleList = new List<GameObject>();
    private GameObject curCircle;
    private bool canMake=false;
    private float curMakeTime = 0;
    private float lastX = 0;

    private void Update()
    {
        if (curCircle == null && canMake)
        {
            var index = Random.Range(0, maxIndex + 1);
            curCircle = MakeCircle(index, new Vector3(lastX,makeHeight,0));
            curCircle.GetComponent<Circle>().Init(this,index);
        }

        if (canMake == false)
        {
            if (curMakeTime >= makeTime)
            {
                canMake = true;
                curMakeTime = 0;
            }
            else
            {
                curMakeTime += Time.deltaTime;
            }
        }
        
        if (curCircle != null && Input.touchCount==1 && (Input.GetTouch(0).phase == TouchPhase.Began||Input.GetTouch(0).phase == TouchPhase.Moved||Input.GetTouch(0).phase==TouchPhase.Stationary))
        {
            Vector2 touchPos =  Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            float x = touchPos.x;
            
            if (x < boundXMin)
                x = boundXMin;
            if (x > boundXMax)
                x = boundXMax;

            Debug.Log(x);
            lastX = x;
            curCircle.transform.position = new Vector3(x,makeHeight,0);
            
        }

        if (curCircle != null && Input.touchCount==1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            var rigid = curCircle.GetComponent<Rigidbody2D>();
            rigid.gravityScale = 1;
            curCircle = null;
            canMake = false;
        }

    }

    public GameObject MakeCircle(int index,Vector3 position)
    {
        var circle = Instantiate(circles[index], position, Quaternion.identity);
        circleList.Add(circle);
        return circle;
    }

    private void RemoveCircle(GameObject circle)
    {
        circleList.Remove(circle);
        Destroy(circle);
    }

    public void Merge(Circle a,Circle b)
    {
        StartCoroutine(ProcMerge(a, b));
    }

    private IEnumerator ProcMerge(Circle a,Circle b)
    {
        float curMergeTime = 0;
        Vector3 center = (a.transform.position + b.transform.position) / 2;

        a.GetComponent<Rigidbody2D>().simulated = false;
        b.GetComponent<Rigidbody2D>().simulated = false;
        
        while (curMergeTime<mergeTime)
        {
            a.transform.position = Vector3.Lerp(a.transform.position, center, 0.5f);
            b.transform.position = Vector3.Lerp(b.transform.position, center, 0.5f);
            a.transform.localScale = a.transform.localScale * (mergeTime - curMergeTime) / mergeTime;
            b.transform.localScale = b.transform.localScale * (mergeTime - curMergeTime) / mergeTime;
            curMergeTime += Time.deltaTime;
            yield return null;
        }
        
        int curLevel = a.level+1;
        if (curLevel >= circles.Length)
        {
            GameOver();
            yield break;
        }

        RemoveCircle(a.gameObject);
        RemoveCircle(b.gameObject);

        var p =Instantiate(particle, center, Quaternion.identity);
        Destroy(p,0.5f);
        MakeCircle(curLevel, center).GetComponent<Circle>().Init(this,curLevel,true);
    }

    public void GameOver()
    {
        for (int i = circleList.Count - 1; i >= 0; i--)
        {
            RemoveCircle(circleList[i]);
        }
    }
}
