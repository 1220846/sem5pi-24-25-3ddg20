export interface OperationRequest {    
    id:string;
    doctorId: string;
    operationTypeId: string;
    operationTypeName: string | null;
    medicalRecordNumber: string;
    deadline: string;
    priority: string;
    status: string;
}