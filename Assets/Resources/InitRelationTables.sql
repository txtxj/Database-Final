use TARRS;

create table if not exists publish(
    tid varchar(5) not null,
    paid integer,
    `rank` integer check (`rank` > 0),
    author boolean,
    primary key (tid, paid, `rank`),
    foreign key (tid) references teachers(tid),
    foreign key (paid) references paper(paid)
);

create table if not exists assumption(
    tid varchar(5) not null,
    pid varchar(256) not null,
    `rank` integer check (`rank` > 0),
    funds float,
    primary key (tid, pid, `rank`),
    foreign key (tid) references teachers(tid),
    foreign key (pid) references project(pid)
);

create table if not exists lecture(
    tid varchar(5) not null,
    cid varchar(256) not null,
    year integer,
    term integer check (term in (1, 2, 3)),
    credit integer,
    primary key (tid, cid),
    foreign key (tid) references teachers(tid),
    foreign key (cid) references course(cid)
)