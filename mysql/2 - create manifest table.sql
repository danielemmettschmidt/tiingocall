CREATE TABLE `stockplanner`.`manifest` (
  `stock` VARCHAR(6) NOT NULL,
  `target_percent` INT NULL,
  `write_date` DATETIME NULL,
  PRIMARY KEY (`stock`));
