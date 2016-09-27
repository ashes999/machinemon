create table [Messages] (
	Id uniqueidentifier primary key not null default newid(),
	Contents varchar(255) not null,
	Sender varchar(64) not null,
	MessageDateTimeUtc datetime not null
)