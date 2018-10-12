using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleGlobantChallenges
{
   class Program
   {
      static void Main(string[] args)
      {
         //PrintNumbersWithoutBasicStructures(1, 100);

         //string word = "reconocer";
         //Console.WriteLine($"The word '{word}' is Palindrome: {IsPalindrome(word)}");

         //GetCountByCharacters("01112233344555");

         string word = "This is a phrase [we want to validate (2018) to know (if this is valid (or not))] with the following method.(())";
         Console.WriteLine($"The word '{word}' is well formatted: {IsValidParenthesesFormat(word)}");         
      }

      /// <summary>
      ///  Print numbers from 1 to 100 without use the control structures like For, ForEach, Do-While, While
      /// </summary>
      /// <param name="from">Initial number</param>
      /// <param name="to">Final number</param>
      private static void PrintNumbersWithoutBasicStructures(int from, int to)
      {
         //My solution was:
         //if (to == 1) {
         //   Console.WriteLine($"Number {from}.");
         //}
         //else {
         //   Console.WriteLine($"Number {from}.");
         //   PrintNumbersWithoutBasicStructures(++from, to - 1);
         //}

         //Refactorization
         Console.WriteLine($"Number {from}.");

         if (to > 1)
         {
            PrintNumbersWithoutBasicStructures(++from, to - 1);
         }
      }

      /// <summary>
      /// Check if a word is palindrome or not.
      /// </summary>
      /// <param name="word">Word to check</param>
      /// <returns>True o False</returns>
      private static bool IsPalindrome(string word)
      {
         //My solution was:
         //StringBuilder newWord = new StringBuilder();

         //for(int i = word.Length - 1; i >= 0; i--)
         //{
         //   newWord.Append(word[i].ToString());
         //}         

         //return word.Equals(newWord.ToString());

         char[] letters = word.ToCharArray();
         Array.Reverse(letters);
         return word.Equals(new string(letters));
      }

      public class CountCharacter {
         public string Character { get; set; }
         public int Count { get; set; }

         public CountCharacter(string character, int count)
         {
            this.Character = character;
            this.Count = count;
         }
      }

      /// <summary>
      /// How many times each letter into WORD is repeated.
      /// </summary>
      /// <param name="word">Word to validate</param>
      private static void GetCountByCharacters(string word)
      {
         List<CountCharacter> list = new List<CountCharacter>();
         //My solution was:
         //CountCharacter character;

         //for (int i = 0; i < word.Length; i++)
         //{
         //   if (list.Any(x => x.Character == word[i].ToString()))
         //   {
         //      character = list.First(x => x.Character == word[i].ToString());
         //      character.Count += 1;
         //   }
         //   else {
         //      list.Add(new CountCharacter(word[i].ToString(), 1));
         //   }
         //}
                  
         list = word.ToCharArray().GroupBy(x => x.ToString(), (key, g) => new CountCharacter(key, g.Count())).ToList();

         list.ForEach(x =>
         {
            Console.WriteLine($"The letter {x.Character} repetas {x.Count} times");
         });
      }
      
      class Character {
         public char Open { get; set; }
         public char Close { get; set; }

         public Character(char open, char close)
         {
            this.Open = open;
            this.Close = close;
         }
      }

      /// <summary>
      /// Validate if a word is well formatted based on Brackets or Parentheses.
      /// </summary>
      /// <param name="word">Word to validate</param>
      /// <returns>true: it's valid, false: it isn't valid</returns>
      private static bool IsValidParenthesesFormat(string word)
      {         
         //char: Characther [()]
         //bool: true: It's a open character.
         List<Character> charactersList = new List<Character>() {
            new Character('[', ']'),
            new Character('(', ')'),
         };

         Stack<char> stack = new Stack<char>();

         foreach (char character in word.ToList())
         {
            if (charactersList.Any(c => c.Open.Equals(character)))
            {
               stack.Push(character);
            }
            else if (charactersList.Any(c => c.Close.Equals(character)))
            {
               if (stack.Count > 0)
               {
                  if (stack.Peek().Equals(charactersList.First(c => c.Close.Equals(character)).Open))
                  {
                     stack.Pop();
                  }
                  else break;
               }
               else return false;               
            }
         }

         return stack.Count == 0;
      }
   }
}