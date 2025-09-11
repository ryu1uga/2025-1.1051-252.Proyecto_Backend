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

CREATE TABLE public."Address" (
    "Id" uuid NOT NULL,
    "CustomerId" uuid NOT NULL,
    "Line1" varchar NOT NULL,
    "Line2" varchar NOT NULL,
    "City" varchar NOT NULL,
    "Region" varchar NOT NULL,
    "PostalCode" varchar NOT NULL,
    "Country" varchar NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."Address" OWNER TO loop;

CREATE TABLE public."Business" (
    "Id" uuid NOT NULL,
    "Name" varchar NOT NULL,
    "TaxId" varchar NOT NULL,
    "State" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."Business" OWNER TO loop;

CREATE TABLE public."BusinessMember" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "BusinessId" uuid NOT NULL,
    "Role" varchar NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."BusinessMember" OWNER TO loop;

CREATE TABLE public."Cart" (
    "Id" uuid NOT NULL,
    "CustomerId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."Cart" OWNER TO loop;

CREATE TABLE public."CartItem" (
    "Id" uuid NOT NULL,
    "CartId" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "Quantity" integer NOT NULL,
    "Price" float NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."CartItem" OWNER TO loop;

CREATE TABLE public."Category" (
    "Id" uuid NOT NULL,
    "Name" varchar NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."Category" OWNER TO loop;

CREATE TABLE public."Customer" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."Customer" OWNER TO loop;

CREATE TABLE public."Favorite" (
    "Id" uuid NOT NULL,
    "CustomerId" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."Favorite" OWNER TO loop;

CREATE TABLE public."Inventory" (
    "Id" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "Quantity" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."Inventory" OWNER TO loop;

CREATE TABLE public."Order" (
    "Id" uuid NOT NULL,
    "CustomerId" uuid NOT NULL,
    "AddressId" uuid NOT NULL,
    "TotalAmount" float NOT NULL,
    "Status" integer NOT NULL,
    "PaymentStatus" integer NOT NULL,
    "ShippingStatus" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."Order" OWNER TO loop;

CREATE TABLE public."OrderItem" (
    "Id" uuid NOT NULL,
    "OrderId" uuid NOT NULL,
    "BusinessId" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "Quantity" integer NOT NULL,
    "Price" float NOT NULL,
    "PaymentStatus" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."OrderItem" OWNER TO loop;

CREATE TABLE public."Payment" (
    "Id" uuid NOT NULL,
    "OrderId" uuid NOT NULL,
    "Provider" varchar NOT NULL,
    "ProviderRef" varchar NOT NULL,
    "Amount" float NOT NULL,
    "Status" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."Payment" OWNER TO loop;

CREATE TABLE public."Payout" (
    "Id" uuid NOT NULL,
    "BusinessId" uuid NOT NULL,
    "From" timestamp with time zone NOT NULL,
    "To" timestamp with time zone NOT NULL,
    "Amount" float NOT NULL,
    "Status" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."Payout" OWNER TO loop;

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
    "Order" integer NOT NULL,
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

CREATE TABLE public."Review" (
    "Id" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "CustomerId" uuid NOT NULL,
    "Rating" integer NOT NULL,
    "Comment" varchar NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."Review" OWNER TO loop;

CREATE TABLE public."Tag" (
    "Id" uuid NOT NULL,
    "Name" varchar NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."Tag" OWNER TO loop;

CREATE TABLE public."User" (
    "Id" uuid NOT NULL,
    "Name" varchar NOT NULL,
    "Email" varchar NOT NULL,
    "Password" varchar NOT NULL,
    "UserType" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL
);

ALTER TABLE public."User" OWNER TO loop;


ALTER TABLE ONLY public."Address"
    ADD CONSTRAINT "Address_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."Business"
    ADD CONSTRAINT "Business_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."BusinessMember"
    ADD CONSTRAINT "BusinessMember_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."Cart"
    ADD CONSTRAINT "Cart_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."CartItem"
    ADD CONSTRAINT "CartItem_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."Category"
    ADD CONSTRAINT "Category_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."Customer"
    ADD CONSTRAINT "Customer_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."Favorite"
    ADD CONSTRAINT "Favorite_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."Inventory"
    ADD CONSTRAINT "Inventory_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."Order"
    ADD CONSTRAINT "Order_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."OrderItem"
    ADD CONSTRAINT "OrderItem_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."Payment"
    ADD CONSTRAINT "Payment_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."Payout"
    ADD CONSTRAINT "Payout_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."Product"
    ADD CONSTRAINT "Product_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."ProductImage"
    ADD CONSTRAINT "ProductImage_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."ProductTag"
    ADD CONSTRAINT "ProductTag_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."Review"
    ADD CONSTRAINT "Review_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."Tag"
    ADD CONSTRAINT "Tag_pkey" PRIMARY KEY ("Id");

ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_pkey" PRIMARY KEY ("Id");


ALTER TABLE ONLY public."Order"
    ADD CONSTRAINT "FK_Order_Address" FOREIGN KEY ("AddressId") REFERENCES public."Address" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."BusinessMember"
    ADD CONSTRAINT "FK_BusinessMember_Business" FOREIGN KEY ("BusinessId") REFERENCES public."Business" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."OrderItem"
    ADD CONSTRAINT "FK_OrderItem_Business" FOREIGN KEY ("BusinessId") REFERENCES public."Business" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."Payout"
    ADD CONSTRAINT "FK_Payout_Business" FOREIGN KEY ("BusinessId") REFERENCES public."Business" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."Product"
    ADD CONSTRAINT "FK_Product_Business" FOREIGN KEY ("BusinessId") REFERENCES public."Business" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."CartItem"
    ADD CONSTRAINT "FK_CartItem_Cart" FOREIGN KEY ("CartId") REFERENCES public."Cart" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."Product"
    ADD CONSTRAINT "FK_Product_Category" FOREIGN KEY ("CategoryId") REFERENCES public."Category" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."Address"
    ADD CONSTRAINT "FK_Address_Customer" FOREIGN KEY ("CustomerId") REFERENCES public."Customer" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."Favorite"
    ADD CONSTRAINT "FK_Favorite_Customer" FOREIGN KEY ("CustomerId") REFERENCES public."Customer" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."Order"
    ADD CONSTRAINT "FK_Order_Customer" FOREIGN KEY ("CustomerId") REFERENCES public."Customer" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."Review"
    ADD CONSTRAINT "FK_Review_Customer" FOREIGN KEY ("CustomerId") REFERENCES public."Customer" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."OrderItem"
    ADD CONSTRAINT "FK_OrderItem_Order" FOREIGN KEY ("OrderId") REFERENCES public."Order" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."Payment"
    ADD CONSTRAINT "FK_Payment_Order" FOREIGN KEY ("OrderId") REFERENCES public."Order" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."CartItem"
    ADD CONSTRAINT "FK_CartItem_Product" FOREIGN KEY ("ProductId") REFERENCES public."Product" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."Favorite"
    ADD CONSTRAINT "FK_Favorite_Product" FOREIGN KEY ("ProductId") REFERENCES public."Product" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."Inventory"
    ADD CONSTRAINT "FK_Inventory_Product" FOREIGN KEY ("ProductId") REFERENCES public."Product" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."OrderItem"
    ADD CONSTRAINT "FK_OrderItem_Product" FOREIGN KEY ("ProductId") REFERENCES public."Product" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."ProductImage"
    ADD CONSTRAINT "FK_ProductImage_Product" FOREIGN KEY ("ProductId") REFERENCES public."Product" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."ProductTag"
    ADD CONSTRAINT "FK_ProductTag_Product" FOREIGN KEY ("ProductId") REFERENCES public."Product" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."Review"
    ADD CONSTRAINT "FK_Review_Product" FOREIGN KEY ("ProductId") REFERENCES public."Product" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."ProductTag"
    ADD CONSTRAINT "FK_ProductTag_Tag" FOREIGN KEY ("TagId") REFERENCES public."Tag" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."Customer"
    ADD CONSTRAINT "FK_Customer_User" FOREIGN KEY ("UserId") REFERENCES public."User" ("Id") ON DELETE CASCADE;

ALTER TABLE ONLY public."BusinessMember"
    ADD CONSTRAINT "FK_BusinessMember_User" FOREIGN KEY ("UserId") REFERENCES public."User" ("Id") ON DELETE CASCADE;