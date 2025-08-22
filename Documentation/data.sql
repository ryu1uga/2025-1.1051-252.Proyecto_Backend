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


INSERT INTO public."User" ("Id", "Name", "Email", "Password", "UpdatedAt", "VerificationCode", "CodeRegisteredDate", "CodeExpirationDate", "Token") VALUES
('7267d866-4506-4983-8143-1dbe25216700', 'coca', 'guest', '$2a$11$a71ErcamtMJb7nJ8keoCKuuAGNbsxv6oQxHk5NHdxfuH4GkQH/4oa', NOW(), 123456, NOW(), NOW() + INTERVAL '1 day', 10000);