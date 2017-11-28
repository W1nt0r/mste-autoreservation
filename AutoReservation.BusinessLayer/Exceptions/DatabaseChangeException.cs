using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoReservation.BusinessLayer.Exceptions
{
    public class DatabaseChangeException : Exception
    {
        public DatabaseChangeException(string message) : base(message) { }
    }
}
