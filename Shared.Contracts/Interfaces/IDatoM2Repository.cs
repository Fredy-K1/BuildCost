

using Shared.Contracts.Entidades;

namespace Shared.Contracts.Interfaces
{
    public interface IDatoM2Repository
    {
        Task<IEnumerable<DatosM2>> GetAll();
        Task<DatosM2?> GetById(Guid id);
        Task<DatosM2> Create (DatosM2 datosM2);
        Task<DatosM2?> Update(Guid id, DatosM2 datosM2);
        Task<bool> Delete(Guid id);

    }
}
