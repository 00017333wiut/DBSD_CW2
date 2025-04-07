CREATE OR ALTER PROCEDURE udpArtworkExportAsJson
    @Title VARCHAR(100) = NULL,
    @Year INT = NULL,
    @SortColumn VARCHAR(50) = 'ArtworkID',
    @SortDesc BIT = 0,
    @JsonData NVARCHAR(MAX) OUTPUT
AS
BEGIN
    DECLARE @sql NVARCHAR(MAX)
    DECLARE @sortDirection NVARCHAR(4) = CASE WHEN @SortDesc = 1 THEN 'DESC' ELSE 'ASC' END
    
    SET @sql = N'
    SET @JsonData = (
        SELECT 
            a.ArtworkID,
            a.Title,
            a.ArtistID,
            art.ArtistName AS ArtistName,
            a.CategoryID,
            c.CategoryName AS CategoryName,
            a.Year,
            a.RentalPrice,
            a.Availability,
            a.IsAvailable
        FROM 
            Artwork a
            LEFT JOIN Artists art ON a.ArtistID = art.ArtistID
            LEFT JOIN Category c ON a.CategoryID = c.CategoryID
        WHERE 
            (@Title IS NULL OR a.Title LIKE ''%'' + @Title + ''%'')
            AND (@Year IS NULL OR a.Year = @Year)
        ORDER BY 
            CASE WHEN @SortColumn = ''ArtworkID'' THEN a.ArtworkID END ' + @sortDirection + ',
            CASE WHEN @SortColumn = ''Title'' THEN a.Title END ' + @sortDirection + ',
            CASE WHEN @SortColumn = ''Year'' THEN a.Year END ' + @sortDirection + ',
            CASE WHEN @SortColumn = ''RentalPrice'' THEN a.RentalPrice END ' + @sortDirection + ',
            CASE WHEN @SortColumn = ''ArtistName'' THEN art.ArtistName END ' + @sortDirection + ',
            CASE WHEN @SortColumn = ''CategoryName'' THEN c.CategoryName END ' + @sortDirection + '
        FOR JSON PATH, ROOT(''Artworks'')
    )'
    
    DECLARE @paramDef NVARCHAR(500) = N'@Title VARCHAR(100), @Year INT, @SortColumn VARCHAR(50), @SortDesc BIT, @JsonData NVARCHAR(MAX) OUTPUT'
    
    EXEC sp_executesql @sql, @paramDef, @Title, @Year, @SortColumn, @SortDesc, @JsonData OUTPUT
END