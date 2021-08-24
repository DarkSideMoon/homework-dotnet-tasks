CREATE DATABASE postgres
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'en_US.utf8'
    LC_CTYPE = 'en_US.utf8'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;


-- Table: public.book

-- DROP TABLE public.book;

CREATE TABLE IF NOT EXISTS public.book
(
    "AuthorFirstName" character varying(500) COLLATE pg_catalog."default",
    "AuthorLastName" character varying(500) COLLATE pg_catalog."default",
    "BookType" character varying(100) COLLATE pg_catalog."default",
    "CountOfPages" integer,
    "ISBN" character varying(100) COLLATE pg_catalog."default",
    "Id" uuid NOT NULL,
    "Language" character varying(100) COLLATE pg_catalog."default",
    "Price" double precision,
    "Title" character varying(500) COLLATE pg_catalog."default",
    "AuthorEmail" character varying(500) COLLATE pg_catalog."default",
    CONSTRAINT book_pkey PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE public.book
    OWNER to postgres;