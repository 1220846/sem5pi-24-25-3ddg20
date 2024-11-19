import { User } from "@auth0/auth0-angular";
import { AvailabilitySlot } from "./AvailabilitySlot";
import { Specialization } from "./Specialization";

export interface Staff {
    id: string;
    firstName: string;
    lastName: string;
    fullName: string;
    email: string;
    phoneNumber: string;
    licenseNumber: string;
    status: string;
    availabilitySlots : AvailabilitySlot[];
    specialization: Specialization;
    user: User;
}