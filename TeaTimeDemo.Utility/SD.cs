using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TeaTimeDemo.Utility
{
    public static class SD
    {
        public const string Role_Customer = "Customer";
        public const string Role_Employee = "Employee";
        public const string Role_Manager = "Manager";
        public const string Role_Admin = "Admin";

        //訂單狀態
        //pending->processing->ready->completed *cancel
        public const string StatusPending = "Pending";

        public const string StatusApproved = "Approved";
        public const string StatusProcess = "Processing";

        public const string StatusReady = "Ready";
        public const string StatusCanclled = "Canclled";
        public const string StatusCompleted = "Completed";

    }
}
