namespace Multithreading_List1;

public class LaplaceExpansion(List<long> matrix, int threads) {
    private List<long> Matrix { get; set; } = matrix;
    public int Threads { get; set; } = threads;

    private struct AlgebraicComplement {
        public List<int> ColsIgnored { get; set; } = [];
        public List<int> RowsIgnored { get; set; } = [];
        public long Factor { get; set; } = 0;

        public AlgebraicComplement(List<int> colsIgnored, List<int> rowsIgnored, long factor) {
            ColsIgnored = colsIgnored;
            RowsIgnored = rowsIgnored;
            Factor = factor;
        }

        public static AlgebraicComplement Empty => new([], [], 0);
    }

    public long UseLaplaceExpansion() {
        return UseLaplaceExpansion(AlgebraicComplement.Empty);
    }

    private long UseLaplaceExpansion(AlgebraicComplement thisComplement) {
        var complements = new List<AlgebraicComplement>();
        var rowId = 0;
        if (thisComplement.RowsIgnored.Count > 0) {
            rowId = thisComplement.RowsIgnored.LastOrDefault(0);
        }

        for (var colId = 0; colId < MatrixSize(); ++colId) {
            if (thisComplement.ColsIgnored.Contains(colId)) continue;
            var x = new AlgebraicComplement([..thisComplement.ColsIgnored],
                [..thisComplement.RowsIgnored],
                thisComplement.Factor);
            var nextComplement = GetAlgebraicComplement(rowId, colId, x);
            complements.Add(nextComplement);
        }

        var complementsSize = MatrixSize() - MatrixSize(complements.First());
        var determinant = (long)0;

        foreach (var item in complements) {
            if (complementsSize > 3) {
                determinant += UseLaplaceExpansion(item);
            }
            else {
                var sgn = (long)Math.Pow(-1, item.RowsIgnored.LastOrDefault(0) + item.ColsIgnored.LastOrDefault(0));
                determinant += sgn *
                               item.Factor *
                               MatrixDeterminant(item);
            }
        }

        return determinant;
    }

    /// <summary>
    ///  Works only for matrices of sizes lower than 3
    /// </summary>
    /// <param name="algebraicComplement"></param>
    /// <returns></returns>
    private long MatrixDeterminant(AlgebraicComplement algebraicComplement) {
        List<long> matrix = [];

        for (var row = 0; row < MatrixSize(); ++row) {
            if (algebraicComplement.RowsIgnored.Contains(row)) continue;
            for (var col = 0; col < MatrixSize(); ++col) {
                if (algebraicComplement.ColsIgnored.Contains(col)) continue;

                var matrixItem = Matrix[row * MatrixSize() + col];
                matrix.Add(matrixItem);
            }
        }

        var matrixSize = MatrixSize() - algebraicComplement.RowsIgnored.Count;
        var determinant = matrixSize switch {
            3 => (matrix[0] * matrix[4] * matrix[8] +
                  matrix[1] * matrix[5] * matrix[6] +
                  matrix[2] * matrix[3] * matrix[7]) -
                 (matrix[2] * matrix[4] * matrix[6] +
                  matrix[1] * matrix[3] * matrix[8] +
                  matrix[0] * matrix[5] * matrix[7]),
            2 => matrix[0] * matrix[3] - matrix[1] * matrix[2],
            1 => matrix[0],
            _ => 0
        };

        return determinant;
    }

    private AlgebraicComplement GetAlgebraicComplement(int row, int col,
        AlgebraicComplement algebraicComplement) {
        if (!algebraicComplement.ColsIgnored.Contains(col)) {
            algebraicComplement.ColsIgnored.Add(col);
        }

        if (!algebraicComplement.RowsIgnored.Contains(row)) {
            algebraicComplement.RowsIgnored.Add(row);
        }

        var factorPosition = row * MatrixSize() + col;
        var factor = Matrix[factorPosition];
        algebraicComplement.Factor = factor;

        return algebraicComplement;
    }

    private int MatrixSize() {
        return (int)Math.Sqrt(Matrix.Count);
    }

    private int MatrixSize(AlgebraicComplement matrix) {
        return MatrixSize() - matrix.RowsIgnored.Count;
    }
}