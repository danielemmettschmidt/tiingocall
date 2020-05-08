CREATE VIEW `display_source_archive` AS
SELECT stock, write_date, CAST((current_value / 10000) as DECIMAL(12,2)) as 'current_value', (quantity / 10000) as 'quantity'
FROM stockplanner.source_archive
order by 2 desc, 1 asc;