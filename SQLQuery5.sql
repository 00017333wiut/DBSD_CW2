CREATE OR ALTER PROCEDURE udpArtworkFilter
    @Title NVARCHAR(160),
    @ArtistID INT = NULL,
    @CategoryID INT = NULL,
    @Year INT = NULL,
    @TotalCount INT OUT,
    @SortColumn NVARCHAR(100) = 'ArtworkID',
    @SortDesc BIT = 0,
    @Page INT = 1,
    @PageSize INT = 100
AS
BEGIN
    DECLARE @sqlWhere NVARCHAR(1000) = ''
    DECLARE @sqlParamsCount NVARCHAR(1000) = '@Title NVARCHAR(160), @ArtistID INT, @CategoryID INT, @Year INT, @TotalCountOUT INT OUT'
    DECLARE @sqlParamsMain NVARCHAR(1000) = '@Title NVARCHAR(160), @ArtistID INT, @CategoryID INT, @Year INT, @OffsetRows INT, @PageSize INT, @SortDir NVARCHAR(5)'
    DECLARE @SortDir NVARCHAR(5) = ' ASC'
    
    IF @SortDesc = 1
        SET @SortDir = ' DESC'

    IF LEN(TRIM(ISNULL(@Title, ''))) > 0
        SET @sqlWhere += ' Title LIKE @Title + ''%'' AND '
        
    IF @ArtistID IS NOT NULL
        SET @sqlWhere += ' ArtistID = @ArtistID AND '
        
    IF @CategoryID IS NOT NULL
        SET @sqlWhere += ' CategoryID = @CategoryID AND '
        
    IF @Year IS NOT NULL
        SET @sqlWhere += ' Year = @Year AND '

    IF LEN(@sqlWhere) > 0
    BEGIN
        SET @sqlWhere = ' WHERE ' + LEFT(@sqlWhere, LEN(@sqlWhere)-4)
    END

    DECLARE @sqlCount NVARCHAR(1000) = N'SELECT @TotalCountOUT = COUNT(*) FROM Artwork ' + @sqlWhere

    EXEC sp_executesql @sqlCount,
        @sqlParamsCount,
        @TotalCountOUT = @TotalCount OUT,
        @Title = @Title,
        @ArtistID = @ArtistID,
        @CategoryID = @CategoryID,
        @Year = @Year

    DECLARE @sql NVARCHAR(1000) = N'SELECT [ArtworkID]
        ,[Title]
        ,[ArtistID]
        ,[CategoryID]
        ,[Year]
        ,[RentalPrice]
        ,[Availability]
        ,[IsAvailable]
        FROM Artwork ' + @sqlWhere
        + ' ORDER BY ' + @SortColumn + @SortDir
        + ' OFFSET @OffsetRows ROWS FETCH NEXT @PageSize ROWS ONLY'

    DECLARE @Offset INT = (@Page - 1)*@PageSize
    IF @Offset < 0
        SET @Offset = 0

    EXEC sp_executesql @sql,
        @sqlParamsMain,
        @Title = @Title,
        @ArtistID = @ArtistID,
        @CategoryID = @CategoryID,
        @Year = @Year,
        @OffsetRows = @Offset,
        @PageSize = @PageSize,
        @SortDir = @SortDir
END


CREATE OR ALTER PROCEDURE udpArtworkExportAsXml
    @Title NVARCHAR(160) = NULL,
    @ArtistID INT = NULL,
    @CategoryID INT = NULL,
    @Year INT = NULL,
    @SortColumn NVARCHAR(100) = 'ArtworkID',
    @SortDesc BIT = 0,
    @XmlData XML OUTPUT
AS
BEGIN
    DECLARE @cnt INT

    DECLARE @tab TABLE (
        [ArtworkID] INT,
        [Title] VARCHAR(100),
        [ArtistID] INT,
        [CategoryID] INT,
        [Year] INT,
        [RentalPrice] DECIMAL(10,2),
        [Availability] DATE,
        [IsAvailable] BIT
    )

    INSERT INTO @tab
    EXEC udpArtworkFilter
        @Title = @Title,
        @ArtistID = @ArtistID,
        @CategoryID = @CategoryID,
        @Year = @Year,
        @TotalCount = @cnt OUTPUT,
        @SortColumn = @SortColumn,
        @SortDesc = @SortDesc,
        @Page = 1,
        @PageSize = 10000

    SET @XmlData = (
        SELECT [ArtworkID] AS "@Id",
               [Title],
               [ArtistID],
               [CategoryID],
               [Year],
               [RentalPrice],
               [Availability],
               [IsAvailable]
        FROM @tab
        FOR XML PATH('Artwork'), ROOT('Artworks')
    )
END