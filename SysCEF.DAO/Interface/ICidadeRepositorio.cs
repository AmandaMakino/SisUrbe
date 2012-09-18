using System.Collections.Generic;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Interface
{
    public interface ICidadeRepositorio
    {
        Cidade Obter(IUnitOfWork unitOfWork, int id);
        Cidade ObterPorNomeUF(IUnitOfWork unitOfWork, string nome, string uf);
        IEnumerable<Cidade> BuscarTodasEstado(IUnitOfWork unitOfWork, string uf);
    }
}
