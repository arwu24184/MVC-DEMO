using System;
namespace mutipler
{
    class Program
    {
        public static void Main(String[] args){
            for (int i = 1; i <= 9; i += 3){ //區塊
                for (int j = 1; j <= 9; j++){ //row
                    for (int k = 0; k < 3; k++){ //collum
                        int row = i + k;
                        int product = row * j;
                        Console.Write($"{row} * {j} = {product}\t");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }
}
