using ImageMeasurement.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ImageMeasurement
{
    public interface IAppContext
    {
        User? CurrentUser { get; set; }
        Image? CurrentImage { get; set; }
    }

    public class AppContext : IAppContext, IDisposable
    {
        private static IAppContext? _appContext;
        public static IAppContext Instance { 
            get {
                if (_appContext == null)
                    _appContext = new AppContext();

                return _appContext;
            } 
            set { _appContext = value; }
        }

        private AppContext()
        {
        }

        public User? CurrentUser { get; set;}
        public Image? CurrentImage { get; set;}

        #region IDisposable Members

        internal bool _isDisposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;
            if (disposing)
            {
                CurrentUser = null;
                CurrentImage = null;
            }
        }

        #endregion
    }
}
