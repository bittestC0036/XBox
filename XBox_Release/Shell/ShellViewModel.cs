using Prism.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XBox
{
    [Export(typeof(IShell))]
    public sealed class ShellViewModel : ObservableObject, IShell
    {
        private readonly IMainViewModel _mainViewModel;
        public IMainViewModel MainViewModel
        {
            get { return _mainViewModel; }
        }
        public double _dHeight = 0;
        public double dHeight
        { 
            get
            {
                return _dHeight;
            }
            set
            {
                _dHeight = value;
                SetProperty(ref _dHeight, value);

            }
        }

        public double _dWidth = 0;
        public double dWidth
        {
            get
            {
                return _dHeight;
            }
            set
            {
                _dWidth = value;
                SetProperty(ref _dWidth, value);
            }
        }

        [ImportingConstructor]
        public ShellViewModel(IMainViewModel mainViewModel)
        //public ShellViewModel()
        {
            dHeight = 600;
            dWidth  = 450;
            _mainViewModel = mainViewModel;
            //mainViewModel.ShowMessage("Hello1234");
        }
        public void Dispose()
        {
            System.Diagnostics.Debug.WriteLine("Hello");
        }


    }
 }
