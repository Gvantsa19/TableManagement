﻿namespace TMS.APP.Models
{
    public struct Error
    {
        public Error(IEnumerable<string> messages)
            : this(messages.ToArray())
        {
        }

        public Error(params string[] messages)
        {
            Messages = messages;
            Date = DateTime.Now;
        }

        public IReadOnlyList<string> Messages { get; }

        public DateTime Date { get; }
    }
}
