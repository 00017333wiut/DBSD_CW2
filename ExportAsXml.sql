CREATE OR ALTER PROCEDURE udpArtworkExportAsXml
    @Title VARCHAR(100) = NULL,
    @Year INT = NULL,
    @SortColumn VARCHAR(50) = 'ArtworkID',
    @SortDesc BIT = 0,
    @XmlData XML OUTPUT
AS
BEGIN
    DECLARE @sql NVARCHAR(MAX)
    DECLARE @sortDirection NVARCHAR(4) = CASE WHEN @SortDesc = 1 THEN 'DESC' ELSE 'ASC' END
    
    SET @sql = N'
    SELECT 
        a.ArtworkID,
        a.Title,
        a.ArtistID,
        art.Name AS ArtistName,
        a.CategoryID,
        c.Name AS CategoryName,
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
        CASE WHEN @SortColumn = ''ArtistName'' THEN art.Name END ' + @sortDirection + ',
        CASE WHEN @SortColumn = ''CategoryName'' THEN c.Name END ' + @sortDirection + '
    FOR XML PATH(''Artwork''), ROOT(''Artworks'')'

    DECLARE @paramDef NVARCHAR(500) = N'@Title VARCHAR(100), @Year INT, @SortColumn VARCHAR(50), @SortDesc BIT'
    
    EXEC sp_executesql @sql, @paramDef, @Title, @Year, @SortColumn, @SortDesc, @XmlData OUTPUT
END

