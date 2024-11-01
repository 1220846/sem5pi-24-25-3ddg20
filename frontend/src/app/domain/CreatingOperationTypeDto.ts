export interface CreatingOperationTypeDto {
    name: string;
    estimatedDuration: number;
    surgeryTime: number;
    anesthesiaTime: number;
    cleaningTime: number;
    staffSpecialization: StaffSpecialization[];
}