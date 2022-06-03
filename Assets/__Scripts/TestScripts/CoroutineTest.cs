using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTest : MonoBehaviour
{
    private int i=0;
    private int j = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start1");
        StartCoroutine(CoroutineSample());
        Debug.Log("Start2");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("第"+ ++i +"帧开始");    
    }

    void LateUpdate()
    {
        Debug.Log("第"+ i +"帧结束");
    }

    IEnumerator CoroutineSample() 
    {
        while (true) 
        {
            Debug.Log("协程"+ ++j +"第一次执行");
            yield return null;
            //在LateUpdate之后第二次，在第二次Update之后，协程1第二次执行
            Debug.Log("协程"+j+"第二次执行");
        }       
    }
}
