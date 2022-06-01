using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordList : MonoBehaviour
{
    public static WordList singleton;

    public TextAsset wordListText;

    public int wordLengthMin = 3;
    public int wordLengthMax = 7;

    public int numToYield = 10000;

    public int currentLine = 0;
    public int totalLines;
    public int longWordCount;
    public int wordCount;

    string[] lines;
    List<string>longWords;
    List<string> words;


    void Awake()
    {
        singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        lines = wordListText.text.Split('\n');
        totalLines = lines.Length;
        StartCoroutine(ParseLine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ParseLine() 
    {
        string word;
        longWords = new List<string>();
        words = new List<string>();

        for (int curLine = 0; curLine < totalLines; curLine++)
        {
            word = lines[curLine];
            if (word.Length==wordLengthMax) 
            {
                longWords.Add(word);
            }
            if (word.Length>=wordLengthMin&&word.Length<wordLengthMax) 
            {
                words.Add(word);
            }

            if (curLine%numToYield==0) 
            {
                longWordCount = longWords.Count;
                wordCount = words.Count;
                //下一帧之后再执行代码
                yield return null;
            }
        }
    }


    //四个方法允许其他类访问私有的longWords和words变量
    public List<string> GetWords() 
    {
        return words;
    }

    public string GetWordsByIndex(int indx) 
    {
        return words[indx];
    }

    public List<string> GetLongWords() 
    {
        return longWords;
    }

    public string GetLongWordsByIndex(int indx) 
    {
        return longWords[indx];
    }

}
