using size.Core.Communication.Notificacoes;
using size.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace size.Core.Communication
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void Notificar(string notificacao);
        string ObterTextoDasNotificacoes();
        void Notificar(Notificacao notificacao);
        void Notificar(DomainException exception);
        void Notificar(IReadOnlyList<string> notificacoes);
        void Remover(string notificacao);
        void LimparNotificacoes();

    }
}
