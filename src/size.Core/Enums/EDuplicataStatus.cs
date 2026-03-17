using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.Core.Enums
{
    public enum EDuplicataStatus
    {
        [Display(Name = "Disponível")]
        DISPONIVEL,
        [Display(Name = "Operada")]
        OPERADA,
        [Display(Name = "Finalizada")]
        FINALIZADA,
        [Display(Name = "Paga")]
        PAGA,
        [Display(Name = "Cancelada")]
        CANCELADA
    }
}
