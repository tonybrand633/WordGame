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
    public Color bigColorDim = new Color(0.8f, 0.8f, 0.8f);
    public Color bigColorSelect = Color.white;
    public Color targetFoundColor = Color.white;
    public Vector3 bigLetterCenter = new Vector3(0, 0, 0);

    public GameMode mode = GameMode.preGame;
    public WordLevel curLevel;
    public List<Wyrd> wyrds;
    public List<Letter> bigLetters;
    public List<Letter> bigLettersActive;
    public string testword;
    public string UpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";


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
        Letter lett;
        char c;

        switch (mode) 
        {
            case GameMode.inLevel:
                foreach (char cInput in Input.inputString)
                {
                    c = System.Char.ToUpperInvariant(cInput);

                    if (UpperCase.Contains(c)) 
                    {
                        lett = FindLetterByNext(c);
                        if (lett!=null) 
                        {
                            //Debug.Log("找到Letter");
                            testword += lett.c.ToString();
                            //Debug.Log(testword);
                            bigLettersActive.Add(lett);
                            bigLetters.Remove(lett);
                            lett.color = bigColorSelect;
                            ArrangeBigLetters();
                        }
                    }
                    //下面几种情况，删除-确认-重排BigLetters
                    if (c == '\b') 
                    {
                        if (bigLettersActive.Count>0) 
                        {
                            int index = bigLettersActive.Count-1;
                            lett = bigLettersActive[index];
                            bigLettersActive.Remove(lett);
                            bigLetters.Add(lett); 

                            if (testword.Length > 0)
                            {
                                testword = testword.Substring(0, testword.Length - 1);
                            }
                            else 
                            {
                                testword = "";
                            }
                            lett.color = bigColorDim;
                            ArrangeBigLetters();
                        }                    
                    }

                    if (c == '\r'||c == '\n') 
                    {
                        CheckWord();
                    }

                    if (c == ' ') 
                    {
                        ShufferBigWord(bigLetters);
                        ArrangeBigLetters(); 
                    }

                }
                break;
        }                        
    }

    void CheckWord() 
    {
        string subWord;
        bool testFound = false;
        List<int> containsWords = new List<int>();

        //这里运用的是curLevel.SubWords的索引和Wyrds的索引是一致的
        for (int i = 0; i < curLevel.subWords.Count; i++)
        {
            subWord = curLevel.subWords[i];
            //如果这个单词已经被找到了，跳出本次循环的其余部分
            if (wyrds[i].found) 
            {
                continue;
            }

            if (string.Equals(subWord, testword))
            {
                HighlightTarget(i);
                testFound = true;
            } else if (testword.Contains(subWord)) 
            {
                //加入包含的单词
                containsWords.Add(i);
            }
        }
        if (testFound) 
        {
            int numContain = containsWords.Count;
            int ndx;
            for (int i = 0; i < numContain; i++) 
            {
                ndx = numContain - i - 1;
                HighlightTarget(containsWords[ndx]);
            }
        }
        ClearBigLettersActive();        
    }

    void HighlightTarget(int index) 
    {
        wyrds[index].found = true;
        wyrds[index].color = targetFoundColor;
        wyrds[index].visiable = true;
    }

    void ClearBigLettersActive() 
    {
        Letter lett;
        testword = "";
        for (int i = 0; i < bigLettersActive.Count; i++)
        {
            lett = bigLettersActive[i];
            lett.color = bigColorDim;
            bigLetters.Add(lett);
        }
        bigLettersActive.Clear();
        ArrangeBigLetters();
    }

    Letter FindLetterByNext(char c) 
    {
        foreach (Letter lett in bigLetters) 
        {
            if (lett.c == c) 
            {
                return lett;
            }
        }
        return null;

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
        //bigLettersMatter
        Color col;
        Wyrd wyrd;

        int numRows = Mathf.RoundToInt(wordArea.height / letterSize);


        
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

                //关于位置，他这里很巧妙的用了y的位置都是一样的概念
                pos = new Vector3(wordArea.x + left +j*letterSize, wordArea.y, 0);
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
                left += (colunmWidth);
            }
        }

        //生成大写字母
        bigLetters = new List<Letter>();
        bigLettersActive = new List<Letter>();

        for (int i = 0; i < curLevel.word.Length; i++)
        {
            c = curLevel.word[i];
            go = Instantiate(prefabLetter) as GameObject;
            lett = go.GetComponent<Letter>();
            lett.c = c;
            go.transform.localScale = Vector3.one * bigLetterSize;
            pos = bigLetterCenter;
            lett.pos = pos;
            col = bigColorDim;
            lett.color = col;
            lett.big = true;
            lett.visiable = true;
            bigLetters.Add(lett);
        }

        bigLetters = ShufferBigWord(bigLetters);
        ArrangeBigLetters();
        mode = GameMode.inLevel;

    }

    List<Letter> ShufferBigWord(List<Letter>bigWordsList) 
    {
        List<Letter> resList = new List<Letter>();
        int index;
        while (bigWordsList.Count>0) 
        {
            //排除（不包括）bigWordsList.Count
            index = Random.Range(0, bigWordsList.Count);
            Letter tempL = bigWordsList[index];
            resList.Add(tempL);
            bigWordsList.RemoveAt(index);
        }
        return resList;
    }

    void ArrangeBigLetters() 
    {
        Letter lett;
        float halfWidth = bigLetters.Count/2f;
        for (int i = 0; i < bigLetters.Count; i++)
        {
            lett = bigLetters[i];
            Vector3 pos = bigLetterCenter;
            pos.x += (i-halfWidth)*bigLetterSize;
            lett.pos = pos;
        }

        halfWidth = bigLettersActive.Count / 2f;
        for (int i = 0; i < bigLettersActive.Count; i++)
        {
            lett = bigLettersActive[i];
            Vector3 pos = bigLetterCenter;
            pos.x += (i - halfWidth) * bigLetterSize;
            pos.y += bigLetterSize * 1.5f;
            lett.pos = pos;
        }
    }
}
