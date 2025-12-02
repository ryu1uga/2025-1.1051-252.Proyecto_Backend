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

CREATE TABLE public."Category" (
    "Id" uuid NOT NULL,
    "Name" varchar NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."Category" OWNER TO loop;

CREATE TABLE public."Inventory" (
    "Id" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "Quantity" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."Inventory" OWNER TO loop;

CREATE TABLE public."Product" (
    "Id" uuid NOT NULL,
    "BusinessId" uuid NOT NULL,
    "CategoryId" uuid NOT NULL,
    "Name" varchar NOT NULL,
    "Description" varchar NOT NULL,
    "Status" integer NOT NULL,
    "Price" float NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."Product" OWNER TO loop;

CREATE TABLE public."ProductImage" (
    "Id" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "Url" varchar NOT NULL,
    "ImageOrder" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."ProductImage" OWNER TO loop;

CREATE TABLE public."ProductTag" (
    "Id" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "TagId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."ProductTag" OWNER TO loop;

CREATE TABLE public."Tag" (
    "Id" uuid NOT NULL,
    "Name" varchar NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."Tag" OWNER TO loop;


ALTER TABLE ONLY public."Category"
    ADD CONSTRAINT "Category_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."Inventory"
    ADD CONSTRAINT "Inventory_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."Product"
    ADD CONSTRAINT "Product_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."ProductImage"
    ADD CONSTRAINT "ProductImage_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."ProductTag"
    ADD CONSTRAINT "ProductTag_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."Tag"
    ADD CONSTRAINT "Tag_pkey" PRIMARY KEY ("Id");


ALTER TABLE ONLY public."Product"
    ADD CONSTRAINT "FK_Product_Category" FOREIGN KEY ("CategoryId") REFERENCES public."Category" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."Inventory"
    ADD CONSTRAINT "FK_Inventory_Product" FOREIGN KEY ("ProductId") REFERENCES public."Product" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."ProductImage"
    ADD CONSTRAINT "FK_ProductImage_Product" FOREIGN KEY ("ProductId") REFERENCES public."Product" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."ProductTag"
    ADD CONSTRAINT "FK_ProductTag_Product" FOREIGN KEY ("ProductId") REFERENCES public."Product" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."ProductTag"
    ADD CONSTRAINT "FK_ProductTag_Tag" FOREIGN KEY ("TagId") REFERENCES public."Tag" ("Id") ON DELETE CASCADE;