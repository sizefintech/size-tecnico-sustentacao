using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace size.Core.DTOs
{
    public class InserirDuplicata
    {
        public  string TomadorId { get; set; }
        public  List<string> DuplicatasIds { get; set; }
    }

    public class RemoverDuplicata
    {
        public string TomadorId { get; set; }
        public List<string> DuplicatasIds { get; set; }
    }
}
