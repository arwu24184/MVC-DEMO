
using System;
namespace Rnumber;
class Rprogame
{

    public static void Main(string[] args)
    {
        Random random = new();
        int secretNumber = random.Next(1, 100);//設定亂數範圍
        int attempts = 0;
        bool correctGuess = false;

        Console.WriteLine("猜字遊戲");

        while (!correctGuess)
        {
            Console.WriteLine("輸入1-100整數");
            string input = Console.ReadLine();
            /*while (!int.TryParse(input, out int guess)) {
                Console.WriteLine("請輸入有效值");
            }*/
            if (!int.TryParse(input, out int guess))
            {
                Console.WriteLine("請輸入有效值");
                continue;
            }

            attempts++;

            if (guess < secretNumber)
            {
                Console.WriteLine("值過於小");
            }
            else if (guess > secretNumber)
            {
                Console.WriteLine("值過於大");
            }
            else
            {
                Console.WriteLine($"猜對了,答案是{secretNumber},共猜了{attempts}次");
                correctGuess = true;
            }
        }
        Console.ReadLine();
    }
}
