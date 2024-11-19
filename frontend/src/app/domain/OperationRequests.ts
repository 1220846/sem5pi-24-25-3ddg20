export interface OperationRequest {    
    id:string;
    DoctorId: string;
    OperationTypeId: string;
    OperationTypeName: string | null;
    MedicalRecordNumber: string;
    Deadline: string;
    Priority: string;
    Status: string;
}