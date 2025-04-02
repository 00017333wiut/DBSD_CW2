select ArtworkID, Title, ArtistID, CategoryID, Year, RentalPrice, Availability, IsAvailable
from Artwork
where Title like concat(@Title, '%')
and Availability >= coalesce(@Availability, '1900-01-01') 
order by ArtworkID 
offset (@Page-1)*@PageSize rows fetch next @PageSize rows only