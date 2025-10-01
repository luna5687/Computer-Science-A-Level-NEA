using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer_Science_A_Level_NEA
{
    static class POSTagging
    {
        private struct word
        {
            public string value;
            public string wordType;
        }
        static public string[] Determiners = { "a", "an", "the", "this", "that", "these", "those", "my", "your", "his", "her", "it's", "our", "their" };
        // Det = determiners, N = noun, V = verb, Adv = adverb,adj = adjactive
        // could add a list of common adjectives e.g new, imporant etc
        // check to see if a word has been tagged later if it has been tagged ealier on 
        static public void POStagging(string input) // tessting POS
        {
            string[] WordsToFilter = { "\n", "\r" };
            char[] Body = input.ToCharArray();
            string Temp = "";
            string text = "";

            foreach (string word in WordsToFilter)
            {
                for (int i = 0; i < Body.Length - word.Length; i++)
                {
                    Temp = "";
                    for (int j = 0; j < word.Length; j++)
                    {
                        Temp += Body[i + j];
                    }
                    if (Temp.ToLower() == "\r" || Temp.ToLower() == "\n")
                    {
                        for (int k = 0; k < word.Length; k++)
                        {
                            Body[i + k] = '.';
                        }
                    }
                }
            }
            foreach (char character in Body)
            { text += character; }
            string[] words = text.Split('.');
            List<List<word>> SplitSentences = new List<List<word>>();
            List<word> TempList = new List<word>();
            word TempWord = new word();
            foreach (string word in words)
            {
                if (!(word == ""))
                {
                    foreach (string word2 in word.Split(' '))
                    { if (!(word2 == ""))
                        {
                            TempWord = new word();
                            TempWord.value = word2;
                            if (Determiners.Contains(word2))
                            {
                                TempWord.wordType = "Det";
                            }
                            TempList.Add(TempWord);
                        }
                    }
                    SplitSentences.Add(TempList);
                    TempList = new List<word>();
                }
            }
            Console.WriteLine();
            for (int i = 0; i < SplitSentences.Count; i++)
            {
                for (int j = 0; j < SplitSentences[i].Count; j++)
                {
                    word temp = SplitSentences[i][j];
                    if (j != 0)
                    {
                        if (SplitSentences[i][j-1].wordType == "Det")
                        {
                            temp.wordType = "N";
                        }
                        if ((int)SplitSentences[i][j].value[0] >= 65 && (int)SplitSentences[i][j].value[0] <= 90)
                        {
                            temp.wordType = "N";
                        }
                    }
                    SplitSentences[i][j] = temp;
                }
            }
            foreach (List<word> sentences in SplitSentences)
            {
                foreach (word sent in sentences)
                {
                    Console.Write($"{sent.value}<{sent.wordType}>");
                }
                Console.WriteLine();
            }
        }
    }
}
