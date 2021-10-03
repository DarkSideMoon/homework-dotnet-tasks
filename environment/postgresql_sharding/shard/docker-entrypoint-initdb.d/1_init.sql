CREATE EXTENSION postgres_fdw;

CREATE SERVER master_server
    FOREIGN DATA WRAPPER postgres_fdw
    OPTIONS( host 'postgresql-master', port '5432', dbname 'postgres' );

CREATE USER MAPPING FOR postgres
    SERVER master_server
    OPTIONS (user 'postgres', password 'my_password');

CREATE TABLE books (
   id bigint not null,
   category_id  int not null,
   author character varying not null,
   title character varying not null,
   year int not null
);