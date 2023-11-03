using System;
using System.Collections.Generic;

namespace ImageMeasurement.DataModels;

public partial class MeasurementAuditTrail
{
    public int Id { get; set; }

    public int? MeasurementId { get; set; }

    public int? UserId { get; set; }

    public DateTime TimeStamp { get; set; }

    public string? Action { get; set; }

    public string? Intent { get; set; }

    public virtual Measurement? Measurement { get; set; }

    public virtual User? User { get; set; }
}
