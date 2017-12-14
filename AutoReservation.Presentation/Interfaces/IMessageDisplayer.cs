using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoReservation.Presentation.Interfaces
{
    public interface IMessageDisplayer
    {
        void DisplayWarning(string title, string message);
        bool DisplayDialog(string title, string message);
    }
}
