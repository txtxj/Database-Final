create database if not exists TARRS;
use TARRS;

create table if not exists teachers(
    tid varchar(5) not null primary key,
    name varchar(256) not null,
    gender integer check (gender in (1, 2)),
    title integer check (title > 0 and title < 12) 
);

create table if not exists paper(
    paid integer primary key,
    name varchar(256) not null,
    source varchar(256) not null,
    time date,
    type integer check (type > 0 and type < 5),
    level integer check (level > 0 and level < 7)
);

create table if not exists project(
    pid varchar(256) not null primary key,
    name varchar(256) not null,
    source varchar(256) not null,
    type integer check (type > 0 and type < 6),
    funds float,
    start integer,
    end integer
);

create table if not exists course(
    cid varchar(256) not null primary key,
    name varchar(256) not null,
    credit integer,
    type integer check (type in (1, 2))
);