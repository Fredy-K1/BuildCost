

using Shared.Contracts.Entidades;

namespace Shared.Contracts.Interfaces
{
    public interface IDatoM2Repository
    {
        Task<IEnumerable<DatosM2>> GetAll();
        Task<DatosM2?> GetById(int id);
        Task<DatosM2> Create (DatosM2 datosM2);
        Task<DatosM2?> Update(int id, DatosM2 datosM2);
        Task<bool> Delete(int id);

    }
}
