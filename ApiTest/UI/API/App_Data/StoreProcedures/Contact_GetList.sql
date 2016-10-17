CREATE PROCEDURE [dbo].[Contact_GetList](
	@countSkip INT,
	@pageSize INT, 
	@keyword NVARCHAR(MAX),
	@keySort NVARCHAR(50), 
	@orderDescending BIT,
	@yayYoId INT
)
AS
BEGIN
declare @orderBy nvarchar(max) =
Case When @keySort = 'Id' and @orderDescending = 0 Then 'c.Id Asc'
	 When @keySort = 'Id' and @orderDescending = 1 Then 'c.Id Desc' 
	 When @keySort = 'FirstName' and @orderDescending = 0 Then 'c.FirstName Asc'
	 When @keySort = 'FirstName' and @orderDescending = 1 Then 'c.FirstName Desc'
	 When @keySort = 'LastName' and @orderDescending = 0 Then 'c.LastName Asc'
	 When @keySort = 'LastName' and @orderDescending = 1 Then 'c.LastName Desc'
	 When @keySort = 'Phone' and @orderDescending = 0 Then 'c.Phone Asc'
	 When @keySort = 'Phone' and @orderDescending = 1 Then 'c.Phone Desc'	 
	 ELSE 'c.Id Asc'
	 END
DECLARE @query NVARCHAR(MAX) ='
;with paging as (
		Select 
		ROW_NUMBER() OVER (ORDER BY ' + @orderBy + ') as RowCounts,
		c.Id,
		c.FirstName,		
		c.LastName,
		c.Phone		
		From Contact c inner join SafetySetting s on c.SafetySettingId = s.Id  where YayYoId ='+convert(nvarchar, @yayYoId)+'' 
	IF NULLIF(@keyword, '') IS NOT NULL
	Begin
	set @query = @query + ' and ( c.FirstName like ''' + @keyword + '%'' or c.LastName like ''' + @keyword  + '%'' or c.PhoneNumber like ''' + @keyword + '%'')'
	End
	set @query = @query + ') select max(p.RowCounts) RowCounts, 
		0 Id, 
		null [FirstName], 
		null [LastName],
		null [Phone]		
	from paging p

	union all

	select * from paging p
	where p.RowCounts > ' + CONVERT(NVARCHAR(10), (@countSkip * @pageSize)) + ' and p.RowCounts <= ' + CONVERT(NVARCHAR(10), ((@countSkip + 1) * @pageSize)) + ''
	
EXECUTE sp_executesql @query

End