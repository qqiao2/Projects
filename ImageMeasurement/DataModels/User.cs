using System;
using System.Collections.Generic;

namespace ImageMeasurement.DataModels;

public partial class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? LoginPassWord { get; set; }

    public virtual ICollection<MeasurementAuditTrail> MeasurementAuditTrails { get; set; } = new List<MeasurementAuditTrail>();
}
