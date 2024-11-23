import { OperationRequestWithAllDataDto } from "./OperationRequestWithAllDataDto";
import { SurgeryRoom } from "./SurgeryRoom";

export interface Appointment {
    id: string;
    surgeryRoomDto: SurgeryRoom;
    operationRequestDto: OperationRequestWithAllDataDto;
    medicalRecordNumber: string;
    status: string;
    dateAndTime: Date;
}