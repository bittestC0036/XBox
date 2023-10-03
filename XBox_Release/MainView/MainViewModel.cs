using Caliburn.Micro;
using System.ComponentModel.Composition;

namespace XBox
{
    [Export(typeof(IMainViewModel))]
    public sealed class MainViewModel : IMainViewModel
    {
        //private readonly IWindowServices _windowServices;
        [ImportingConstructor]
        //public MainViewModel(IWindowServices windowServices)
        public MainViewModel()
        {
            //_windowServices = windowServices;
        }

        public void ShowMessage(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }
    }
}
