import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import { FloatLabelModule } from 'primeng/floatlabel';
import { CalendarModule } from 'primeng/calendar';
import { CommonModule } from '@angular/common';
import { ToastModule } from 'primeng/toast';
import { DropdownModule } from 'primeng/dropdown';
import { DialogModule } from 'primeng/dialog';
import { EditingOperationRequestDto } from '../../../../domain/EditingOperationRequestDto';
import { OperationRequest } from '../../../../domain/OperationRequests';
import { OperationRequestService } from '../../../../services/operation-request.service';
import { MessageService } from 'primeng/api';

interface Options {
  label: string;
}

@Component({
  selector: 'modal-update-operation-requests',
  standalone: true,
  providers:[MessageService],
  imports: [CommonModule, InputTextModule, FormsModule, ReactiveFormsModule,ToastModule ,FloatLabelModule,CalendarModule,DropdownModule,DialogModule],
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
  visible: boolean = false;
  changes: boolean = false;

  @Output() operationRequestEdited = new EventEmitter<EditingOperationRequestDto>();
  @Input() operationRequest: OperationRequest | null = null;


  constructor(private fb: FormBuilder, 
    private opRequestService: OperationRequestService,
    private messageService: MessageService
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

    showDialog() {
        this.visible = true;
    }

  changeInfo(){
    if(this.operationRequestForm.valid){
      const operationRequest:EditingOperationRequestDto={
        deadline:this.operationRequestForm.value.deadline ? this.operationRequestForm.value.deadline : null,
        priority:this.operationRequestForm.value.selectedPriority ? this.operationRequestForm.value.selectedPriority : null
      }
      this.opRequestService.edit(this.operationRequest?.id!,operationRequest).subscribe(
        (response) => {
          this.changes = true;
          this.messageService.add({severity: 'success', summary: 'Success', detail: 'Operation Request edited successfully', life: 2000});
          this.operationRequestEdited.emit();
        },
        (error) => {
          console.error("Error editing Operation request:", error);
          this.messageService.add({severity: 'error', summary: 'Error', detail:'Failed to edit Operation Request', life: 2000});
        }
      );
    }else {
      this.messageService.add({severity: 'warn', summary: 'Error', detail:'Some inputted data is invalid', life: 2000});
    }
    this.visible=false;
  }

  closeDialog(){
    this.visible=false;
  }
}
