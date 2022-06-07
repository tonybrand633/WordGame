using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Myrd 
{
    //该单词的字符串表示
    public string str;

    public List<Letter> letters = new List<Letter>();

    //如果玩家找到该词,返回true
    public bool found = false;

    public bool visiable
    {
        get
        {
            if (letters.Count == 0) return false;
            return letters[0].visiable;
        }
        set 
        {
            foreach (Letter l in letters)
            {
                l.visiable = value;
            }
        }
    }

    public Color color 
    {
        get 
        {
            if (letters.Count == 0) return Color.black;
            return letters[0].color; 
        }
        set 
        {
            foreach (Letter l in letters)
            {
                l.color = value;
            }
        }
    }

    public void AddLetter(Letter l) 
    {
        letters.Add(l);
        str += l.c.ToString();
    }
}
