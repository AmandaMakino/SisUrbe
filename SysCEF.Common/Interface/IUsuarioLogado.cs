using DataAccess;
using SysCEF.DAO.Interface;
using SysCEF.Model;

namespace SysCEF.Common.Interface
{
    public interface IUsuarioLogado
    {
        int UsuarioId { get; }
        Usuario Usuario { get; }
        IUsuarioRepositorio UsuarioRepositorio { get; set; }
        IUnitOfWork UnitOfWork { get; set; }
        bool EAdministrador { get; }
    }
}
