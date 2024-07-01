using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons
{
    public class StreamDetails
    {
        public static string STREAM_NAME = "Stream1";
        public static string SUBJECT_NAME = "Subject1";
        public static int TOTAL_MESSAGES_PER_TASK = 10000;
        public static int NUMBER_OF_TASKS = 8;
        public static long STREAM_SIZE_LIMIT = -1;
        public static int MAX_CONSUMER_BYTES = 20000;
        public static int MAX_CONSUMER_MESSAGES = 10;
        public static int MAX_RETENTION_PERIOD_MINUTES = 10;

    }
}
