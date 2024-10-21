using System.Threading.Tasks;
using System.Collections.Generic;
using DDDSample1.Domain.Shared;
using System.Reflection.Emit;
using DDDSample1.Domain.OperationTypes;
using System;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.DataAnnotations.Staffs;
using DDDSample1.Domain.Staffs;
using DDDSample1.Domain.Patients;
using DDDSample1.DataAnnotations.Patients;
using System.Linq;

namespace DDDSample1.Domain.OperationRequests
{
    public class OperationRequestService{

        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IOperationRequestRepository _repoOp;

        private readonly IOperationTypeRepository _repoOpTy;

        private readonly IPatientRepository _repoPat;

        private readonly IStaffRepository _repoS;

        public OperationRequestService(IUnitOfWork unitOfWork, IOperationRequestRepository repo, IStaffRepository repoS, IOperationTypeRepository repoOpTy, IPatientRepository repoPat){
            this._unitOfWork = unitOfWork;
            this._repoOp = repo;
            this._repoS=repoS;
            this._repoOpTy=repoOpTy;
            this._repoPat=repoPat;
        }

        public async Task<OperationRequestDto> GetByIdAsync(OperationRequestId id)
        {
            var operationRequest = await this._repoOp.GetByIdAsync(id);
            
            if(operationRequest == null)
                return null;

            return new OperationRequestDto{Id=operationRequest.Id.AsGuid(), DoctorId=operationRequest.StaffId.AsString(), OperationTypeId=operationRequest.OperationTypeId.ToString(), MedicalRecordNumber=operationRequest.MedicalRecordNumber.AsString(), Deadline=operationRequest.DeadlineDate.ToString(), Priority=operationRequest.Priority.ToString(), Status=operationRequest.Status.ToString()};
        }

        public async Task<OperationRequestDto> AddOperationRequestAsync(CreatingOperationRequestDto dto)
        {
            var doctor=await _repoS.GetByIdAsync(new StaffId(dto.DoctorId));
            var operationType= await _repoOpTy.GetByIdAsync(new OperationTypeId(dto.OperationTypeId));
            if(doctor==null){
                    throw new NullReferenceException("The doctor with that Id does not exist!");
            }
            Console.WriteLine(doctor.Specialization.Id.AsString());
            Console.WriteLine(operationType.OperationTypeSpecializations.First().Specialization.Id.AsString());
            if(operationType.OperationTypeSpecializations.Any(specialization => specialization.Specialization.Id == doctor.Specialization.Id)){
                /*var patient = await _repoPat.GetByIdAsync(new MedicalRecordNumber(dto.MedicalRecordNumber));
                if(patient==null){
                    throw new NullReferenceException("The patient with that Id does not exist!");
                }*/
                var priority = Enum.Parse<Priority>(dto.Priority.ToUpper());
                var operationRequest = new OperationRequest(new MedicalRecordNumber(dto.MedicalRecordNumber), new StaffId(dto.DoctorId), new OperationTypeId(dto.OperationTypeId), DeadlineDate.FromString(dto.Deadline), priority);
                await _repoOp.AddAsync(operationRequest);
                await this._unitOfWork.CommitAsync();
                return new OperationRequestDto {DoctorId = operationRequest.StaffId.AsString(), OperationTypeId=operationRequest.OperationTypeId.AsString(), MedicalRecordNumber=operationRequest.MedicalRecordNumber.AsString(),
                    Deadline=operationRequest.DeadlineDate.ToString(), Priority=operationRequest.Priority.ToString(), Status=operationRequest.Status.ToString()};
            }else {
                throw new BusinessRuleValidationException("The Doctor Specialization not match with the Operation Type");
            }
        }
    }
}