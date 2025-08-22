SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

CREATE TABLE public."User" (
    "Id" uuid NOT NULL,
    "Name" varchar NOT NULL,
    "Email" varchar NOT NULL,
    "Password" varchar NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    "VerificationCode" integer,
    "CodeRegisteredDate" timestamp with time zone,
    "CodeExpirationDate" timestamp with time zone,
    "Token" integer
);

ALTER TABLE public."User" OWNER TO loop;

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."Profile"
    ADD CONSTRAINT "FK_Profile_User" FOREIGN KEY ("UserId") REFERENCES public."User" ("Id") ON DELETE CASCADE;