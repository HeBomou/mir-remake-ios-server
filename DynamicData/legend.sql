-- create database legend;
use legend;

DROP TABLE IF EXISTS `character`;
CREATE TABLE `character` (
  `charid` int NOT NULL AUTO_INCREMENT,
  `playerid` int NOT NULL,
  `occupation` VARCHAR(255) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
   primary key (`charid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;
CREATE INDEX `playerid` ON `character`(`playerid`);
CREATE UNIQUE INDEX `name` ON `character`(`name`);

DROP TABLE IF EXISTS `character_attribute`;
CREATE TABLE `character_attribute` (
  `charid` int NOT NULL,
  `level` int NOT NULL,
  `experience` int NOT NULL,
  `attributes` VARCHAR(45),
  primary key (`charid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;


DROP TABLE IF EXISTS `wallet`;
CREATE TABLE `wallet` (
  `charid` int NOT NULL,
  `virtual` int NOT NULL,
  `charge` int NOT NULL,
   primary key (`charid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS `skill`;
create table `skill`( 
  `realid` int NOT NULL AUTO_INCREMENT,
  `skillid` int NOT NULL, 
  `charid` int NOT NULL, 
  `masterly` int NOT NULL ,
  `level` int NOT NULL, 
   primary key (`realid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE INDEX `charid` ON `skill`(`charid`);

DROP TABLE IF EXISTS `item`;
create table `item`(
  `realid` int NOT NULL AUTO_INCREMENT,
  `itemid` int NOT NULL,
  `charid` int NOT NULL,
  `num` int NOT NULL,
  `place` VARCHAR(45) NOT NULL,
  `position` int NOT NULL,
   primary key (`realid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE INDEX `charid` ON `item`(`charid`);

DROP TABLE IF EXISTS `equipment`;
CREATE table `equipment`(
  `realid` int not null,
  `charid` int not null,
  `strength_num` int not null,
  `gem_list` VARCHAR(255) NOT NULL,
  `enchant_attr` VARCHAR(255) NOT NULL,
   primary key (`realid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE INDEX `charid` ON `equipment`(`charid`);

DROP TABLE IF EXISTS `mission`;
create table `mission`(
  `missionid` int NOT NULL,
  `charid` int NOT NULL,
  `targets` VARCHAR(150) NOT NULL,
  `status` VARCHAR(255) NOT NULL
)ENGINE=InnoDB DEFAULT CHARSET=utf8;
CREATE INDEX `missionid` ON `mission`(`missionid`);
CREATE INDEX `charid` ON `mission`(`charid`);

DROP TABLE IF EXISTS `title`;
create table `title`(
  `titleid` int NOT NULL,
  `charid` int NOT NULL,
  `target` VARCHAR(150) NOT NULL,
  `status` VARCHAR(255) NOT NULL
)ENGINE=InnoDB DEFAULT CHARSET=utf8;
CREATE INDEX `titleid` ON `title`(`titleid`);
CREATE INDEX `charid` ON `title`(`charid`);

DROP TABLE IF EXISTS `character_position`;
CREATE TABLE `character_position` (
  `charid` int NOT NULL AUTO_INCREMENT,
  `x` VARCHAR(20),
  `y` VARCHAR(20),
   primary key (`charid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS `user`;
create table `user`( 
  `userid` int NOT NULL AUTO_INCREMENT,
  `user_name` VARCHAR(155) NOT NULL,
  `password` VARCHAR(155) NOT NULL,
  `question` VARCHAR(255),
  `answer` VARCHAR(255),
  primary key (`userid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE UNIQUE INDEX `user_name` ON `user`(`user_name`);

DROP TABLE IF EXISTS `vip`;
create table `vip`( 
  `userid` int NOT NULL AUTO_INCREMENT,
  `vip_level` int NOT NULL,
  `charge_money` int NOT NULL,
  primary key (`userid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS `enchantment`;
create table `enchantment`( 
  `realid` int NOT NULL AUTO_INCREMENT,
  `charid` int NOT NULL,
  `enchant_attr` VARCHAR(255),
  primary key (`realid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;
CREATE INDEX `charid` ON `enchantment`(`charid`);

DROP TABLE IF EXISTS `combat_effect`;
create table `combat_effect`( 
  `charid` int NOT NULL,
  `combat` int,
  `name` VARCHAR(45) NOT NULL,
  `occupation` VARCHAR(45) NOT NULL,
  `level` int,
  primary key (`charid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;
CREATE UNIQUE INDEX `name` ON `combat_effect`(`name`);

DROP TABLE IF EXISTS `mall`;
create table `mall`(
  `itemid` int NOT NULL,
  `classid` int NOT NULL,
  `charge_price` int NOT NULL,
  `virtal_price` int NOT NULL,
  primary key (`itemid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;
CREATE INDEX `classid` ON `mall`(`itemid`);

DROP TABLE IF EXISTS `mail`;
create table `mail`(
  `mailid` int NOT NULL AUTO_INCREMENT,
  `senderid` int NOT NULL,
  `sender_name` VARCHAR(55),
  `receiverid` int NOT NULL,
  `title` VARCHAR(55) NOT NULL,
  `detail` VARCHAR(555) NOT NULL,
  `item_array` VARCHAR(255) NOT NULL,
  `charge` int NOT NULL,
  `virtual` int NOT NULL,
  `time` datetime,
  `is_read` int NOT NULL,
  `is_received` int NOT NULL,
  primary key(`mailid`)
);
CREATE INDEX `receiverid` ON `mail`(`receiverid`);

DROP TABLE IF EXISTS `notice`;
create table `notice`(
  `noticeid` int NOT NULL AUTO_INCREMENT,
  `time` datetime,
  `title` VARCHAR(255),
  `detail` VARCHAR(2555),
  primary key(`noticeid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS `attached_title`;
create table `attached_title`(
  `charid` int NOT NULL,
  `titleid` int NOT NULL,
  primary key(`charid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS `mission_log`;
create table `mission_log`(
  `charid` int NOT NULL,
  `target_type` VARCHAR(45),
  `parameter1` int NOT NULL,
  `parameter2` int NOT NULL,
  `parameter3` int NOT NULL,
  key(`charid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS `shortcut`;
create table `shortcut`(
    `charid` int NOT NULL,
    `position` int NOT NULL,
    `type` VARCHAR(25),
    `data` int DEFAULT NULL,
    key(`charid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS `shortcut`;
create table `shortcut`(
    `charid` int NOT NULL,
    `position` int NOT NULL,
    `type` VARCHAR(25),
    `data` int DEFAULT NULL,
    key(`charid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS `alliance`;
create table `alliance`(
    `allianceid` int NOT NULL AUTO_INCREMENT,
    `name` VARCHAR(155),
    key(`allianceid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

DROP TABLE IF EXISTS `alliance_member`;
create table `alliance_member`(
    `charid` int NOT NULL,
    `allianceid` int NOT NULL,
    `alliance_job` VARCHAR(25),
    primary key(`charid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;
CREATE INDEX `allianceid` ON `alliance_member`(`allianceid`);

DROP TABLE IF EXISTS `alliance_apply`;
create table `alliance_apply`(
    `applyid` int NOT NULL AUTO_INCREMENT,
    `charid` int NOT NULL,
    `allianceid` int NOT NULL,
    primary key(`applyid`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;
CREATE INDEX `allianceid` ON `alliance_apply`(`allianceid`);
CREATE INDEX `charid` ON `alliance_apply`(`charid`);

