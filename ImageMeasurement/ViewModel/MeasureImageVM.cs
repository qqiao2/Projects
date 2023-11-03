using ImageMeasurement.DataModels;
using ImageMeasurement.MeasurementTools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ImageMeasurement.ViewModel
{
    public class MeasureImageVM : ViewModelBase
    {
        //private TestDbContext testDbContext;
        private Image? _selectedImage;

        public MeasureImageVM() {
            AddOrModifyMeasurementCommand = new RelayCommand("AddNewMeasurementCommand", AddOrModifyMeasurement, (o) => true);
            SaveCurrentMeasurementCommand = new RelayCommand("SaveCurrentMeasurementCommand", SaveCurrentMeasurement, (o) => true);
        }

        public ObservableCollection<Image> Images
        {
            get
            {
                try
                {
                    using (var context = new TestDbContext())
                    {
                        DbSet<Image> images = context.Images;
                        return new ObservableCollection<Image>(images);
                    }
                }
                catch (Exception /*ex*/)
                {
                    return new ObservableCollection<Image>();
                }
            }
        }

        public ICommand AddOrModifyMeasurementCommand { get; set; }

        public ICommand SaveCurrentMeasurementCommand { get; set; }


        public Image? SelectedImage
        {
            get => _selectedImage; 
            set
            {
                _selectedImage = value;
                AppContext.Instance.CurrentImage = value;
                OnPropertyChanged(nameof(SelectedImage));
                OnPropertyChanged(nameof(SelectedImageFiePath));
                OnPropertyChanged(nameof(MeasurementTreeItems));
                ResetSelectedMeasurement(null);
            }
        }

        public string SelectedImageFiePath { 
            get {
                if (SelectedImage == null)
                    return string.Empty;
                return SelectedImage.FileLocation;
            } 
        }

        public int? SelectedImageId
        {
            get
            {
                if (SelectedImage == null)
                    return null;
                return SelectedImage.Id;
            }
        }

        public ObservableCollection<ITreeItem> MeasurementTreeItems { 
            get
            {
                try
                {
                    if (_selectedImage == null)
                        return new ObservableCollection<ITreeItem>();

                    using (var context = new TestDbContext())
                    {
                        var anatomicalFeatures = context.AnatomicalFeatures
                        .Include(a => a.MeasurementTypes)
                        .Include(a => a.Measurements);
                        var items = anatomicalFeatures.Select(x => new MeasurementTreeGroupItem(x, ItemType.Anatomy, null));

                        return new ObservableCollection<ITreeItem>(items);
                    }
                }
                catch (Exception /*ex*/)
                {
                    return new ObservableCollection<ITreeItem>();
                }
            }
        }

        public MeasurementTreeItem SelectedMeasurement { get; set; }

        public string SelectedMeasurementInfor
        {
            get
            {
                if (SelectedMeasurement == null)
                    return string.Empty;

                else
                    return SelectedMeasurement.Name + ", " + SelectedMeasurement.Anatomy + ", " + SelectedMeasurement.MeasurementType;
            }
        }

        public string CurrentMeasurementOperation { get; set; }

        public string CurrentMeasurementOperationIntent { get; set; }

        public Visibility ShowSelectedMeasurement
        {
            get
            {
                if  (  SelectedMeasurement ==null)
                    return Visibility.Collapsed;
                return Visibility.Visible;
            }
        }

        private void AddOrModifyMeasurement(object args)
        {
            MeasurementTreeItem? leaf = null;
            if (args is MeasurementTreeItem)
            {
                leaf = (MeasurementTreeItem)args;
                ResetSelectedMeasurement(leaf);
            }
            else
            {
                leaf = new MeasurementTreeItem(null, (ITreeItem)args);
            }
            ResetSelectedMeasurement(leaf);
        }

        private void SaveCurrentMeasurement(object args)
        {
            if (SelectedMeasurement == null)
                return;

            using (var context = new TestDbContext())
            {
                if (SelectedMeasurement.IsNewMeasurement)
                {
                    // Temporary code for testing
                    string mtext = string.Empty;
                    if (SelectedMeasurement.MeasurementType == "Line")
                    {
                        mtext = JsonSerializer.Serialize(new LineTool(new System.Drawing.Point(5,6), new System.Drawing.Point(20,14)));
                    }
                    else if (SelectedMeasurement.MeasurementType == "Circle")
                    {
                        mtext = JsonSerializer.Serialize(new CircleTool(new System.Drawing.Point(5, 6), 4.5));
                    }

                    Measurement measurement = new Measurement() { ImageId = CurrentImageID, MeasurementTypeId = SelectedMeasurement.Parent?.ID,
                        AnatomicalFeatureId = SelectedMeasurement.Parent?.Parent?.ID, FloatValue = SelectedMeasurement.Value, 
                        MeasurementText = mtext
                    };
                    context.Measurements.Add(measurement);
                    context.SaveChanges();
                    MeasurementAuditTrail auditTrail = new MeasurementAuditTrail()
                    {
                        MeasurementId = measurement.Id,
                        TimeStamp = DateTime.Now,
                        UserId = CurrentUserID,
                        Action = CurrentMeasurementOperation,
                        Intent = CurrentMeasurementOperationIntent
                    };
                    context.MeasurementAuditTrails.Add(auditTrail);
                    context.SaveChanges();
                }
                else
                {
                    var item = context.Measurements.FirstOrDefault(a => a.Id == SelectedMeasurement.ID);
                    if (item != null)
                    {
                        item.FloatValue = SelectedMeasurement.Value;
                        MeasurementAuditTrail auditTrail = new MeasurementAuditTrail()
                        {
                            MeasurementId = item.Id,
                            TimeStamp = DateTime.Now,
                            UserId = CurrentUserID,
                            Action = CurrentMeasurementOperation,
                            Intent = CurrentMeasurementOperationIntent
                        };
                        context.MeasurementAuditTrails.Add(auditTrail);
                        context.SaveChanges();
                    }
                }
            }
            OnPropertyChanged(nameof(MeasurementTreeItems));
            ResetSelectedMeasurement(null);

        }

        private void ResetSelectedMeasurement(MeasurementTreeItem item)
        {
            SelectedMeasurement = item;
            CurrentMeasurementOperation = string.Empty;
            CurrentMeasurementOperationIntent = string.Empty;
            OnPropertyChanged(nameof(ShowSelectedMeasurement));
            OnPropertyChanged(nameof(SelectedMeasurement));
            OnPropertyChanged(nameof(SelectedMeasurementInfor));
            OnPropertyChanged(nameof(ShowSelectedMeasurement));
            OnPropertyChanged(nameof(CurrentMeasurementOperation));
            OnPropertyChanged(nameof(CurrentMeasurementOperationIntent));
        }

        private int? CurrentImageID
        {
            get
            {
                return AppContext.Instance?.CurrentImage?.Id;
            }
        }

        private int? CurrentUserID
        {
            get
            {
                return AppContext.Instance?.CurrentUser?.Id;
            }
        }

    }
}
