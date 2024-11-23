import { OperationType } from "./OperationType";

export interface OperationRequestWithAllDataDto {
    id: string;
    doctorId: string;
    operationType: OperationType;
    medicalRecordNumber: string;
    deadline: string;
    priority: string;
    status: string;
}