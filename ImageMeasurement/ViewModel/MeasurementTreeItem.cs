using ImageMeasurement.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImageMeasurement.ViewModel
{
    public enum ItemType
    {
        Anatomy,
        MeasurementType
    }
    public interface ITreeItem
    {
        string Name { get; }
        int ID { get; }
        double? Value { get; }
        ITreeItem? Parent { get; }
        ObservableCollection<ITreeItem> Children { get; }
        string IconFilePath { get; }
        Visibility ShowAddOrModifyMeasurementButton { get; }
        Visibility ShowValue {  get; }

        string AddOrModifyMeasurementButtonIcon { get; }

    }

    public class MeasurementTreeItem : ITreeItem
    {
        private Measurement? _measurement;
        private ITreeItem _parent;
        private double? _mvalue;
        public MeasurementTreeItem(Measurement meas, ITreeItem parent)
        {
            _measurement = meas;
            _parent = parent;
            _mvalue = meas?.FloatValue;
        }
        public string Name { 
            get {
                if (_measurement == null)
                    return "Measurement_new";

                return "Measurement_" + ID; 
            } 
        }
        public int ID { 
            get {
                if (_measurement == null)
                    return int.MinValue;
                
                return _measurement.Id;
            } 
        }

        public double? Value { 
            get {
                return _mvalue;
            } 
            set
            {
                _mvalue = value;
            }
        }

        public string MeasurementType
        {
            get
            {
                return _parent ==null? "":_parent.Name;
            }
        }

        public string Anatomy
        {
            get
            {
                return _parent==null || _parent.Parent == null? "": _parent.Parent.Name;
            }
        }

        public ITreeItem? Parent { get {  return _parent; } }

        public ObservableCollection<ITreeItem> Children { get; } = new ObservableCollection<ITreeItem>();

        public bool IsNewMeasurement
        {
            get { return _measurement == null; }
        }

        public string IconFilePath
        {
            get
            {
                var dirpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var src = Path.Combine(dirpath, @"Icons\Measurement.jpg");
                return src;
            }
        }

        public Visibility ShowAddOrModifyMeasurementButton { get { return Visibility.Visible; } }
        public Visibility ShowValue { get { return Visibility.Visible; } }

        public string AddOrModifyMeasurementButtonIcon
        {
            get
            {
                var dirpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var src = Path.Combine(dirpath, @"Icons\Modify.jpg");
                return src;
            }
        }
    }

    public class MeasurementTreeGroupItem : ITreeItem
    {
        private Object? _datamodel;
        private ItemType _itemtype;
        private const string Wrong_Item = "Wrong_Item";
        private ITreeItem? _parent;

        public MeasurementTreeGroupItem(Object datamodel, ItemType type, ITreeItem? parent)
        {
            _itemtype = type;
            _datamodel = datamodel;
            _parent = parent;
        }

        public string Name { 
            get {
                if (_datamodel == null)
                    return Wrong_Item;
                if (_itemtype == ItemType.Anatomy)
                {
                    AnatomicalFeature a = (AnatomicalFeature)_datamodel;
                    return a == null? Wrong_Item : a.Name;
                }

                MeasurementType mt = (MeasurementType)_datamodel;

                return mt == null ? Wrong_Item : mt.Name;
            } 
        }

        public int ID
        {
            get
            {
                if (_datamodel == null)
                    return int.MinValue;
                if (_itemtype == ItemType.Anatomy)
                {
                    AnatomicalFeature a = (AnatomicalFeature)_datamodel;
                    return a == null ? int.MinValue : a.Id;
                }

                MeasurementType mt = (MeasurementType)_datamodel;

                return mt == null ? int.MinValue : mt.Id;
            }
        }

        public double? Value { get; } = null;

        public ITreeItem? Parent { get { return _parent; } }

        public string IconFilePath { 
            get 
            {
                string iconpath = @"Icons\Anatomy.png";
                if (_itemtype == ItemType.MeasurementType)
                    iconpath = @"Icons\shape.png";

                string? assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var src = Path.Combine(assemblyFolder, iconpath);
                return src;
            } 
        }

        public Visibility ShowAddOrModifyMeasurementButton
    { 
            get { 
                if (_itemtype == ItemType.MeasurementType)                    
                    return Visibility.Visible; 
                return Visibility.Hidden;
            } 
        }
        public Visibility ShowValue { get { return Visibility.Hidden; } }

        public string AddOrModifyMeasurementButtonIcon
    {
            get
            {
                var dirpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var src = Path.Combine(dirpath, @"Icons\add.jpg");
                return src;
            }
        }

        public ObservableCollection<ITreeItem> Children
        {
            get
            {
                try
                {
                    if (_datamodel == null || AppContext.Instance == null)
                        return new ObservableCollection<ITreeItem>();
                    if (_itemtype == ItemType.Anatomy)
                    {
                        AnatomicalFeature a = (AnatomicalFeature)_datamodel;
                        if (a == null)
                            return new ObservableCollection<ITreeItem>();

                        var mtList = a.MeasurementTypes.Select(mt => new MeasurementTreeGroupItem(mt, ItemType.MeasurementType, this));

                        return new ObservableCollection<ITreeItem>(mtList);
                    }

                    MeasurementType mt = (MeasurementType)_datamodel;
                    
                    var mList = mt.Measurements.Where(m=>m.ImageId == AppContext.Instance.CurrentImage?.Id && 
                         m.MeasurementTypeId == mt.Id && m.AnatomicalFeatureId == Parent?.ID).Select(m => new MeasurementTreeItem(m, this));
                    return new ObservableCollection<ITreeItem>(mList);

                }
                catch (Exception /*ex*/)
                {
                    return new ObservableCollection<ITreeItem>();
                }
            }
        }
    }


}
