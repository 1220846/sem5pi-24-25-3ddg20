import { Component, OnInit } from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import { FloatLabelModule } from 'primeng/floatlabel';
import { CalendarModule } from 'primeng/calendar';
import { ToastModule } from 'primeng/toast';
import { DropdownModule } from 'primeng/dropdown';
import { DialogModule } from 'primeng/dialog';

interface Options {
  label: string;
}

@Component({
  selector: 'modal-update-operation-requests',
  standalone: true,
  imports: [InputTextModule, FormsModule, ReactiveFormsModule,ToastModule ,FloatLabelModule,CalendarModule,DropdownModule,DialogModule],
  templateUrl: './modal-update-operation-requests.component.html',
  styleUrls: ['./modal-update-operation-requests.component.scss']
})
export class ModalUpdateOperationRequestsComponent implements OnInit{
  Deadline: Date | undefined;
  Priority: string | undefined;
  MedicalRecordNumber: string | undefined;
  options: Options[] | undefined;
  operationRequestForm: FormGroup;
  selectedPriority: Options | undefined;

  constructor(private fb: FormBuilder
    
  ){
    this.operationRequestForm = this.fb.group({
      selectedPriority: [null, Validators.required],
      deadline: ['', Validators.required]
    });
  }

  optionsPriority = [
    { label: 'Elective', value: 'ELECTIVE' },
    { label: 'Urgent', value: 'URGENT' },
    { label: 'Emergency', value: 'EMERGENCY' }
  ];

  ngOnInit() {
    
  }

  visible: boolean = false;

    showDialog() {
        this.visible = true;
    }

  saveData(){

  }
}
