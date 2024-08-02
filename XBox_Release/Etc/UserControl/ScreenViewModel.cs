using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XBox.Etc.UserControl
{
    public abstract class ScreenViewModel : ObservableErrorInfo, IScreen, IViewAware
    {
        //private static readonly LoggerProxy Log = new LoggerProxy();

        private readonly IWindowServices _windowServices;
        private object _view;

        protected ScreenViewModel(IWindowServices windowServices)
        {
            _windowServices = windowServices;
            _isAvailable = true;
            IsEnabled = true;
        }

        protected IWindowServices WindowServices
        {
            get { return _windowServices; }
        }

        public string DisplayName { get; set; }

        public string DisplayImage { get; set; }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            protected set { SetProperty(ref _isEnabled, value); }
        }

        private bool _isAvailable;
        public bool IsAvailable
        {
            get { return _isAvailable; }
            protected set { SetProperty(ref _isAvailable, value); }
        }

        private bool _supportsShowInWindow;
        public bool SupportsShowInWindow
        {
            get { return _supportsShowInWindow; }
            protected set { SetProperty(ref _supportsShowInWindow, value); }
        }

        private bool _needsAttention;
        /// <summary>
        /// When true, the screen's menu button will flash red. When the screen is activated, this will be
        /// set to false automatically and the flashing will stop.
        /// </summary>
        public bool NeedsAttention
        {
            get { return _needsAttention; }
            set { SetProperty(ref _needsAttention, value); }
        }

        protected virtual bool CacheView
        {
            get { return true; }
        }

        protected object View
        {
            get { return _view; }
        }

        public void ShowInWindow()
        {
            if (SupportsShowInWindow)
            {
                _windowServices.ShowWindow(this);
            }
        }

        public void Activate()
        {
            IsActive = true;
            NeedsAttention = false;
            OnActivate();
            var handler = Activated;
            if (handler != null) handler(this, new ActivationEventArgs());
        }

        protected virtual void OnActivate()
        {
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            private set { SetProperty(ref _isActive, value); }
        }

        public event EventHandler<ActivationEventArgs> Activated;

        public void Deactivate(bool close)
        {
            IsActive = false;
            OnDeactive(close);
            var handler = Deactivated;
            if (handler != null) handler(this, new DeactivationEventArgs { WasClosed = close });
        }

        protected virtual void OnDeactive(bool close)
        {
        }

        public event EventHandler<DeactivationEventArgs> AttemptingDeactivation;

        public event EventHandler<DeactivationEventArgs> Deactivated;

        public void TryClose(bool? closing)
        {
            var handler = AttemptingDeactivation;
            if (handler != null) handler(this, new DeactivationEventArgs());

            Action<bool> closeIfCan = canClose =>
            {
                if (canClose)
                    Deactivate(true);
            };
            CanClose(closeIfCan);
        }

        public virtual void CanClose(Action<bool> callback)
        {
            callback(true);
        }

        void IViewAware.AttachView(object view, object context = null)
        {
            _view = view;
            OnViewAttached(view, context);
            var handler = _viewAttached;
            if (handler != null)
                handler(this, new ViewAttachedEventArgs { Context = context, View = view });
        }

        protected virtual void OnViewAttached(object view, object context)
        {
        }

        object IViewAware.GetView(object context = null)
        {
            if (CacheView)
                return _view;

            return null;
        }

        public Task ActivateAsync(CancellationToken cancellationToken = default)
        {
            return null;
        }

        public Task DeactivateAsync(bool close, CancellationToken cancellationToken = default)
        {
            return null;
        }

        public Task<bool> CanCloseAsync(CancellationToken cancellationToken = default)
        {
            return null;
        }

        public Task TryCloseAsync(bool? dialogResult = null)
        {
            return null;
        }

        private event EventHandler<ViewAttachedEventArgs> _viewAttached;
        event EventHandler<ViewAttachedEventArgs> IViewAware.ViewAttached
        {
            add { this._viewAttached += value; }
            remove { this._viewAttached -= value; }
        }

        event AsyncEventHandler<DeactivationEventArgs> IDeactivate.Deactivated
        {
            add
            {
                //throw new NotImplementedException();
            }

            remove
            {
                //throw new NotImplementedException();
            }
        }
    }
}
