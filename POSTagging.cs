namespace Computer_Science_A_Level_NEA
{
    public static class POSTagging
    {
        public struct word
        {
            public word(string value, string wordType)
            {
                this.value = value; this.wordType = wordType;
            }
            public string value;
            public string wordType;
        }
        // Det = determiners, N = noun, V = verb, Adv = adverb,Adj = adjactive
        // could add a list of common adjectives e.g new, imporant etc
        // follow stepin in paper
        static private List<word> Lexicon;
        static public void FillLexicon()
        {
            Lexicon = new List<word>();
            string Words = File.ReadAllText("Lexicon.txt");
            string[] strings = Words.Split("\n");
            for (int i = 0; i < strings.Length - 1; i++)
            {
                strings[i] = strings[i].Substring(0, strings[i].Length - 1);
            }
            word temp;
            foreach (string s in strings)
            {
                if (s != "")
                {
                    temp = new word(s.Substring(0, s.IndexOf(',')), s.Substring(s.IndexOf(",") + 1));
                    Lexicon.Add(temp);
                }
            }
        }
        static string Getsuffixtype(string input)
        {
            word[] Suffixes = { new word("ours", "Adj"), new word("est", "Adj"), new word("ful", "Adj"), new word("fy", "V"), new word("ly", "Adv"), new word("ment", "N") };
            foreach (word w in Suffixes)
            {
                if (input.Length > w.value.Length && input.Substring(input.Length - w.value.Length) == w.value)
                {
                    return w.wordType;
                }
            }
            return null;
        }
        static private bool IsAllCaps(string input)
        {
            string temp = "";

            for (int i = 0; i < input.Length; i++)
            {
                temp += (char)((int)input[i] + 32);
            }
            if (temp == input.ToLower())
            {
                return true;
            }

            return false;
        }
        static private string removeChars(string input)
        {
            char[] ToRemove = { '(', ')', '!', '"', ',', '.' }; // add punciation etc
            for (int i = 0; i < ToRemove.Length; i++)
            {
                for (int j = 0; j < input.Length; j++)
                {
                    if (input[j] == ToRemove[i])
                    {
                        input = input.Remove(j, 1); // has a bug with two remove characters
                    }
                }
            }

            return input;
        }
        static public List<List<word>> POStagging(string input) // tessting POS
        {
            FillLexicon();

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
                    {
                        if (!(word2 == ""))
                        {
                            TempWord = new word();
                            TempWord.value = removeChars(word2);
                            TempWord.wordType = Getsuffixtype(TempWord.value);
                            if (TempWord.wordType == null)
                            {

                                foreach (word w in Lexicon)
                                {
                                    if (TempWord.value == w.value)
                                    {
                                        TempWord.wordType = w.wordType;
                                    }
                                }
                            }
                            TempList.Add(TempWord);
                        }
                    }
                    SplitSentences.Add(TempList);
                    TempList = new List<word>();
                }
            }
            for (int i = 0; i < SplitSentences.Count; i++)
            {
                if (SplitSentences[i].Count == 0)
                {
                    SplitSentences.RemoveAt(i);
                }

            }
            Console.WriteLine();
            word temp;
            for (int i = 0; i < SplitSentences.Count; i++)
            {
                for (int j = 0; j < SplitSentences[i].Count; j++)
                {
                    temp = SplitSentences[i][j];
                    if (temp.wordType == null)
                    {
                        if (j != 0)
                        {
                            if (SplitSentences[i][j - 1].wordType == "Det" || SplitSentences[i][j - 1].wordType == "Adj")
                            {
                                temp.wordType = "N";
                            }

                            if ((int)SplitSentences[i][j].value[0] >= 65 && (int)SplitSentences[i][j].value[0] <= 90 && !IsAllCaps(SplitSentences[i][j].value))
                            {
                                temp.wordType = "N";
                            }
                        }
                        SplitSentences[i][j] = temp;
                    }
                }
            }
            int count1 = 0;
            int count2 = 0;

            /*
            for (int i = 0;i < SplitSentences.Count;i++)
            {
                for (int j = 0; j < SplitSentences[i].Count; j++)
                {
                    count1 = 0;
                    count2 = 0;
                    while (SplitSentences[i][j].wordType == null && count1 < SplitSentences.Count)
                    {
                        
                        if (SplitSentences[count1][count2].wordType != null && SplitSentences[count1][count2].value == SplitSentences[i][j].value)
                        {
                            temp = SplitSentences[i][j];
                            temp.wordType = SplitSentences[count1][count2].wordType;
                            temp.Certinity = 2;
                            SplitSentences[i][j] = temp;
                        }
                        count2++;
                        if (count2 >= SplitSentences[count1].Count)
                        {
                            count1++;
                            count2 = 0;
                        }
                    }
                }
            }
            */
            temp = new word();
            for (int i = 0; i < SplitSentences.Count; i++)
            {
                for (int j = 0; j < SplitSentences[i].Count; j++)
                {
                    temp = SplitSentences[i][j];
                    temp.value = temp.value.ToLower();
                    SplitSentences[i][j] = temp;
                }
            }

            return SplitSentences;
        }
    }
}
