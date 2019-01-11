SELECT TOP 1
    COUNT(*), [Name]
FROM dbo.Badges
GROUP BY [name]
ORDER BY COUNT(*) DESC