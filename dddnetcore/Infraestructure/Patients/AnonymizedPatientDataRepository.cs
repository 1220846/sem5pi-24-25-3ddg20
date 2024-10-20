using DDDSample1.DataAnnotations.Patients;
using DDDSample1.Domain.Patients;
using DDDSample1.Infrastructure.Shared;

namespace DDDSample1.Infrastructure.Patients
{
    public class AnonymizedPatientDataRepository : BaseRepository<AnonymizedPatientData, AnonymizedPatientDataId>, IAnonymizedPatientDataRepository
    {
    
        public AnonymizedPatientDataRepository(DDDSample1DbContext context):base(context.AnonymizedPatientsData)
        {
           
        }


    }
}