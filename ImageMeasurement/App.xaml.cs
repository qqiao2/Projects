using ImageMeasurement.DataModels;
using ImageMeasurement.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ImageMeasurement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IDisposable
    {
        private MeasureImageVM? _appWinVm;
        private TestDbContext? dbContext;

        protected override void OnStartup(StartupEventArgs e)
        {
            dbContext = new TestDbContext(); 
            AppContext.Instance.CurrentUser = dbContext.Users.First();
            _appWinVm = new MeasureImageVM();
            MeasureImageWindow appwin = new MeasureImageWindow();
            appwin.DataContext = _appWinVm;
            appwin.Show();
            base.OnStartup(e);
        }

        private void CleanUp()
        {
            _appWinVm?.Dispose();
            dbContext?.Dispose();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            CleanUp();
        }

        #region IDisposable Support
        private bool disposedValue = false; 
        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;

                if (disposing)
                {
                    CleanUp();
                }
            }
        }

        ~App()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
