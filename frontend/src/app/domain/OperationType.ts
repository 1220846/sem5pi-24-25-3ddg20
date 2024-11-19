import { StaffSpecialization } from "./StaffSpecialization";

export interface OperationType{
    id: string;
    name: string;
    estimatedDuration: number;
    surgeryTime: number;
    anesthesiaTime: number;
    cleaningTime: number;
    operationTypeStatus:string;
    staffSpecializationDtos: StaffSpecialization[];
}