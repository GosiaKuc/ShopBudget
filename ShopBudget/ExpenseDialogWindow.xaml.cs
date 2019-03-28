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

namespace ShopBudget
{
    /// <summary>
    /// Interaction logic for ExpenseDialogWindow.xaml
    /// </summary>
    public partial class ExpenseDialogWindow : Window
    {
        public Post post { get; set; }

        public ExpenseDialogWindow() // uzupelnienia danych okna wydatku
        {
            InitializeComponent();

            ComboBoxItem[] items = new ComboBoxItem[4];

            for (int i = 0; i < items.Length; i++)
                items[i] = new ComboBoxItem();

            items[0].IsSelected = true;
            items[0].Content = "Zakup produktów";
            items[1].Content = "Czynsz";
            items[2].Content = "Opłata pracowników";
            items[3].Content = "Inne";

            foreach (ComboBoxItem cbi in items)
                categoryComboBox.Items.Add(cbi);

            for (int i = 0; i < yearComboBox.Items.Count; i++)
            {
                ComboBoxItem cbi = yearComboBox.Items[i] as ComboBoxItem;

                if (cbi.Content.ToString() == DateTime.Now.Year.ToString())
                {
                    string a = ((ComboBoxItem)yearComboBox.Items[i]).Content.ToString();
                    string b = ((ComboBoxItem)yearComboBox.SelectedItem).Content.ToString();
                    FlipStrings(ref a, ref b);
                    ((ComboBoxItem)yearComboBox.Items[i]).Content = a;
                    ((ComboBoxItem)yearComboBox.SelectedItem).Content = b;
                    break;
                }
            }

            for (int i = 0; i < monthComboBox.Items.Count; i++)
            {
                ComboBoxItem cbi = monthComboBox.Items[i] as ComboBoxItem;

                if (cbi.Content.ToString() == MainWindow.months[DateTime.Now.Month - 1])
                {
                    string a = ((ComboBoxItem)monthComboBox.Items[i]).Content.ToString();
                    string b = ((ComboBoxItem)monthComboBox.SelectedItem).Content.ToString();
                    FlipStrings(ref a, ref b);
                    ((ComboBoxItem)monthComboBox.Items[i]).Content = a;
                    ((ComboBoxItem)monthComboBox.SelectedItem).Content = b;
                    break;
                }
            }

            for (int i = 0; i < dayComboBox.Items.Count; i++)
            {
                ComboBoxItem cbi = dayComboBox.Items[i] as ComboBoxItem;

                if (cbi.Content.ToString() == DateTime.Now.Day.ToString())
                {
                    string a = ((ComboBoxItem)dayComboBox.Items[i]).Content.ToString();
                    string b = ((ComboBoxItem)dayComboBox.SelectedItem).Content.ToString();
                    FlipStrings(ref a, ref b);
                    ((ComboBoxItem)dayComboBox.Items[i]).Content = a;
                    ((ComboBoxItem)dayComboBox.SelectedItem).Content = b;
                    break;
                }
            }
        }

