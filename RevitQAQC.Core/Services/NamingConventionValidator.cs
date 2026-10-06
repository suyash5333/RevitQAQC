using RevitQAQC.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    levelName,
                    standard,
                    usedMarks);

                return CreateIssue(
                    elementId,
                    category,
                    "",
                    "Mark is missing.",
                    suggestedMark);
            }

            // 2. Duplicate Mark
            if (usedMarks.Contains(mark))
            {
                string suggestedMark = GenerateNextMark(
                    levelName,
                    standard,
                    usedMarks);

                return CreateIssue(
                    elementId,
                    category,
                    mark,
                    "Duplicate Mark value.",
                    suggestedMark);
            }

            // 3. Validate naming format
            string expectedPattern = BuildPattern(standard);

            if (!Regex.IsMatch(mark, expectedPattern))
            {
                string suggestedMark = GenerateNextMark(
                    levelName,
                    standard,
                    usedMarks);

                return CreateIssue(
                    elementId,
                    category,
                    mark,
                    $"Invalid naming convention. Expected format: {BuildExample(standard)}",
                    suggestedMark);
            }

            return null;
        }

        private QAIssue CreateIssue(
            long elementId,
            string category,
            string currentValue,
            string problem,
            string suggestedValue)
        {
            return new QAIssue
            {
                ElementId = elementId,
                Category = category,
                CurrentValue = currentValue,
                Problem = problem,
                SuggestedValue = suggestedValue,
                Severity = "Major"
            };
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

            for (int number = 1; ; number++)
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
                {
                    return candidate;
                }
            }
        }

        private string ExtractLevelNumber(string levelName)
        {
            Match match =
                Regex.Match(
                    levelName ?? "",
                    @"\d+");

            if (match.Success)
            {
                return match.Value;
            }

            return "1";
        }
    }
}