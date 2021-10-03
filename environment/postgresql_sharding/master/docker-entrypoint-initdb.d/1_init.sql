CREATE EXTENSION postgres_fdw;
CREATE SERVER books_0_server
    FOREIGN DATA WRAPPER postgres_fdw
    OPTIONS( host 'postgresql-shard0', port '5432', dbname 'postgres' );

CREATE USER MAPPING FOR postgres
    SERVER books_0_server
    OPTIONS (user 'postgres', password 'my_password');

CREATE SERVER books_1_server
    FOREIGN DATA WRAPPER postgres_fdw
    OPTIONS( host 'postgresql-shard1', port '5432', dbname 'postgres' );

CREATE USER MAPPING FOR postgres
    SERVER books_1_server
    OPTIONS (user 'postgres', password 'my_password');

CREATE FOREIGN TABLE books_0 (
    id bigint not null,
    category_id  int not null,
    author character varying not null,
    title character varying not null,
    year int not null
) SERVER books_0_server OPTIONS (schema_name 'public', table_name 'books');


CREATE FOREIGN TABLE books_1 (
    id bigint not null,
    category_id  int not null,
    author character varying not null,
    title character varying not null,
    year int not null
) SERVER books_1_server OPTIONS (schema_name 'public', table_name 'books');


CREATE VIEW books AS
  SELECT * FROM books_0
       UNION ALL
  SELECT * FROM books_1;


CREATE RULE books_insert AS ON INSERT TO books DO INSTEAD NOTHING;
CREATE RULE books_update AS ON UPDATE TO books DO INSTEAD NOTHING;
CREATE RULE books_delete AS ON DELETE TO books DO INSTEAD NOTHING;

CREATE RULE books_insert_to_0 AS ON INSERT TO books
    WHERE ( category_id % 2 = 0 ) DO INSTEAD INSERT INTO books_0 VALUES (NEW.*);

CREATE RULE books_insert_to_1 AS ON INSERT TO books
    WHERE ( category_id % 2 = 1 ) DO INSTEAD INSERT INTO books_1 VALUES (NEW.*);
