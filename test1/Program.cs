/*
writeline相當於c++的cout
readline相當於c++的cin

/t TAB鍵

 */
using System;
namespace test1 {
    class Program {
        static void Main(string[] args) {
            int a, b, c;
            Console.WriteLine("輸入整數值a:");
            a =int.Parse(Console.ReadLine()); //readline回傳是str，需要轉換型別
            Console.WriteLine("輸入整數值b:");
            b =int.Parse(Console.ReadLine());
            Console.WriteLine("輸入整數值c:");
            c =int.Parse(Console.ReadLine());

            bool ans1 = (a + b) > (b + c);
            bool ans2 = (a - b) > (b - c);
            bool ans3 = b == 25;

            //輸出

            Console.WriteLine($"a + b > b + c, 回傳{ans1}");
            Console.WriteLine($"a - b > b - c, 回傳{ans2}");
            Console.WriteLine($"b == 25, 回傳{ans3}");
            
            //doller sign 使其可以在敘述式中回傳變數

            Console.ReadKey();//按任意鍵離開
        }
    }

}
