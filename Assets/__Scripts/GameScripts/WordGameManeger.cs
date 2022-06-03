using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordGameManeger : MonoBehaviour
{
    public static WordGameManeger W;

    public GameMode mode = GameMode.preGame;
    public WordLevel curLevel;

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
        List<string> subWords = new List<string>();
        string s;
        for (int i = 0; i < WordList.singleton.wordCount; i++)
        {
            s = allWords[i];
            string sa = level.word;
            //if (s[0]==sa[0]&&s[1]==sa[1]&&s[2]==sa[2]) 
            //{
            //    Debug.Log("Find");
            //}
            if (WordLevel.CheckWordInLevel(s, level))
            {
                subWords.Add(s);
            }
        }
        level.subWords = subWords;
        foreach (string w in level.subWords) 
        {
            Debug.Log(w);
        }

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
    public void SubSearchComplete() 
    {
        mode = GameMode.preGame;
    }

    public void WordListParseComplete() 
    {
        mode = GameMode.makeLevel;
    }
}
