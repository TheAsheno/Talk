create table [user](
	uid varchar(20) primary key,
	username varchar(20) not null,
	password varchar(50) not null,
	birthday date,
	email varchar(20),
	sex varchar(2),
	regdate date,
	lastcheck date,
	lastlog smalldatetime,
	checkdays int,
	avatar image,
	avatarLastScaleX float,
	avatarLastScaleY float,
	lastCenterPointX float,
	lastCenterPointY float,
	lastX float,
	lastY float,
	introduce varchar(50)
)

create table headinfo(
	hid varchar(20) primary key,
	text varchar(100) not null,
	author varchar(20) foreign key references [user](uid),
	anonymous varchar(20),
	examine varchar(10),
	submit_time smalldatetime not null,
	audit_time smalldatetime
)

create table section(
	sid varchar(20) primary key,
	name varchar(20),
	master varchar(20) foreign key references [user](uid),
	statement varchar(100),
	clickcount int,
	postcount int
)

create table post(
	pid varchar(20) primary key,
	section varchar(20) not null foreign key references section(sid) on delete cascade,
	author varchar(20) not null foreign key references [user](uid) on delete cascade,
	title varchar(50) not null,
	content text not null,
	time smalldatetime not null,
	clickcount int,
	replycount int,
	lastclick smalldatetime,
	lastreply smalldatetime
)

create table reply(
	rid varchar(20) primary key,
	post varchar(20) not null foreign key references post(pid) on delete cascade,
	author varchar(20) not null foreign key references [user](uid),
	content text not null,
	time smalldatetime not null
)

create table content(
	pid varchar(20) not null foreign key references post(pid),
	part varchar(20) not null,
)

create table collect(
	userid varchar(20) foreign key references [user](uid),
	headid varchar(20) not null foreign key references headinfo(hid) on delete cascade,
	primary key(userid, headid)
)