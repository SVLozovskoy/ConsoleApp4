using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace ConsoleApp4
{
    class Program
    {
        static void OutputList(List<string[]> list) //Метод для вывода списка массивов 
        {
            foreach (string[] item in list)
                Console.WriteLine("Слово " + item[0] + " встретилось  " + item[1] + " раз");
            Console.WriteLine();
        }
 
        class CountComparer : IComparer<string[]>  //компаратор для сравнения элементов (массивов) в списке
        {
            public int Compare(string[] o1, string[] o2)
            {
                int a = Convert.ToInt32(o1[1]);
                int b = Convert.ToInt32(o2[1]);

                if (a < b)
                {
                    return 1;
                }
                else if (a > b)
                {
                    return -1;
                }

                return 0;
            }
        }
                static void Main(string[] args)
                {
            try //вводится для того, чтобы обработать исключение 
            {
       //комент для теста слова abstract

                string textFromFile = File.ReadAllText(args[0]); // читаем файл в строку. файл передается как параметр
                //Паттерн для регулярного выражения исключающего коменты из текста
                string pattern = @"(?x) //
                ( ""
                (?> (?<=@.) (?>[^""]+|"""")*  
                | (?> [^""\\]+ | \\. )* 
                  ) 
                  ""
                | ' (?> [^'\\]+ | \\. )* '
                )
                | // .* 
                | /\* (?s) .*? \*/ ";

                textFromFile = Regex.Replace(textFromFile, pattern, "$1");
                string[] listOfWords = textFromFile.Split(' ', ',', '.', '<', '>', '(', ')', '[', ']'); // делим в строке слова по разделителю split  и пишем их в массив allwords
                int wordsCount = 0; //метрика кол-ва вхождений слова в массив
                string[] keyWords = {"while" , "break", "char", "as", "byte", "checked", "bool", "const", "catch",
                "base", "case", "class", "continue", "decimal", "default", "delegate",
                "do", "double", "else", "enum", "event","explicit" ,"extern", "false","finally", "fixed", "float", "for",
                "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal","is", "lock", "long", "namespace",
                "new", "null", "object", "operator","out", "override","params","private","protected", "public", "readonly", "ref",
                "return",   "sbyte", "sealed", "short","sizeof","stackalloc","static", "string","struct", "switch", "this", "throw",
                "true","try","typeof","uint","ulong","unchecked","unsafe", "ushort","using","virtual", "void","volatile","abstract" }; //ключевые слова из доки ms
                Array.Sort(keyWords); //Сортируем массив words по алфавиту по возрастанию
                string wordsCount_string; // переменная для хранения кол-ва в виде строки
                List<string[]> resultList = new List<string[]>(); // коллекция для хранения результата программы
                int m = keyWords.Length; //переменная метрики длинны массива ключевых слов. 
                for (int i = 0; i < m - 1; i++)
                {
                    string word = keyWords[i];//на каждой итерации получаем новое слово для анализа из массива words
                    foreach (string s in listOfWords)
                    {
                        if (s == word) wordsCount++;//проверяем совпадает ли текущее слово с словами из массива words

                    }
                    wordsCount_string = wordsCount.ToString();//приводим int к string
                    resultList.Add(new string[] { word, wordsCount_string });//записываем слово и кол-во
                    wordsCount = 0;//сбрасываем метрику 
                }
                OutputList(resultList); //выводим список, через созданный ранее метод
                CountComparer CountWord = new CountComparer(); //компаратор
                resultList.Sort(CountWord);// сортировка своим компаратором
                Console.WriteLine("Топ 5 слов: ");
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine("{0}", resultList[i]);//вывод пяти самых частых слов
                }
                Console.WriteLine();               
            }
            catch //Обработка исключения если файл не задан 
            {
                Console.WriteLine("Извините, вы не указали файл. При запуске программы укажите KeyWords.exe Filename (Где Filename имя целевого файла)");

            }
        }
    }
}
