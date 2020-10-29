using System;
using System.Collections.Generic;
using System.Text;

namespace CRMYIA.Data.ViewModel
{
    public class VisitaViewModel
    {
        public long sourceId { get; set; }
        public string title { get; set; }
        public DateTime start { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public bool allDay { get; set; }
    }
}