        public void FlipStrings(ref string a, ref string b)
        {
            string c = new string(a.ToCharArray());
            a = new string(b.ToCharArray());
            b = new string(c.ToCharArray());
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if (post != null)
            {
                nameTextBox.Text = post.Name;

                for (int i = 0; i < categoryComboBox.Items.Count; i++)
                {
                    if (((ComboBoxItem)categoryComboBox.Items[i]).Content.ToString() == post.Category)
                    {
                        string a = ((ComboBoxItem)categoryComboBox.Items[i]).Content.ToString();
                        string b = ((ComboBoxItem)categoryComboBox.SelectedItem).Content.ToString();
                        FlipStrings(ref a, ref b);
                        ((ComboBoxItem)categoryComboBox.Items[i]).Content = a;
                        ((ComboBoxItem)categoryComboBox.SelectedItem).Content = b;
                    }
                }

                for (int i = 0; i < dayComboBox.Items.Count; i++)
                {
                    if (((ComboBoxItem)dayComboBox.Items[i]).Content.ToString() == post.PostDate.Day.ToString())
                    {
                        string a = ((ComboBoxItem)dayComboBox.Items[i]).Content.ToString();
                        string b = ((ComboBoxItem)dayComboBox.SelectedItem).Content.ToString();
                        FlipStrings(ref a, ref b);
                        ((ComboBoxItem)dayComboBox.Items[i]).Content = a;
                        ((ComboBoxItem)dayComboBox.SelectedItem).Content = b;
                    }
                }

                for (int i = 0; i < monthComboBox.Items.Count; i++)
                {
                    if (((ComboBoxItem)monthComboBox.Items[i]).Content.ToString() == post.PostDate.Month.ToString())
                    {
                        string a = ((ComboBoxItem)monthComboBox.Items[i]).Content.ToString();
                        string b = ((ComboBoxItem)monthComboBox.SelectedItem).Content.ToString();
                        FlipStrings(ref a, ref b);
                        ((ComboBoxItem)monthComboBox.Items[i]).Content = a;
                        ((ComboBoxItem)monthComboBox.SelectedItem).Content = b;
                    }
                }

                for (int i = 0; i < yearComboBox.Items.Count; i++)
                {
                    if (((ComboBoxItem)yearComboBox.Items[i]).Content.ToString() == post.PostDate.Year.ToString())
                    {
                        string a = ((ComboBoxItem)yearComboBox.Items[i]).Content.ToString();
                        string b = ((ComboBoxItem)yearComboBox.SelectedItem).Content.ToString();
                        FlipStrings(ref a, ref b);
                        ((ComboBoxItem)yearComboBox.Items[i]).Content = a;
                        ((ComboBoxItem)yearComboBox.SelectedItem).Content = b;
                    }
                }

                descTextBox.Text = post.Description;
                amountTextBox.Text = post.Amount.ToString();
            }
        }

        private void TextValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                isValid--;
            else
                isValid++;
        }

        //metoda sygnalizowania dostepnosci przycisku OK
        private void AcceptEntryCanExecute(object sender, CanExecuteRoutedEventArgs e) 
        {
            notEmpty = 0;

            if (nameTextBox.Text != string.Empty)
                notEmpty++;
            if (descTextBox.Text != string.Empty)
                notEmpty++;
            if (amountTextBox.Text != string.Empty)
                notEmpty++;

            e.CanExecute = (notEmpty == 3) && (isValid == 2);
        }

        //metoda zatwierdzenia dodania wydatku
        private void AcceptEntryExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            bool? isGood = true;

            byte day = byte.Parse(((ComboBoxItem)dayComboBox.SelectedItem).Content.ToString());
            string month = ((ComboBoxItem)monthComboBox.SelectedItem).Content.ToString();
            int year = int.Parse(((ComboBoxItem)yearComboBox.SelectedItem).Content.ToString());
            decimal amount = 0;

            try { amount = decimal.Parse(amountTextBox.Text); }
            catch (FormatException)
            { 
                 MessageBox.Show("Kwota zawiera niepożądane znaki (Może zawierać tylko liczby dodatnie)!", "Błąd walidacji!", MessageBoxButton.OK, MessageBoxImage.Error);
                   isGood = null;

            }
            if (amount > 0)
            {
                MessageBox.Show("Kwota może zawierać tylko liczby ujemne!", "Błąd walidacji!", MessageBoxButton.OK, MessageBoxImage.Error);
                isGood = null;
            }

            if (post == null)
                post = new Expense();
            post.Name = nameTextBox.Text;
            post.Category = ((ComboBoxItem)categoryComboBox.SelectedItem).Content.ToString();
            post.PostDate = new Date(day, month, year);

            post.Amount = amount;
            post.Description = descTextBox.Text;

            //kiedy wybrano wpis prywatny
            if (protectedEntryCheckBox.IsChecked == true) 
            {
                int key = 0;
                try { key = int.Parse(keyTextBox.Text); }
                catch (FormatException)
                {
                    MessageBox.Show("Kwota zawiera niepożądane znaki (Może zawierać tylko liczby)!", "Błąd walidacji!", MessageBoxButton.OK, MessageBoxImage.Error);
                    isGood = null;
                }

                Post temp = new DecoratorEncryption(post, key); // wywołanie dekoratora w celu zaszyfrowania wpisu
                post.Name = temp.Name;
                post.Amount = temp.Amount;
                post.Category = temp.Category;
                post.Description = temp.Description;
                post.PostDate = temp.PostDate;
            }

            if (isGood != null && isGood == true)
                DialogResult = true;
            else if (isGood != null)
                DialogResult = false;
        }

        private int isValid = 2;
        private int notEmpty = 0;
    }
}
