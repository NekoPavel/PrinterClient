using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterClient
{
    class Computer
    {
        public Computer(string computerName, string serial, string macAdress)
        {
            this.computerName = computerName;
            this.serial = serial;
            this.macAdress = macAdress.Replace(":", "");
        }
        public string computerName { get; set; }
        public string serial { get; set; }
        public string macAdress { get; set; }
    }
}
