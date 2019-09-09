using System;
using static System.Console;
namespace MyApp {
    class Program {
        static void Main (string[] args) {

            ConsoleSavingsCalculate.Run ();
        }
    }

    public class ConsoleSavingsCalculate {

        static ConsoleSavingsCalculate() {

            WriteLine ("Calculate this years avarage expenses and earnings. Check your savings rate");
            WriteLine ("Example Calculation:");
            decimal expenses = 100_704.1m;
            decimal earnings = 246_585m;
            var savingsCalculate = new SavingsCalculate (expenses, earnings);
            var formatted = SavingsCalculateFormatter.Format(savingsCalculate, new RoundDecimals_2 (), Formatters.FormatForConsole);
            WriteLine(formatted);
            WriteLine ("");
        }

        public static void Run () {
            
            try {   
                WriteLine ("What are total expenses?");
                decimal expenses = Convert.ToDecimal (ReadLine ());
                WriteLine ("What are total earnings?");
                decimal earnings = Convert.ToDecimal (ReadLine ());
                var savingsCalculate = new SavingsCalculate (expenses, earnings);
                var formatted = SavingsCalculateFormatter.Format(savingsCalculate, new RoundDecimals_2 (), Formatters.FormatForConsole);
                WriteLine(formatted);
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

    public class SavingsCalculatePresenter 
    {
        public string Expenses = "The total expenses amounts to: ";
        public string Earnings = "The total earnings amounts to: ";
        public string AvgExpenses = "The avarage expenses per month amounts to: ";
        public string AvgEarnings = "The avarage earnings per month amounts to: ";
        public string SavingsRate = "The savings rate is estimated to: ";

    }

    public static class Formatters {
          public static string FormatForConsole (SavingsCalculatePresenter presenter) {

            return "\n" + presenter.Expenses +
                "\n" + presenter.Earnings +
                "\n" + presenter.AvgExpenses +
                "\n" + presenter.AvgEarnings +
                "\n" + presenter.SavingsRate + "%";
        }

        public static string FormatForHTML (SavingsCalculatePresenter presenter) {

            return "<p>" + presenter.Expenses + "</p>" +
                "<p>" + presenter.Earnings + "</p>" + 
                "<p>" + presenter.AvgExpenses + "</p>" + 
                "<p>" + presenter.AvgEarnings + "</p>" + 
                "<p>" + presenter.SavingsRate + "%" + "</p>";
        }
    }

    

    public class SavingsCalculateFormatter {

        public static string Format(SavingsCalculate savingsCalculate, IToDecimals decimalFormat, Func<SavingsCalculatePresenter, string> formatter)
        {
            var presenter = new SavingsCalculatePresenter();

            presenter.Expenses += decimalFormat.DecimalRound (savingsCalculate.Expenses);
            presenter.Earnings += decimalFormat.DecimalRound (savingsCalculate.Earnings);
            presenter.AvgExpenses += decimalFormat.DecimalRound (savingsCalculate.AvgExpenses ());
            presenter.AvgEarnings += decimalFormat.DecimalRound (savingsCalculate.AvgEarnings ());
            presenter.SavingsRate +=  decimalFormat.DecimalRound (savingsCalculate.SavingsRate ());

            return formatter(presenter);
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