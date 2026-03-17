namespace size.Core.DomainObjects.ValueObjects
{
    public class Cnpj
    {
        public Cnpj(string numero)
        {
            if (!Validar(numero)) throw new Exception("Cnpj Inválido");
            Numero = numero.Trim();
        }

        public string Numero { get; private set; }

        public static bool Validar(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj?.Trim()))
                return false;

            if (cnpj.Length != 14)
                return false;

            if (cnpj.Equals("00000000000000") ||
                cnpj.Equals("11111111111111") ||
                cnpj.Equals("22222222222222") ||
                cnpj.Equals("33333333333333") ||
                cnpj.Equals("44444444444444") ||
                cnpj.Equals("55555555555555") ||
                cnpj.Equals("66666666666666") ||
                cnpj.Equals("77777777777777") ||
                cnpj.Equals("88888888888888") ||
                cnpj.Equals("99999999999999"))
                return false;

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;
            string tempCnpj = cnpj.Substring(0, 12);
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            int resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            string digito = resto.ToString();
            tempCnpj += digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito += resto.ToString();
            if (!cnpj.EndsWith(digito))
                return false;
            return true;
        }
    }
}
