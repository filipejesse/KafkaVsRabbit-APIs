using System;

namespace ProducerA
{
    public class MessagesConfig
    {
        public int MessagesCount { get; set; }
        public int MessageSize { get; set; }
        public bool UseParallelism { get; set; }
        public int? ParallelismLimit { get; set; }
        public QueueType QueueType { get; set; }
    }
}
