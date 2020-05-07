CREATE VIEW `display_manifest` AS
SELECT stock, target_percentage / 1000000 as 'target_percent', write_date
FROM `stockplanner`.`manifest`
order by 'stock' asc;
