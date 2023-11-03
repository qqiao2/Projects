using System;
using System.Collections.Generic;

namespace ImageMeasurement.DataModels;

public partial class MeasurementType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Measurement> Measurements { get; set; } = new List<Measurement>();

    public virtual ICollection<AnatomicalFeature> AnatomicalFeatures { get; set; } = new List<AnatomicalFeature>();
}
