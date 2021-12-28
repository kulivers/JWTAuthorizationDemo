using System;

namespace Auth.Api.Models
{
    //2
    public class Account
    {
        public Guid Id { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public Role[] Roles { get; set; }
    }

    //3
    public enum Role
    {
        User, Admin
    }
}