using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Letter类封装了一些显示
public class Letter : MonoBehaviour
{
    private char _c;

    public TextMesh textMesh;
    public Renderer render;

    public bool big = false;//对于大字母有不同的操作

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponentInChildren<TextMesh>();        
        render = textMesh.GetComponent<Renderer>();
        visiable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public char c
    {
        get 
        {
            return _c; 
        }
        set 
        {
            _c = value;
            textMesh.text = _c.ToString();
        }
    }

    //设置或者获取_c为一个字符串
    public string str 
    {
        get 
        {
            return _c.ToString();
        }
        set 
        {
            //c = value[0]的意义在哪？
            c = value[0];
        }
    }

    public bool visiable 
    {
        get 
        {
            return render.enabled;
        }
        set 
        {
            render.enabled = value;
        }
    }

    public Color color 
    {
        get 
        {
            return render.material.color;
        }
        set 
        {
            render.material.color = value;
        }
    }

    public Vector3 pos
    {
        set 
        {
            transform.position = value;
        }
    }


}
