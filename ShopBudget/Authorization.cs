using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ShopBudget
{

    public class Authorization
    {
        public Authorization()
        {
            autorizationState = IdleState.Instance;
        }

        // walidacja wprowadzonych danych logowania
        // zwraca true, jeśli wprowadzono poprawne dane
        // w innym przypadku false
        public bool AuthorizeUser(string login, string password) { return autorizationState.AuthorizeUser(login, password, this); }

        // liczba prób logowania
        public static int LoginAmount
        {
            get { return loginAmount; }
            set { loginAmount = value; }
        }

        // zmiana stanu autoryzacji użytkownika
        public void ChangeState(IState state) { this.autorizationState = state; }

        private IState autorizationState;  // stan autoryzacji użytkownika
        private static int loginAmount = 3;  // liczba prób logowania
    }
}
