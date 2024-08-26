using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Models
{
    public class ActionCode
    {
        public int Code { get; private set; }
        public Guid UserId { get; private set; }
        public int Action { get; private set; }
        public bool IsUsed { get; private set; }
        public DateTimeOffset CreatedDateTime { get; private set; }
        public DateTimeOffset UsedDateTime { get; private set; }

        private ActionCode(
            int code,
            Guid userId,
            int action,
            bool isUsed,
            DateTimeOffset createdDateTime,
            DateTimeOffset usedDateTime)
        {
            Code = code;
            UserId = userId;
            Action = action;
            IsUsed = isUsed;
            CreatedDateTime = createdDateTime;
            UsedDateTime = usedDateTime;
        }

        public static ActionCode CreateNew(
            Guid userId,
            int action
            )
        {
            int code = new Random().Next(1000, 9999);

            return new(
                code,
                userId,
                action,
                false,
                DateTime.Now,
                DateTime.Now
            );
        }
    }
}