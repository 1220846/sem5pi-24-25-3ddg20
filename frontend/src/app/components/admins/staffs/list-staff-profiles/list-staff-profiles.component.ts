import { Component, OnInit, ViewChild } from '@angular/core';
import { StaffService } from '../../../../services/staff.service';
import { SpecializationService } from '../../../../services/specialization.service';
import { OverlayPanel, OverlayPanelModule } from 'primeng/overlaypanel';
import { Specialization } from '../../../../domain/specialization';
import { Staff } from '../../../../domain/Staff';
import { AccordionModule } from 'primeng/accordion';
import { AvatarModule } from 'primeng/avatar';
import { BadgeModule } from 'primeng/badge';
import { TagModule } from 'primeng/tag';
import { CommonModule } from '@angular/common';
import { ScrollerModule } from 'primeng/scroller';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { ModalDeactivateStaffProfileComponent } from '../modal-deactivate-staff-profile/modal-deactivate-staff-profile.component';
import { ModalEditStaffProfileComponent } from '../modal-edit-staff-profile/modal-edit-staff-profile.component';

@Component({
  selector: 'app-list-staff-profiles',
  standalone: true,
  imports: [AccordionModule, AvatarModule, BadgeModule, TagModule, CommonModule, ScrollerModule, DropdownModule, InputTextModule, FormsModule, OverlayPanelModule, ButtonModule, ModalDeactivateStaffProfileComponent, ModalEditStaffProfileComponent],
  templateUrl: './list-staff-profiles.component.html',
  styleUrl: './list-staff-profiles.component.scss'
})
export class ListStaffProfilesComponent implements OnInit {

  @ViewChild('filterPanel') filterPanel!: OverlayPanel;

  staffs: Staff[] = [];
  specializations: Specialization[] = [];

  filterFirstName: string = '';
  filterLastName: string = '';
  filterFullName: string = '';
  filterEmail: string = '';
  filterSpecializationId: string = '';
  filterPhoneNumber: string = '';
  filterId: string = '';
  filterLicenseNumber: string = '';
  filterStatus: string = '';

  statusOptions = [
    { label: 'None', value: null },
    { label: 'Active', value: 'ACTIVE' },
    { label: 'Deactivated', value: 'DEACTIVATED' }
  ];

  constructor(
    private staffService: StaffService,
    private specializationService: SpecializationService
  ) { }


  ngOnInit(): void {
    this.loadStaffs();
    this.loadSpecializations();
  }

  applyFilters(): void {
    this.loadStaffs();
    this.filterPanel.hide();
  }

  clearFilters(): void {
    this.filterFirstName = '';
    this.filterLastName = '';
    this.filterFullName = '';
    this.filterEmail = '';
    this.filterSpecializationId = '';
    this.filterPhoneNumber = '';
    this.filterId = '';
    this.filterLicenseNumber = '';
    this.filterStatus = '';
  }

  loadStaffs(): void {
    this.staffService.getAllAndFilter(
      this.filterFirstName,
      this.filterLastName,
      this.filterFullName,
      this.filterEmail,
      this.filterSpecializationId,
      this.filterPhoneNumber,
      this.filterId,
      this.filterLicenseNumber,
      this.filterStatus
    ).subscribe({
      next: (data) => { this.staffs = data; },
      error: (error) => console.error('Error fetching staffs:', error)
    });
  }

  loadSpecializations(): void {
    this.specializationService.getAll().subscribe({
      next: (data) => {
        this.specializations = [{ id: '', name: 'None' }, ...data];
        this.filterSpecializationId = '';
      },
      error: (error) => console.error('Error fetching specializations:', error)
    });
  }

  
  onStaffProfileDeacivated() {
    this.loadStaffs();
  }
  /*getSpecializationName(specializationId: number): string {
    this.specializationService.getById(specializationId)
  }*/
}
