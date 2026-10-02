using RevitQAQC.Shared.Models;
using System.Text.RegularExpressions;

namespace RevitQAQC.Core.Services
{
    public class NamingConventionValidator
    {
        public QAIssue? Validate(
            long elementId,
            string category,
            string mark,
            string levelName,
            NamingStandard standard,
            HashSet<string> usedMarks)
        {
            // 1. Missing Mark
            if (string.IsNullOrWhiteSpace(mark))
            {
                string suggestedMark = GenerateNextMark(
                    category,
                    levelName,
                    standard,
                    usedMarks);

                return new QAIssue
                {
                    ElementId = elementId,
                    Category = category,
                    CurrentValue = "",
                    Problem = "Mark is missing.",
                    SuggestedValue = suggestedMark,
                    Severity = "Major"
                };
            }

            // 2. Duplicate Mark
            if (usedMarks.Contains(mark))
            {
                string suggestedMark = GenerateNextMark(
                    category,
                    levelName,
                    standard,
                    usedMarks);

                return new QAIssue
                {
                    ElementId = elementId,
                    Category = category,
                    CurrentValue = mark,
                    Problem = "Duplicate Mark value.",
                    SuggestedValue = suggestedMark,
                    Severity = "Major"
                };
            }

            // 3. Generate expected format
            string expectedPattern = BuildPattern(
                standard);

            // 4. Validate format
            if (!Regex.IsMatch(mark, expectedPattern))
            {
                string suggestedMark = GenerateNextMark(
                    category,
                    levelName,
                    standard,
                    usedMarks);

                return new QAIssue
                {
                    ElementId = elementId,
                    Category = category,
                    CurrentValue = mark,
                    Problem = $"Invalid naming convention. Expected format: {BuildExample(standard)}",
                    SuggestedValue = suggestedMark,
                    Severity = "Major"
                };
            }

            return null;
        }

        private string BuildPattern(NamingStandard standard)
        {
            return "^"
                + Regex.Escape(standard.Prefix)
                + Regex.Escape(standard.Separator)
                + Regex.Escape(standard.LevelPrefix)
                + "\\d{" + standard.LevelDigits + "}"
                + Regex.Escape(standard.Separator)
                + "\\d{" + standard.NumberDigits + "}"
                + "$";
        }

        private string BuildExample(NamingStandard standard)
        {
            string level =
                standard.LevelPrefix +
                new string('0', standard.LevelDigits - 1) +
                "1";

            string number =
                new string('0', standard.NumberDigits - 1) +
                "1";

            return standard.Prefix
                + standard.Separator
                + level
                + standard.Separator
                + number;
        }

        private string GenerateNextMark(
            string category,
            string levelName,
            NamingStandard standard,
            HashSet<string> usedMarks)
        {
            string levelNumber = ExtractLevelNumber(levelName);

            string levelPart =
                standard.LevelPrefix +
                levelNumber.PadLeft(
                    standard.LevelDigits,
                    '0');

            int number = 1;

            while (true)
            {
                string numberPart =
                    number.ToString(
                        $"D{standard.NumberDigits}");

                string candidate =
                    standard.Prefix
                    + standard.Separator
                    + levelPart
                    + standard.Separator
                    + numberPart;

                if (!usedMarks.Contains(candidate))
                    return candidate;

                number++;
            }
        }

        private string ExtractLevelNumber(string levelName)
        {
            Match match =
                Regex.Match(levelName ?? "", @"\d+");

            if (match.Success)
                return match.Value;

            return "1";
        }
    }
}