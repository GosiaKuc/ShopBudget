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
    /// Interaction logic for LoginDialogWindow.xaml
    /// </summary>
    public partial class LoginDialogWindow : Window
    {
        public LoginDialogWindow()
        {
            InitializeComponent();
        }

        // metoda logująca użytkownika lub anulująca jego logowanie (w zależności od klikniętego guzika)
        private void Accept(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn.Content.ToString() == "OK")
                DialogResult = auth.AuthorizeUser(loginTextBox.Text, passTextBox.Password);  // do wzorca state -> wywołanie kontekstu, walidacja wprowadzonych danych

            else if (btn.Content.ToString() == "Anuluj")
                DialogResult = false;
        }

        private Authorization auth = new Authorization();  // do wzorca State -> kontekst
    }
}
