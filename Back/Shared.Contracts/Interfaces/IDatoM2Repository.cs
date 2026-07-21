using Shared.Contracts.Entidades;

namespace Shared.Contracts.Interfaces
{
    public interface IDatoM2Repository
    {
        Task<IEnumerable<DatosM2>> GetAllByContratistaAsync(Guid contratistaId);

        Task<DatosM2?> GetByIdAsync(int id,Guid contratistaId);

        Task<DatosM2> CreateAsync(DatosM2 datosM2);

        Task<DatosM2?> UpdateAsync(int id,Guid contratistaId,DatosM2 datosM2);

        Task<bool> DeleteAsync(int id,Guid contratistaId
        );
    }
}