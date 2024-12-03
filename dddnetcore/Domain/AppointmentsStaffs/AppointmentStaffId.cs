using System;
using System.Text.Json.Serialization;
using DDDSample1.Domain.Appointments;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Staffs;

namespace DDDSample1.Domain.AppointmentsStaffs {
  public class AppointmentStaffId : EntityId {

    public AppointmentId AppointmentId { get; private set; }

    public StaffId StaffId { get; private set; }

    public AppointmentStaffId(string id): base(id) {
        var parts = id.Split('-');
        if (parts.Length == 2) {
            AppointmentId = new AppointmentId(parts[0]);
            StaffId = new StaffId(parts[1]);
        }
    }

    [JsonConstructor]
    public AppointmentStaffId(AppointmentId appointmentId, StaffId staffId) : base($"{appointmentId.AsGuid()}-{staffId.Id}") {
        AppointmentId = appointmentId;
        StaffId = staffId;
    }
    public override string AsString() {
        return $"{AppointmentId.AsGuid()}-{StaffId.Id}";
    }

    protected override object createFromString(String text) {
        return text;
    }

  }
}