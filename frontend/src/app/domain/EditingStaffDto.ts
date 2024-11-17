export interface EditingStaffDto {
    email: string | null;
    phoneNumber: string | null;
    specializationId: string | null;
    newAvailabilitySlotStartTime: Date | null;
    newAvailabilitySlotEndTime: Date | null;
    toRemoveAvailabilitySlotId: string | null;
}