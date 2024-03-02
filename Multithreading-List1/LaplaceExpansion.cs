using System.Collections;

namespace Multithreading_List1;

public class LaplaceExpansion {
    private List<long> _matrix = [0];

    public List<long> Matrix {
        get => _matrix;
        set {
            if (!IsSquareMatrix(value) || value.Count <= 0) {
                Console.WriteLine("Matrix is not square or matrix is empty");
                return;
            }

            _matrix = value;
            Size = (int)Math.Sqrt(value.Count);
        }
    }

    public int Size { get; private set; }

    public LaplaceExpansion(List<long> matrix) {
        Matrix = matrix;
    }

    public long UseLaplaceExpansion() {
        if (Size <= 3) {
            return MatrixDeterminant();
        }

        var matrices = new List<LaplaceExpansion>();
        var factors = new List<long>();
        
        // First try is on first row
        for (var erCol = 0; erCol < Size; erCol++) {
            var matrixArr = new List<long>();

            for (var row = 0; row < Size; ++row) {
                for (var col = 0; col < Size; ++col) {
                    if (row == 0) continue;
                    if (col == erCol) continue;

                    var cell = Matrix[col + Size * row];
                    matrixArr.Add(cell);
                }
            }

            factors.Add((int)Math.Pow(-1, erCol) * Matrix[erCol]);
            matrices.Add(new LaplaceExpansion(matrixArr));
        }

        var result = 0L;
        
        for (var i = 0; i < matrices.Count; ++i) {
            if (matrices[i].Size > 3) {
                result += matrices[i].UseLaplaceExpansion();
            }
            else {
                var x = matrices[i].MatrixDeterminant();
                x *= factors[i];
                
                result += x;
            }
        }

        return result;
    }

    private long MatrixDeterminant() {
        var determinant = Size switch {
                              3 => _matrix[0] * _matrix[4] * _matrix[8] + _matrix[1] * _matrix[5] * _matrix[6] +
                                   _matrix[2] * _matrix[3] * _matrix[7] - _matrix[2] * _matrix[4] * _matrix[6] -
                                   _matrix[1] * _matrix[3] * _matrix[8] - _matrix[0] * _matrix[5] * _matrix[7],
                              2 => _matrix[0] * _matrix[3] - _matrix[1] * _matrix[2],
                              1 => _matrix.First(),
                              _ => 0
                          };

        return determinant;
    }

    private static bool IsSquareMatrix(ICollection matrix) {
        var size = (int)Math.Sqrt(matrix.Count);
        return size * size == matrix.Count;
    }
}