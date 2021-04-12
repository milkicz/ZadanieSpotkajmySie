using System;
using System.Collections.Generic;

namespace ZadanieSpotkajmySie
{
    public class Calendar
    {
        public TimeInterval working_hours { get; set; }
        public List<TimeInterval> planned_meeting { get; set; }
    }
}