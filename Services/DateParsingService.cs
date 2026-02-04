using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WeatherHistoryDataRecorder.Services
{
    public class DateParseResult
    {
        public string OriginalInput { get; set; }
        public string? IsoDate { get; set; }
        public bool IsValid { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class DateParsingService
    {
        private readonly string _filePath;

        public DateParsingService(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Reads all dates from the dates.txt file and parses them into ISO format
        /// </summary>
        /// <returns>List of DateParseResult objects with parsed dates or error messages</returns>
        public List<DateParseResult> ReadAndParseAllDates()
        {
            var results = new List<DateParseResult>();

            try
            {
                if (!File.Exists(_filePath))
                {
                    return new List<DateParseResult>
                    {
                        new DateParseResult
                        {
                            OriginalInput = _filePath,
                            IsValid = false,
                            ErrorMessage = $"File not found: {_filePath}"
                        }
                    };
                }

                var lines = File.ReadAllLines(_filePath);

                foreach (var line in lines)
                {
                    var trimmedLine = line.Trim();

                    // Skip empty lines
                    if (string.IsNullOrWhiteSpace(trimmedLine))
                        continue;

                    var result = ParseSingleDate(trimmedLine);
                    results.Add(result);
                }
            }
            catch (Exception ex)
            {
                results.Add(new DateParseResult
                {
                    OriginalInput = _filePath,
                    IsValid = false,
                    ErrorMessage = $"Error reading file: {ex.Message}"
                });
            }

            return results;
        }

        /// <summary>
        /// Parses a single date string in multiple formats
        /// </summary>
        private DateParseResult ParseSingleDate(string dateString)
        {
            // Array of date format patterns to try
            var dateFormats = new[]
            {
                "MM/dd/yyyy",           // 02/27/2021
                "MMMM d, yyyy",         // June 2, 2022
                "MMM-dd-yyyy" ,          // Jul-13-2020
                "M/d/yyyy", 
                "yyyy-MM-dd"
            };

            foreach (var format in dateFormats)
            {
                if (DateTime.TryParseExact(dateString, format, System.Globalization.CultureInfo.InvariantCulture, 
                    System.Globalization.DateTimeStyles.None, out var parsedDate))
                {
                    // Validate that the date is actually valid (e.g., April 31 doesn't exist)
                    // The TryParseExact should handle this, but we verify
                    var isoDate = parsedDate.ToString("yyyy-MM-dd");
                    
                    return new DateParseResult
                    {
                        OriginalInput = dateString,
                        IsoDate = isoDate,
                        IsValid = true,
                        ErrorMessage = null
                    };
                }
            }

            // If none of the formats matched, return an error result
            return new DateParseResult
            {
                OriginalInput = dateString,
                IsoDate = null,
                IsValid = false,
                ErrorMessage = $"Could not parse date: '{dateString}'. Unsupported format or invalid date."
            };
        }
    }
}
