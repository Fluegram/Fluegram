using System.Text.RegularExpressions;

namespace Fluegram.Commands.Parsing;

internal class TextSegmentCollection
{
    private readonly CommandDataSegment[] _sourceSegments;

    public TextSegmentCollection(string source, string splitRegex, bool useQuote, bool useDoubleQuote)
    {
        CurrentIndex = 0;

        _sourceSegments = Regex.Matches(source, splitRegex)
            .Select(_ =>
            {
                CommandDataSegment segmentValue;

                if (useQuote && _.Value.StartsWith("\'") && _.Value.EndsWith("\'"))
                    segmentValue = new CommandDataSegment(_.Value.Trim('\''), StringSegmentTrimMode.Quote);
                
                if (useDoubleQuote && _.Value.StartsWith("\"") && _.Value.EndsWith("\""))
                    segmentValue = new CommandDataSegment(_.Value.Trim('"'), StringSegmentTrimMode.Quote);
                
                segmentValue = new CommandDataSegment(_.Value, StringSegmentTrimMode.None);

                return segmentValue;
            }).ToArray();
    }

    public int CurrentIndex { get; private set; }

    public bool SegmentsAvailable => CurrentIndex < _sourceSegments.Length;

    public string this[int index] => _sourceSegments[index].Value;

    public override string ToString()
    {
        var segments = _sourceSegments[CurrentIndex..];

        var segmentsArray = new string[segments.Length];

        for (var i = 0; i < segments.Length; i++)
        {
            var segment = segments[i];

            var segmentValue = segment.Value;

            var strToAdd = segment.TrimMode is StringSegmentTrimMode.Quote ? "\'" :
                segment.TrimMode is StringSegmentTrimMode.DoubleQuote ? "\"" : "";

            segmentValue = $"{strToAdd}{segmentValue}{strToAdd}";

            segmentsArray[i] = segmentValue;
        }

        return string.Join(" ", segmentsArray);
    }

    public string Take()
    {
        return _sourceSegments[CurrentIndex++].Value;
    }
}