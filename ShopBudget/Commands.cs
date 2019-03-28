using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ShopBudget
{
    class WPFCommands // klasa polecen dla wyroznionych akcji
    {
        static WPFCommands()
        {
            AcceptEntry = new RoutedUICommand("Odblokuj przycisk", "AcceptEntry", typeof(WPFCommands));
            AddEntry = new RoutedUICommand("Dodaj wpis", "AddEntry", typeof(WPFCommands));
            EditEntry = new RoutedUICommand("Edytuj wpis", "EditEntry", typeof(WPFCommands));
            Redo = new RoutedUICommand("Powtórz operację", "Redo", typeof(WPFCommands));
            RemoveEntry = new RoutedUICommand("Usuń wpis", "RemoveEntry", typeof(WPFCommands));
            ShowCharts = new RoutedUICommand("Wyświetl wykresy", "ShowCharts", typeof(WPFCommands));
            Undo = new RoutedUICommand("Cofnij operację", "Undo", typeof(WPFCommands));
        }

        public static RoutedUICommand AcceptEntry { get; private set; }
        public static RoutedUICommand AddEntry { get; private set; }
        public static RoutedUICommand EditEntry { get; private set; }
        public static RoutedUICommand Redo { get; private set; }
        public static RoutedUICommand RemoveEntry { get; private set; }
        public static RoutedUICommand ShowCharts { get; private set; }
        public static RoutedUICommand Undo { get; private set; }
    }
}
