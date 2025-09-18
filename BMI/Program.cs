using System;
namespace BMI
{
    class Program
    {
        static void Main()
        {
            BMIcalculator calculator = new BMIcalculator();

            Console.Write("輸入身高");
            double height = double.Parse(Console.ReadLine());
            calculator.Height = height;
            Console.Write("輸入體重");
            double weight = double.Parse(Console.ReadLine());
            calculator.Weight = weight;

            double bmi = calculator.CalculateBMI();
            string bmiDescription = calculator.GetBMIDescription();
            Console.WriteLine($"你的BMI為: {bmi}，體重: {bmiDescription} 標準中");
            Console.ReadLine();
        }
    }

    //類別
    class BMIcalculator()
    {
        //屬性
        private double height;
        private double weight;

        public double Height
        {
            get
            {
                return height;
            }
            set
            {
                if (value > 0) height = value;
                else throw new ArgumentException("身高必須大於零");
            }
        }
        public double Weight
        {
            get
            {
                return weight;
            }
            set
            {
                if (value > 0) weight = value;
                else throw new ArgumentException("體重必須大於零");
            }
        }
        //方法
        public double CalculateBMI()
        {
            double bmi = Weight / ((Height / 100) * (Height / 100));
            return bmi;
        }

        public string GetBMIDescription()
        {
            double bmi = CalculateBMI();
            if (bmi < 18.5) return "過瘦";
            else if (bmi >= 18.5 && bmi < 24) return "正常";
            else return "過胖";
        }
    }
}
