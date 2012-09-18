using System.Collections.Generic;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Interface
{
    public interface IImovelRepositorio
    {
        void Salvar(IUnitOfWork unitOfWork, Imovel imovel);
        void Atualizar(IUnitOfWork unitOfWork, Imovel imovel);
        void Excluir(IUnitOfWork unitOfWork, int imovelId);
        Imovel Obter(IUnitOfWork unitOfWork, int imovelId);
        IEnumerable<Imovel> BuscarTodos(IUnitOfWork unitOfWork);
    }
}
