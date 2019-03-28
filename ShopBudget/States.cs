using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ShopBudget
{
    public interface IState // stan
    {
        bool AuthorizeUser(string login, string password, Authorization parent);
    }

    #region User States
    // konkretny stan
    // główny stan autoryzacji użytkownika, decyduje o przejściu w kolejne stany
    public class IdleState : IState
    {
        // walidacja wprowadzonych danych logowania
        // zwraca true, jeśli wprowadzono poprawne dane
        // w innym przypadku false
        public bool AuthorizeUser(string login, string password, Authorization parent)
        {
            if ((login == "gosia" && password == "1234" && Authorization.LoginAmount > 0) 
                || (login == "mati" && password == "4321" && Authorization.LoginAmount > 0) 
                || (login == "aga" && password == "9876" && Authorization.LoginAmount > 0))
                parent.ChangeState(UserLoggedState.Instance);  // przejście w stan zalogowanego użytkownika
            else
            {
                if (Authorization.LoginAmount > 0)
                    parent.ChangeState(FailState.Instance);  // przejście w stan nieudanego logowania
                else
                    parent.ChangeState(BlockedState.Instance);  // przejście w stan zablokowanego użytkownika
            }
            return parent.AuthorizeUser(login, password);
        }

        public static IdleState Instance
        {
            get
            {
                if (instance == null)
                    instance = new IdleState();
                return instance;
            }
        }

        private IdleState() { }
        private static IdleState instance;
    }

    // konkretny stan
    // stan zalogowanego użytkownika
    public class UserLoggedState : IState
    {
        // walidacja wprowadzonych danych logowania
        // zwraca true, jeśli wprowadzono poprawne dane - udało się zalogować
        // w innym przypadku false
        public bool AuthorizeUser(string login, string password, Authorization parent)
        {
            MessageBox.Show("Zalogowałeś się pomyślnie do systemu!", "Komunikat", MessageBoxButton.OK, MessageBoxImage.Information);
            Authorization.LoginAmount = 3;
            return true;
        }

        public static UserLoggedState Instance
        {
            get
            {
                if (instance == null)
                    instance = new UserLoggedState();
                return instance;
            }
        }

        private UserLoggedState() { }
        private static UserLoggedState instance;
    }

    // konkretny stan
    // stan użytkownika po nieudanej próbie logowania (logowanie jest jeszcze możliwe)
    public class FailState : IState
    {
        // walidacja wprowadzonych danych logowania
        // zwraca true, jeśli wprowadzono poprawne dane
        // w innym przypadku false
        public bool AuthorizeUser(string login, string password, Authorization parent)
        {
            Authorization.LoginAmount--;
            MessageBox.Show(string.Format("Niepoprawny login lub hasło! Liczba pozostałych prób: {0}", Authorization.LoginAmount), "Ostrzeżenie", MessageBoxButton.OK, MessageBoxImage.Warning);
            parent.ChangeState(IdleState.Instance);
            return false;
        }

        public static FailState Instance
        {
            get
            {
                if (instance == null)
                    instance = new FailState();
                return instance;
            }
        }

        private FailState() { }
        private static FailState instance;
    }

    // konkretny stan
    // stan zablokowanego użytkownika (próby logowania nie są już możliwe)
    public class BlockedState : IState
    {
        // walidacja wprowadzonych danych logowania
        // zwraca false
        // kolejne próby logowania są niemożliwe
        public bool AuthorizeUser(string login, string password, Authorization parent)
        {
            MessageBox.Show("Możliwość logowania jest zablokowana!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            parent.ChangeState(IdleState.Instance);
            return false;
        }

        public static BlockedState Instance
        {
            get
            {
                if (instance == null)
                    instance = new BlockedState();
                return instance;
            }
        }

        private BlockedState() { }
        private static BlockedState instance;
    }
    #endregion
}
