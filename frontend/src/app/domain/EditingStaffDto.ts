export interface EditingStaffDto {
    email: string | null;
    phoneNumber: string | null;
    specializationId: string | null;
    newAvailabilitySlotStartTime: number | null;
    newAvailabilitySlotEndTime: number | null;
    toRemoveAvailabilitySlotId: string | null;
}