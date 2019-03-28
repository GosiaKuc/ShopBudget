using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace ShopBudget
{
    public class EntryTemplateSelector : DataTemplateSelector // klasa do wybierania prawidlowego szablonu danych
    {
        public override DataTemplate  SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            Post chosen = item as Post;
            Window win = Application.Current.MainWindow;

            if (chosen is Expense)
                return (DataTemplate)win.FindResource("ExpenseDataTemplate");
            else if (chosen is Income)
                return (DataTemplate)win.FindResource("IncomeDataTemplate");

            return null;
        }
    }
}
