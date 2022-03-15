using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PS04._1
{
    class Program
    {

        // Alphabet size
        static readonly int SIZE = 26;

        // trie Node
        public class TrieNode
        {
            public TrieNode[] Child = new TrieNode[SIZE];

            // isLeaf is true if the node represents
            // end of a word
            public Boolean leaf;

            // Constructor
            public TrieNode()
            {
                leaf = false;
                for (int i = 0; i < SIZE; i++)
                    Child[i] = null;
            }
        }

        // If not present, inserts key into trie
        // If the key is prefix of trie node, just
        // marks leaf node
        static void insert(TrieNode root, String Key)
        {
            int n = Key.Length;
            TrieNode pChild = root;

            for (int i = 0; i < n; i++)
            {
                int index = Key[i] - 'a';

                if (pChild.Child[index] == null)
                    pChild.Child[index] = new TrieNode();

                pChild = pChild.Child[index];
            }

            // make last node as leaf node
            pChild.leaf = true;
        }

        // A recursive function to print all possible valid
        // words present in array
        static void searchWord(TrieNode root, Boolean[] Hash,
                                                String str)
        {

            // if we found word in trie / dictionary
            if (root.leaf == true)
            {
                using (StreamWriter output = new StreamWriter("platnaSlova.txt", true))
                {
                    output.WriteLine(str);
                }
            }

            // traverse all child's of current root
            for (int K = 0; K < SIZE; K++)
            {
                if (Hash[K] == true && root.Child[K] != null)
                {
                    // add current character
                    char c = (char)(K + 'a');

                    // Recursively search reaming character
                    // of word in trie
                    searchWord(root.Child[K], Hash, str + c);
                }
            }
        }

        // Prints all words present in dictionary.
        static void PrintAllWords(char[] Arr, TrieNode root,
                                                int n)
        {
            // create a 'has' array that will store all
            // present character in Arr[]
            Boolean[] Hash = new Boolean[SIZE];

            for (int i = 0; i < n; i++)
                Hash[Arr[i] - 'a'] = true;

            // temporary node
            TrieNode pChild = root;

            // string to hold output words
            String str = "";

            // Traverse all matrix elements. There are only
            // 26 character possible in char array
            for (int i = 0; i < SIZE; i++)
            {
                // we start searching for word in dictionary
                // if we found a character which is child
                // of Trie root
                if (Hash[i] == true && pChild.Child[i] != null)
                {
                    str = str + (char)(i + 'a');
                    searchWord(pChild.Child[i], Hash, str);
                    str = "";
                }
            }
        }

        private static readonly Dictionary<string, int> LetterScores = new Dictionary<string, int>
        {
            ["AEIOULNRST"] = 1,
            ["DG"] = 2,
            ["BCMP"] = 3,
            ["FHVWY"] = 4,
            ["K"] = 5,
            ["JX"] = 8,
            ["QZ"] = 10
        };

        public static int Score(string input)
            => input
            .Select(c => LetterScores.First(kvp => kvp.Key.Contains(char.ToUpper(c))).Value)
            .Sum();


        static void Main(string[] args)
        {

            string[] sequence = new string[] 
            {
                "KSYXBUR",
                "MFHDDKO",
                "RWSIORS",
                "AORMYLL",
                "ZIOSBKW",
                "XIUCEVU",
                "YFCMCVR",
                "LBUPBUJ",
                "FCYYNMQ",
                "KWZXWHK",
                "VHGCICD",
                "BXIUTTZ",
                "CAVLLFX",
                "QOFAWXK",
            };

            string[] input = null;
            string inputWord;
            string[] hand = new string[7];

            //string[] splitWords = null;
            //string message = null;

            Console.Write("Zadejte cestu (název) souboru se slovy: ");
            try
            {
                input = File.ReadAllLines(Console.ReadLine() ?? string.Empty);
            }
            catch (Exception e)
            {
                Console.WriteLine("Chyba. {0}", e.Message);
            }
            Console.WriteLine();

            //for (int i = 0; i < 7; i++)
            //{
            //    Random rd = new Random();
            //    int rand_num = rd.Next(0, 26);
            //    hand[i] += letterInfos[rand_num].Character.ToString();
            //}

            Console.Write("Vaše kameny: ");
            Random rd = new Random();
            int rand_num = rd.Next(0, 14);
            string tiles = sequence[rand_num];

            Console.WriteLine(tiles);
            Console.WriteLine();
            Console.WriteLine("Všechna možná slova která lze složit pomocí kamenů byla zapsána do souboru");

            char[] letters = tiles.ToLower().ToCharArray();

            // Root Node of Trie
            TrieNode root = new TrieNode();

            // insert all words of dictionary into trie
            int n = input.Length;
            for (int i = 0; i < n; i++)
                insert(root, input[i]);

            int N = letters.Length;

            using (StreamWriter output = new StreamWriter("platnaSlova.txt"))
            {
                output.WriteLine("");
            }
            PrintAllWords(letters, root, N);

            Console.WriteLine();
            Console.Write("Pro pokračování programu stiskněte libovolné tlačítko...");
            Console.ReadKey();
            Console.WriteLine();

            Console.WriteLine();
            Console.Write("Slovo které chcete složit: ");
            inputWord = Console.ReadLine();
            Console.WriteLine();

            string stringToCheck = inputWord;
            int winCount = 0;
            foreach (string x in input)
            {
                if (stringToCheck.Contains(x) && winCount == 0)
                {
                    Console.WriteLine("Slovo je platné a dostanete za něj {0} bodů!", Score(inputWord));
                    winCount = 1;
                }
            }

            Console.WriteLine();
            Console.Write("Pro pokračování programu stiskněte libovolné tlačítko...");
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine();

            string[] inputValid = File.ReadAllLines("platnaSlova.txt" ?? string.Empty);
            string longest = inputValid.OrderByDescending(s => s.Length).First();
            Console.WriteLine("Nejdelší možné slovo z těch to kamenů je: {0}", longest);

            Console.ReadKey();

        }
    }
}
