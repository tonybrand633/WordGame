using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordGameManeger : MonoBehaviour
{
    public static WordGameManeger W;

    //LayOut Useage
    public GameObject prefabLetter;
    public Rect wordArea = new Rect(-24,19,40,25);
    public float letterSize = 1.5f;
    public bool showAllWyrd = true;
    public float bigLetterSize = 4f;

    public GameMode mode = GameMode.preGame;
    public WordLevel curLevel;
    public List<Wyrd> wyrds;

    public enum GameMode 
    {
        preGame,
        loading,
        makeLevel,
        levelPrep,
        inLevel
    }

    void Awake()
    {
        W = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        mode = GameMode.loading;
        WordList.singleton.Init();
        curLevel = MakeWordLevel();
        //Debug.Log(curLevel.subWords.Count);
        LayOut();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public WordLevel MakeWordLevel(int curLevelIndex = -1) 
    {
        WordLevel level = new WordLevel();
        if (curLevelIndex == -1)
        {            
            level.longWordIndex = Random.Range(0, WordList.singleton.longWordCount);
        }
        else 
        {
            
        }
        level.levelNum = curLevelIndex;
        level.word = WordList.singleton.GetLongWordsByIndex(level.longWordIndex);
        level.charDic = WordLevel.MakeDic(level.word);

        subWordParse(level);
        //StartCoroutine(subWordParse(level));

        return level;
    }

    //public IEnumerator subWordParse(WordLevel level) 
    //{
    //    List<string> allWords = WordList.singleton.GetWords();
    //    string s;
    //    for (int i = 0; i < WordList.singleton.wordCount; i++)
    //    {
    //        s = allWords[i];
    //        if (WordLevel.CheckWordInLevel(s,level)) 
    //        {
    //            level.subWords.Add(s);
    //        }

    //        if (i%WordList.singleton.numToYield==0)
    //        {
    //            yield return null;
    //        }
    //    }

    //    //按照默认字母顺序排列
    //    level.subWords.Sort();

    //    //按照单词长度排列
    //    level.subWords = SortWordsByLength((level.subWords)).ToList();
    //    SubSearchComplete();
    //}
    public void subWordParse(WordLevel level)
    {
        List<string> allWords = WordList.singleton.GetWords();
        level.subWords = new List<string>();
        //List<string> subWords = new List<string>();
        string s;
        for (int i = 0; i < WordList.singleton.wordCount; i++)
        {
            s = allWords[i];
            string sa = level.word;
            if (WordLevel.CheckWordInLevel(s, level))
            {
                level.subWords.Add(s);
            }
        }
        //level.subWords = subWords;
        //按照默认字母顺序排列
        level.subWords.Sort();

        //按照单词长度排列
        level.subWords = SortWordsByLength((level.subWords)).ToList();
        SubSearchComplete();
    }

    public static IEnumerable<string> SortWordsByLength(IEnumerable<string>subWords) 
    {
        //用到了LINQ,orderby通过升序进行排序,select选择返回排序后的结果
        var sorted = from s in subWords
                     orderby s.Length ascending
                     select s;
        return sorted;
    }
    public void WordListParseComplete() 
    {
        mode = GameMode.makeLevel;
    }

    public void SubSearchComplete()
    {
        mode = GameMode.preGame;           
    }

    void LayOut() 
    {
        wyrds = new List<Wyrd>();

        //声明需要使用到的变量
        GameObject go;
        Letter lett;
        string word;
        Vector3 pos;
        float left = 0;
        float colunmWidth = 3;
        char c;
        //Color col;
        Wyrd wyrd;

        int numRows = Mathf.RoundToInt(wordArea.width / letterSize);


        
        //生成每个level.subWord的Wyrd;
        for (int i = 0; i <curLevel.subWords.Count ; i++)
        {
            wyrd = new Wyrd();
            word = curLevel.subWords[i];
            colunmWidth = Mathf.Max(numRows, word.Length);

            for (int j = 0; j <word.Length; j++)
            {
                c = word[j];
                go = Instantiate(prefabLetter) as GameObject;
                lett = go.GetComponent<Letter>();                
                lett.c = c;

                //关于位置
                pos = new Vector3(wordArea.x + left + j * letterSize, wordArea.y, 0);
                pos.y -= (i % numRows) * letterSize;
                lett.pos = pos;
                go.transform.localScale = Vector3.one * letterSize;
                wyrd.AddLetter(lett);                
            }

            if (showAllWyrd) 
            {
                wyrd.visiable = true;
            }

            wyrds.Add(wyrd);

            if (i%numRows==numRows-1) 
            {
                left += (colunmWidth + 0.5f) * letterSize;
            }
        }
    }
}
