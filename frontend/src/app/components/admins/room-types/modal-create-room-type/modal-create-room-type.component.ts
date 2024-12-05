import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { CreatingRoomTypeDto } from '../../../../domain/CreatingRoomTypeDto';
import { RoomTypeService } from '../../../../services/room-type.service';
import { CommonModule } from '@angular/common';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { RadioButtonModule } from 'primeng/radiobutton';

@Component({
  selector: 'app-modal-create-room-type',
  standalone: true,
  imports: [InputTextModule, ButtonModule, DialogModule, FormsModule,ReactiveFormsModule,ToastModule,CommonModule,RadioButtonModule,InputTextareaModule],
  providers: [MessageService],
  templateUrl: './modal-create-room-type.component.html',
  styleUrl: './modal-create-room-type.component.scss'
})
export class ModalCreateRoomTypeComponent implements OnInit{
  @Output() roomTypeCreated = new EventEmitter<CreatingRoomTypeDto>();

  roomTypeForm: FormGroup;

  visible: boolean = false;
  descriptionBoxVisible: boolean = false;
  submitted: boolean = false;

  constructor(private roomTypeService: RoomTypeService, private messageService: MessageService,private fb: FormBuilder) { 
    this.roomTypeForm = this.fb.group({
      code: ['', [
        Validators.required, 
        Validators.minLength(8),   
        Validators.maxLength(8), 
        Validators.pattern('^[A-Za-z0-9-]+$') 
      ]],
      designation: ['', [
        Validators.required, 
        Validators.maxLength(100)
      ]],
      description: [''],
      isSurgical: ['', Validators.required],
    });
  }

  ngOnInit(): void {
  }

  showDialog() {
    this.visible = true;
    this.descriptionBoxVisible = false;
    this.roomTypeForm.reset();
    this.submitted = false;
  }
  

  showDescriptionBox() {
    this.descriptionBoxVisible = true;
  }

  saveData() {
    this.submitted = true;
    if (this.roomTypeForm.valid) {
      const roomType: CreatingRoomTypeDto = {
        code: this.roomTypeForm.value.code,  
        designation: this.roomTypeForm.value.designation,
        description: this.roomTypeForm.value.description || null,
        isSurgical: this.roomTypeForm.value.isSurgical === 'true' ? true : false,
      };
      this.roomTypeService.add(roomType).subscribe(
        (response) => {
          this.visible = false; 
          this.roomTypeForm.reset();
          this.submitted = false;
          this.descriptionBoxVisible = false;
          this.roomTypeCreated.emit(roomType);
          this.messageService.add({severity:'success', summary: 'Success', detail: 'Operation type added successfully',life: 2000});
        },
        (error) => {
          this.submitted = false;
          this.messageService.add({severity: 'error', summary: 'Error', detail: error, life: 3000});
        }
      );
    } else {
      this.messageService.add({severity: 'warn', summary: 'Warning', detail: 'Invalid form data!', life: 3000});
    }
  }
}
