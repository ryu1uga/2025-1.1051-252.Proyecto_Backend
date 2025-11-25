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

-- -----------------------------------------------------
-- 1. Categorías (Category)
-- -----------------------------------------------------

INSERT INTO public."Category" ("Id", "Name", "CreatedAt", "UpdatedAt") VALUES
('f0e1c2d3-4a5b-6c7d-8e9f-0a1b2c3d4e5f', 'Tops y Polos', NOW(), NOW()),
('a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d', 'Pantalones y Jeans', NOW(), NOW()),
('d2c3e4f5-a6b7-8c9d-0e1f-2a3b4c5d6e7f', 'Calzado', NOW(), NOW());

-- -----------------------------------------------------
-- 2. Productos (Product)
-- Usa el BusinessId de prueba: 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c'
-- -----------------------------------------------------

INSERT INTO public."Product" ("Id", "BusinessId", "CategoryId", "Name", "Description", "Status", "Price", "CreatedAt", "UpdatedAt") VALUES
('6d5c4b3a-2a1b-3c4d-5e6f-7a8b9c0d1e2f', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', 'f0e1c2d3-4a5b-6c7d-8e9f-0a1b2c3d4e5f', 'Polo Clásico Algodón Orgánico (Blanco)', 'Polo 100% algodón orgánico, corte regular fit.', 1, 49.90, NOW(), NOW()),
('7e8f9a0b-1c2d-3e4f-5a6b-7c8d9e0f1a2b', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', 'a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d', 'Jeans Slim Fit Elásticos (Denim)', 'Jeans de corte ajustado, cómodos para el uso diario.', 1, 149.90, NOW(), NOW());

-- -----------------------------------------------------
-- 3. Inventario (Inventory)
-- -----------------------------------------------------

INSERT INTO public."Inventory" ("Id", "ProductId", "Quantity", "CreatedAt", "UpdatedAt") VALUES
('c1a2b3d4-5e6f-7a8b-9c0d-1e2f3a4b5c6d', '6d5c4b3a-2a1b-3c4d-5e6f-7a8b9c0d1e2f', 200, NOW(), NOW()), -- 200 Polos
('d5e6f7a8-b9c0-d1e2-f3a4-b5c6d7e8f9a0', '7e8f9a0b-1c2d-3e4f-5a6b-7c8d9e0f1a2b', 85, NOW(), NOW()); -- 85 Jeans

-- -----------------------------------------------------
-- 4. Tags (Tag)
-- -----------------------------------------------------

INSERT INTO public."Tag" ("Id", "Name", "CreatedAt", "UpdatedAt") VALUES
('1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d', 'Tendencia', NOW(), NOW()),
('a0b1c2d3-e4f5-6a7b-8c9d-0e1f2a3b4c5d', 'Eco-Friendly', NOW(), NOW());

-- -----------------------------------------------------
-- 5. Relación Producto-Tag (ProductTag)
-- -----------------------------------------------------

INSERT INTO public."ProductTag" ("Id", "ProductId", "TagId", "CreatedAt", "UpdatedAt") VALUES
(gen_random_uuid(), '6d5c4b3a-2a1b-3c4d-5e6f-7a8b9c0d1e2f', 'a0b1c2d3-e4f5-6a7b-8c9d-0e1f2a3b4c5d', NOW(), NOW()); -- Polo es 'Eco-Friendly'