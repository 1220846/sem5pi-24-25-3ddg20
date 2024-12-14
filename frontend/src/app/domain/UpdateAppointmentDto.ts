export interface UpdateAppointmentDto{
    surgeryRoomId: string | null;
    dateAndTime: string | null;
    staffsIds: string[] | null;
}