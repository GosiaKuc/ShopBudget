using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace ShopBudget
{
    public interface IObserver
    {
        void Update(); //definicja interfejsu informowania o zmianach
    }

    public class ExpensesChart : IObserver // konkretny obiekt zalezny : ExpansesChart
    {
        public ExpensesChart(MainWindow win)
        {
            categories = new int[4]{0, 0, 0, 0};
            this.win = win; 
        }

        public void Update() // implementacja metody Update
        {
            for (int i = 0; i < categories.Length; i++)
                categories[i] = 0;

            foreach (Post e in win.PostList)
            {
                if (e is Expense)
                {
                    if (e.Category == "Czynsz")
                        categories[0]++;
                    else if (e.Category == "Zakup produktów")
                        categories[1]++;
                    else if (e.Category == "Opłata pracowników")
                        categories[2]++;
                    else if (e.Category == "Inne")
                        categories[3]++;
                }
            }

            ChartWindow.Instance.DrawExpensesChart(categories); // narysowanie wykresu wydatków w oknie ChartWindow
        }

        private int[] categories;
        private MainWindow win;
    }

    public class IncomesChart : IObserver // konkretny obiekt zalezny : IncomesChart
    {
        public IncomesChart(MainWindow win)
        {
            categories = new int[4] { 0, 0, 0, 0 };
            this.win = win; 
        }

        public void Update() // implementacja metody Update
        {
            for (int i = 0; i < categories.Length; i++)
                categories[i] = 0;

            foreach (Post e in win.PostList)
            {
                if (e is Income)
                {
                    if (e.Category == "Sprzedaż odzieży")
                        categories[0]++;
                    else if (e.Category == "Sprzedaż obuwia")
                        categories[1]++;
                    else if (e.Category == "Sprzedaż biżuterii")
                        categories[2]++;
                    else if (e.Category == "Inne")
                        categories[3]++;
                }
            }

            ChartWindow.Instance.DrawIncomesChart(categories); // narysowanie wykresu dochodów w oknie ChartWindow
        }

        private int[] categories;
        private MainWindow win;
    }

    public class BalanceLabel : IObserver  // konkretny obiekt zalezny : Balance Label
    {
        public BalanceLabel(MainWindow win) { this.win = win; }

        public void Update() // implementacja metody Update
        {
            decimal balance = 0;

            foreach (Post e in win.PostList)
                balance += e.Amount;

            ChartWindow.Instance.SetBalance(balance);
        }

        private MainWindow win;
    }
}
