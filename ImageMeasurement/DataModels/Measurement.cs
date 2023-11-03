using System;
using System.Collections.Generic;

namespace ImageMeasurement.DataModels;

public partial class Measurement
{
    public int Id { get; set; }

    public int? ImageId { get; set; }

    public int? AnatomicalFeatureId { get; set; }

    public int? MeasurementTypeId { get; set; }

    public double? FloatValue { get; set; }

    public string? MeasurementText { get; set; }

    public virtual AnatomicalFeature? AnatomicalFeature { get; set; }

    public virtual Image? Image { get; set; }

    public virtual ICollection<MeasurementAuditTrail> MeasurementAuditTrails { get; set; } = new List<MeasurementAuditTrail>();

    public virtual MeasurementType? MeasurementType { get; set; }
}
