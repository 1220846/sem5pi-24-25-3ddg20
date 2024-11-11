import { Component, OnInit, ViewChild } from '@angular/core';
import { PatientService } from '../../../../services/patient.service';
import { OverlayPanel, OverlayPanelModule } from 'primeng/overlaypanel';
import { Patient } from '../../../../domain/Patient';
import { AccordionModule } from 'primeng/accordion';
import { AvatarModule } from 'primeng/avatar';
import { BadgeModule } from 'primeng/badge';
import { TagModule } from 'primeng/tag';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { ScrollerModule } from 'primeng/scroller';
import { DialogModule } from 'primeng/dialog';
import { PaginatorModule, PaginatorState } from 'primeng/paginator';

@Component({
  selector: 'app-list-patients',
  standalone: true,
  imports: [AccordionModule,AvatarModule,BadgeModule,TagModule,CommonModule,ScrollerModule,DropdownModule,InputTextModule,FormsModule,OverlayPanelModule,ButtonModule, DialogModule, PaginatorModule],
  templateUrl: './list-patients.component.html',
  styleUrl: './list-patients.component.scss'
})
export class ListPatientsComponent implements OnInit{

  @ViewChild('filterPanel') filterPanel!: OverlayPanel;
  
  patients: Patient[] = [];
  filterfirstName: string = '';
  filterlastName: string = '';
  filterfullName: string = '';
  filteremail: string = '';
  filterbirthDate: string = '';
  filterphoneNumber: string = '';
  filterid: string = '';
  filtergender: string = '';
  pageNumber: number = 0;
  pageSize: number = 10;
  totalPatients: number = 0;

  genderOptions = [
    { label: 'Male', value: 'Male' },
    { label: 'Female', value: 'Female' },
    { label: 'Undefined', value: 'Undefined' },
    { label: 'Other', value: 'Other' }
  ];

  

  visible: boolean = false;
  selectedPatient: any = null;

  constructor(
    private patientService: PatientService
  ){}

  ngOnInit(): void {
    this.countTotalPatients();
    console.log(this.totalPatients);
    this.loadPatients();
  }

  countTotalPatients(){
    this.patientService.patientCount().subscribe({
      next: (data) => {
        this.totalPatients = data;
        console.log(this.totalPatients);},
        error: (error) => console.error('Error fetching patient count:', error)
      });
  }
  
  loadPatients() {
    this.patientService.getPage(this.filterfirstName, this.filterlastName, this.filterfullName, this.filteremail,
      this.filterbirthDate, this.filterphoneNumber, this.filterid, this.filtergender, this.pageNumber+1, this.pageSize
    ).subscribe({next: (data) => {
      this.patients = data;},
    error: (error) => console.error('Error fetching patients:', error)
  });
  }

  applyFilters(): void {
    this.pageNumber = 0;
    this.loadPatients();
    this.filterPanel.hide();
  }

  clearFilters(): void {
    this.filterfirstName = '';
    this.filterlastName = '';
    this.filterfullName = '';
    this.filteremail = '';
    this.filterbirthDate= '';
    this.filterphoneNumber = '';
    this.filterid = '';
    this.filtergender = '';
    this.pageNumber = 0;
    this.loadPatients();
    this.filterPanel.hide();
  }

  calculateAge(dateOfBirthStr: string): number{
    const [day, month1, year] = dateOfBirthStr.split('/').map(Number);
    const dateOfBirth = new Date(year, month1 - 1, day);

    const today = new Date();
    let idade = today.getFullYear() - dateOfBirth.getFullYear();
    const month = today.getMonth() - dateOfBirth.getMonth();

    if (month < 0 || (month === 0 && today.getDate() < dateOfBirth.getDate())) {
        idade--;
    }

    return idade;
  }

  showDialog(patient: any) {
    this.selectedPatient = patient;
    this.visible = true;
  }

  onPageChange(event: PaginatorState) {
    this.pageNumber = (event.first ?? 0) / (event.rows ?? 10);
    this.pageSize = event.rows ?? 10;
    this.loadPatients();
  }
}
