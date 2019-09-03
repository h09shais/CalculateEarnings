using System;
namespace MyApp {
    class Program {
        static void Main (string[] args) {

            ConsoleSavingsCalculate.Run ();
        }
    }

    public class ConsoleSavingsCalculate {

        public static void Run () {
            try {
                System.Console.WriteLine ("What are total expenses?");
                decimal expenses = Convert.ToDecimal (Console.ReadLine ());
                System.Console.WriteLine ("What are total earnings?");
                decimal earnings = Convert.ToDecimal (Console.ReadLine ());
                var savingsCalculate = new SavingsCalculate (expenses, earnings);
                System.Console.WriteLine (SavingsCalculateFormatter.FormatForConsole (savingsCalculate, new Round2Decimals ()));
            } catch (Exception e) {
                System.Console.WriteLine (e.Message);
                Run ();
            }
        }

    }

    public interface IToDecimals {
        string DecimalRound (decimal value);
    }

    public class Round2Decimals : IToDecimals {
        public string DecimalRound (decimal value) => value.ToString ("0.00");
    }

    public class Round3Decimals : IToDecimals {
        public string DecimalRound (decimal value) => value.ToString ("0.000");
    }

    public class SavingsCalculateFormatter {
        public static IToDecimals DecimalFormatter;

        public static string FormatForConsole (SavingsCalculate savingsCalculate, IToDecimals decimalFormatter) {
            DecimalFormatter = decimalFormatter;
            var expenses = "The total expenses amounts to: " + DecimalFormatter.DecimalRound (savingsCalculate.Expenses);
            var earnings = "The total earnings amounts to: " + DecimalFormatter.DecimalRound (savingsCalculate.Earnings);
            var avgExpenses = "The avarage expenses per month amounts to: " + DecimalFormatter.DecimalRound (savingsCalculate.AvgExpenses ());
            var avgEarnings = "The avarage earnings per month amounts to: " + DecimalFormatter.DecimalRound (savingsCalculate.AvgEarnings ());
            var savingsRate = "The savings rate is estimated to: " + DecimalFormatter.DecimalRound (savingsCalculate.SavingsRate ()) + "%";

            return "\n" + expenses +
                "\n" + earnings +
                "\n" + avgExpenses +
                "\n" + avgEarnings +
                "\n" + savingsRate;
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