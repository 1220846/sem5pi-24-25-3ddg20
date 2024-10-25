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
using System.Reflection.Metadata.Ecma335;

namespace DDDSample1.Domain.OperationRequests
{
    public class OperationRequestService{

        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IOperationRequestRepository _repoOperationRequest;

        private readonly IOperationTypeRepository _repoOperationType;

        private readonly IPatientRepository _repoPatient;

        private readonly IStaffRepository _repoStaff;

        public OperationRequestService(IUnitOfWork unitOfWork, IOperationRequestRepository repo, IStaffRepository repoS, IOperationTypeRepository repoOpTy, IPatientRepository repoPat){
            this._unitOfWork = unitOfWork;
            this._repoOperationRequest = repo;
            this._repoStaff=repoS;
            this._repoOperationType=repoOpTy;
            this._repoPatient=repoPat;
        }

        public async Task<OperationRequestDto> GetByIdAsync(OperationRequestId id)
        {
            var operationRequest = await this._repoOperationRequest.GetByIdAsync(id);
            
            if(operationRequest == null)
                return null;

            return new OperationRequestDto{Id=operationRequest.Id.AsGuid(), DoctorId=operationRequest.StaffId.AsString(), OperationTypeId=operationRequest.OperationTypeId.ToString(), MedicalRecordNumber=operationRequest.MedicalRecordNumber.AsString(), Deadline=operationRequest.DeadlineDate.ToString(), Priority=operationRequest.Priority.ToString(), Status=operationRequest.Status.ToString()};
        }

        public async Task<OperationRequestDto> AddOperationRequestAsync(CreatingOperationRequestDto dto)
        {
            var doctor=(await _repoStaff.GetStaffsAsync(id: dto.DoctorId)).FirstOrDefault();
            Console.WriteLine(doctor.ToString());
            var operationType= await _repoOperationType.GetByIdAsync(new OperationTypeId(dto.OperationTypeId));
            Console.WriteLine(operationType);
            if(doctor==null){
                    throw new NullReferenceException("The doctor with that Id does not exist!");
            }
            if(operationType.OperationTypeSpecializations.Any(specialization => specialization.Specialization.Id == doctor.Specialization.Id)){
                /*var patient = (await _repoPatient.GetPatientAsync(id: dto.MedicalRecordNumber)).FirstOrDefault();
                if(patient==null){
                    throw new NullReferenceException("The patient with that Id does not exist!");
                }*/
                var priority = Enum.Parse<Priority>(dto.Priority.ToUpper());
                var operationRequest = new OperationRequest(new MedicalRecordNumber(dto.MedicalRecordNumber), new StaffId(dto.DoctorId), new OperationTypeId(dto.OperationTypeId), DeadlineDate.FromString(dto.Deadline), priority);
                await _repoOperationRequest.AddAsync(operationRequest);
                await this._unitOfWork.CommitAsync();
                return new OperationRequestDto {Id = operationRequest.Id.AsGuid(),
                        DoctorId = operationRequest.StaffId.Id, 
                        OperationTypeId=operationRequest.OperationTypeId.Value, 
                        MedicalRecordNumber=operationRequest.MedicalRecordNumber.Id,
                        Deadline=operationRequest.DeadlineDate.Date.ToString(), 
                        Priority=operationRequest.Priority.ToString(), 
                        Status=operationRequest.Status.ToString()};
            }else {
                throw new BusinessRuleValidationException("The Doctor Specialization not match with the Operation Type");
            }
        }

        public async Task<List<OperationRequestDto>> GetOperationRequestsAsync(string patientId = null, Guid? operationTypeId = null, string priority=null ,string status = null){
            var operationRequests =await _repoOperationRequest.GetOperationRequestsAsync(patientId,operationTypeId,priority,status);
            List<OperationRequestDto> operationRequestsDto = operationRequests.ConvertAll<OperationRequestDto>(operationRequest => new OperationRequestDto{
                        Id = operationRequest.Id.AsGuid(),
                        DoctorId = operationRequest.StaffId.Id, 
                        OperationTypeId=operationRequest.OperationTypeId.Value, 
                        MedicalRecordNumber=operationRequest.MedicalRecordNumber.Id,
                        Deadline=operationRequest.DeadlineDate.Date.ToString(), 
                        Priority=operationRequest.Priority.ToString(), 
                        Status=operationRequest.Status.ToString()});
            return operationRequestsDto;
        }

        public async Task<OperationRequestDto> RemoveAsync(Guid id) {
            OperationRequest operationRequest = await this._repoOperationRequest.GetByIdAsync(new OperationRequestId(id)) ?? throw new NullReferenceException("Operation request not found!");

            //TODO check if operation request is mine

            if (operationRequest.Status.Equals(OperationRequestStatus.SCHEDULED))
                throw new BusinessRuleValidationException("Cannot remove scheduled operation requests!");

            OperationRequestDto dto = new(operationRequest);

            this._repoOperationRequest.Remove(operationRequest);

            return dto;
        }
    }
}