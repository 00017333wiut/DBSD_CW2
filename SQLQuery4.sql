CREATE OR ALTER PROCEDURE udpGetAllArtworks
AS
BEGIN
  SELECT ArtworkID, Title, ArtistID, CategoryID, 
         Year, RentalPrice, Availability, IsAvailable, ArtworkImage
  FROM Artwork
END
GO
-- Test
-- EXEC udpGetAllArtworks
GO

CREATE OR ALTER FUNCTION fnGetAllArtworks()
RETURNS TABLE 
AS RETURN(
  SELECT ArtworkID, Title, ArtistID, CategoryID, 
         Year, RentalPrice, Availability, IsAvailable, ArtworkImage
  FROM Artwork
)
GO
-- Test
-- SELECT * FROM fnGetAllArtworks()
GO

CREATE OR ALTER PROCEDURE GetArtworkById(@Id INT)
AS
BEGIN
  SELECT ArtworkID, Title, ArtistID, CategoryID, 
         Year, RentalPrice, Availability, IsAvailable, ArtworkImage
  FROM Artwork
  WHERE ArtworkID = @Id
END
GO
-- Test
-- EXEC GetArtworkById @Id = 1
GO

CREATE OR ALTER PROCEDURE dbo.udpInsertArtwork(
   @Title VARCHAR(100),
   @ArtistID INT = NULL,
   @CategoryID INT = NULL,
   @Year INT = NULL,
   @RentalPrice DECIMAL(10,2) = NULL,
   @Availability DATE = NULL,
   @IsAvailable BIT = 1,
   @ArtworkImage VARBINARY(MAX) = NULL,
   @Error NVARCHAR(2000) OUT
) AS
BEGIN
    BEGIN TRY
      INSERT INTO Artwork(Title, ArtistID, CategoryID, Year, RentalPrice, Availability, IsAvailable, ArtworkImage)
      OUTPUT inserted.*
      VALUES(@Title, @ArtistID, @CategoryID, @Year, @RentalPrice, @Availability, @IsAvailable, @ArtworkImage)
      RETURN(0)
    END TRY
    BEGIN CATCH
       SET @Error = ERROR_MESSAGE()
       DECLARE @errCode INT;
       SET @errCode = ERROR_NUMBER()
       RETURN(@errCode)
    END CATCH
END
GO
-- Test
/* 
DECLARE @err NVARCHAR(2000)
DECLARE @ret INT
EXEC @ret = udpInsertArtwork 
   'Mona Lisa', 
   1, 
   1, 
   1503, 
   1000.00, 
   NULL, 
   1, 
   NULL, 
   @err OUT
PRINT @ret
PRINT @err 
*/
GO