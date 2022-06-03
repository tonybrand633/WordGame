using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class WordLevel 
{
    //关卡的单词数量
    public int levelNum;

    //选取WordList里面的longWordIndex
    public int longWordIndex;

    //用来构建目标字典的单词
    public string word;

    //用word构建的词典
    public Dictionary<char, int> charDic;

    //可以组成目标单词的单词,长度的话不一定
    public List<string> subWords;


    public static Dictionary<char, int> MakeDic(string word) 
    {
        char c;
        Dictionary<char, int> dict=new Dictionary<char, int>();
        for (int i = 0; i < word.Length; i++)
        {
            c = word[i];
            if (dict.ContainsKey(c))
            {
                dict[c]++;
            }
            else 
            {
                dict.Add(c, 1);
            }
        }
        return dict;
    }

    public static bool CheckWordInLevel(string word,WordLevel level) 
    {        
        Dictionary<char, int> wordDic = new Dictionary<char, int>();
        for (int i = 0; i < word.Length; i++)
        {
            char c = word[i];
            if (level.charDic.ContainsKey(c))
            {
                if (!wordDic.ContainsKey(c))
                {
                    wordDic.Add(c, 1);                    
                }
                else 
                {
                    wordDic[c]++;
                }

                if (wordDic[c]>level.charDic[c]) 
                {
                    return false;
                }
            }
            else 
            {
                return false;
            }
        }
        return true;
    }
}
