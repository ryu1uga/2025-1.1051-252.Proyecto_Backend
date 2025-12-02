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

INSERT INTO public."Category" ("Id", "Name", "CreatedAt", "UpdatedAt") VALUES
('f0e1c2d3-4a5b-6c7d-8e9f-0a1b2c3d4e5f', 'Tops y Polos', NOW(), NOW()),
('a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d', 'Pantalones y Jeans', NOW(), NOW()),
('d2c3e4f5-a6b7-8c9d-0e1f-2a3b4c5d6e7f', 'Calzado', NOW(), NOW()),
('b435fa52-9426-4a41-b77f-9b29b52a6eb8', 'Accesorios', NOW(), NOW()),
('cf5b739b-f390-4c63-9db2-60ad06c938d0', 'Ropa Deportiva', NOW(), NOW()),
('4f779e48-a89e-4c9b-a820-1d1e8fb6e9f6', 'Abrigos y Casacas', NOW(), NOW());

INSERT INTO public."Tag" ("Id", "Name", "CreatedAt", "UpdatedAt") VALUES
('1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d', 'Tendencia', NOW(), NOW()),
('a0b1c2d3-e4f5-6a7b-8c9d-0e1f2a3b4c5d', 'Eco-Friendly', NOW(), NOW()),
('e1e2e3e4-e5e6-e7e8-e9e0-e1e2e3e4e5e6', 'Verano 2025', NOW(), NOW()),
('f1f2f3f4-f5f6-f7f8-f9f0-f1f2f3f4f5f6', 'Oferta', NOW(), NOW()),
('f1f2f3f4-f5f6-f7f8-f9f0-f1f2f3f4f5f7', 'Premium', NOW(), NOW()),
('f1f2f3f4-f5f6-f7f8-f9f0-f1f2f3f4f5f8', 'Impermeable', NOW(), NOW());

