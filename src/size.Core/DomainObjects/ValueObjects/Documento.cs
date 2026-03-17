using size.Core.Enums;
using size.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.Core.DomainObjects.ValueObjects
{
    public class Documento
    {
        private Documento() { }

        public Documento(string numero, EPessoaTipo tipo)
        {
            if (tipo == EPessoaTipo.PJ)
            {
                if (!ValidaCnpj(numero)) throw new Exception("CNPJ inválido");
            }
            else
            {
                if (!ValidaCpf(numero)) throw new Exception("CPF inválido");
            }
            Numero = numero.Trim();
            Tipo = tipo;
        }

        public string Numero { get; private set; }
        public EPessoaTipo Tipo { get; private set; }

        public static bool ValidaCnpj(string numero)
        {
            return Cnpj.Validar(numero);
        }
        public static bool ValidaCpf(string numero)
        {
            return Cpf.Validar(numero);
        }

        public static bool Validar(string numero)
        {
            var pessoaTipo = ObterPessoaTipo(numero);
            if (pessoaTipo == EPessoaTipo.PF)
                return Cpf.Validar(numero);
            else if (pessoaTipo == EPessoaTipo.PJ)
                return Cnpj.Validar(numero);

            return false;
        }

        public static EPessoaTipo ObterPessoaTipo(string numeroDocumento)
        {
            if (numeroDocumento?.SomenteNumeros()?.Length == 11)
                return EPessoaTipo.PF;
            else if (numeroDocumento?.SomenteNumeros()?.Length == 14)
                return EPessoaTipo.PJ;

            return EPessoaTipo.PF;
        }

        /// <summary>
        /// Compara raíz do CNPJ do objeto com a raíz do CNPJ passado por parâmetro
        /// </summary>
        /// <param name="numeroDocumento"></param>
        /// <returns>boolean</returns>
        public bool EhCnpjFilialOuMatriz(string numeroDocumento)
        {
            if (Tipo == EPessoaTipo.PF) return false;
            if (!ValidaCnpj(numeroDocumento)) return false;

            var raizCnpjObjeto = Numero.SomenteNumeros().Substring(0, 8);
            var raizCnpjParametro = numeroDocumento.SomenteNumeros().Substring(0, 8);

            if (!string.Equals(raizCnpjObjeto, raizCnpjParametro)) return false;

            return true;
        }

    }
}
