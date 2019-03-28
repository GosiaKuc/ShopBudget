using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace ShopBudget
{
    /// <summary>
    /// Interaction logic for ChartWindow.xaml
    /// </summary>
    public sealed partial class ChartWindow : Window
    {
        private DoubleAnimation anim = new DoubleAnimation();
        private LinearGradientBrush[] expenseRectBrushes;
        private LinearGradientBrush[] incomeRectBrushes;
        private Rectangle[] expensesRect;
        private Rectangle[] incomesRect;
        private static volatile ChartWindow instance;  // do wzorca Singleton, instancja okna
        private static readonly object mutex = new object();  // do wzorca Singleton, zapewnienie synchronizacji obiektów

        // rysowanie wykresu słupkowego wydatków
        // categories -> tablica, której kolejne rekordy zawierają liczbę wpisów dodanych w danej kategorii
        public void DrawExpensesChart(int[] categories)
        {
            foreach (Rectangle er in expensesRect)
                expensesChartCanvas.Children.Remove(er);

            for (int i = 0; i < 4; i++)
            {
                anim.From = 0.0;
                anim.To = (double)categories[i] * 10.0;
                anim.AccelerationRatio = 0.1;
                anim.DecelerationRatio = 0.9;
                anim.Duration = TimeSpan.FromSeconds(1.8);
                expensesRect[i].BeginAnimation(Rectangle.HeightProperty, anim);
                expensesRect[i].ToolTip = "Liczba wydatków: " + categories[i].ToString();
                expensesRect[i].Height = (double)categories[i] * 10.0;
                expensesChartCanvas.Children.Add(expensesRect[i]);
            }
        }

        // rysowanie wykresu słupkowego wydatków
        // categories -> tablica, której kolejne rekordy zawierają liczbę wpisów dodanych w danej kategorii
        public void DrawIncomesChart(int[] categories)
        {
            foreach (Rectangle er in incomesRect)
                incomesChartCanvas.Children.Remove(er);

            for (int i = 0; i < 4; i++)
            {
                anim.From = 0.0;
                anim.To = (double)categories[i] * 10.0;
                anim.AccelerationRatio = 0.1;
                anim.DecelerationRatio = 0.9;
                anim.Duration = TimeSpan.FromSeconds(1.8);
                incomesRect[i].BeginAnimation(Rectangle.HeightProperty, anim);
                incomesRect[i].ToolTip = "Liczba dochodów: " + categories[i].ToString();
                incomesRect[i].Height = (double)categories[i] * 10.0;
                incomesChartCanvas.Children.Add(incomesRect[i]);
            }
        }

        // ustawienie zawartości wyświetlanej w kontrolce Label wskazującej bilans
        public void SetBalance(decimal balance) { balanceLabel.Content = balance.ToString() + " zł"; }

        public static ChartWindow Instance //Singleton , pozna inicjalizacja - tworzony jedynie jeden obiekt tej klasy
        {
            get
            {
                if (instance == null)
                {
                    lock (mutex)
                    {
                        if (instance == null)
                            instance = new ChartWindow();
                    }
                }
                return instance;
            }
        }

        private ChartWindow()
        {
            InitializeComponent();

            // przygotowanie canvasa do wykresu wydatków
            expenseRectBrushes = new LinearGradientBrush[4] {new LinearGradientBrush(), new LinearGradientBrush(), new LinearGradientBrush(), new LinearGradientBrush() };

            foreach (LinearGradientBrush erb in expenseRectBrushes)
            {
                erb.StartPoint = new Point(0.0, 1.0);
                erb.EndPoint = new Point(0.0, 0.0);
            }

            expensesRect = new Rectangle[4] {new Rectangle(), new Rectangle(), new Rectangle(), new Rectangle() };

            foreach (Rectangle r in expensesRect)
                r.ToolTip = "Liczba wydaktów: 0";

            expenseRectBrushes[0].GradientStops.Add(new GradientStop(Colors.Red, 1.5));
            expenseRectBrushes[0].GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
            expenseRectBrushes[1].GradientStops.Add(new GradientStop(Colors.Green, 1.5));
            expenseRectBrushes[1].GradientStops.Add(new GradientStop(Colors.LimeGreen, 0.0));
            expenseRectBrushes[2].GradientStops.Add(new GradientStop(Colors.Blue, 1.5));
            expenseRectBrushes[2].GradientStops.Add(new GradientStop(Colors.Violet, 0.0));
            expenseRectBrushes[3].GradientStops.Add(new GradientStop(Colors.Orange, 1.5));
            expenseRectBrushes[3].GradientStops.Add(new GradientStop(Colors.LightYellow, 0.0));

            for (int i = 0; i < expensesRect.Length; i++)
            {
                expensesRect[i].Fill = expenseRectBrushes[i];
                expensesRect[i].Width = 30.0;
                Canvas.SetBottom(expensesRect[i], 1.0);
                Canvas.SetLeft(expensesRect[i], ((double)i + 1.0) * 50.0);
            }
            // koniec wykresu wydatków

            // przygotowanie canvasa do wykresu dochodów
            incomeRectBrushes = new LinearGradientBrush[4] { new LinearGradientBrush(), new LinearGradientBrush(), new LinearGradientBrush(), new LinearGradientBrush() };

            foreach (LinearGradientBrush irb in incomeRectBrushes)
            {
                irb.StartPoint = new Point(0.0, 1.0);
                irb.EndPoint = new Point(0.0, 0.0);
            }

            incomesRect = new Rectangle[4] { new Rectangle(), new Rectangle(), new Rectangle(), new Rectangle() };

            foreach (Rectangle r in incomesRect)
                r.ToolTip = "Liczba dochodów: 0";

            incomeRectBrushes[0].GradientStops.Add(new GradientStop(Colors.Red, 1.5));
            incomeRectBrushes[0].GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
            incomeRectBrushes[1].GradientStops.Add(new GradientStop(Colors.Green, 1.5));
            incomeRectBrushes[1].GradientStops.Add(new GradientStop(Colors.LimeGreen, 0.0));
            incomeRectBrushes[2].GradientStops.Add(new GradientStop(Colors.Blue, 1.5));
            incomeRectBrushes[2].GradientStops.Add(new GradientStop(Colors.Violet, 0.0));
            incomeRectBrushes[3].GradientStops.Add(new GradientStop(Colors.Orange, 1.5));
            incomeRectBrushes[3].GradientStops.Add(new GradientStop(Colors.LightYellow, 0.0));

            for (int i = 0; i < incomesRect.Length; i++)
            {
                incomesRect[i].Fill = incomeRectBrushes[i];
                incomesRect[i].Width = 30.0;
                Canvas.SetBottom(incomesRect[i], 1.0);
                Canvas.SetLeft(incomesRect[i], ((double)i + 1.0) * 50.0);
            }
            // koniec wykresu dochodów
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        // schowanie okna w momencie naciśnięcia na guzik zamykający je
        private void HideWindow(object sender, RoutedEventArgs e) { this.Visibility = Visibility.Hidden; } 
    }
}