INSERT INTO public."Product" ("Id", "BusinessId", "CategoryId", "Name", "Description", "Status", "Price", "CreatedAt", "UpdatedAt") VALUES
('6d5c4b3a-2a1b-3c4d-5e6f-7a8b9c0d1e2f', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', 'f0e1c2d3-4a5b-6c7d-8e9f-0a1b2c3d4e5f', 'Polo Clásico Algodón Orgánico (Blanco)', 'Polo 100% algodón orgánico, corte regular fit.', 1, 49.90, NOW(), NOW()),
('7e8f9a0b-1c2d-3e4f-5a6b-7c8d9e0f1a2b', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', 'a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d', 'Jeans Slim Fit Elásticos (Denim)', 'Jeans de corte ajustado, cómodos para el uso diario.', 1, 149.90, NOW(), NOW()),

('2c9da8bb-1f84-4c0a-9e64-d2dfe763a962', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', 'cf5b739b-f390-4c63-9db2-60ad06c938d0', 'Leggings Yoga Fit', 'Leggings de alta compresión ideales para yoga y pilates.', 1, 89.90, NOW(), NOW()),
('3a4b8e5b-51b5-40eb-9af2-7b71a45a7aa0', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', 'cf5b739b-f390-4c63-9db2-60ad06c938d0', 'Camiseta Running Dry-Fit', 'Camiseta transpirable para correr, secado rápido.', 1, 55.00, NOW(), NOW()),
('8c2e5ce7-8f38-4f52-bb4c-1f2a5a54842c', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', 'cf5b739b-f390-4c63-9db2-60ad06c938d0', 'Short Deportivo Básico', 'Short ligero con bolsillos laterales con cierre.', 1, 45.50, NOW(), NOW()),

('bbd30b63-f67b-4f43-8f8d-0b10a0cda8a4', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', 'b435fa52-9426-4a41-b77f-9b29b52a6eb8', 'Gorra Negra Urbana', 'Gorra ajustable estilo snapback, logo bordado.', 1, 39.90, NOW(), NOW()),
('c1b55d82-e48f-4a21-a939-2ddf323dce43', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', 'b435fa52-9426-4a41-b77f-9b29b52a6eb8', 'Mochila Canvas Vintage', 'Mochila resistente con compartimento para laptop.', 1, 120.00, NOW(), NOW()),
('e06f9783-f3b2-4d5c-a2b0-2e1dfae61563', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', 'b435fa52-9426-4a41-b77f-9b29b52a6eb8', 'Set de Calcetines (3 pares)', 'Calcetines de algodón peinado, colores variados.', 1, 25.00, NOW(), NOW()),

('feb16b1d-8616-437c-bf49-133f63e5b88c', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', '4f779e48-a89e-4c9b-a820-1d1e8fb6e9f6', 'Casaca Cortaviento', 'Casaca ligera impermeable, ideal para lluvia ligera.', 1, 110.00, NOW(), NOW()),
('c89e514d-1b68-4c65-acbc-f78f3280e0fc', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', '4f779e48-a89e-4c9b-a820-1d1e8fb6e9f6', 'Abrigo de Lana Elegante', 'Abrigo largo color camel, botones frontales.', 1, 299.90, NOW(), NOW()),

('4a09cc25-5acd-4ef2-8805-015fe54167ab', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', 'f0e1c2d3-4a5b-6c7d-8e9f-0a1b2c3d4e5f', 'Blusa de Seda Estampada', 'Blusa formal con estampado floral sutil.', 1, 85.00, NOW(), NOW()),
('29c8cfe1-90d4-4d32-8dbe-074ee3ad9f27', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', 'f0e1c2d3-4a5b-6c7d-8e9f-0a1b2c3d4e5f', 'Polo Oversize Gráfico', 'Polo de corte ancho con diseño urbano.', 1, 59.90, NOW(), NOW()),
('85ef0e4d-2c33-46f4-a8c2-4d91fe43cd77', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', 'f0e1c2d3-4a5b-6c7d-8e9f-0a1b2c3d4e5f', 'Camisa Oxford Celeste', 'Camisa clásica para oficina.', 1, 95.00, NOW(), NOW()),

('1020aa86-7d2e-4bc0-b355-b9c10cf8091f', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', 'a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d', 'Pantalón Chino Beige', 'Pantalón casual de gabardina.', 1, 109.00, NOW(), NOW()),
('c71f9ea4-c287-4095-be96-e0a4374e1c0e', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', 'a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d', 'Jogger Cargo Negro', 'Jogger con bolsillos múltiples.', 1, 99.50, NOW(), NOW()),

('b4f87190-a405-4e32-943a-cf4a7eac5dd6', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', 'd2c3e4f5-a6b7-8c9d-0e1f-2a3b4c5d6e7f', 'Zapatillas Blancas Clásicas', 'Zapatillas de cuero sintético.', 1, 180.00, NOW(), NOW()),
('7f987888-1c46-47fb-9fba-d3c48437ab26', 'b33c7f1a-5e4d-4c31-89a1-02a8e8f9a03c', 'd2c3e4f5-a6b7-8c9d-0e1f-2a3b4c5d6e7f', 'Botines de Cuero Marrón', 'Botines casuales resistentes al agua.', 1, 250.00, NOW(), NOW());

INSERT INTO public."Inventory" ("Id", "ProductId", "Quantity", "CreatedAt", "UpdatedAt") VALUES
(gen_random_uuid(), '6d5c4b3a-2a1b-3c4d-5e6f-7a8b9c0d1e2f', 200, NOW(), NOW()),
(gen_random_uuid(), '7e8f9a0b-1c2d-3e4f-5a6b-7c8d9e0f1a2b', 85, NOW(), NOW()),
(gen_random_uuid(), '2c9da8bb-1f84-4c0a-9e64-d2dfe763a962', 50, NOW(), NOW()),
(gen_random_uuid(), '3a4b8e5b-51b5-40eb-9af2-7b71a45a7aa0', 120, NOW(), NOW()),
(gen_random_uuid(), '8c2e5ce7-8f38-4f52-bb4c-1f2a5a54842c', 80, NOW(), NOW()),
(gen_random_uuid(), 'bbd30b63-f67b-4f43-8f8d-0b10a0cda8a4', 200, NOW(), NOW()),
(gen_random_uuid(), 'c1b55d82-e48f-4a21-a939-2ddf323dce43', 35, NOW(), NOW()),
(gen_random_uuid(), 'e06f9783-f3b2-4d5c-a2b0-2e1dfae61563', 500, NOW(), NOW()),
(gen_random_uuid(), 'feb16b1d-8616-437c-bf49-133f63e5b88c', 40, NOW(), NOW()),
(gen_random_uuid(), 'c89e514d-1b68-4c65-acbc-f78f3280e0fc', 15, NOW(), NOW()),
(gen_random_uuid(), '4a09cc25-5acd-4ef2-8805-015fe54167ab', 60, NOW(), NOW()),
(gen_random_uuid(), '29c8cfe1-90d4-4d32-8dbe-074ee3ad9f27', 95, NOW(), NOW()),
(gen_random_uuid(), '85ef0e4d-2c33-46f4-a8c2-4d91fe43cd77', 70, NOW(), NOW()),
(gen_random_uuid(), '1020aa86-7d2e-4bc0-b355-b9c10cf8091f', 110, NOW(), NOW()),
(gen_random_uuid(), 'c71f9ea4-c287-4095-be96-e0a4374e1c0e', 45, NOW(), NOW()),
(gen_random_uuid(), 'b4f87190-a405-4e32-943a-cf4a7eac5dd6', 90, NOW(), NOW()),
(gen_random_uuid(), '7f987888-1c46-47fb-9fba-d3c48437ab26', 25, NOW(), NOW());

INSERT INTO public."ProductTag" ("Id", "ProductId", "TagId", "CreatedAt", "UpdatedAt") VALUES
(gen_random_uuid(), '6d5c4b3a-2a1b-3c4d-5e6f-7a8b9c0d1e2f', 'a0b1c2d3-e4f5-6a7b-8c9d-0e1f2a3b4c5d', NOW(), NOW()),
(gen_random_uuid(), '2c9da8bb-1f84-4c0a-9e64-d2dfe763a962', 'a0b1c2d3-e4f5-6a7b-8c9d-0e1f2a3b4c5d', NOW(), NOW()),
(gen_random_uuid(), '3a4b8e5b-51b5-40eb-9af2-7b71a45a7aa0', 'f1f2f3f4-f5f6-f7f8-f9f0-f1f2f3f4f5f6', NOW(), NOW()),
(gen_random_uuid(), '8c2e5ce7-8f38-4f52-bb4c-1f2a5a54842c', 'e1e2e3e4-e5e6-e7e8-e9e0-e1e2e3e4e5e6', NOW(), NOW()),
(gen_random_uuid(), 'bbd30b63-f67b-4f43-8f8d-0b10a0cda8a4', '1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d', NOW(), NOW()),
(gen_random_uuid(), 'feb16b1d-8616-437c-bf49-133f63e5b88c', 'f1f2f3f4-f5f6-f7f8-f9f0-f1f2f3f4f5f8', NOW(), NOW()),
(gen_random_uuid(), 'c89e514d-1b68-4c65-acbc-f78f3280e0fc', 'f1f2f3f4-f5f6-f7f8-f9f0-f1f2f3f4f5f7', NOW(), NOW()),
(gen_random_uuid(), '29c8cfe1-90d4-4d32-8dbe-074ee3ad9f27', '1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d', NOW(), NOW()),
(gen_random_uuid(), '7f987888-1c46-47fb-9fba-d3c48437ab26', 'f1f2f3f4-f5f6-f7f8-f9f0-f1f2f3f4f5f7', NOW(), NOW());

-- Polo Clásico Algodón Orgánico (Blanco)
INSERT INTO public."ProductImage" VALUES
(gen_random_uuid(), '6d5c4b3a-2a1b-3c4d-5e6f-7a8b9c0d1e2f', 'https://ecological.eco/wp-content/uploads/2021/03/polo-algodon-organico-hombre-blanco.jpg', 1, NOW(), NOW()),
(gen_random_uuid(), '6d5c4b3a-2a1b-3c4d-5e6f-7a8b9c0d1e2f', 'https://ezzetacompany.com/wp-content/uploads/2025/06/Polo-Blanco-Clasico-Hombre.jpeg', 2, NOW(), NOW()),
(gen_random_uuid(), '6d5c4b3a-2a1b-3c4d-5e6f-7a8b9c0d1e2f', 'https://www.multiuniformes.com/productos/imagenes/img_167067_86df8ab8b1b1e51b33efedff7d944485_20.jpg', 3, NOW(), NOW());

-- Jeans Slim Fit Elásticos (Denim)
INSERT INTO public."ProductImage" VALUES
(gen_random_uuid(), '7e8f9a0b-1c2d-3e4f-5a6b-7c8d9e0f1a2b', 'https://mauiandsons.com.pe/media/catalog/product/cache/df6235d5d5f05cc092911bf968f32080/5/n/5n102-mt25lisodenim-1.jpg', 1, NOW(), NOW()),
(gen_random_uuid(), '7e8f9a0b-1c2d-3e4f-5a6b-7c8d9e0f1a2b', 'https://m.media-amazon.com/images/I/61hBY4YocEL._AC_UY1000_.jpg', 2, NOW(), NOW()),
(gen_random_uuid(), '7e8f9a0b-1c2d-3e4f-5a6b-7c8d9e0f1a2b', 'https://media.falabella.com/falabellaPE/126384499_1/w=800,h=800,fit=pad', 3, NOW(), NOW());

-- Leggings Yoga Fit
INSERT INTO public."ProductImage" VALUES
(gen_random_uuid(), '2c9da8bb-1f84-4c0a-9e64-d2dfe763a962', 'https://m.media-amazon.com/images/I/416taRDRtgL._AC_UF894,1000_QL80_.jpg', 1, NOW(), NOW()),
(gen_random_uuid(), '2c9da8bb-1f84-4c0a-9e64-d2dfe763a962', 'https://files.cdn.printful.com/o/upload/variant-image-jpg/63/6324ff58f876471b409aa0dfe330db9c_l?v=60e6a9699964794ac247264dad618168', 2, NOW(), NOW()),
(gen_random_uuid(), '2c9da8bb-1f84-4c0a-9e64-d2dfe763a962', 'https://beyondyoga.com/cdn/shop/files/SD3243_dark-spruce-heather_02581.jpg?v=1759864543', 3, NOW(), NOW());

-- Camiseta Running Dry-Fit
INSERT INTO public."ProductImage" VALUES
(gen_random_uuid(), '3a4b8e5b-51b5-40eb-9af2-7b71a45a7aa0', 'https://sparta.cl/media/catalog/product/p/o/polera-running-hombre-nike-dri-fit-miler-negra-frontal.png', 1, NOW(), NOW()),
(gen_random_uuid(), '3a4b8e5b-51b5-40eb-9af2-7b71a45a7aa0', 'https://photos.enjoei.com.br/camiseta-nike-running-dri-fit-tamanho-m-original/1200xN/czM6Ly9waG90b3MuZW5qb2VpLmNvbS5ici9wcm9kdWN0cy85MDUxNTQvMjQ1YTE4Mzg4OWZlZjgzZjExM2JlY2ZiYTgyNjY4YTYuanBn', 2, NOW(), NOW()),
(gen_random_uuid(), '3a4b8e5b-51b5-40eb-9af2-7b71a45a7aa0', 'https://d3fvqmu2193zmz.cloudfront.net/items_2/uid_commerces.1/uid_items_2.FDM7AON2SZ1W/500x500/672BF8494B531-Camiseta-Futbol-Hombre-Dri-Fit-Academy23-Br.webp', 3, NOW(), NOW());

-- Short Deportivo Básico
INSERT INTO public."ProductImage" VALUES
(gen_random_uuid(), '8c2e5ce7-8f38-4f52-bb4c-1f2a5a54842c', 'https://assets.adidas.com/images/w_600,f_auto,q_auto/cd428faafcc54b0a8ea4af4f0155af9b_9366/Shorts_de_Entrenamiento_Train_Essentials_Tejidos_Negro_IC6976_01_laydown.jpg', 1, NOW(), NOW()),
(gen_random_uuid(), '8c2e5ce7-8f38-4f52-bb4c-1f2a5a54842c', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSwgPQVMEVzMNyeil_ZYQ23ob3piudSho9XDQ&s', 2, NOW(), NOW()),
(gen_random_uuid(), '8c2e5ce7-8f38-4f52-bb4c-1f2a5a54842c', 'https://www.ultralon.com.pe/cdn/shop/files/WM-6563W-1_1_700x700.jpg?v=1755639745', 3, NOW(), NOW());

-- Gorra Negra Urbana
INSERT INTO public."ProductImage" VALUES
(gen_random_uuid(), 'bbd30b63-f67b-4f43-8f8d-0b10a0cda8a4', 'https://elenexperu.com/wp-content/uploads/2023/04/Black-Up-2.jpg', 1, NOW(), NOW()),
(gen_random_uuid(), 'bbd30b63-f67b-4f43-8f8d-0b10a0cda8a4', 'https://d3fvqmu2193zmz.cloudfront.net/items_2/uid_commerces.1/uid_items_2.FDM7TU0C3K6Y/1500x1500/6744B7655AF98-Gorra-Lifestyle-Hombre-Branded-Snapback.webp', 2, NOW(), NOW()),
(gen_random_uuid(), 'bbd30b63-f67b-4f43-8f8d-0b10a0cda8a4', 'https://media.falabella.com/falabellaPE/20519332_1/w=800,h=800,fit=pad', 3, NOW(), NOW());

-- Mochila Canvas Vintage
INSERT INTO public."ProductImage" VALUES
(gen_random_uuid(), 'c1b55d82-e48f-4a21-a939-2ddf323dce43', 'https://m.media-amazon.com/images/I/61iCMt5camL._AC_SL1000_.jpg', 1, NOW(), NOW()),
(gen_random_uuid(), 'c1b55d82-e48f-4a21-a939-2ddf323dce43', 'https://m.media-amazon.com/images/I/61xm4n+0ojL.jpg', 2, NOW(), NOW()),
(gen_random_uuid(), 'c1b55d82-e48f-4a21-a939-2ddf323dce43', 'https://m.media-amazon.com/images/I/61HZ0OIALBL._AC_SL1500_.jpg', 3, NOW(), NOW());

-- Set de Calcetines (3 pares)
INSERT INTO public."ProductImage" VALUES
(gen_random_uuid(), 'e06f9783-f3b2-4d5c-a2b0-2e1dfae61563', 'https://assets.adidas.com/images/w_600,f_auto,q_auto/19d7b50f686a4a0c8fd8aefc00f86a64_9366/Medias_A_Media_Pantorrilla_Acolchadas_3_Tiras_3_Pares_Plomo_IC1323_03_standard.jpg', 1, NOW(), NOW()),
(gen_random_uuid(), 'e06f9783-f3b2-4d5c-a2b0-2e1dfae61563', 'https://assets.adidas.com/images/w_600,f_auto,q_auto/1b7c33fb1cfe499d8852afc40090cf17_9366/Medias_Tobilleras_3_Pares_Blanco_IJ0733_01_01_00_standard.jpg', 2, NOW(), NOW()),
(gen_random_uuid(), 'e06f9783-f3b2-4d5c-a2b0-2e1dfae61563', 'https://assets.adidas.com/images/w_600,f_auto,q_auto/a7ca5a4933494b33b036ad9000156cae_9366/Calcetines_Soquetes_3_Pares_Plomo_HF4716_01_standard.jpg', 3, NOW(), NOW());

-- Casaca Cortaviento
INSERT INTO public."ProductImage" VALUES
(gen_random_uuid(), 'feb16b1d-8616-437c-bf49-133f63e5b88c', 'https://4nomadsperu.com/wp-content/uploads/2025/01/Casaca-impermeable-y-cortaviento-Origin-2-Mac-in-a-Sac-09.jpg', 1, NOW(), NOW()),
(gen_random_uuid(), 'feb16b1d-8616-437c-bf49-133f63e5b88c', 'https://mamamountainperu.com/wp-content/uploads/Casaca-cortaviento-impermeable-Quechua-NH550.4.webp', 2, NOW(), NOW()),
(gen_random_uuid(), 'feb16b1d-8616-437c-bf49-133f63e5b88c', 'https://thecult.vtexassets.com/arquivos/ids/194059/CS024-24%20NEGRO%20-1-.jpg.jpg?v=638754195900900000', 3, NOW(), NOW());

-- Abrigo de Lana Elegante
INSERT INTO public."ProductImage" VALUES
(gen_random_uuid(), 'c89e514d-1b68-4c65-acbc-f78f3280e0fc', 'https://m.media-amazon.com/images/I/61YES-+opWL._AC_UY1000_.jpg', 1, NOW(), NOW()),
(gen_random_uuid(), 'c89e514d-1b68-4c65-acbc-f78f3280e0fc', 'https://media.falabella.com/falabellaPE/145264753_01/w=800,h=800,fit=pad', 2, NOW(), NOW()),
(gen_random_uuid(), 'c89e514d-1b68-4c65-acbc-f78f3280e0fc', 'https://media.falabella.com/falabellaPE/145264038_01/w=800,h=800,fit=pad', 3, NOW(), NOW());

-- Blusa de Seda Estampada
INSERT INTO public."ProductImage" VALUES
(gen_random_uuid(), '4a09cc25-5acd-4ef2-8805-015fe54167ab', 'https://m.media-amazon.com/images/I/61bW53NeLXL._AC_UF894,1000_QL80_.jpg', 1, NOW(), NOW()),
(gen_random_uuid(), '4a09cc25-5acd-4ef2-8805-015fe54167ab', 'https://unusualexclusive.co/cdn/shop/files/Blusa911611.jpg?v=1740420226', 2, NOW(), NOW()),
(gen_random_uuid(), '4a09cc25-5acd-4ef2-8805-015fe54167ab', 'https://nadapersonal.pe/wp-content/uploads/2023/04/IMG_8336.jpg', 3, NOW(), NOW());

-- Polo Oversize Gráfico
INSERT INTO public."ProductImage" VALUES
(gen_random_uuid(), '29c8cfe1-90d4-4d32-8dbe-074ee3ad9f27', 'https://overtake.com.pe/wp-content/uploads/2023/06/Negro-delante.png', 1, NOW(), NOW()),
(gen_random_uuid(), '29c8cfe1-90d4-4d32-8dbe-074ee3ad9f27', 'https://home.ripley.com.pe/Attachment/WOP_5/2015272661697/2015272661697-3.jpg', 2, NOW(), NOW()),
(gen_random_uuid(), '29c8cfe1-90d4-4d32-8dbe-074ee3ad9f27', 'https://www.timg.pe/cdn/shop/files/Polo-Oversize-Blanco-Rd-20-1-Imprimible-DTF-DTG-Bo-3_168x230.webp?v=1718713797', 3, NOW(), NOW());

-- Camisa Oxford Celeste
INSERT INTO public."ProductImage" VALUES
(gen_random_uuid(), '85ef0e4d-2c33-46f4-a8c2-4d91fe43cd77', 'https://safetystoreperu.vtexassets.com/arquivos/ids/159252/4505002301_1.jpg?v=638247001925830000', 1, NOW(), NOW()),
(gen_random_uuid(), '85ef0e4d-2c33-46f4-a8c2-4d91fe43cd77', 'https://b2btreckpe.vtexassets.com/arquivos/ids/171866-800-auto?v=638570156511100000&width=800&height=auto&aspect=true', 2, NOW(), NOW()),
(gen_random_uuid(), '85ef0e4d-2c33-46f4-a8c2-4d91fe43cd77', 'https://www.pionier.pe/assets/upload/producto/5040516733_1.jpg', 3, NOW(), NOW());

-- Pantalón Chino Beige
INSERT INTO public."ProductImage" VALUES
(gen_random_uuid(), '1020aa86-7d2e-4bc0-b355-b9c10cf8091f', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRQnAD-7YzJhVVk1rFf0cxkhCvof51Lc0spnw&s', 1, NOW(), NOW()),
(gen_random_uuid(), '1020aa86-7d2e-4bc0-b355-b9c10cf8091f', 'https://i.pinimg.com/736x/b6/4e/c2/b64ec24d02cf882d688f8d6f30ec7782.jpg', 2, NOW(), NOW()),
(gen_random_uuid(), '1020aa86-7d2e-4bc0-b355-b9c10cf8091f', 'https://static2.goldengoose.com/public/Style/ECOMM/GMP01190.P000786-15369-2.jpg', 3, NOW(), NOW());

-- Jogger Cargo Negro
INSERT INTO public."ProductImage" VALUES
(gen_random_uuid(), 'c71f9ea4-c287-4095-be96-e0a4374e1c0e', 'https://mauiandsons.com.pe/media/catalog/product/cache/df6235d5d5f05cc092911bf968f32080/5/n/5n255-mt25cargoblack-1.jpg', 1, NOW(), NOW()),
(gen_random_uuid(), 'c71f9ea4-c287-4095-be96-e0a4374e1c0e', 'https://thecult.vtexassets.com/arquivos/ids/181316-800-800?v=638364637276300000&width=800&height=800&aspect=true', 2, NOW(), NOW()),
(gen_random_uuid(), 'c71f9ea4-c287-4095-be96-e0a4374e1c0e', 'https://img.kwcdn.com/product/fancy/d368e91b-f445-44af-812c-82e886ac9f6e.jpg?imageMogr2/auto-orient%7CimageView2/2/w/800/q/70/format/webp', 3, NOW(), NOW());

-- Zapatillas Blancas Clásicas
INSERT INTO public."ProductImage" VALUES
(gen_random_uuid(), 'b4f87190-a405-4e32-943a-cf4a7eac5dd6', 'https://gnuz.pe/wp-content/uploads/2025/03/Zapatillas-Clasicas-Gualind-blanco-222.webp', 1, NOW(), NOW()),
(gen_random_uuid(), 'b4f87190-a405-4e32-943a-cf4a7eac5dd6', 'https://img.kwcdn.com/product/fancy/79ad0b08-5733-4738-a6d2-689750db9075.jpg?imageMogr2/auto-orient%7CimageView2/2/w/800/q/70/format/webp', 2, NOW(), NOW()),
(gen_random_uuid(), 'b4f87190-a405-4e32-943a-cf4a7eac5dd6', 'https://img.kwcdn.com/product/fancy/5373ed23-e3af-4d76-bb99-520c460e1231.jpg?imageMogr2/auto-orient%7CimageView2/2/w/800/q/70/format/webp', 3, NOW(), NOW());

-- Botines de Cuero Marrón
INSERT INTO public."ProductImage" VALUES
(gen_random_uuid(), '7f987888-1c46-47fb-9fba-d3c48437ab26', 'https://passarelape.vtexassets.com/arquivos/ids/1378896/Botines-Casual-Top-Model-Mujeres-Td-021-Marlene-Cuero-Marron---37.jpg?v=638145951158170000', 1, NOW(), NOW()),
(gen_random_uuid(), '7f987888-1c46-47fb-9fba-d3c48437ab26', 'https://oechsle.vteximg.com.br/arquivos/ids/14846530-1000-1000/1453123.jpg?v=638279207058030000', 2, NOW(), NOW()),
(gen_random_uuid(), '7f987888-1c46-47fb-9fba-d3c48437ab26', 'https://brunoferrini.vtexassets.com/arquivos/ids/229351/1118685_01.jpg?v=638451772990830000', 3, NOW(), NOW());