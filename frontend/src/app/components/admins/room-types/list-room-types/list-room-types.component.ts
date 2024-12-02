import { Component, OnInit } from '@angular/core';
import { RoomType } from '../../../../domain/RoomType';
import { RoomTypeService } from '../../../../services/room-type.service';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { CardModule } from 'primeng/card';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-list-room-types',
  standalone: true,
  imports: [CardModule,ScrollPanelModule,CommonModule],
  templateUrl: './list-room-types.component.html',
  styleUrl: './list-room-types.component.scss'
})
export class ListRoomTypesComponent implements OnInit{

  roomTypes: RoomType[] = [];
  responsiveOptions: any[] | undefined;

  constructor(
    private roomTypeService: RoomTypeService
  ) {}

  ngOnInit(): void {
    this.loadRoomTypes();
  }

  loadRoomTypes(): void {
    this.roomTypeService.getAll().subscribe({next: (data) => {
        this.roomTypes = data;},
      error: (error) => console.error('Error fetching room types:', error)
    });
  }
}
