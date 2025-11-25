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


-- data.sql para loop_db (Order-User Service)

-- Reinicio de comandos estándar
SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';

-- -----------------------------------------------------
-- 1. Usuarios (User)
-- -----------------------------------------------------

-- Usuario Admin (UserType = 1)
INSERT INTO public."User" ("Id", "Name", "Email", "UserType", "CreatedAt", "UpdatedAt") VALUES
('7267d866-4506-4983-8143-1dbe25216700', 'Admin', 'admin@test.com', 1, NOW(), NOW());

-- Usuario Ryuichi (UserType = 0, Cliente)
INSERT INTO public."User" ("Id", "Name", "Email", "UserType", "CreatedAt", "UpdatedAt") VALUES
('4a8f9d0c-2e11-4b71-b8f9-4674a2a1b94d', 'Ryuichi Oshiro', '20211953@aloe.ulima.edu.pe', 0, NOW(), NOW());

-- -----------------------------------------------------
-- 2. Clientes (Customer)
-- -----------------------------------------------------

-- Cliente asociado a Ryuichi
INSERT INTO public."Customer" ("Id", "UserId", "CreatedAt", "UpdatedAt") VALUES
('e1d4c2b9-7a32-4e00-8f6a-4c28f731e84b', '4a8f9d0c-2e11-4b71-b8f9-4674a2a1b94d', NOW(), NOW());

-- -----------------------------------------------------
-- 3. Negocios (Business)
-- -----------------------------------------------------

-- Negocio de prueba (Tienda Test)
INSERT INTO public."Business" ("Id", "Name", "TaxId", "State", "CreatedAt", "UpdatedAt") VALUES
('b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', 'Tienda Test de Ryu', '123456789', 1, NOW(), NOW());

-- -----------------------------------------------------
-- 4. Miembros del Negocio (BusinessMember)
-- Ryuichi como miembro de su tienda
-- -----------------------------------------------------

INSERT INTO public."BusinessMember" ("Id", "UserId", "BusinessId", "Role", "CreatedAt", "UpdatedAt") VALUES
('d2c6e9f8-8a75-4c62-8e1d-34a9b6c5f789', '4a8f9d0c-2e11-4b71-b8f9-4674a2a1b94d', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', 'Owner', NOW(), NOW());

-- -----------------------------------------------------
-- 5. Dirección (Address)
-- -----------------------------------------------------

-- Dirección de prueba para Ryuichi
INSERT INTO public."Address" ("Id", "CustomerId", "Line1", "Line2", "City", "Region", "PostalCode", "Country", "CreatedAt", "UpdatedAt") VALUES
('7b38a4e5-9c21-4d0f-a2e4-98c7b6d5a1f2', 'e1d4c2b9-7a32-4e00-8f6a-4c28f731e84b', 'Av. Test 123', 'Apt 4B', 'Lima', 'Lima', 'L15001', 'Perú', NOW(), NOW());