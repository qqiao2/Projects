
USE TestDB

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[All_Measurement_view]'))
    DROP VIEW [dbo].All_Measurement_view
GO

SET ANSI_NULLS ON
GO
 
SET QUOTED_IDENTIFIER ON
GO

create view All_Measurement_view
as
select a.ID "Measurement_ID", a.FloatValue "Measurement_Value", a.MeasurementText, a.AnatomicalFeatureID, c.Name "Anatomy_Name", a.MeasurementTypeID, 
b.Name "MeasurementType", a.ImageID, d.InstanceUID, d.SeriesInstanceUID, d.Modality, d.FileLocation
from Measurements a
inner join MeasurementType b on a.MeasurementTypeID = b.ID
inner join AnatomicalFeature c on a.AnatomicalFeatureID = c.ID
inner join Image d on a.ImageID = d.ID
go

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[all_measurement_with_audit]'))
    DROP VIEW [dbo].all_measurement_with_audit
GO

create view all_measurement_with_audit
as 
select a.ID "Audit_Trail_ID", a.TimeStamp, a.Action, a.Intent, a.UserID, c.UserName, b.*
from MeasurementAuditTrail a
left join All_Measurement_view b on a.MeasurementID = b.Measurement_ID
left join Users c on a.UserID = c.ID
go


