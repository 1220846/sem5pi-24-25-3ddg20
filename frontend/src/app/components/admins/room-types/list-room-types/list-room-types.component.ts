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

  rooms = [
    { name: 'Room 1' },
    { name: 'Room 2' },
    { name: 'Room 3' },
    { name: 'Room 4' },
    { name: 'Room 5' },
    { name: 'Room 6' },
    { name: 'Room 7' },
    { name: 'Room 8' },
    { name: 'Room 9' },
    { name: 'Room 10' }
  ];
  

  constructor(
    private roomTypeService: RoomTypeService
  ) {}

  ngOnInit(): void {
    this.loadRoomTypes();

    this.responsiveOptions = [
      {
          breakpoint: '1199px',
          numVisible: 1,
          numScroll: 1
      },
      {
          breakpoint: '991px',
          numVisible: 2,
          numScroll: 1
      },
      {
          breakpoint: '767px',
          numVisible: 1,
          numScroll: 1
      }
  ];
  }

  loadRoomTypes(): void {
    this.roomTypeService.getAll().subscribe({next: (data) => {
        this.roomTypes = data;},
      error: (error) => console.error('Error fetching room types:', error)
    });
  }
}
