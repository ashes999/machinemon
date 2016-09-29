create table [Message] (
	Id uniqueidentifier primary key not null default newid(),
	Metric varchar(255) not null,
	Contents varchar(255) not null,
	Sender varchar(64) not null,
	MessageDateTimeUtc datetime not null
)