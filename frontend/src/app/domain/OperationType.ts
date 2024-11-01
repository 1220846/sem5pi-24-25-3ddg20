
export interface OperationType{
    id: number;
    name: string;
    estimatedDuration: number;
    surgeryTime: number;
    anesthesiaTime: number;
    cleaningTime: number;
    staffSpecialization: StaffSpecialization[];
}