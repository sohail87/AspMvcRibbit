 
using RibbitMvc.Models;
using RibbitMvc.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RibbitMvc.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISecurityService
    {
        //authentication
        bool Authenticate(string username, string password);

        User CreateUser(SignupViewModel signupmodel, bool login = true);

        bool DoesUserExist(string username);

        User GetCurrentUser();

        // if a user has been authenticated. does not need to be a method it can be a property.
        bool IsAuthenticated { get; }
        //login - does not need to return a value
        void Login(User user);
        //login - overload
        void Login(string username);
        //dont need anything for logout
        void Logout();
        int UserId { get; set; }

        
    }
}