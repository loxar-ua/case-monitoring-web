using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalData.Common
{
    public static class Constants
    {
        //The minimum search term length required to switch from 'ILike' to Trigram search.
        public const int TrigramThresholdLength = 5;

        // The minimum similarity threshold required for Trigram search results (0.0 to 1.0).
        public const double TrigramAccuracyThreshold = 0.1;

        //The default number of items to display on a single page if not specified.
        public const int DefaultPageSize = 10;

        // The maximum allowed number of items per page to prevent performance issues.
        public const int MaxPageSize = 100;
    }
}
