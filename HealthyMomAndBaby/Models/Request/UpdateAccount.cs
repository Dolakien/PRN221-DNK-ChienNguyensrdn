﻿using HealthyMomAndBaby.Entity;

namespace HealthyMomAndBaby.Models.Request
{
    public class UpdateAccount
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public string RoleName { get; set; }
    }
}
