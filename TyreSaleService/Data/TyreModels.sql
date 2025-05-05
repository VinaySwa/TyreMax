BEGIN TRANSACTION;

INSERT INTO TyreModels (Name, CompanyId) VALUES
  -- Maxxis (MsModel1…MsModel10)
  ('MsModel1',  1), ('MsModel2',  1), ('MsModel3',  1), ('MsModel4',  1), ('MsModel5',  1),
  ('MsModel6',  1), ('MsModel7',  1), ('MsModel8',  1), ('MsModel9',  1), ('MsModel10', 1),

  -- Goodyear (GrModel1…GrModel10)
  ('GrModel1',  2), ('GrModel2',  2), ('GrModel3',  2), ('GrModel4',  2), ('GrModel5',  2),
  ('GrModel6',  2), ('GrModel7',  2), ('GrModel8',  2), ('GrModel9',  2), ('GrModel10', 2),

  -- Vredestein (VnModel1…VnModel10)
  ('VnModel1',  3), ('VnModel2',  3), ('VnModel3',  3), ('VnModel4',  3), ('VnModel5',  3),
  ('VnModel6',  3), ('VnModel7',  3), ('VnModel8',  3), ('VnModel9',  3), ('VnModel10', 3),

  -- Maxam (MmModel1…MmModel10)
  ('MmModel1',  4), ('MmModel2',  4), ('MmModel3',  4), ('MmModel4',  4), ('MmModel5',  4),
  ('MmModel6',  4), ('MmModel7',  4), ('MmModel8',  4), ('MmModel9',  4), ('MmModel10', 4),

  -- Hifly (HyModel1…HyModel10)
  ('HyModel1',  5), ('HyModel2',  5), ('HyModel3',  5), ('HyModel4',  5), ('HyModel5',  5),
  ('HyModel6',  5), ('HyModel7',  5), ('HyModel8',  5), ('HyModel9',  5), ('HyModel10', 5),

  -- Starmaxx (SxModel1…SxModel10)
  ('SxModel1',  6), ('SxModel2',  6), ('SxModel3',  6), ('SxModel4',  6), ('SxModel5',  6),
  ('SxModel6',  6), ('SxModel7',  6), ('SxModel8',  6), ('SxModel9',  6), ('SxModel10', 6),

  -- Alliance (AeModel1…AeModel10)
  ('AeModel1',  7), ('AeModel2',  7), ('AeModel3',  7), ('AeModel4',  7), ('AeModel5',  7),
  ('AeModel6',  7), ('AeModel7',  7), ('AeModel8',  7), ('AeModel9',  7), ('AeModel10', 7),

  -- Rovelo (RoModel1…RoModel10)
  ('RoModel1',  8), ('RoModel2',  8), ('RoModel3',  8), ('RoModel4',  8), ('RoModel5',  8),
  ('RoModel6',  8), ('RoModel7',  8), ('RoModel8',  8), ('RoModel9',  8), ('RoModel10', 8);

COMMIT;