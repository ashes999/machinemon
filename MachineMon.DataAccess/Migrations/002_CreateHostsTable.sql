create table Host (
	-- Generated on the client and associated to the hostname on first setup/message
	id uniqueidentifier primary key not null,
	fqdn varchar(255) not null,
	friendlyName varchar(128) not null,
	userName varchar(255) not null,
	password varchar(max) not null
)