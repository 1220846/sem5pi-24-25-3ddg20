import { StaffSpecialization } from "./staffSpecialization";

export interface OperationType{
    id: string;
    name: string;
    estimatedDuration: number;
    surgeryTime: number;
    anesthesiaTime: number;
    cleaningTime: number;
    staffSpecialization: StaffSpecialization
}