using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.Core.Communication.Notificacoes
{
    public struct Notificacao
    {
        public Notificacao(string mensagem)
        {
            Mensagem = mensagem;
            TentarNovamente = true;
        }

        public Notificacao(string mensagem, bool tentarNovamente)
        {
            Mensagem = mensagem;
            TentarNovamente = tentarNovamente;
        }

        public string Mensagem { get; }
        public bool TentarNovamente { get; }
    }
}
