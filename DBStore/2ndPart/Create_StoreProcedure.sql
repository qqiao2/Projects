USE TestDB
GO

IF EXISTS ( SELECT * FROM sys.objects 
		WHERE name="sp_MeasurementAuditByAnatomicFeature" )
	DROP PROCEDURE dbo.sp_MeasurementAuditByAnatomicFeature

GO

CREATE PROCEDURE sp_MeasurementAuditByAnatomicFeature @anatomy varchar(64)
AS
SELECT [TimeStamp]
      ,[Action]
      ,[Intent]
      ,[UserName]
      ,[Measurement_Value]
      ,[Anatomy_Name]
      ,[MeasurementType]
      ,[InstanceUID]
      ,[SeriesInstanceUID]
      ,[Modality]
      ,[FileLocation]
  FROM [TestDB].[dbo].[all_measurement_with_audit]
  WHERE [Anatomy_Name] = @anatomy
  Order By [Modality] ASC

IF EXISTS ( SELECT * FROM sys.objects 
		WHERE name="sp_MeasurementAuditByModality" )
	DROP PROCEDURE dbo.sp_MeasurementAuditByModality

GO

CREATE PROCEDURE sp_MeasurementAuditByModality @modality varchar(64)
AS
SELECT [TimeStamp]
      ,[Action]
      ,[Intent]
      ,[UserName]
      ,[Measurement_Value]
      ,[Anatomy_Name]
      ,[MeasurementType]
      ,[InstanceUID]
      ,[SeriesInstanceUID]
      ,[Modality]
      ,[FileLocation]
  FROM [TestDB].[dbo].[all_measurement_with_audit]
  WHERE [Modality] = @modality
  Order By [Anatomy_Name] ASC

IF EXISTS ( SELECT * FROM sys.objects 
		WHERE name="sp_MeasurementAuditBetweenDates" )
	DROP PROCEDURE dbo.sp_MeasurementAuditBetweenDates

GO

CREATE PROCEDURE sp_MeasurementAuditBetweenDates 
   @date1 datetime,
   @date2 datetime
AS
SELECT [TimeStamp]
      ,[Action]
      ,[Intent]
      ,[UserName]
      ,[Measurement_Value]
      ,[Anatomy_Name]
      ,[MeasurementType]
      ,[InstanceUID]
      ,[SeriesInstanceUID]
      ,[Modality]
      ,[FileLocation]
  FROM [TestDB].[dbo].[all_measurement_with_audit]
  WHERE [TimeStamp] BETWEEN @date1 AND @date2
  ORDER BY [Anatomy_Name], [Modality]

