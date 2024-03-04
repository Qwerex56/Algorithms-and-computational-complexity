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

    /// <summary>
    /// Returns ulong value where positions are set to high bits
    /// </summary>
    public ulong RefMatrix {
        get {
            var referenceMatrix = 0UL;
            for (var i = 0; i < _matrix.Count; ++i) {
                referenceMatrix |= 1UL << i;
            }

            return referenceMatrix;
        }
    }

    public int Size { get; private set; }

    public LaplaceExpansion(List<long> matrix, uint maxThreads = 1) {
        Matrix = matrix;
    }

    public long UseLaplaceExpansion(ulong refMatrix) {
        if (GetMatrixSize(refMatrix) <= 3) {
            return MatrixDeterminant(RefToMatrix(refMatrix));
        }

        var matrices = new List<ulong>();
        var factors = new List<long>();

        // First try is on first row
        for (var erCol = 0; erCol < GetMatrixSize(refMatrix);) {
            var ignoredColumn = erCol;

            var factor = (int)Math.Pow(-1, ignoredColumn) * RefToMatrix(refMatrix)[ignoredColumn];
            if (factor == 0) {
                ++erCol;
                continue;
            }

            factors.Add(factor);
            matrices.Add(GetAlgebraicComplement(refMatrix, (uint)(Size / GetMatrixSize(refMatrix)), (uint)ignoredColumn));
            
            ++erCol;
        }

        var result = 0L;

        for (var i = 0; i < matrices.Count; ++i) {
            if (GetMatrixSize(matrices[i]) > 3) {
                result += factors[i] * UseLaplaceExpansion(matrices[i]);
            }
            else {
                var x = MatrixDeterminant(RefToMatrix(matrices[i]));
                x *= factors[i];

                result += x;
            }
        }

        return result;
    }

    private List<long> RefToMatrix(ulong refMatrix) {
        var matrix = new List<long>();

        for (var pos = 0; pos < Size * Size; ++pos) {
            if (!HasReferenceInBinaryPosition((byte)pos, refMatrix)) {
                continue;
            }

            matrix.Add(_matrix[pos]);
        }

        return matrix;
    }

    private static long MatrixDeterminant(IList<long> matrix) {
        var determinant = GetMatrixSize(matrix) switch {
                              3 => matrix[0] * matrix[4] * matrix[8] + matrix[1] * matrix[5] * matrix[6] +
                                   matrix[2] * matrix[3] * matrix[7] - matrix[2] * matrix[4] * matrix[6] -
                                   matrix[1] * matrix[3] * matrix[8] - matrix[0] * matrix[5] * matrix[7],
                              2 => matrix[0] * matrix[3] - matrix[1] * matrix[2],
                              1 => matrix[0],
                              _ => 0
                          };

        return determinant;
    }

    // TODO: The binary states are wrong for matrices bigger than 4x4
    private ulong GetAlgebraicComplement(ulong refMatrix, uint ignoredRow, uint ignoredCol) {
        var algebraicComplement = 0Ul;
        
        for (var row = 0; row < Size; ++row) {
            for (var col = 0; col < Size; ++col) {
                if (row == ignoredRow) continue;
                if (col == ignoredCol) continue;

                var posDec = col + row * Size;
                if (!HasReferenceInBinaryPosition((byte)posDec, refMatrix)) {
                    continue;
                }
                
                algebraicComplement |= 1UL << col + row * Size;
            }
        }

        return algebraicComplement;
    }

    private static bool HasReferenceInBinaryPosition(byte position, ulong binPosition) {
        return ((binPosition >> position) & 1) == 1;
    }

    private static bool IsSquareMatrix(ICollection<long> matrix) {
        var size = (int)Math.Sqrt(matrix.Count);
        return size * size == matrix.Count;
    }

    private static int GetMatrixSize(ulong matrix) {
        return (int)Math.Sqrt(ulong.PopCount(matrix));
    }

    private static int GetMatrixSize(ICollection<long> matrix) {
        return (int)Math.Sqrt(matrix.Count);
    }
}