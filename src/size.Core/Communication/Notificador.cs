using size.Core.Communication.Notificacoes;
using size.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.Core.Communication
{
    public class Notificador : INotificador
    {
        private List<Notificacao> _notificacoes;

        public Notificador()
        {
            _notificacoes = new List<Notificacao>();
        }

        public void LimparNotificacoes()
        {
            _notificacoes = new List<Notificacao>();
        }

        public void Remover(string mensagem)
        {
            if (!_notificacoes.Any())
                return;

            var mensagens = _notificacoes.Select(x => x.Mensagem).ToList();
            if (mensagens.Contains(mensagem))
            {
                var indice = mensagens.FindIndex(x => x == mensagem);
                _notificacoes.RemoveAt(indice);
            }
        }

        public void Notificar(string notificacao)
        {
            _notificacoes.Add(new Notificacao(notificacao));
        }

        public void Notificar(Notificacao notificacao)
        {
            _notificacoes.Add(notificacao);
        }

        public void Notificar(DomainException exception)
        {
            if (!exception.InnerExceptions.Any())
            {
                Notificar(exception.Message);
                return;
            }

            var notificacoes = new List<string>();

            for (int i = 0; i < exception.InnerExceptions.Count; i++)
                if (!notificacoes.Contains(exception.InnerExceptions[i].Message))
                    notificacoes.Add(exception.InnerExceptions[i].Message);

            Notificar(notificacoes);
        }

        public void Notificar(IReadOnlyList<string> notificacoes)
        {
            for (int i = 0; i < notificacoes.Count; i++)
                Notificar(notificacoes[i]);
        }

        public List<Notificacao> ObterNotificacoes()
        {
            return _notificacoes;
        }

        public bool TemNotificacao()
        {
            return _notificacoes.Any();
        }

        public string ObterTextoDasNotificacoes()
        {
            return string.Join(". ", _notificacoes.Select(n => n.Mensagem));
        }
    }
}
