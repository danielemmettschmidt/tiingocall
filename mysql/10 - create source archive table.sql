CREATE TABLE `stockplanner`.`source_archive` (
  `stock` VARCHAR(6) NOT NULL,
  `write_date` DATETIME NOT NULL,
  `current_value` INT NOT NULL,
  `quantity` INT NOT NULL,
  PRIMARY KEY (`stock`, `write_date`));
