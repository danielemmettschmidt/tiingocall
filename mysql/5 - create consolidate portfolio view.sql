CREATE VIEW `consolidate_portfolio` AS
SELECT 	m.stock, 
		ifnull(s.current_value, 0)  as "current_value", 
		ifnull(s.quantity, 0) as "quantity",
        m.target_percentage as "target_percent"
FROM `stockplanner`.`source` as s
RIGHT JOIN `stockplanner`.`manifest` as m
ON (m.stock is not null and m.stock = s.stock)
order by 'stock' asc;