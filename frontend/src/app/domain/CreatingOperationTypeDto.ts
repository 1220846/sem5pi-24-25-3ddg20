import { CreatingStaffSpecializationDto } from "./creatingStaffSpecializationDto";

export interface CreatingOperationTypeDto {
    name: string;
    estimatedDuration: number;
    surgeryTime: number;
    anesthesiaTime: number;
    cleaningTime: number;
    staffSpecializations: CreatingStaffSpecializationDto[];
}