program CorrectProgram;
const
  Limit = 500;
var
  x, y : integer;
  result : real;
function GetCoefficient : integer;
begin
  GetCoefficient := 2;
end;
begin
  x := 10;
  y := x + Limit;
  result := y * GetCoefficient;
end.