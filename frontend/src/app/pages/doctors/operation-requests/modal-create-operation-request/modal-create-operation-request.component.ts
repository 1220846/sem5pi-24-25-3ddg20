import { Component, OnInit } from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';
import { FloatLabelModule } from 'primeng/floatlabel';
import { CalendarModule } from 'primeng/calendar';
import { DropdownModule } from 'primeng/dropdown';
import { DialogModule } from 'primeng/dialog';
import { SplitButtonModule } from 'primeng/splitbutton';
import { MessageService } from 'primeng/api';


interface Options {
  label: string;
}

@Component({
  selector: 'modal-create-operation-request',
  standalone: true,
  imports: [InputTextModule, FormsModule, FloatLabelModule,CalendarModule,DropdownModule,DialogModule,SplitButtonModule],
  templateUrl: './modal-create-operation-request.component.html',
  styleUrl: './modal-create-operation-request.component.scss'
})
export class ModalCreateOperationRequestComponent implements OnInit{
  DoctorId: string | undefined;
  OperationTypeId: string | undefined;
  MedicalRecordNumber: string | undefined;
  Deadline: Date | undefined;
  Priority: string | undefined;
  options: Options[] | undefined;

  selectedPriority: Options | undefined;

  ngOnInit() {
    this.options = [
        {label:'Elective'},
        {label:'Urgent'},
        {label:'Emergency'}
    ];
  }

  visible: boolean = false;

    showDialog() {
        this.visible = true;
    }
}
