using Microsoft.VisualBasic.CompilerServices;

namespace Multithreading_List1;

/// <summary>
/// Class test for numbers in range [LowThreshold; HighThreshold).
/// </summary>
public class NumberFactors {
    private long _lowThreshold = long.MinValue;
    private long _highThreshold = long.MaxValue;

    public long LowThreshold {
        get => _lowThreshold;
        set {
            if (value > _highThreshold) {
                return;
            }

            if (value > Number && Number.HasValue) {
                _lowThreshold = Number.Value;
            }
            else {
                _lowThreshold = value;
            }
        }
    }

    public long HighThreshold {
        get => _highThreshold;
        set {
            if (value < _lowThreshold) {
                return;
            }

            if (value > MaxThresholdValue && MaxThresholdValue.HasValue) {
                _highThreshold = MaxThresholdValue.Value;
            }
            else {
                _highThreshold = value;
            }
        }
    }

    public long? Number { get; set; }
    public long? MaxThresholdValue => Number + 1;

    public NumberFactors(long lowThreshold, long highThreshold, long number) {
        Number = number;
        LowThreshold = lowThreshold;
        HighThreshold = highThreshold;
    }
    
    public IEnumerable<long> GetFactors() {
        if (Number is null) {
            return [];
        }

        List<long> factors = [];
        for (var factor = _lowThreshold; factor < _highThreshold; ++factor) {
            if (factor == 0 || Number % factor != 0) {
                continue;
            }

            factors.Add(factor);

            // Console.WriteLine(
            //     threadId > -1
            //         ? $"thread Id: {threadId}, thread result: {factor}"
            //         : $"Thread result: {factor}"
            // );
        }

        // if (factors.Count <= 0) {
        //     Console.WriteLine($"In range of thread {threadId} no factors were found");
        // }

        return factors;
    }

    public bool SetRange(long lowThreshold, long highThreshold) {
        if (lowThreshold > highThreshold) {
            return false;
        }

        if (lowThreshold > Number || highThreshold < Number) {
            return false;
        }

        _lowThreshold = lowThreshold;
        _highThreshold = highThreshold;

        return true;
    }
}