using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ShopBudget
{
    // klasa formatująca datę, potrzeba do poszczególnych wpisów
    [Serializable]
    public class Date
    {
        public Date()
        {
            Day = 1;
            Month = "Styczeń";
            Year = 1980;
        }

        public Date(byte day, string month, int year)
        {
            Day = day;
            Month = month;
            Year = year;
        }

        public override bool Equals(object obj) { return base.Equals(obj); }
        public override int GetHashCode() { return base.GetHashCode(); }
        public override string ToString() { return string.Format("{0} {1} {2}", Day.ToString(), Month, Year.ToString()); }

        public static bool operator ==(Date a, Date b) { return a.Day == b.Day && a.Month == b.Month && a.Year == b.Year; }
        public static bool operator !=(Date a, Date b) { return !(a == b); }

        public static bool operator >(Date a, Date b)
        {
            if (a.Year > b.Year)
                return true;
            else if (a.Year == b.Year)
            {
                int j, k;
                j = k = 0;

                for (int i = 0; i < 12; i++)
                {
                    if (MainWindow.months[i] == a.Month)
                        j = i;
                    else if (MainWindow.months[i] == b.Month)
                        k = i;
                }

                if (j > k)
                    return true;
                else if (j == k)
                {
                    if (a.Day > b.Day)
                        return true;
                }
            }
            return false;
        }

        public static bool operator <(Date a, Date b) { return !(a > b); }

        public static bool operator >=(Date a, Date b)
        {
            if (a.Year >= b.Year)
            {
                int j, k;
                j = k = 0;

                for (int i = 0; i < 12; i++)
                {
                    if (MainWindow.months[i] == a.Month)
                        j = i;
                    else if (MainWindow.months[i] == b.Month)
                        k = i;
                }

                if (j >= k)
                    if (a.Day >= b.Day)
                        return true;
            }
            return false;
        }

        public static bool operator <=(Date a, Date b) { return !(a >= b); }

        public byte Day { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
    }

    // klasa abstrakcyjna dla wszystkich rodzajów wpisów
    // do wzorca dekorator -> obiekt dekorowany
    // do wzorca Command -> wykowawca polecenia
    [Serializable]
    public abstract class Post
    {
        protected Post() { }

        protected string name;  // nazwa wpisu
        protected string category; // kategoria wpisu
        protected Date date;  // data wpisu
        protected decimal amount;   // kwota wpisu
        protected string desc;  // opis wpisu

        public abstract string Name { get; set; }
        public abstract string Category { get; set; }
        public abstract Date PostDate { get; set; }
        public abstract decimal Amount { get; set; }
        public abstract string Description { get; set; }

        // czy wpis jest zaszyfrowany
        // test -> atrybut wpisu, którego zaszyfrowanie będzie sprawdzane
        // zwraca true, jeśli zaszyfrowany, w innym przypadku false
        public abstract bool IsEncrypted(string text);
    }

    // klasa wydatku
    // do wzorca Dekorator -> konkretny obiekt dekorowany
    // do wzorca Obserwator -> konkretny obiekt obserwowany
    [Serializable]
    public class Expense : Post, IDataErrorInfo
    {
        public Expense()
        {
            this.name = string.Empty;
            this.category = string.Empty;
            this.date = new Date();
            this.amount = 0;
            this.desc = string.Empty;
        }

        public Expense(string name, string category, Date date, decimal amount, string desc)
        {
            this.name = name;
            this.category = category;
            this.date = date;
            this.amount = amount;
            this.desc = desc;
        }

        public Expense(Expense e)
        {
            this.name = e.name;
            this.category = e.category;
            this.date = e.date;
            this.amount = e.amount;
            this.desc = e.desc;
        }

        public override string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string Category
        {
            get { return category; }
            set { category = value; }
        }

        public override Date PostDate
        {
            get { return date; }
            set { date = value; }
        }

        public override decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public override string Description
        {
            get { return desc; }
            set { desc = value; }
        }

        public override bool IsEncrypted(string text)
        {
            if(text.Length==0) return false;
            if ((int)text[0] == 19) return true;  // na początku wpisu prywatnego dołożony jest znak 19 w kodzie ASCII
            else return false;
        }

        public override bool Equals(object obj) { return base.Equals(obj); }
        public override int GetHashCode() { return base.GetHashCode(); }

        public static bool operator ==(Expense a, Expense b) { return a.Amount == b.Amount && a.Category == b.Category && a.Description == b.Description && a.PostDate == b.PostDate; }
        public static bool operator !=(Expense a, Expense b) { return !(a == b); }

        public string Error { get { return null; } }
        public string this[string columnName]
        {
            get 
            {
                if (columnName == "Amount")
                {
                    if (amount > 0)
                        return "Podana kwota powinna być mniejsza od 0!";
                }

                else if (columnName == "Name")
                {
                    foreach (char c in name)
                        if ((char.IsLetterOrDigit(c) | c == 32) != true)
                            return "Podana nazwa zawiera niedozwolone znaki (Muszą być tylko litery i cyfry)!";
                }

                return null;
            }
        }
    }

    // klasa przychodu
    // do wzorca Dekorator -> konkretny obiekt dekorowany
    // do wzorca Obserwator -> konkretny obiekt obserwowany
    [Serializable]
    public class Income : Post, IDataErrorInfo
    {
        public Income()
        {
            this.name = string.Empty;
            this.category = string.Empty;
            this.date = new Date();
            this.amount = 0;
            this.desc = string.Empty;
        }

        public Income(string name, string category, Date date, decimal amount, string desc)
        {
            this.name = name;
            this.category = category;
            this.date = date;
            this.amount = amount;
            this.desc = desc;
        }

        public Income(Income i)
        {
            this.name = i.name;
            this.category = i.category;
            this.date = i.date;
            this.amount = i.amount;
            this.desc = i.desc;
        }

        public override string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string Category
        {
            get { return category; }
            set { category = value; }
        }

        public override Date PostDate
        {
            get { return date; }
            set { date = value; }
        }

        public override decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public override string Description
        {
            get { return desc; }
            set { desc = value; }
        }

        public override bool IsEncrypted(string text)
        {
            if (text.Length == 0) return false;
            if ((int)text[0] == 19) return true; // na początku wpisu prywatnego dołożony jest znak 19 w kodzie ASCII
            else return false;
        }

        public override bool Equals(object obj) { return base.Equals(obj); }
        public override int GetHashCode() { return base.GetHashCode(); }

        public static bool operator ==(Income a, Income b) { return a.Amount == b.Amount && a.Category == b.Category && a.Description == b.Description && a.PostDate == b.PostDate; }
        public static bool operator !=(Income a, Income b) { return !(a == b); }

        public string Error { get { return null; } }
        public string this[string columnName]
        {
            get
            {
                if (columnName == "Amount")
                {
                    if (amount < 0)
                        return "Podana kwota powinna być większa od 0!";

                }

                else if (columnName == "Name")
                {
                    foreach (char c in name)
                        if ((char.IsLetterOrDigit(c) | c == 32) != true)
                            return "Podana nazwa zawiera niedozwolone znaki (Muszą być tylko litery i cyfry)!";
                }

                return null;
            }
        }
    }

    // do wzorca Dekorator -> klasa dekorująca wpis
    public class Decorator : Post
    {
        protected Post decorated;

        protected Decorator(Post p) : base()
        {
            decorated = p;
        }
        public override string Name
        {
            get { return decorated.Name; }
            set { decorated.Name = value; }
        }

        public override string Category
        {
            get { return decorated.Category; }
            set { decorated.Category = value; }
        }

        public override Date PostDate
        {
            get { return decorated.PostDate; }
            set { decorated.PostDate = value; }
        }

        public override decimal Amount
        {
            get { return decorated.Amount; }
            set { decorated.Amount = value; }
        }

        public override string Description
        {
            get { return decorated.Description; }
            set { decorated.Description = value; }
        }

        public override bool IsEncrypted(string text)
        {
           return decorated.IsEncrypted(text);
        }
    }

    // do wzorca Dekorator -> klasa konkretnego dekoratora
    // użycie tego dekoratora powoduje zaszyfrowanie lub odszyfrowanie wpisu
    public class DecoratorEncryption : Decorator
    {
        private string key;  // klucz odszyfrowujący     
        private int keyLength;  // długość klucza odszyfrowującego
       
        // ikey - klucz odszyfrowujący wprowadzony przez użytkownika
        public DecoratorEncryption(Post p, int ikey) : base(p)
        {
            this.key = ikey.ToString();
            this.keyLength = key.Length;           
        }

        // funkcja szyfrująca/odszyfrowująca łańcuch
        // wejście łańcuch do zaszyfrowania/odszyfrowania
        // zwraca zaszyfrowany/odszyfrowany łańcuch
        public string Encrypt(string startString)
        {
            Crc32 crc32 = new Crc32();
            int i;
            string endString = "";  // łańcuch zwracany
            string sumCrc32 = "";  // suma kontrolna dla łańcucha wejściowego
            string startStringLocal = "";  // łańcuch wejściowy lokalny
            char character = startString[0];  // pierwszy znak łańcucha wejściowego

            bool encrypted;  // true -> łańcuch wejściowy będzie zaszyfrowany
                             // false -> łańcuch wejściowy będzie odszyfrowany
            i = 0;
            startStringLocal = startString;
            if ((int)character == 19)  // czy na początku wpisu jest znak 19 w ASCII, jeśli tak to trzeba odszyfrować
            {
                sumCrc32 = startString.Substring(1, 8);
                startString = startString.Substring(9);
                encrypted = false;
            }
            else encrypted = true;
            
            // pętla szyfrująca/odszyfrowująca łańcuch
            foreach (char c in startString)
            {
                char d = key[i];
                char w = (char)(c ^ d);
                endString += w;
                i++;
                if (i >= keyLength) i = 0;
            }

            if(encrypted)  // łańcuch był zaszyfrowywany
            {
                i = 19; 
                // do łańcucha wyjściowego dodawany jest znak 19 w ASCII oznaczający, że wpis jest prywatny oraz suma kontrolna łańcucha wejściowego
                endString = (char)i +  crc32.GetCrcSum(startString) + endString;
            }
            else  // łańcuch był odszyfrowywany
            {
                if (crc32.GetCrcSum(endString) != sumCrc32)
                {
                    endString = startStringLocal;  // przywrócenie łańcucha wejściowego
                    throw new Exception("Wprowadzono błędny kod. Nie udało się odszyfrować danych");
                }
            }
            
            return endString;
        }
        public override string Name
        {
            get { return Encrypt(decorated.Name); }
            set { decorated.Name = Encrypt(value); }  
        }
        public override string Description
        {
            get { return Encrypt(decorated.Description); }
            set { decorated.Description = Encrypt(value); }
        }
        public override string Category
        {
            get { return Encrypt(decorated.Category); }
            set { decorated.Category = Encrypt(value); }
        }
    }
}
