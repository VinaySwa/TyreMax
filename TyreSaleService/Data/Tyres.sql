PRAGMA foreign_keys = ON;

BEGIN TRANSACTION;

-- 1) Generate ModelId = 1..80
WITH RECURSIVE Models(i) AS (
  SELECT 1
  UNION ALL
  SELECT i + 1 FROM Models WHERE i < 80
),

-- 2) Define four size presets
Sizes(Width, Profile, RimSize) AS (
  SELECT 185, 65, 15 UNION ALL
  SELECT 205, 55, 16 UNION ALL
  SELECT 225, 45, 17 UNION ALL
  SELECT 195, 60, 15
)

-- 3) Insert one tyre per (Model × Size)
INSERT INTO Tyres
(
  Dimensions_Width,
  Dimensions_Profile,
  Dimensions_RimSize,
  ModelId,
  Price,
  DiscountPercentage,
  LoadIndex,
  SpeedIndex,
  Availability
)
SELECT
  s.Width,
  s.Profile,
  s.RimSize,
  m.i AS ModelId,

  -- Price: base 120 + model# * 0.5 + size adjustment
  printf('%.2f', 120.0 + m.i * 0.5 + (s.Width - 185) * 0.1),

  -- Discount: varies by model
  (m.i % 5) * 2.5,  

  -- LoadIndex: cycles 91–99
  91 + ((m.i + s.RimSize) % 9),

  -- SpeedIndex: alternate V/W
  CASE WHEN (m.i % 2) = 0 THEN 'V' ELSE 'W' END,

  -- Availability: every 3rd tyre is out of stock
  CASE WHEN (m.i % 3) = 0 THEN 0 ELSE 1 END

FROM Models m
CROSS JOIN Sizes s;

COMMIT;
