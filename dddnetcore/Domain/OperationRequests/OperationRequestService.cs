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
using dddnetcore.Domain.OperationRequests.UpdateOperationRequestDto;
using DDDSample1.Domain.SystemLogs;
using System.Net;
using DDDSample1.Domain.Auth;
using dddnetcore.Domain.OperationRequests;

namespace DDDSample1.Domain.OperationRequests
{
    public class OperationRequestService{

        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IOperationRequestRepository _repoOperationRequest;

        private readonly ISystemLogRepository _repoSystemLog;

        private readonly IOperationTypeRepository _repoOperationType;

        private readonly IPatientRepository _repoPatient;

        private readonly IStaffRepository _repoStaff;
        private readonly AuthenticationService _authService;

        public OperationRequestService(IUnitOfWork unitOfWork, IOperationRequestRepository repo, IStaffRepository repoS, IOperationTypeRepository repoOpTy, IPatientRepository repoPat, ISystemLogRepository systemLogRepository, AuthenticationService authService){
            this._unitOfWork = unitOfWork;
            this._repoOperationRequest = repo;
            this._repoStaff=repoS;
            this._repoOperationType=repoOpTy;
            this._repoPatient=repoPat;
            this._repoSystemLog=systemLogRepository;
            this._authService = authService;
        }

        public async Task<OperationRequestDto> GetByIdAsync(OperationRequestId id)
        {
            var operationRequest = await this._repoOperationRequest.GetByIdAsync(id);
            if(operationRequest == null)
                return null;

            return new OperationRequestDto{Id = operationRequest.Id.AsGuid(),
                        DoctorId = operationRequest.StaffId.Id, 
                        OperationTypeId=operationRequest.OperationTypeId.Value, 
                        MedicalRecordNumber=operationRequest.MedicalRecordNumber.Id,
                        Deadline=operationRequest.DeadlineDate.Date.ToString(), 
                        Priority=operationRequest.Priority.ToString(), 
                        Status=operationRequest.Status.ToString()};
        }

        public async Task<OperationRequestDto> AddOperationRequestAsync(CreatingOperationRequestDto dto)
        {
            var doctor=(await _repoStaff.GetStaffsAsync(id: dto.DoctorId)).FirstOrDefault();
            if(doctor==null){
                throw new NullReferenceException("The doctor with that Id does not exist!");
            }
            var operationType= await _repoOperationType.GetByIdAsync(new OperationTypeId(dto.OperationTypeId));
            if(operationType.OperationTypeStatus.Equals(OperationTypeStatus.INACTIVE)){
                throw new BusinessRuleValidationException("The operation Type Is Inactive");
            }
            if(operationType.OperationTypeSpecializations.Any(specialization => specialization.Specialization.Id == doctor.Specialization.Id)){
                var patient = await _repoPatient.GetByIdAsync(new MedicalRecordNumber(dto.MedicalRecordNumber));
                if(patient==null){
                    throw new NullReferenceException("The patient with that Id does not exist!");
                }
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
           

        public async Task<OperationRequestDto> UpdateOperationRequestAsync(Guid operationRequestId, UpdateOperationRequestDto updateOperationRequestDto){
            
            var operationRequest=await this._repoOperationRequest.GetByIdAsync(new OperationRequestId(operationRequestId));
          

            if(operationRequest==null){
                throw new BusinessRuleValidationException("The operation Request Inserted doesn't exists!");
            }
            var operationRequestChanges=new List<string>();

            if(updateOperationRequestDto.Priority != null){
                if (Enum.TryParse<Priority>(updateOperationRequestDto.Priority.ToUpper(), out var priority))
                {
                    operationRequestChanges.Add($"Priority changed from {operationRequest.Priority.ToString()} to {updateOperationRequestDto.Priority}");
                    operationRequest.ChangePriority(priority);
                }
                else
                {
                    throw new BusinessRuleValidationException("Invalid priority value.");
                }
            }

            if(updateOperationRequestDto.Deadline != null){
                operationRequestChanges.Add($"Last Date changed from {operationRequest.DeadlineDate.Date.ToString()} to {updateOperationRequestDto.Deadline.ToString()}");
                operationRequest.ChangeDeadline(DeadlineDate.FromString(updateOperationRequestDto.Deadline));
            }

            await _repoOperationRequest.UpdateAsync(operationRequest);

            if (operationRequestChanges.Count > 0){
                string operationRequestLog = string.Join(", ", operationRequestChanges);
                await this._repoSystemLog.AddAsync(new SystemLog(Operation.UPDATE, Entity.OPERATION_REQUEST, operationRequestLog,operationRequest.Id.AsGuid().ToString()));
                Console.WriteLine(operationRequest.Id.AsGuid().ToString());
            }

           
            await this._unitOfWork.CommitAsync();


            return new OperationRequestDto{Id = operationRequest.Id.AsGuid(),
                        DoctorId = operationRequest.StaffId.Id, 
                        OperationTypeId=operationRequest.OperationTypeId.Value, 
                        MedicalRecordNumber=operationRequest.MedicalRecordNumber.Id,
                        Deadline=operationRequest.DeadlineDate.Date.ToString(), 
                        Priority=operationRequest.Priority.ToString(), 
                        Status=operationRequest.Status.ToString()};
        }


        public async Task<OperationRequestDto> RemoveAsync(Guid id, RemoveOperationRequestDto removeDto) {
            OperationRequest operationRequest = await this._repoOperationRequest.GetByIdAsync(new OperationRequestId(id)) ?? throw new NullReferenceException("Operation request not found!");
            Staff staff = (await this._repoStaff.GetStaffsAsync(id: operationRequest.StaffId.Id)).FirstOrDefault() ?? throw new NullReferenceException("Owner of operation request not found!");
            
            if (!staff.Username.Name.Equals(_authService.GetLoggedInUsername(removeDto.token)))
                throw new BusinessRuleValidationException("Cannot remove other's operation requests!");

            if (operationRequest.Status.Equals(OperationRequestStatus.SCHEDULED))
                throw new BusinessRuleValidationException("Cannot remove scheduled operation requests!");

            OperationRequestDto dto = new(operationRequest);

            this._repoOperationRequest.Remove(operationRequest);
            await this._unitOfWork.CommitAsync();

            return dto;
        }
    }
}