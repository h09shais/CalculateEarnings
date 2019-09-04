using System;
using static System.Console;
namespace MyApp {
    class Program {
        static void Main (string[] args) {

            ConsoleSavingsCalculate.Run ();
        }
    }

    public class ConsoleSavingsCalculate {

        public static void Run () {

            WriteLine ("Calculate this years avarage expenses and earnings. Check your savings rate");

            void ExampleCalculate () {
                WriteLine ("Example Calculation:");
                decimal expenses = 100_704.1m;
                decimal earnings = 246_585m;
                var savingsCalculate = new SavingsCalculate (expenses, earnings);
                var formatter = new SavingsCalculateFormatter(savingsCalculate, new RoundDecimals_2 ());
                WriteLine(formatter.FormatForConsole());
                WriteLine ("");
            }

            ExampleCalculate();
            
            try {
                
                WriteLine ("What are total expenses?");
                decimal expenses = Convert.ToDecimal (ReadLine ());
                WriteLine ("What are total earnings?");
                decimal earnings = Convert.ToDecimal (ReadLine ());
                var savingsCalculate = new SavingsCalculate (expenses, earnings);
                var formatter = new SavingsCalculateFormatter(savingsCalculate, new RoundDecimals_2 ());
                WriteLine(formatter.FormatForConsole());
            } catch (Exception e) {
                WriteLine (e.Message);
                Run ();
            }
        }

    }

    public interface IToDecimals {
        string DecimalRound (decimal value);
    }

    public class RoundDecimals_2 : IToDecimals {
        public string DecimalRound (decimal value) => value.ToString ("0.00");
    }

    public class RoundDecimals_3 : IToDecimals {
        public string DecimalRound (decimal value) => value.ToString ("0.000");
    }



    public class SavingsCalculateFormatter {
        private static IToDecimals DecimalFormat;

        private string Expenses = "The total expenses amounts to: ";
        private string Earnings = "The total earnings amounts to: ";
        private string AvgExpenses = "The avarage expenses per month amounts to: ";
        private string AvgEarnings = "The avarage earnings per month amounts to: ";
        private string SavingsRate = "The savings rate is estimated to: ";

        public SavingsCalculateFormatter(SavingsCalculate savingsCalculate, IToDecimals decimalFormat)
        {
            DecimalFormat = decimalFormat;

            Expenses += DecimalFormat.DecimalRound (savingsCalculate.Expenses);
            Earnings += DecimalFormat.DecimalRound (savingsCalculate.Earnings);
            AvgExpenses += DecimalFormat.DecimalRound (savingsCalculate.AvgExpenses ());
            AvgEarnings += DecimalFormat.DecimalRound (savingsCalculate.AvgEarnings ());
            SavingsRate +=  DecimalFormat.DecimalRound (savingsCalculate.SavingsRate ());
        }

        public string FormatForConsole () {

            return "\n" + Expenses +
                "\n" + Earnings +
                "\n" + AvgExpenses +
                "\n" + AvgEarnings +
                "\n" + SavingsRate + "%";
        }

        public string FormatForHTML () {
   
            return "<p>" + Expenses + "</p>" +
                "<p>" + Earnings + "</p>" + 
                "<p>" + AvgExpenses + "</p>" + 
                "<p>" + AvgEarnings + "</p>" + 
                "<p>" + SavingsRate + "%" + "</p>";
        }
    }

     public abstract class SavingsCalculateErrors {
        public SavingsCalculateErrors (decimal expenses, decimal earnings) {
            if (expenses < 0 || earnings < 0) throw new Exception ("Expenses and Earnings can not be negative!");
        }
    }

    public class SavingsCalculate : SavingsCalculateErrors {
        public SavingsCalculate (decimal expenses, decimal earnings) : base (expenses, earnings) {
            Expenses = expenses;
            Earnings = earnings;
        }
        private decimal MonthsPassed () => (decimal)(DateTime.Now.DayOfYear / (365.2422 / 12));
        public decimal Expenses { get; }
        public decimal Earnings { get; }
        public decimal AvgExpenses () => AvgPrMonth (Expenses);
        public decimal AvgEarnings () => AvgPrMonth (Earnings);

        public decimal SavingsRate () {
            if (Earnings == 0) return 0;
            decimal diff = Earnings - Expenses;
            return (diff / Earnings) * 100;
        }
        private decimal AvgPrMonth (decimal number) => number / MonthsPassed ();
    }
}