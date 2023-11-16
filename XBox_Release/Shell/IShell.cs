using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBox
{
    public interface IShell : IWindow,IDisposable
    {
        double dHeight { get; set; }

        double dWidth { get; set; }
    }

    public interface IWindow
    {
        double dHeight { get; set; }

        double dWidth { get; set; }
    }
}
