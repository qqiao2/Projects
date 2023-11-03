using System;
using System.Collections.Generic;

namespace ImageMeasurement.DataModels;

public partial class AnatomicalFeature
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Measurement> Measurements { get; set; } = new List<Measurement>();

    public virtual ICollection<MeasurementType> MeasurementTypes { get; set; } = new List<MeasurementType>();
}
