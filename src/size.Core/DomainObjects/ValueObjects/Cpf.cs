namespace size.Core.DomainObjects.ValueObjects
{
    public class Cpf
    {
        public Cpf(string numero)
        {
            if (!Validar(numero)) throw new Exception("CPF inválido");
            Numero = numero.Trim();
        }

        public string Numero { get; private set; }
        public static bool Validar(string numero)
        {
            if (string.IsNullOrEmpty(numero?.Trim()))
                return false;

            if (numero.Length != 11)
                return false;

            if (numero.Equals("00000000000") ||
                numero.Equals("11111111111") ||
                numero.Equals("22222222222") ||
                numero.Equals("33333333333") ||
                numero.Equals("44444444444") ||
                numero.Equals("55555555555") ||
                numero.Equals("66666666666") ||
                numero.Equals("77777777777") ||
                numero.Equals("88888888888") ||
                numero.Equals("99999999999"))
                return false;

            string cpf = numero;
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            if (!cpf.EndsWith(digito))
                return false;

            return true;
        }
    }
}
