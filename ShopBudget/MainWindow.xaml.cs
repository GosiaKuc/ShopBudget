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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Collections;

namespace ShopBudget
{
    #region Init

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            postListBox.ItemsSource = postList;

            this.Attach(new BalanceLabel(this));
            this.Attach(new ExpensesChart(this));
            this.Attach(new IncomesChart(this));

            View.Filter = (object o) =>
            {
                Post en = o as Post;
                return en.IsEncrypted(en.Name);
            };
        }

        private ListCollectionView View { get { return (ListCollectionView)CollectionViewSource.GetDefaultView(postList); } }

        // metoda logowania lub wylogowywania użytkownika
        private void Login(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn.Tag.ToString() == "icons/login.png")
            {
                LoginDialogWindow ldw = new LoginDialogWindow();
                ldw.Owner = this;

                if (ldw.ShowDialog() == true)
                {
                    if (View.Filter != null)
                        View.Filter = null;
                    loginButton.Tag = "icons/logout.png";
                }
            }

            else if (btn.Tag.ToString() == "icons/logout.png")
            {
                View.Filter = (object o) =>
                {
                    Post en = o as Post;
                    return !en.IsEncrypted(en.Name);
                };
                loginButton.Tag = "icons/login.png";
            }
        }

        public CommandsListAdapter PostList
        {
            get { return postList; }
            set { postList = value; }
        }

        // zmiana wybranego wpisu na liście
        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (postListBox.SelectedItem != null)
            {
                Post temp = (Post)postListBox.SelectedItem;
                if (temp.IsEncrypted(temp.Name))
                {
                    keyTextBox.IsEnabled = true;
                    decipherButton.IsEnabled = true;
                }

                else
                {
                    keyTextBox.IsEnabled = false;
                    decipherButton.IsEnabled = false;
                }
            }
        }

        // funkcja pomocnicza zwracająca index postu na liście postList dla wybranego na listboxie elementu
        private int GetPostIndex()
        {
            int index = 0; 
            Post en = (Post)postListBox.SelectedItem;
            int i = 0;
            foreach (var item in postList)
            {
                if (item == en)
                {
                    index = i;
                    break;
                }
                i++;
            }
            return index;
        }

        // metoda odszyfrowująca zaszyfrowany wpis
        private void Decipher(object sender, RoutedEventArgs e)
        {
            if (keyTextBox.Text != string.Empty)
            {
                int key = 0;
                try
                {
                    key = int.Parse(keyTextBox.Text);
                    int index = GetPostIndex();
                    Post en = (Post)postListBox.SelectedItem;
                    if (en.IsEncrypted(en.Name))
                    {
                        try
                        {
                            Post temp = new DecoratorEncryption(en, key);  // wywołanie dekoratora w celu odszyfrowania wpisu
                            en.Amount = temp.Amount;
                            en.Category = temp.Category;
                            en.Description = temp.Description;
                            en.PostDate = temp.PostDate;
                            en.Name = temp.Name;
                            postList[index] = en;
                            postListBox.Items.Refresh();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Błędny kod", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (FormatException) { MessageBox.Show("Niewłaściwe znaki w kluczu!", "Błąd!", MessageBoxButton.OK, MessageBoxImage.Error); }
            }
        }

        // Filtrowanie wpisow
        private void FilterEntries(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn.Content.ToString() == "Filtruj")
            {
                if (((ComboBoxItem)filteringComboBox.SelectedItem).Content.ToString() == "Dochodach")
                {
                    View.Filter = (object o) =>
                    {
                        Post en = o as Post;
                        return en is Income;
                    };
                }

                else if (((ComboBoxItem)filteringComboBox.SelectedItem).Content.ToString() == "Wydatkach")
                {
                    View.Filter = (object o) =>
                    {
                        Post en = o as Post;
                        return en is Expense;
                    };
                }
            }

            else if (btn.Content.ToString() == "Usuń filtr")
                View.Filter = null;
        }

        // Grupowanie wpisow
        private void GroupPostsByCategory(object sender, RoutedEventArgs e)
        {
            View.GroupDescriptions.Clear();
            View.GroupDescriptions.Add(new PropertyGroupDescription("Category", new GroupByCategory()));
        }

        private void GroupPostsNone(object sender, RoutedEventArgs e)
        {
            View.GroupDescriptions.Clear();
        }

        // Sortowanie wpisow
        private void SortPostsByName(object sender, RoutedEventArgs e)
        {
            View.SortDescriptions.Clear();
            View.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }
        private void SortPostsByDate(object sender, RoutedEventArgs e)
        {
            View.SortDescriptions.Clear();
            View.CustomSort = new SortByDate();
        }
        private void SortPostsNone(object sender, RoutedEventArgs e)
        {
            View.SortDescriptions.Clear();
            View.CustomSort = null;
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void HideArrow(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness();
            }
        }

        public static string[] months = {"Styczeń", "Luty", "Marzec",
                                        "Kwiecień", "Maj", "Czerwiec",
                                        "Lipiec", "Sierpień", "Wrzesień",
                                        "Październik", "Listopad", "Grudzień"};

        private CommandsListAdapter postList = new CommandsListAdapter();  // musi być, inaczej szyfrowanie/odszyfrowywanie źle działa
    }


    #endregion

    #region WPFCommands

    public partial class MainWindow : Window
    {
        //metoda sygnalizowania mozliwosci dodania wpisu
        private void AddEntryCanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = loginButton.Tag.ToString() == "icons/logout.png"; }

        //metoda wybrania opcji dodania wpisu
        private void AddEntryExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (expenseRadioBtn.IsChecked == true)
            {
                ExpenseDialogWindow edw = new ExpenseDialogWindow();
                edw.Owner = this;
                if (edw.ShowDialog() == true)
                {
                    postList.Add(edw.post);
                    Notify(); // zawiadomienie obserwatora
                }
            }

            else if (incomeRadioBtn.IsChecked == true)
            {
                IncomeDialogWindow idw = new IncomeDialogWindow();
                idw.Owner = this;
                if (idw.ShowDialog() == true)
                {
                    postList.Add(idw.post);
                    Notify(); // zawiadomienie obserwatora
                }
            }
        }

        //metoda sygnalizowania mozliwosci edycji wpisu
        private void EditEntryCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            bool isPrivate = false;

            if (postListBox.SelectedItem != null)
            {
                Post en = postListBox.SelectedItem as Post;

                if (en.IsEncrypted(en.Name))
                    isPrivate = true;
            }

            e.CanExecute = postListBox.SelectedItem != null && !isPrivate && loginButton.Tag.ToString() == "icons/logout.png";
        }

        //metoda wybrania opcji edycji wpisu
        private void EditEntryExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            int index = GetPostIndex();
            if (postList[index] is Expense)
            {
                ExpenseDialogWindow edw = new ExpenseDialogWindow();
                edw.Owner = this;
                edw.post = postList[index];

                if (edw.ShowDialog() == true)
                {
                    postList[index] = edw.post;
                    postListBox.Items.Refresh();
                    Notify(); // zawiadomienie obserwatora
                }
            }

            else if (postList[index] is Income)
            {
                IncomeDialogWindow idw = new IncomeDialogWindow();
                idw.Owner = this;
                idw.post = postList[index];

                if (idw.ShowDialog() == true)
                {
                    postList[index] = idw.post;
                    postListBox.Items.Refresh();
                    Notify(); // zawiadomienie obserwatora
                }
            }
        }

        //metoda sygnalizowania mozliwosci usuniecia wpisu
        private void RemoveEntryCanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = postListBox.SelectedItem != null && loginButton.Tag.ToString() == "icons/logout.png"; }

        //metoda wybrania opcji usuniecia wpisu
        private void RemoveEntryExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            int index = GetPostIndex();
            PostList.RemoveAt(index);
            Notify(); // zawiadomienie obserwatora
        }
        private void RedoCanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = PostList.IsRedoPossible && loginButton.Tag.ToString() == "icons/logout.png"; }
        private void RedoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            PostList.Redo();
            Notify();  // zawiadomienie obserwatora
        }
        private void UndoCanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = PostList.IsUndoPossible && loginButton.Tag.ToString() == "icons/logout.png"; }
        private void UndoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            PostList.Undo();
            Notify();  // zawiadomienie obserwatora
        }
        private void ShowChartsCanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = loginButton.Tag.ToString() == "icons/logout.png"; }
        private void ShowChartsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ChartWindow cw = ChartWindow.Instance;
            cw.Visibility = Visibility.Visible;
        }

        private class SortByDate : IComparer
        {
            public int Compare(object x, object y)
            {
                Post a = x as Post;
                Post b = y as Post;

                if (a.PostDate > b.PostDate)
                    return 1;
                else if (a.PostDate == b.PostDate)
                    return 0;
                else
                    return -1;
            }
        }
    }

    #endregion WPFCommands

    #region Observation

    public partial class MainWindow : Window
    {
        public void Attach(IObserver o) { observers.Add(o); }  // rejestracja obserwatora
        public void Detach(IObserver o) { observers.Remove(o); }   // wyrejestrowanie obserwatora

        public void Notify() // inicjacja procesu synchronizacji u obserwatorow
        {
            foreach (IObserver o in observers)
                o.Update();     //wywolanie Update
        }

        private List<IObserver> observers = new List<IObserver>(); // lista obserwatorow
    }

    #endregion
}
