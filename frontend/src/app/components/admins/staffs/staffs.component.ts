import { Component, ViewChild } from '@angular/core';
import { CreateStaffProfileComponent } from './modal-create-staff-profile/modal-create-staff-profile.component';
import { ListStaffProfilesComponent } from './list-staff-profiles/list-staff-profiles.component';

@Component({
  selector: 'app-staffs',
  standalone: true,
  imports: [CreateStaffProfileComponent, ListStaffProfilesComponent],
  templateUrl: './staffs.component.html',
  styleUrl: './staffs.component.scss'
})
export class StaffsComponent {
  @ViewChild(CreateStaffProfileComponent) modalCreateStaffProfileComponent!: CreateStaffProfileComponent;
  @ViewChild(ListStaffProfilesComponent, { static: false}) listStaffProfilesComponent!: ListStaffProfilesComponent;

  onStaffProfileCreated() {
    this.listStaffProfilesComponent.loadStaffs();
  }
}
