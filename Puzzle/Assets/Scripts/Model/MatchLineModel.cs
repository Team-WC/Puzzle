using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public enum MatchLineType 
{
    V = 0, H, S
}

public class MatchedLineInfo 
{
    public List<MatchLineModel> matchLineModels;
    public List<GemModel> gemModels;
    public bool isMerged;
    public GemModel newAdded;

    internal void Merge(MatchedLineInfo anotherMatchedLineInfo) 
    {
        matchLineModels = matchLineModels
            .Union(anotherMatchedLineInfo.matchLineModels.Where(
                anotherMatchLineModel => !matchLineModels.Any(
                    matchLineModel => matchLineModel.type == anotherMatchLineModel.type
                )
            ))
            .ToList();
        gemModels = gemModels.Union(anotherMatchedLineInfo.gemModels).ToList();
        anotherMatchedLineInfo.isMerged = true;
    }

    public override string ToString() 
    {
        return string.Format("MatchLineInfo : {0}, {1}\n", matchLineModels.Count, gemModels.Count);
    }

}

[System.Serializable]
public class MatchLineModel 
{
    public int cols;
    public int rows;
    public MatchLineType type;
    public int magnitude;
    public List<WhereCanMatch> wheresCanMatch;

    public MatchLineModel(int startCol, int startRow, int cols, int rows) 
    {
        this.cols = cols;
        this.rows = rows;
        
        type = (cols == rows) ? MatchLineType.S : (cols > rows) ? MatchLineType.H : MatchLineType.V;
        magnitude = Math.Max(cols, rows);

        wheresCanMatch = new List<WhereCanMatch>();
        
        var rowPivot = startRow;
        for (var row = 0; row < rows; row++) 
        {
            var colPivot = startCol;
            for (var col = 0; col < cols; col++) {
                wheresCanMatch.Add(new WhereCanMatch(colPivot, cols, rowPivot, rows));
                colPivot++;
            }
            rowPivot++;
        }
    }

    public override string ToString() 
    {
        return string.Format("{1} {0} MatchLine", type, magnitude);
    }
}

public class WhereCanMatch 
{
    public List<int[]> MatchOffsets { get; private set; }

    public WhereCanMatch(int startCol, int cols, int startRow, int rows) 
    {
        MatchOffsets = new List<int[]>();
        for (var row = 0; row < rows; row++) 
        {
            for (var col = 0; col < cols; col++) {
                MatchOffsets.Add(new int[2]{ startCol + col, startRow + row });
            }
        }
    }

    public WhereCanMatch(List<int[]> matchOffsets) 
    {
        MatchOffsets = matchOffsets;
    }

    public override string ToString()
    {
        return string.Format(
            "({0}, {1}) => ({2}, {3})", 
            MatchOffsets[0][0], 
            MatchOffsets[0][1],
            MatchOffsets[MatchOffsets.Count - 1][0],
            MatchOffsets[MatchOffsets.Count - 1][1]
        );
    }
}