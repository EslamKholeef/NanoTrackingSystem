using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Identity
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Employee = "Employee";
        public const string Finance = "Finance";
        public const string HR = "HR";

        public static readonly string[] AllRoles =
        {
            Admin, Manager, Employee, Finance, HR
        };
    }
}
