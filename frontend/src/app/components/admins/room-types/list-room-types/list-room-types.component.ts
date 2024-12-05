import { Component, OnInit } from '@angular/core';
import { RoomType } from '../../../../domain/RoomType';
import { RoomTypeService } from '../../../../services/room-type.service';
import { CommonModule } from '@angular/common';
import { TagModule } from 'primeng/tag';
import { AccordionModule } from 'primeng/accordion';
import { AvatarModule } from 'primeng/avatar';
import { BadgeModule } from 'primeng/badge';
import { ScrollerModule } from 'primeng/scroller';

@Component({
  selector: 'app-list-room-types',
  standalone: true,
  imports: [AccordionModule,
    AvatarModule,
    BadgeModule,
    TagModule,
    CommonModule,
    ScrollerModule],
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

  roomTypeDescription(roomType: RoomType): string {
    return roomType.description ? roomType.description : "No description";
  }
}
