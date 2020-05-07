SELECT 	m.stock, 
		s.current_value / 1000 as "current_value", 
		s.quantity / 1000 as "quantity", 
        (current_value / (SELECT sum(current_value) FROM `stockplanner`.`source`)) as 'current_percentage',
        m.target_percent as 'target_percentage'
FROM `stockplanner`.`source` as s
LEFT JOIN `stockplanner`.`view_manifest` as m
ON m.stock = s.stock
order by 'stock' asc;