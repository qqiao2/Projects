using System;
using System.Collections.Generic;

namespace ImageMeasurement.DataModels;

public partial class Image
{
    public int Id { get; set; }

    public string InstanceUid { get; set; } = null!;

    public string SeriesInstanceUid { get; set; } = null!;

    public string Modality { get; set; } = null!;

    public string FileLocation { get; set; } = null!;

    public virtual ICollection<Measurement> Measurements { get; set; } = new List<Measurement>();
}
