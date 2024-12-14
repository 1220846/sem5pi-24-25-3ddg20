using System;
using System.Collections.Generic;
using dddnetcore.Domain.SurgeryRooms;
using DDDSample1.Domain.AppointmentsStaffs;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Domain.Shared;
using Microsoft.VisualBasic;

namespace DDDSample1.Domain.Appointments{

    public class Appointment : Entity<AppointmentId>, IAggregateRoot
    {
        private AppointmentDateAndTime appointmentDateAndTime;

        public SurgeryRoom SurgeryRoom { get;  private set; }
        public OperationRequest OperationRequest { get;  private set; }
        public RoomNumber RoomNumber { get;  private set; }
        public OperationRequestId OperationRequestId { get;  private set; }
        public AppointmentStatus Status { get;  private set; }
        public AppointmentDateAndTime DateAndTime { get;  private set; }
        public ICollection<AppointmentStaff> AppointmentStaffs {get; private set;} = new List<AppointmentStaff>();

        private Appointment(){}

        public Appointment(SurgeryRoom surgeryRoom,OperationRequest operationRequest, AppointmentDateAndTime dateAndTime, object pENDING)
        {
            this.Id = new AppointmentId(Guid.NewGuid());
            this.SurgeryRoom = surgeryRoom;
            this.OperationRequest = operationRequest;
            this.DateAndTime = dateAndTime;
            this.Status = AppointmentStatus.SCHEDULED;
        }

        public Appointment(SurgeryRoom surgeryRoom, OperationRequest operationRequest, AppointmentDateAndTime appointmentDateAndTime)
        {
            SurgeryRoom = surgeryRoom;
            OperationRequest = operationRequest;
            this.appointmentDateAndTime = appointmentDateAndTime;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (Appointment)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public void Cancel(){
            this.Status = AppointmentStatus.CANCELED;
        }

        public void Complete(){
            this.Status = AppointmentStatus.COMPLETED;
        }

        public override string ToString() {
            return $"Appointment: [ID={Id}, RoomNumber={SurgeryRoom?.Id?.Value ?? "N/A"}, " +
                $"OperationRequestID={OperationRequest?.Id?.Value ?? "N/A"}, Status={Status}, " +
                $"DateAndTime={DateAndTime.DateAndTime:yyyy-MM-dd HH:mm}]";
        }

        public void ChangeSurgeryRoom(SurgeryRoom newSurgeryRoom){
            ArgumentNullException.ThrowIfNull(newSurgeryRoom);
            this.SurgeryRoom = newSurgeryRoom;
        }

        public void ChangeDateAndTime(AppointmentDateAndTime newDateAndTime){
            ArgumentNullException.ThrowIfNull(newDateAndTime);
            this.DateAndTime = newDateAndTime;
        }
    }
}