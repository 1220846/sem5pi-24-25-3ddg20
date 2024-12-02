import { Component, ViewChild } from '@angular/core';
import { ModalCreateRoomTypeComponent } from './modal-create-room-type/modal-create-room-type.component';
import { ListRoomTypesComponent } from './list-room-types/list-room-types.component';

@Component({
  selector: 'app-room-types',
  standalone: true,
  imports: [ModalCreateRoomTypeComponent,ListRoomTypesComponent],
  templateUrl: './room-types.component.html',
  styleUrl: './room-types.component.scss'
})
export class RoomTypesComponent {

  @ViewChild(ListRoomTypesComponent, { static: false }) 
  listRoomTypesComponent!: ListRoomTypesComponent;

  onRoomTypeCreated() {
    this.listRoomTypesComponent.loadRoomTypes(); 
  }
}
